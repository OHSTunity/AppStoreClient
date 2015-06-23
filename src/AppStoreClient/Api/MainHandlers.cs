using Starcounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolyjuiceNamespace;

namespace AppStoreClient {
    internal class MainHandlers {
        public void Register() {
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

//                page.refreshItems();
                master.CurrentPage = page;

                return master;
            });

            Polyjuice.Map("/appstoreclient/menu", "/polyjuice/menu");
            Polyjuice.Map("/appstoreclient/app-name", "/polyjuice/app-name");
            Polyjuice.Map("/appstoreclient/app-icon", "/polyjuice/app-icon");
        }
    }
}
