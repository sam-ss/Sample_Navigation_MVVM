using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SampleNavApp.Business.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace SampleNavApp.ViewModel
{
    public class EmployeeViewModel : BaseViewModel
    {
        private readonly INavService _navService;
        private RelayCommand _myNavigation;

        public EmployeeViewModel(INavService NavigationService) : base(NavigationService)
        {
            _navService = NavigationService;
        }

        /// <summary>
        /// Gets the NavigateCommand.
        /// </summary>
        public RelayCommand NavigateCommand
        {
            get
            {
                return _myNavigation
                    ?? (_myNavigation = new RelayCommand(
                    () =>
                    {
                        // _navService.NavigateTo("StudentPage",null);
                        _navService.SetMainPage("StudentPage",null,true);
                    }));
            }
        }
    }
}
