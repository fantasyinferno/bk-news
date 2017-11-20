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
    class NotificationPageViewModel: INotifyPropertyChanged
    {
        // skip & step
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 5;
        // mixed collection of news
        public ObservableCollection<News> NewsCollection;
        // command to bind with button
        // list for storing scrapers
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
        public NotificationPageViewModel()
        {
            NewsCollection = new ObservableCollection<News>();
            // command for button
        }
    }
}