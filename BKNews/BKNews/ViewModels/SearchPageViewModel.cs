﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System;
using System.ComponentModel;
namespace BKNews
{
    class SearchPageViewModel: INotifyPropertyChanged
    {
        // the results that are displayed to the user
        public ObservableCollection<News> SearchCollection { get; private set; }
        // the actual results, to be displayed as the user scrolls down
        public ObservableCollection<News> AllResults { get; private set; }
        int step = 0;
        string _headerString = "- Kết quả -";
        public string HeaderString
        {
            get
            {
                return _headerString;
            }
            set
            {
                if (_headerString != value)
                {
                    _headerString = value;
                    OnPropertyChanged("HeaderString");
                }

            }
        }
        public ICommand SearchCommand { get; private set; }
        public ICommand LoadMoreCommand { get; private set; }
        public SearchPageViewModel()
        {
            SearchCollection = new ObservableCollection<News>();
            LoadMoreCommand = new Command(LoadMore);
            SearchCommand = new Command<string>(async (text) => await Search(text));
        }

        async Task Search(string text)
        {
            step = 0;
            Debug.WriteLine(text);
            try
            {
                SearchCollection.Clear();
                AllResults = await NewsManager.DefaultManager.GetNewsAsync((news) => news.Title.ToLower().Contains(text.ToLower()));
                if (AllResults != null)
                {
                    HeaderString = "- " + AllResults.Count + " kết quả -";
                    for (int i = 0; i < AllResults.Count && i < 5; ++i)
                    {
                        SearchCollection.Add(AllResults[i]);
                    }
                    step += 5;
                } else
                {
                    HeaderString = "- Không có kết quả -";
                }
            } catch(Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        public void LoadMore()
        {
            for (int i = step; i < AllResults.Count && i < step + 5; ++i)
            {
                SearchCollection.Add(AllResults[i]);
            }
            step += 5;
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
    }
}
