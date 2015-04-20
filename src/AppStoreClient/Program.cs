using System;
using Starcounter;

namespace AppStoreClient {
    class Program {
        static void Main() {
            MainHandlers main = new MainHandlers();

            main.Register();
        }
    }
}