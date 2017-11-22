using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Text;

namespace BKNews
{
    public class SidebarPageMasterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SidebarPageMenuItem> MenuItems { get; set; }
        public SidebarPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<SidebarPageMenuItem>(new[]
            {
                    new SidebarPageMenuItem { Id = 0, Title = "BKExpress", TargetType = typeof(MainFeedPage) },
                    new SidebarPageMenuItem { Id = 1, Title ="Tìm kiếm", TargetType = typeof(SearchPage)},
                    new SidebarPageMenuItem { Id = 2, Title ="Bookmarks", TargetType = typeof(BookmarkPage)},
                    new SidebarPageMenuItem { Id = 3, Title = "Thông báo", TargetType = typeof(NotificationPage) },
                    new SidebarPageMenuItem { Id = 4, Title = "HCMUT", TargetType = typeof(NewsPage) },
                    new SidebarPageMenuItem { Id = 5, Title = "AAO", TargetType = typeof(NewsPage) },
                    new SidebarPageMenuItem { Id = 6, Title = "OISP", TargetType = typeof(NewsPage) },
                    new SidebarPageMenuItem { Id = 7, Title = "PGS", TargetType = typeof(NewsPage) }
                });
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
