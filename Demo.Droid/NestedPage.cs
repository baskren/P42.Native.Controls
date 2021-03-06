using System;
using Android.Widget;
using Android.Views;
using Android.Graphics;
using P42.Native.Controls;

namespace Demo.Droid
{
    public class NestedPage : Page
    {
        static Random Random = new Random();

        int pageNumber;

        public NestedPage(int number = 1)
        {
            pageNumber = number;
            Build();
        }

        void Build()
        {
            Title = "Page " + pageNumber;

            SetBackgroundColor(Color.Argb(255, Random.Next(255), Random.Next(255), Random.Next(255)));

            var grid = new GridLayout(Context)
            {
                RowCount = 1,
                ColumnCount = 2,
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
            };
            var backNumber = pageNumber - 1;
            var backButton = new Button(Context)
            {
                Text = backNumber > 0 ? "Back to " + backNumber : "Home" ,
                Gravity = GravityFlags.Center,
                Background = Color.Blue.AsDrawable(),
                LayoutParameters = new GridLayout.LayoutParams(
                    GridLayout.InvokeSpec(0, 1f),
                    GridLayout.InvokeSpec(0, 1f))
            };
            ((GridLayout.LayoutParams)backButton.LayoutParameters).SetGravity(GravityFlags.Center);
            ((GridLayout.LayoutParams)backButton.LayoutParameters).SetMargins(10, 10, 10, 10);
            backButton.Click += OnBackButton_Click;
            grid.AddView(backButton);


            var forewardNumber = pageNumber + 1;
            var forewardButton = new Button(Context)
            {
                Text = "Go to " + forewardNumber,
                Gravity = GravityFlags.Center,
                Background = Color.Blue.AsDrawable(),
                LayoutParameters = new GridLayout.LayoutParams(
                    GridLayout.InvokeSpec(0, 1f),
                    GridLayout.InvokeSpec(1, 1f))
            };
            ((GridLayout.LayoutParams)forewardButton.LayoutParameters).SetGravity(GravityFlags.Center);
            ((GridLayout.LayoutParams)forewardButton.LayoutParameters).SetMargins(10, 10, 10, 10);
            forewardButton.Click += OnForewardButton_Click;
            grid.AddView(forewardButton);

            Content = grid;
        }

        async void OnForewardButton_Click(object sender, EventArgs e)
        {
            if (this.FindAncestor<NavigationPage>() is NavigationPage)
                await this.PushAsync(new NestedPage(pageNumber + 1));
            else
                await this.PushModalAsync(new NestedPage(pageNumber + 1));
        }

        async void OnBackButton_Click(object sender, EventArgs e)
        {
            if (this.FindAncestor<NavigationPage>() is NavigationPage)
                await this.PopAsync();
            else
                await this.PopModalAsync();
        }
    }
}
