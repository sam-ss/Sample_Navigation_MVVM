using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleNavApp.CustomViews
{
    public class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage() : base()
        {
            SetBarBackgroundColor();
        }

        public CustomNavigationPage(Page root)
            : base(root)
        {
            SetBarBackgroundColor();
        }

        public CustomNavigationPage(Page root, string title)
            : base(root)
        {
            root.Title = title;
            SetBarBackgroundColor();
        }

        private void SetBarBackgroundColor()
        {
            BarBackgroundColor = Color.FromHex("#245d9f");
             BarTextColor = Color.Gray;//White color code
        }
    }
}