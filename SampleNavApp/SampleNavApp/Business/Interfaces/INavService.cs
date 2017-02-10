using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleNavApp.Business.Interfaces
{
    /// <summary>
    /// An interface defining how navigation between pages should
    ///             be performed in various frameworks such as Windows,
    ///             Windows Phone, Android, iOS etc.
    ///
    /// </summary>
    public interface INavService
    {
        /// <summary>
        /// The key corresponding to the currently displayed page.
        ///
        /// </summary>
        string CurrentPageKey { get; }

        /// <summary>
        /// The key corresponding to the currently displayed modal page.
        /// </summary>
        string CurrentModalPageKey { get; }

        /// <summary>
        /// The key corresponding to the currently displayed modal page.
        /// </summary>
        int ModalStackCount { get; }

        /// <summary>
        /// If possible, instructs the navigation service
        ///             to discard the current page and display the previous page
        ///             on the navigation stack.
        ///
        /// </summary>
        Task GoBack();

        /// <summary>
        /// Pop to root navigation page
        ///
        /// </summary>
        Task Home();

        /// <summary>
        /// Push a modal page on to the navigation stack
        ///
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        ///             that should be displayed.</param>
        Task PushModal(string pageKey);

        /// <summary>
        /// Pop a modal page friom the navigation stack
        ///
        /// </summary>
        Task PopModal();

        /// <summary>
        /// Instructs the navigation service to display a new page
        ///             corresponding to the given key. Depending on the platforms,
        ///             the navigation service might have to be configured with a
        ///             key/page list.
        ///
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        ///             that should be displayed.</param>
        Task NavigateTo(string pageKey);

        /// <summary>
        /// Instructs the navigation service to display a new page
        ///             corresponding to the given key, and passes a parameter
        ///             to the new page.
        ///             Depending on the platforms, the navigation service might
        ///             have to be Configure with a key/page list.
        ///
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        ///             that should be displayed.</param><param name="parameter">The parameter that should be passed
        ///             to the new page.</param>
        Task NavigateTo(string pageKey, object parameter);

        /// <summary>
        /// Sets the detail page of the MasterDetailPage only if the MasterDetailPage is MainPage
        /// </summary>
        /// <param name="pagekey">the key of the page set while configuring navigation service</param>
        /// <param name="parameter">any parameters required by the page constructor</param>
        void SetDetailPage(string pagekey, object parameter);

        /// <summary>
        /// Sets the MainPage for the app
        /// </summary>
        /// <param name="pagekey">the key of the page set while configuring navigation service</param>
        /// <param name="parameter">any parameters required by the page constructor</param>
        /// <param name="isNavigationPage">is the new page to be set as NavigationPage?</param>
        void SetMainPage(string pagekey, object parameter, bool isNavigationPage);

        /// <summary>
        /// Navigate the Detail page of MasterDetailPage to a page with parameter
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        /// <param name="parameter">Navigation parameter</param>
        Task NavigateDetailTo(string pagekey, object parameter);

        /// <summary>
        /// Navigate the Detail page of MasterDetailPage to a page with parameter & removes second last page from navigation stack
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        /// <param name="parameter">Navigation parameter</param>
        Task PopAndNavigateDetailTo(string pagekey, object parameter);

        /// <summary>
        /// Pops the n-2 page from nav stack
        /// </summary>
        /// <returns></returns>
        Task NavigateAndPop();
    }
}