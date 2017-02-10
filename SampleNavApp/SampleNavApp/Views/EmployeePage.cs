using SampleNavApp.CustomViews;
using SampleNavApp.ViewModel;
using Xamarin.Forms;

namespace SampleNavApp.Views
{
    public class EmployeePage : BaseContentPage
    {
        private EmployeeViewModel _viewModel;

        public EmployeePage()
        {
            BindingContext = App.Locator.EmployeeViewModel;
            _viewModel = (EmployeeViewModel)BindingContext;
            //  StackLayout MainStack = new StackLayout();
            Title = "Employee Page";
            Label testLabel = new Label() { Text = "Employee Page", VerticalOptions = LayoutOptions.Center,HorizontalOptions= LayoutOptions.Center};
            Button navBtn = new Button() { Text = "Next Page",VerticalOptions = LayoutOptions.End,HorizontalOptions =LayoutOptions.EndAndExpand};
            navBtn.SetBinding(Button.CommandProperty, "NavigateCommand");
            Content = new StackLayout
            {
                Children = {
                   testLabel,navBtn
                }
            };
        }
    }
}
