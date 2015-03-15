using Starcounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolyjuiceNamespace;

namespace AppStoreApp.Server.Handlers {

    public class LauncherHooks {

        public static void RegisterLauncherHooks() {

            #region Launcher hooks

            // Menu
            Starcounter.Handle.GET("/AppStoreClient/menu", () => {

                var menuPage = new AppStoreMenu() {
                    Html = "/AppStoreClient/appstoremenu.html"
                };

                return menuPage;
            });

            // App-icon (Launchpad)
            Starcounter.Handle.GET("/AppStoreClient/app-icon", () =>
            {
                return new AppStoreMenu()
                {
                    Html = "/AppStoreClient/app-icon.html"
                };
            });

            // App-name (Launchpad)
            Starcounter.Handle.GET("/AppStoreClient/app-name", () =>
            {
                return new AppName();
            });

            // Workspace root (Launchpad)
            Starcounter.Handle.GET( "/AppStoreClient", () =>
            {
                Starcounter.Response resp;
                Starcounter.X.GET("/AppStoreClient/apps", out resp);
                return resp;
            });

            Polyjuice.Map("/AppStoreClient/menu", "/polyjuice/menu");
            Polyjuice.Map("/AppStoreClient/app-name", "/polyjuice/app-name");
            Polyjuice.Map("/AppStoreClient/app-icon", "/polyjuice/app-icon");

            #endregion
        }
    }
}
