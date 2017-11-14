// To add offline sync support: add the NuGet package WindowsAzure.MobileServices.SQLiteStore
// to all projects in the solution and uncomment the symbol definition OFFLINE_SYNC_ENABLED
// For Xamarin.iOS, also edit AppDelegate.cs and uncomment the call to SQLitePCL.CurrentPlatform.Init()
// For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342 
//#define OFFLINE_SYNC_ENABLED

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace BKNews
{
    public partial class NewsManager
    {
        static NewsManager defaultInstance = new NewsManager();
        MobileServiceClient client;

        // NewsUser is a table connecting Users and News
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<News> NewsTable;
        IMobileServiceSyncTable<NewsUser> NewsUserTable;
#else
        IMobileServiceTable<News> NewsTable;
        IMobileServiceTable<NewsUser> NewsUserTable;
#endif

        private NewsManager()
        {
            this.client = new MobileServiceClient(
                Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<News>();
            store.DefineTable<NewsUser>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.NewsTable = client.GetSyncTable<News>();
            this.NewsUserTable = client.GetTable<NewsUser>();
#else
            this.NewsTable = client.GetTable<News>();
            this.NewsUserTable = client.GetTable<NewsUser>();
#endif
        }

        public static NewsManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return NewsTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<News>; }
        }
        /// <summary>
        /// Get the news that a user has saved.
        /// </summary>
        /// <param name="userId">The Id of the user</param>
        /// <returns></returns>
        public async Task<ObservableCollection<News>> GetNewsForUser(string userId)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<string> newsIds = await NewsUserTable.Where((item) => item.UserId == userId).Select((newsUser) => newsUser.NewsId).ToEnumerableAsync();
                // replace this with something efficient
                ObservableCollection<News> items = new ObservableCollection<News>();
                foreach (var newsId in newsIds)
                {
                    var news = await NewsTable.LookupAsync(newsId);
                    items.Add(news);
                }

                return items;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }
        /*addition*/
        /// <summary>
        /// Get the latest news from the database
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ObservableCollection<News>> GetLatestNews(int num = 0)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<News> items = await NewsTable.OrderByDescending(news => news.NewsDate).Take(num).ToEnumerableAsync();
                return new ObservableCollection<News>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }
        /*addition*/
        /// <summary>
        /// Get News with a LINQ expression.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ObservableCollection<News>> GetNewsAsync(System.Linq.Expressions.Expression<System.Func<BKNews.News, bool>> action)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<News> items = await NewsTable.Where(action).OrderByDescending(news => news.NewsDate).ToEnumerableAsync();
                return new ObservableCollection<News>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }
        /// <summary>
        /// Get news with a specific type
        /// </summary>
        /// <param name="type">The type of the parameters</param>
        /// <param name="skip">The number of items to skip beginning from the newest item (i.e, the item with the highest NewsDate value)</param>
        /// <param name="take">The number of items to take after the skipped items</param>
        /// <returns></returns>
        public async Task<ObservableCollection<News>> GetNewsFromCategoryAsync(string type, int skip, int take)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<News> items = await NewsTable.OrderByDescending(news => news.NewsDate).Where(news => news.Type == type).Skip(skip).Take(take).ToEnumerableAsync();
                return new ObservableCollection<News>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }
        public async Task<ObservableCollection<News>> GetNewsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<News> items = await NewsTable
                    .ToEnumerableAsync();

                return new ObservableCollection<News>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SaveNewsAsync(News item)
        {
            if (item.Id == null)
            {
                await NewsTable.InsertAsync(item);
            }
            else
            {
                await NewsTable.UpdateAsync(item);
            }
        }
        public async Task SaveNewsUserAsync(NewsUser item)
        {
            if (item.Id == null)
            {
                await NewsUserTable.InsertAsync(item);
            }
            else
            {
                await NewsUserTable.UpdateAsync(item);
            }
        }
#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.NewsTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allNewss",
                    this.NewsTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
    }
}