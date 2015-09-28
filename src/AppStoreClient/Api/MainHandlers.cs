using Starcounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStoreClient {
    internal class MainHandlers {
        public void Register() {
            string[] defaultVisibleStores = new string[] { "Developer Samples", "Essentials" };

            // Menu
            Handle.GET("/appstoreclient/menu", () => {
                return new Page() { Html = "/AppStoreClient/viewmodels/MenuPage.html" };
            });

            // App-icon (Launchpad)
            Starcounter.Handle.GET("/appstoreclient/app-icon", () => {
                return new Page() {
                    Html = "/AppStoreClient/viewmodels/AppIconPage.html"
                };
            });

            // App-name (Launchpad)
            Starcounter.Handle.GET("/appstoreclient/app-name", () => {
                return new AppNamePage();
            });

            // Workspace root (Launchpad)
            Starcounter.Handle.GET("/appstoreclient", () => {
                return Self.GET("/appstoreclient/apps");
            });

            Handle.GET("/appstoreclient/standalone", () => {
                Session session = Session.Current;

                if (session != null && session.Data != null) {
                    return session.Data;
                }

                StandalonePage page = new StandalonePage();

                if (session == null) {
                    session = new Session(SessionOptions.PatchVersioning);
                    page.Html = "/AppStoreClient/viewmodels/StandalonePage.html";
                }

                //page.CurrentPage = Self.GET<ListAppStoreAppsPage>("/AppStoreClient/apps");
                page.Session = session;

                return page;
            });

            //
            // App Store apps
            //
            Handle.GET("/appstoreclient/apps", (Request request) => {
                StandalonePage master = Self.GET<StandalonePage>("/appstoreclient/standalone");
                ListAppStoreAppsPage page = new ListAppStoreAppsPage() {
                    Html = "/AppStoreClient/viewmodels/AppStoreAppsPage.html",
                    Uri = request.Uri
                };

                StoreSetting setting = Db.SQL<StoreSetting>("SELECT ss FROM AppStoreClient.StoreSetting ss").First;

                if (setting != null && setting.DisplayAllStores) {
                    page.DisplayAllStores = true;
                } else if (setting == null || string.IsNullOrEmpty(setting.DisplayStores)) {
                    foreach (var store in defaultVisibleStores) {
                        page.DisplayStores.Add().StringValue = store;
                    }
                    
                    page.DisplayAllStores = false;
                } else {
                    foreach (string s in setting.DisplayStores.Split(new char[] { ',', ';' })) {
                        string name = s.Trim();

                        if (string.IsNullOrEmpty(name)) {
                            continue;
                        }

                        page.DisplayStores.Add().StringValue = name;
                    }
                }
                
                master.CurrentPage = page;

                return master;
            });

            Handle.GET("/appstoreclient/setdisplaystores/{?}", (string value) => {
                StoreSetting setting = Db.SQL<StoreSetting>("SELECT ss FROM AppStoreClient.StoreSetting ss").First;

                value = System.Web.HttpUtility.UrlDecode(value);

                Db.Transact(() => {
                    if (setting == null) {
                        setting = new StoreSetting();
                    }

                    if (value == "all") {
                        setting.DisplayAllStores = true;
                        setting.DisplayStores = null;
                    } else {
                        setting.DisplayAllStores = false;
                        setting.DisplayStores = value;
                    }
                });

                return 200;
            });

            UriMapping.Map("/appstoreclient/menu", "/sc/mapping/menu");
            UriMapping.Map("/appstoreclient/app-name", "/sc/mapping/app-name");
            UriMapping.Map("/appstoreclient/app-icon", "/sc/mapping/app-icon");
        }
    }
}
