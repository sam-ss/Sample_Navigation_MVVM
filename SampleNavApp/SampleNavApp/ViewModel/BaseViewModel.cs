using GalaSoft.MvvmLight;
using SampleNavApp.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleNavApp.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        public BaseViewModel(INavService NavigationService)
        {

        }
    }
}
