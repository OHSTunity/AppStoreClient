using Starcounter;
using Starcounter.Internal;

namespace AppStoreApp.Server.Partials {
    partial class ListAppStoreAppsPage : Json {


        public void refreshItems() {

            // Get Available app store items
            Response response = Node.LocalhostSystemPortNode.GET("/api/admin/appstore/apps");

            if (response.StatusCode != (ushort)System.Net.HttpStatusCode.OK) {

                this.ErrorResponce = new AppStoreApp.Server.Partials.ListAppStoreAppsPage.ErrorResponceJson();
                this.ErrorResponce.PopulateFromJson(response.Body);
            }
            else {
                Representations.JSON.AppStoreApplications apps = new Representations.JSON.AppStoreApplications();
                apps.PopulateFromJson(response.Body);

                foreach (var store in apps.Stores) {

                    var storeItem = this.AppStores.Add();
                    storeItem.DisplayName = store.DisplayName;

                    foreach (var appItem in store.Items) {

                        var item = storeItem.Items.Add();
                        item.SetItemProperties(appItem);
                    }
                }
            }
        }

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

    [ListAppStoreAppsPage_json.AppStores.Items]
    partial class ListAppStoreAppsItem : Json {

        /// <summary>
        /// Install Application
        /// </summary>
        /// <param name="action"></param>
        void Handle(Input.Install action) {

            Representations.JSON.ApplicationTask task = new Representations.JSON.ApplicationTask();
            task.Type = "Install";
            task.ID = this.ID;
            task.SourceUrl = this.SourceUrl;

            this.ExecuteTask(task);
        }

        /// <summary>
        /// Upgrade Application
        /// </summary>
        /// <param name="action"></param>
        void Handle(Input.Upgrade action) {

            Representations.JSON.ApplicationTask task = new Representations.JSON.ApplicationTask();
            task.Type = "Upgrade";
            task.ID = this.ID;

            this.ExecuteTask(task);
        }

        /// <summary>
        /// Uninstall Application
        /// </summary>
        /// <param name="action"></param>
        void Handle(Input.Uninstall action) {

            Representations.JSON.ApplicationTask task = new Representations.JSON.ApplicationTask();
            task.Type = "Uninstall";
            task.ID = this.ID;

            this.ExecuteTask(task);
        }

        /// <summary>
        /// Start Application
        /// </summary>
        /// <param name="action"></param>
        void Handle(Input.Start action) {

            Representations.JSON.ApplicationTask task = new Representations.JSON.ApplicationTask();
            task.Type = "Start";
            task.ID = this.ID;
            task.DatabaseName = StarcounterEnvironment.DatabaseNameLower;
            task.Arguments = string.Empty;

            this.ExecuteTask(task);

            ListAppStoreAppsPage page = this.Parent.Parent.Parent.Parent as ListAppStoreAppsPage;
            page.RedirectUrl = "/launcher";
        }

        /// <summary>
        /// Stop Application
        /// </summary>
        /// <param name="action"></param>
        void Handle(Input.Stop action) {

            Representations.JSON.ApplicationTask task = new Representations.JSON.ApplicationTask();
            task.Type = "Stop";
            task.ID = this.ID;
            task.DatabaseName = StarcounterEnvironment.DatabaseNameLower;

            this.ExecuteTask(task);

            ListAppStoreAppsPage page = this.Parent.Parent.Parent.Parent as ListAppStoreAppsPage;
            page.RedirectUrl = "/launcher";

        }

        /// <summary>
        /// Execute Application Task
        /// </summary>
        /// <param name="task"></param>
        private void ExecuteTask(Representations.JSON.ApplicationTask task) {

            this.IsBusy = true;

            Response response = Node.LocalhostSystemPortNode.POST("/api/admin/installed/task", task.ToJson(), null);

            if (response.StatusCode >= 200 && response.StatusCode < 300) {
                this.refreshItem();
            }
            else {
                ListAppStoreAppsPage page = this.Parent.Parent.Parent.Parent as ListAppStoreAppsPage;
                if (page != null) {
                    page.ErrorResponce = new AppStoreApp.Server.Partials.ListAppStoreAppsPage.ErrorResponceJson();
                    page.ErrorResponce.PopulateFromJson(response.Body);
                }
            }


            this.IsBusy = false;
        }

        /// <summary>
        /// Refresh item 
        /// Retrive (GET) item data from Starcounter Administrator 
        /// </summary>
        private void refreshItem() {

            // Get Available app store items
            Response response = Node.LocalhostSystemPortNode.GET("/api/admin/appstore/apps/" + this.ID);

            if (response.StatusCode >= 200 && response.StatusCode < 300) {

                Representations.JSON.AppStoreApplication app = new Representations.JSON.AppStoreApplication();
                app.PopulateFromJson(response.Body);

                this.SetItemProperties(app);
            }
            else {
                ListAppStoreAppsPage page = this.Parent.Parent.Parent.Parent as ListAppStoreAppsPage;
                if (page != null) {
                    page.ErrorResponce = new AppStoreApp.Server.Partials.ListAppStoreAppsPage.ErrorResponceJson();
                    page.ErrorResponce.PopulateFromJson(response.Body);
                }
            }
        }

        /// <summary>
        /// Set local item properties based on remote item properties
        /// </summary>
        /// <param name="item"></param>
        internal void SetItemProperties(Representations.JSON.AppStoreApplication item) {

            this.ID = item.ID;
            this.DisplayName = item.DisplayName;
            this.Version = item.Version;
            this.VersionDate = item.VersionDate;
            this.IsInstalled = item.IsInstalled;
            this.NewVersionAvailable = item.NewVersionAvailable;
            this.SourceUrl = item.SourceUrl;
            this.ImageUrl = item.ImageUrl;
        }
    }
}
