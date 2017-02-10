using SampleNavApp.CustomViews;
using SampleNavApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SampleNavApp.Views
{
    public class StudentPage : BaseContentPage
    {
        private StudentViewModel _viewModel;

        public StudentPage()
        {
            BindingContext = App.Locator.StudentViewModel;
            _viewModel = (StudentViewModel)BindingContext;
            //  StackLayout MainStack = new StackLayout();
            Title = "Student Page";
            Label testLabel = new Label() { Text = "Student Page", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
            Button navBtn = new Button() { Text = "Previous Page", VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.EndAndExpand };
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
