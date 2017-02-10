using SampleNavApp.Business.Interfaces;
using SampleNavApp.CustomViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleNavApp.Business.Services
{
    public class NavigationService : INavService, IDisposable
    {
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private NavigationPage _navigation;

        /// <summary>
        ///     Get the current page key
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                {
                    lock (_pagesByKey)
                    {
                        if (((NavigationPage)Application.Current.MainPage).CurrentPage == null)
                        {
                            return null;
                        }

                        var pageType = ((NavigationPage)Application.Current.MainPage).CurrentPage.GetType();

                        return _pagesByKey.ContainsValue(pageType)
                            ? _pagesByKey.First(p => p.Value == pageType).Key
                            : null;
                    }
                }
            }
        }

        /// <summary>
        ///     Get the current modal page key
        /// </summary>
        public string CurrentModalPageKey
        {
            get
            {
                {
                    lock (_pagesByKey)
                    {
                        //
                        if (Application.Current.MainPage.Navigation.ModalStack.Count == 1)
                        {
                            return null;
                        }

                        var pageType = Application.Current.MainPage.Navigation.ModalStack.Last().GetType();

                        return _pagesByKey.ContainsValue(pageType)
                            ? _pagesByKey.First(p => p.Value == pageType).Key
                            : null;
                    }
                }
            }
        }

        /// <summary>
        ///     Count of modal pages on the stack. 1 = 0 pages (only navigation parent)
        /// </summary>
        public int ModalStackCount
        {
            get { return Application.Current.MainPage.Navigation.ModalStack.Count; }
        }

        /// <summary>
        ///     Navigate back
        /// </summary>
        public async Task GoBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        ///     Go to home page
        /// </summary>
        public async Task Home()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        /// <summary>
        ///     Push a modal page on to the navigation stack
        /// </summary>
        /// <param name="pageKey">Modal page to navigate to</param>
        public async Task PushModal(string pageKey)
        {
            if (_pagesByKey.ContainsKey(pageKey))
            {
                var type = _pagesByKey[pageKey];
                ConstructorInfo constructor = null;
                object[] parameters = null;

                constructor = type.GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(c => !c.GetParameters().Any());

                parameters = new object[]
                {
                };

                var page = constructor.Invoke(parameters) as Page;
                await Application.Current.MainPage.Navigation.PushModalAsync(page);
            }
            else
            {
                throw new ArgumentException(
                    string.Format(
                        "No such page: {0}. Did you forget to call NavigationService.Configure?",
                        pageKey),
                    "pageKey");
            }
        }

        /// <summary>
        ///     Closes the current modal page. Forces VM cleanup if IPageLifetime is implemented
        /// </summary>
        public async Task PopModal()
        {
            var modalPage = Application.Current.MainPage.Navigation.ModalStack.LastOrDefault();

            if (!Equals(modalPage, null))
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        /// <summary>
        ///     Navigate to a page with no parameter
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        public async Task NavigateTo(string pagekey)
        {
            await NavigateTo(pagekey, null);
        }

        /// <summary>
        ///     Navigate to a page with parameter
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        /// <param name="parameter">Navigation parameter</param>
        public async Task NavigateTo(string pagekey, object parameter)
        {
            var page = await CreatePageInstance(pagekey, parameter);
            //await Application.Current.MainPage.Navigation.PushAsync(page);
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        /// <summary>
        ///     Configure navigation page
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        /// <param name="pageType">Type of page</param>
        public void Configure(string pagekey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pagekey))
                {
                    _pagesByKey[pagekey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pagekey, pageType);
                }
            }
        }

        /// <summary>
        /// Sets the detail page of the MasterDetailPage only if the MasterDetailPage is MainPage
        /// </summary>
        /// <param name="pagekey">the key of the page set while configuring navigation service</param>
        /// <param name="parameter">any parameters required by the page constructor</param>
        public async void SetDetailPage(string pagekey, object parameter)
        {
            var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
            if (masterDetailPage != null)
            {
                if (masterDetailPage.Detail.Navigation != null)
                {
                    var page = await CreatePageInstance(pagekey, parameter);
                    masterDetailPage.Detail = new CustomNavigationPage(page);
                    masterDetailPage.IsPresented = false;
                }
            }
            else
            {
                throw new InvalidOperationException("Current MainPage is not of type MasterDetailPage.");
            }
        }

        /// <summary>
        /// Sets the MainPage for the app
        /// </summary>
        /// <param name="pagekey">the key of the page set while configuring navigation service</param>
        /// <param name="parameter">any parameters required by the page constructor</param>
        /// <param name="isNavigationPage">is the new page to be set as NavigationPage?</param>
        public async void SetMainPage(string pagekey, object parameter, bool isNavigationPage)
        {
            var page = await CreatePageInstance(pagekey, parameter);
            if (isNavigationPage)
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
            else
            {
                Application.Current.MainPage = page;
            }
        }

        /// <summary>
        ///     Navigate the Detail page of MasterDetailPage to a page with parameter
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        /// <param name="parameter">Navigation parameter</param>
        public async Task NavigateDetailTo(string pagekey, object parameter)
        {
            var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
            if (masterDetailPage != null)
            {
                if (masterDetailPage.Detail.Navigation != null)
                {
                    var page = await CreatePageInstance(pagekey, parameter);
                    await masterDetailPage.Detail.Navigation.PushAsync(page);
                }
                else
                {
                    throw new InvalidOperationException("Navigation property of Detail Page is null, hence cannot navigate Detail Page.");
                }
            }
            else
            {
                throw new InvalidOperationException("Current MainPage is not of type MasterDetailPage.");
            }
        }

        /// <summary>
        /// Navigate the Detail page of MasterDetailPage to a page with parameter & removes second last page from navigation stack
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        /// <param name="parameter">Navigation parameter</param>
        public async Task PopAndNavigateDetailTo(string pagekey, object parameter)
        {
            var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
            if (masterDetailPage != null)
            {
                if (masterDetailPage.Detail.Navigation != null)
                {
                    masterDetailPage.Detail.Navigation.RemovePage(masterDetailPage.Detail.Navigation.NavigationStack[masterDetailPage.Detail.Navigation.NavigationStack.Count - 1]);
                }
                else
                {
                    throw new InvalidOperationException("Navigation property of Detail Page is null, hence cannot navigate Detail Page.");
                }
            }
            else
            {
                throw new InvalidOperationException("Current MainPage is not of type MasterDetailPage.");
            }
        }

        /// <summary>
        /// Pops the n-2 page from nav stack
        /// </summary>
        /// <returns></returns>
        public async Task NavigateAndPop()
        {
            var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
            if (masterDetailPage != null)
            {
                if (masterDetailPage.Detail.Navigation != null)
                {
                    masterDetailPage.Detail.Navigation.RemovePage(masterDetailPage.Detail.Navigation.NavigationStack[masterDetailPage.Detail.Navigation.NavigationStack.Count - 2]);
                }
                else
                {
                    throw new InvalidOperationException("Navigation property of Detail Page is null, hence cannot navigate Detail Page.");
                }
            }
            else
            {
                throw new InvalidOperationException("Current MainPage is not of type MasterDetailPage.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pagekey"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private async Task<Page> CreatePageInstance(string pageKey, object parameter)
        {
            // using (var userForm = new UserFormService())
            // {
            //  var user = Settings.CurrentUser;
            //bool pageAccess = await userForm.PullUserFormDataForPageFromLocalDBAsync(x => x.FormName.Trim() == pageKey.Trim() && x.RoleName.Trim().ToLower() == user.PreferredRole.Trim().ToLower());
            //if (!pageAccess && pageKey != PageConstants.LOADING_PAGE && pageKey != PageConstants.BASE_KEYPAD_PAGE && pageKey != PageConstants.LOGIN_PAGE && pageKey != PageConstants.MASTER_PAGE && pageKey != PageConstants.DASHBOARD_PAGE)
            //{
            //    pageKey = "AccessDeniedPage";
            //    parameter = null;
            //}
            if (_pagesByKey.ContainsKey(pageKey))
            {
                Type type;
                if (!_pagesByKey.TryGetValue(pageKey, out type))
                    throw new ArgumentException($"No such page: {pageKey}. Did you forget to call NavigationService.Configure?", nameof(pageKey));

                ConstructorInfo constructor;
                object[] parameters;
                if (parameter == null)
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(c => !c.GetParameters().Any());

                    parameters = new object[]
                    {
                    };
                }
                else
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(
                            c =>
                            {
                                var p = c.GetParameters();
                                return p.Count() == 1
                                        && p[0].ParameterType == parameter.GetType();
                            });

                    parameters = new[]
                    {
                        parameter
                    };
                }

                if (constructor == null)
                {
                    throw new InvalidOperationException(
                        "No suitable constructor found for page " + pageKey);
                }
                return constructor.Invoke(parameters) as Page;
            }
            else
            {
                throw new ArgumentException(
                string.Format(
                    "No such page: {0}. Did you forget to call NavigationService.Configure?",
                    pageKey),
                "pagekey");
                //var page = constructor.Invoke(parameters) as Page;
                //if (isNavigationPage)
                //{
                //    Application.Current.MainPage = new NavigationPage(page);
                //}
                //else
                //{
                //    Application.Current.MainPage = page;
                //}
            }
            //}
        }

        public void Dispose()
        {
        }
    }
}
