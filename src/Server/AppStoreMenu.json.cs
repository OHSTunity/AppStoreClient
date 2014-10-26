using Starcounter;

namespace AppStoreApp.Server {

    [AppStoreMenu_json]
    partial class AppStoreMenu : Json {

        #region Base

        /// <summary>
        /// The way to get a URL for HTML partial if any.
        /// </summary>
        /// <returns></returns>
        public override string GetHtmlPartialUrl() {
            return Html;
        }

        #endregion
    }
}
