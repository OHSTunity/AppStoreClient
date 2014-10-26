using System;
using Starcounter;

namespace AppStoreApp {
    class Program {

        static void Main() {

            AppStoreApp.Server.Handlers.LauncherHooks.RegisterLauncherHooks();
            AppStoreApp.Server.Handlers.AppStoreApps.Register();
        }
    }
}