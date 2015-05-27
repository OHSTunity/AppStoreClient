using Starcounter;
using Starcounter.Internal;

namespace AppStoreClient {
    partial class ListAppStoreAppsPage : Json {

        public void refreshItems() {


            string uri = string.Format("/api/admin/database/{0}/appstore/stores", StarcounterEnvironment.DatabaseNameLower);

            // Get Stores
            Response response = Node.LocalhostSystemPortNode.GET(uri, 5000);

            this.AppStores.Clear();

            if (response.StatusCode != (ushort)System.Net.HttpStatusCode.OK) {

                this.ErrorResponce = new ListAppStoreAppsPage.ErrorResponceJson();
                this.ErrorResponce.PopulateFromJson(response.Body);
            }
            else {

                AppStoreStoresJson stores = new AppStoreStoresJson();
                stores.PopulateFromJson(response.Body);

                foreach (var store in stores.Items) {

                    var storeItem = this.AppStores.Add();
                    storeItem.DisplayName = store.DisplayName;


                    // Get store items
                    string storeuri = string.Format("/api/admin/database/{0}/appstore/stores/{1}/applications", StarcounterEnvironment.DatabaseNameLower, store.ID);
                    Response storeApplicationsResponse = Node.LocalhostSystemPortNode.GET(storeuri, 5000);
                    if (response.StatusCode != (ushort)System.Net.HttpStatusCode.OK) {

                        this.ErrorResponce = new ListAppStoreAppsPage.ErrorResponceJson();
                        this.ErrorResponce.PopulateFromJson(storeApplicationsResponse.Body);
                    }
                    else {

                        AppStoreApplications appStoreApplications = new AppStoreApplications();
                        appStoreApplications.PopulateFromJson(storeApplicationsResponse.Body);

                        foreach (var appItem in appStoreApplications.Items) {
                            var item = storeItem.Items.Add();
                            item.SetItemProperties(appItem);
                        }
                    }

                    //foreach (var appItem in store.Items) {

                    //    var item = storeItem.Items.Add();
                    //    item.SetItemProperties(appItem);
                    //}
                }
            }
        }
        //public void refreshItems_OLD_ADMIN() {

        //    // Get Available app store items
        //    Response response = Node.LocalhostSystemPortNode.GET("/api/admin/appstore/apps");

        //    this.AppStores.Clear();

        //    if (response.StatusCode != (ushort)System.Net.HttpStatusCode.OK) {

        //        this.ErrorResponce = new ListAppStoreAppsPage.ErrorResponceJson();
        //        this.ErrorResponce.PopulateFromJson(response.Body);
        //    }
        //    else {
        //        AppStoreApplications apps = new AppStoreApplications();
        //        apps.PopulateFromJson(response.Body);

        //        foreach (var store in apps.Stores) {

        //            var storeItem = this.AppStores.Add();
        //            storeItem.DisplayName = store.DisplayName;

        //            foreach (var appItem in store.Items) {

        //                var item = storeItem.Items.Add();
        //                item.SetItemProperties(appItem);
        //            }
        //        }
        //    }
        //}

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

            ApplicationTask task = new ApplicationTask();
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

            ApplicationTask task = new ApplicationTask();
            task.Type = "Upgrade";
            task.ID = this.ID;

            this.ExecuteTask(task);
        }

        /// <summary>
        /// Uninstall Application
        /// </summary>
        /// <param name="action"></param>
        void Handle(Input.Uninstall action) {

            ApplicationTask task = new ApplicationTask();
            task.Type = "Uninstall";
            task.ID = this.ID;

            this.ExecuteTask(task);
        }

        /// <summary>
        /// Start Application
        /// </summary>
        /// <param name="action"></param>
        void Handle(Input.Start action) {

            ApplicationTask task = new ApplicationTask();
            task.Type = "Start";
            task.ID = this.ID;
            task.DatabaseName = StarcounterEnvironment.DatabaseNameLower;
            task.Arguments = string.Empty;

            this.ExecuteTask(task);
        }

        /// <summary>
        /// Stop Application
        /// </summary>
        /// <param name="action"></param>
        void Handle(Input.Stop action) {

            ApplicationTask task = new ApplicationTask();
            task.Type = "Stop";
            task.ID = this.ID;
            task.DatabaseName = StarcounterEnvironment.DatabaseNameLower;

            this.ExecuteTask(task);
        }

        /// <summary>
        /// Execute Application Task
        /// </summary>
        /// <param name="task"></param>
        private void ExecuteTask(ApplicationTask task) {

            this.IsBusy = true;

            Response response = Node.LocalhostSystemPortNode.POST("/api/admin/installed/task", task.ToJson(), null);

            if (response.StatusCode >= 200 && response.StatusCode < 300) {
                this.refreshItem();
            }
            else {
                ListAppStoreAppsPage page = this.Parent.Parent.Parent.Parent as ListAppStoreAppsPage;
                if (page != null) {
                    page.ErrorResponce = new ListAppStoreAppsPage.ErrorResponceJson();
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
            ListAppStoreAppsPage page = this.Parent.Parent.Parent.Parent as ListAppStoreAppsPage;
            page.refreshItems();
        }

        /// <summary>
        /// Set local item properties based on remote item properties
        /// </summary>
        /// <param name="item"></param>
        internal void SetItemProperties(AppStoreApplication item) {

            this.ID = item.ID;
            this.DisplayName = item.DisplayName;
            this.Version = item.Version;
            this.VersionDate = item.VersionDate;
            this.Description = item.Description;
            this.IsInstalled = item.IsInstalled;
            this.NewVersionAvailable = item.NewVersionAvailable;
            this.SourceUrl = item.SourceUrl;
            this.ImageUrl = item.ImageUrl;
            this.Heading = item.Heading;
            this.Rating = item.Rating;
        }
    }
}
