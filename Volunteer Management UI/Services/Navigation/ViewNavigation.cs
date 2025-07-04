using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIBaseClass.MVVM;

namespace Volunteer_Management_UI.Services.Navigation
{
    public class ViewNavigation : ViewModelBase
    {

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ViewNavigation()
        {
        }
    }
}
