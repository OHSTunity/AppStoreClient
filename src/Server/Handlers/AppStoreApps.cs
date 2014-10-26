using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starcounter;

namespace AppStoreApp.Server.Handlers {

    public class AppStoreApps {

        public static void Register() {

            //
            // App Store apps
            //
            Starcounter.Handle.GET( "/appstore/apps", (Request request) => {

                Partials.ListAppStoreAppsPage page = new Partials.ListAppStoreAppsPage() {
                    Html = "/appstoreclient/partials/appstoreapps.html",
                    Uri = request.Uri
                };

                page.refreshItems();

                return page;
            });
        }
    }
}
