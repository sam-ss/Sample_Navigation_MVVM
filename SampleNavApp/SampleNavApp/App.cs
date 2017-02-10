using GalaSoft.MvvmLight.Ioc;
using SampleNavApp.Business.Interfaces;
using SampleNavApp.Business.Services;
using SampleNavApp.ViewModel;
using SampleNavApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SampleNavApp
{
    public class App : Application
    {
     
        public App()
        {
            // The root page of your application
            //var content = new ContentPage
            //{
            //    Title = "SampleNavApp",
            //    Content = new StackLayout
            //    {
            //        VerticalOptions = LayoutOptions.Center,
            //        Children = {
            //            new Label {
            //                HorizontalTextAlignment = TextAlignment.Center,
            //                Text = "Welcome to Xamarin Forms!"
            //            }
            //        }
            //    }
            //};

            //MainPage = new NavigationPage(content);

            if (SimpleIoc.Default.IsRegistered<INavService>())
            {
                MainPage = new NavigationPage(new EmployeePage());

                return;
            }
            var navigationService = new NavigationService();
            CreateNavigationService(navigationService);
            SimpleIoc.Default.Register<INavService>(() => navigationService);
           // SimpleIoc.Default.Register(() => UserDialogs.Instance);
            MainPage = new NavigationPage(new EmployeePage());
        }

        /// <summary>
        /// Locator to get instances of viewmodels
        /// </summary>
        private static ViewModelLocator _locator;

        public static ViewModelLocator Locator
        {
            get { return _locator ?? new ViewModelLocator(); }
        }
        private NavigationService CreateNavigationService(NavigationService nav)
        {
            try
            {
                nav.Configure("EmployeePage", typeof(EmployeePage));
                nav.Configure("StudentPage", typeof(StudentPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error on CreateNavigationService: {0}", ex);
            }
            return nav;

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
