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
                    Html = "/appstoreclient/appstoremenu.html"
                };

                return menuPage;
            });

            #endregion
        }
    }
}
