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
                RowCount = 2,
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
                LayoutParameters = new GridLayout.LayoutParams( GridLayout.InvokeSpec(1), GridLayout.InvokeSpec(0))
            };
            grid.AddView(navigateNextPage);
            navigateNextPage.SetTextColor(Color.White);
            navigateNextPage.Click += OnNavigateNextPageButton_Click;

            var modalNextPage = new Button(Context)
            {
                Text = "Modal Push Page",
                Gravity = GravityFlags.Center,
                Background = Color.Green.AsDrawable(),
                LayoutParameters = new GridLayout.LayoutParams( GridLayout.InvokeSpec(0), GridLayout.InvokeSpec(0))
            };
            grid.AddView(modalNextPage);
            modalNextPage.SetTextColor(Color.White);
            modalNextPage.Click += OnModalNextPageButton_Click;
            
            Content = grid;
            
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
