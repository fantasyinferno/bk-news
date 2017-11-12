using System.Collections.ObjectModel;
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
        public ObservableCollection<News> SearchCollection { get; private set; }
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


        public SearchPageViewModel()
        {
            SearchCollection = new ObservableCollection<News>();
            SearchCommand = new Command<string>(async (text) => await Search(text));
        }

        async Task Search(string text)
        {
            Debug.WriteLine(text);
            try
            {
                ObservableCollection<News> results = await NewsManager.DefaultManager.GetNewsAsync((news) => news.Title.ToLower().Contains(text.ToLower()));
                if (results.Count > 0)
                {
                    HeaderString = "- " + results.Count + " kết quả -";
                } else
                {
                    HeaderString = "- Không có kết quả -";
                }
                SearchCollection.Clear();
                foreach (var item in results)
                {
                    SearchCollection.Add(item);
                }
            } catch(Exception e)
            {
                Debug.WriteLine(e);
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
    }
}
