using System;
using Android.Widget;
using Android.Views;
using Android.Graphics;
using P42.Native.Controls.Droid;

namespace Demo.Droid
{
    public class MainPage : Page
    {
        public MainPage()
        {
            Build();
        }

        void Build()
        {
            Title = "Main Page";

            //HasNavigationBar = false;
            //Content = new TextView(P42.Utils.Droid.Settings.Context) { Text = "TEXT VIEW" };

            /*
            var grid = new LinearLayout(Context)
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
            };

            grid.AddView(new TextView(P42.Utils.Droid.Settings.Context)
            {
                Text = "TEXT VIEW",
                Background = Color.Blue.AsDrawable()
            });
            */

            var grid = new GridLayout(Context)
            {
                RowCount = 3,
                ColumnCount = 1,
                LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent),
            };
            grid.SetBackgroundColor(Color.Pink);

            var navigateNextPage = new Button(Context)
            {
                Text = "Navigate Push Page",
                Gravity=GravityFlags.Center,
                Background = Color.Blue.AsDrawable(),
                //LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
                LayoutParameters = new GridLayout.LayoutParams( GridLayout.InvokeSpec(0, 1f), GridLayout.InvokeSpec(0, 1f))
            };
            ((GridLayout.LayoutParams)navigateNextPage.LayoutParameters).SetGravity(GravityFlags.Center);
            ((GridLayout.LayoutParams)navigateNextPage.LayoutParameters).SetMargins(10, 10, 10, 10);
            navigateNextPage.SetTextColor(Color.White);
            navigateNextPage.Click += OnNavigateNextPageButton_Click;
            grid.AddView(navigateNextPage);

            var modalNextPage = new Button(Context)
            {
                Text = "Modal Push Page",
                Gravity = GravityFlags.Center,
                Background = Color.Green.AsDrawable(),
                LayoutParameters = new GridLayout.LayoutParams( GridLayout.InvokeSpec(1, 1f), GridLayout.InvokeSpec(0, 1f))
            };
            ((GridLayout.LayoutParams)modalNextPage.LayoutParameters).SetGravity(GravityFlags.Center);
            ((GridLayout.LayoutParams)modalNextPage.LayoutParameters).SetMargins(10, 10, 10, 10);
            modalNextPage.SetTextColor(Color.White);
            modalNextPage.Click += OnModalNextPageButton_Click;
            grid.AddView(modalNextPage);

            var showPopupButton = new Button(Context)
            {
                Text = "Show Popup",
                Gravity = GravityFlags.Center,
                Background = Color.Red.AsDrawable(),
                LayoutParameters = new GridLayout.LayoutParams(GridLayout.InvokeSpec(2, 1f), GridLayout.InvokeSpec(0, 1f))
            };
            ((GridLayout.LayoutParams)showPopupButton.LayoutParameters).SetGravity(GravityFlags.Center);
            ((GridLayout.LayoutParams)showPopupButton.LayoutParameters).SetMargins(10, 10, 10, 10);
            showPopupButton.SetTextColor(Color.White);
            showPopupButton.Click += OnShowPopupButton_Click;
            grid.AddView(showPopupButton);



            Content = grid;
            
        }

        async void OnShowPopupButton_Click(object sender, EventArgs e)
        {
            var content = new TextView(Context)
            {
                Text = "CONTENT",
                Background = Color.Orange.AsDrawable()
            };
            var popup = new P42.Native.Controls.Droid.TargetedPopup(sender as Button)
            {
                Content = content,
                BackgroundColor = Color.Aquamarine,
                BorderColor = Color.Black,
                DipBorderWidth = 2,
                PreferredPointerDirection = P42.Native.Controls.PointerDirection.None,
                PageOverlayMode = P42.Native.Controls.PageOverlayMode.TouchTransparent
            };

            await popup.PushAsync();
        }

        async void OnModalNextPageButton_Click(object sender, EventArgs e)
        {
            await this.PushModalAsync(new NestedPage());
        }

        async void OnNavigateNextPageButton_Click(object sender, EventArgs e)
        {
            await this.PushAsync(new NestedPage());
        }
    }
}
