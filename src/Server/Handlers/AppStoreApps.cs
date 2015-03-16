using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStoreApp.Server.Partials;
using AppStoreClient.Server.JSON;
using Starcounter;

namespace AppStoreApp.Server.Handlers {

    public class AppStoreApps {

        public static void Register() {

            Handle.GET("/AppStoreClient/standalone", () => {
                var session = Session.Current;
                if (session == null)
                    session = new Session(SessionOptions.PatchVersioning);

                if (session.Data != null)
                    return session.Data;

                var appStore = new AppStore();
                appStore.AppList = X.GET<ListAppStoreAppsPage>("/AppStoreClient/apps");
                appStore.Session = session;
                appStore.Html = "/AppStoreClient/standalone-appstore.html";
                return appStore;
            });

            //
            // App Store apps
            //
            Handle.GET( "/AppStoreClient/apps", (Request request) => {

                Partials.ListAppStoreAppsPage page = new Partials.ListAppStoreAppsPage() {
                    Html = "/AppStoreClient/partials/appstoreapps.html",
                    Uri = request.Uri
                };

                page.refreshItems();

                return page;
            });
        }
    }
}
