using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using Xamarin.Forms;
using System.ComponentModel;
//adb connect 169.254.138.177 
using System.Threading.Tasks;

namespace BKNews
{
    class NewsViewModel : INotifyPropertyChanged
    {
        // skip & step
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 5;
        // category's name
        public string Category { get; set; }
        // mixed collection of news
        public ObservableCollection<News> NewsCollection { get; private set; }
        // command to bind with button
        public ICommand ScrapeCommand { get; set; }
        public ICommand LoadMore { get; set; }
        // list for storing scrapers
        public IScrape Scraper;
        // IsRefreshing property of ListView
        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged("IsBusy");
                }

            }
        }
        // propagate property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        // Scrape function. Once done, sent the scraped news to NewsCollection.
        public async Task ScrapeToCollectionAsync(bool append = false)
        {
            IsBusy = true;
            try
            {
                // Scrape first 1000 pages if possible
                for (int i = 1; i < 1000; ++i)
                {
                    var list = await Scraper.Scrape(i);
                    // individually add each item to the list (because we have to use ObservableCollection)
                    if (append)
                    {
                        foreach (var item in list)
                        {
                            // should fail on duplicates
                            await NewsManager.DefaultManager.SaveTaskAsync(item);
                            NewsCollection.Add(item);
                        }
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            // should fail on duplicates
                            await NewsManager.DefaultManager.SaveTaskAsync(item);
                            NewsCollection.Insert(0, item);
                        }
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                // duplicates
                Debug.WriteLine("Unique key constant fails");
            }
            catch (Exception e)
            {
                // do nothing
                Debug.WriteLine("Either an error occured or there are no pages left to scrape.");
            }
            finally
            {
                IsBusy = false;
            }
        }
        // load items from database with pagination
        public async Task LoadFromDatabaseAsync()
        {
            try
            {
                var collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync(Category, Skip, Take);
                foreach (var item in collection)
                {
                    NewsCollection.Add(item);
                }
                Skip += Take;
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        public NewsViewModel(String category, IScrape scraper)
        {
            Category = category;
            Scraper = scraper;
            NewsCollection = new ObservableCollection<News>();
            // command for button
            ScrapeCommand = new Command(async () =>
            {
                await ScrapeToCollectionAsync();
            });

            LoadMore = new Command(async () =>
            {
                await LoadFromDatabaseAsync();
            });
        }
    }
}