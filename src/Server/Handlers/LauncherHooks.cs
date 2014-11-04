using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStoreApp.Server.Handlers {

    public class LauncherHooks {

        public static void RegisterLauncherHooks() {

            #region Launcher hooks

            // Menu
            Starcounter.Handle.GET(8080, "/menu", () => {

                var menuPage = new AppStoreMenu() {
                    Html = "/AppStoreClient/appstoremenu.html"
                };

                return menuPage;
            });

            // App-icon (Launchpad)
            Starcounter.Handle.GET(8080, "/app-icon", () =>
            {
                return new AppStoreMenu()
                {
                    Html = "/AppStoreClient/app-icon.html"
                };
            });

            // App-name (Launchpad)
            Starcounter.Handle.GET(8080, "/app-name", () =>
            {
                return new AppName();
            });

            // Workspace root (Launchpad)
            Starcounter.Handle.GET(8080, "/AppStoreClient", () =>
            {
                Starcounter.Response resp;
                Starcounter.X.GET("/AppStoreClient/apps", out resp);
                return resp;
            });

            #endregion
        }
    }
}
