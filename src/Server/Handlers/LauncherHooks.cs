using Starcounter;
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
            Starcounter.Handle.GET("/launcher/menu", () => {

                var menuPage = new AppStoreMenu() {
                    Html = "/AppStoreClient/appstoremenu.html"
                };

                return menuPage;
            }, HandlerOptions.ApplicationLevel);

            // App-icon (Launchpad)
            Starcounter.Handle.GET("/launcher/app-icon", () =>
            {
                return new AppStoreMenu()
                {
                    Html = "/AppStoreClient/app-icon.html"
                };
            }, HandlerOptions.ApplicationLevel);

            // App-name (Launchpad)
            Starcounter.Handle.GET("/launcher/app-name", () =>
            {
                return new AppName();
            }, HandlerOptions.ApplicationLevel);

            // Workspace root (Launchpad)
            Starcounter.Handle.GET( "/AppStoreClient", () =>
            {
                Starcounter.Response resp;
                Starcounter.X.GET("/AppStoreClient/apps", out resp);
                return resp;
            });

            #endregion
        }
    }
}
