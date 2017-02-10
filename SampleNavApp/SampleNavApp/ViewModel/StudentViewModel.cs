using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleNavApp.Business.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace SampleNavApp.ViewModel
{
    public class StudentViewModel : BaseViewModel
    {
        private readonly INavService _navService;
        private RelayCommand _myNavigation;

        public StudentViewModel(INavService NavigationService) : base(NavigationService)
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
                        _navService.NavigateTo("EmployeePage", null);
                    }));
            }
        }
    }
}
