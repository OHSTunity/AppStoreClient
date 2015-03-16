using Starcounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolyjuiceNamespace;
using AppStoreClient.Server.JSON;
using AppStoreApp.Server.Partials;

namespace AppStoreApp.Server.Handlers {

    public class LauncherHooks {

        public static void RegisterLauncherHooks() {

            #region Launcher hooks

            // Menu
            Handle.GET("/AppStoreClient/menu", () => {
                return new Page() { Html = "/AppStoreClient/menu.html" };
            });

            // App-icon (Launchpad)
            Starcounter.Handle.GET("/AppStoreClient/app-icon", () =>
            {
                return new Page()
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
            Starcounter.Handle.GET("/AppStoreClient", () => {
                var master = (AppStore)X.GET("/AppStoreClient/standalone");
                return master.AppList;
            });

            Polyjuice.Map("/AppStoreClient/menu", "/polyjuice/menu");
            Polyjuice.Map("/AppStoreClient/app-name", "/polyjuice/app-name");
            Polyjuice.Map("/AppStoreClient/app-icon", "/polyjuice/app-icon");

            #endregion
        }
    }
}
