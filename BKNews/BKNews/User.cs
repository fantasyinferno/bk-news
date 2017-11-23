using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace BKNews
{
    class User: INotifyPropertyChanged
    {
        private User() { }
        public static User CurrentUser = new User();
        private string _name = "Guest";
        private string _avatarUrl = "http://www.hcmut.edu.vn/img/logoBK.png";
        public bool _authenticated = false;
        public void Reset()
        {
            Name = "Guest";
            AvatarUrl = "http://www.hcmut.edu.vn/img/logoBK.png";
            Authenticated = false;
            Bookmarks.Clear();
        }
        public string Id { get; set; }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string AvatarUrl
        {
            get
            {
                return _avatarUrl;
            }
            set
            {
                if (_avatarUrl != value)
                {
                    _avatarUrl = value;
                    OnPropertyChanged("AvatarUrl");
                }
            }
        }
        public bool Authenticated
        {
            get
            {
                return _authenticated;
            }
            set
            {
                if (_authenticated != value)
                {
                    _authenticated = value;
                    OnPropertyChanged("Authenticated");
                }
                // call UserChanged() whether Authenticated is tampered with
                OnUserChanged(EventArgs.Empty);
            }
        }
        private HashSet<News> _bookmarks = new HashSet<News>();
        public HashSet<News> Bookmarks
        {
            get
            {
                return _bookmarks;
            }
            set
            {
                if (_bookmarks != value)
                {
                    _bookmarks = value;
                    OnPropertyChanged("Bookmarks");
                }
            }
        }
        public event EventHandler UserChanged;
        protected virtual void OnUserChanged(EventArgs args)
        {
            UserChanged?.Invoke(this, args);
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
