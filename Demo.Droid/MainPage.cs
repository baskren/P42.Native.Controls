using System;
using Android.Widget;
using Android.Views;
using Android.Graphics;
using P42.Native.Controls;
using System.Linq;
using Android.Content;

namespace Demo.Droid
{
    public class MainPage : Page
    {
        SegmentedControl HzAlign, VtAlign;

        public MainPage()
        {
            Build();
        }

        public MainPage(Context context) : base(context)
        {
            Build();
        }

        void Build()
        {
            DipTitle = "Main Page";

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
                RowCount = 7,
                ColumnCount = 1,
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
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

            /*
            var showPopupButton = new Button(Context)
            {
                Text = "Show Popup",
                Gravity = GravityFlags.Center,
                Background = Color.Red.AsDrawable(),
                LayoutParameters = new GridLayout.LayoutParams(GridLayout.InvokeSpec(1, 1f), GridLayout.InvokeSpec(0, 1f))
            };
            */


            var showPopupButton = new SegmentedControl(Context)
            {
                LayoutParameters = new GridLayout.LayoutParams(GridLayout.InvokeSpec(2, 1f), GridLayout.InvokeSpec(0, 1f)),
            };
            var rightSegment = new SegmentButton(Context)
            {
                Text = "LEFT",
            };
            rightSegment.Click += OnSegment_Click;
            showPopupButton.AddView(rightSegment);

            var upSegment = new SegmentButton(Context)
            {
                Text = "UP",
            };
            upSegment.Click += OnSegment_Click;
            showPopupButton.AddView(upSegment);

            var downSegment = new SegmentButton(Context)
            {
                Text = "DOWN",
            };
            downSegment.Click += OnSegment_Click;
            showPopupButton.AddView(downSegment);

            var leftSegment = new SegmentButton(Context)
            {
                Text = "RIGHT",
            };
            leftSegment.Click += OnSegment_Click;
            showPopupButton.AddView(leftSegment);
            ((GridLayout.LayoutParams)showPopupButton.LayoutParameters).SetGravity(GravityFlags.Center);
            ((GridLayout.LayoutParams)showPopupButton.LayoutParameters).SetMargins(10, 10, 10, 10);
            grid.AddView(showPopupButton);


            HzAlign = new SegmentedControl(Context)
            {
                LayoutParameters = new GridLayout.LayoutParams(GridLayout.InvokeSpec(3, 1f), GridLayout.InvokeSpec(0, 1f)),
            };
            var hzStartSegment = new SegmentButton(Context) { Text = "START" };
            HzAlign.AddView(hzStartSegment);
            var hzCenterSegment = new SegmentButton(Context) { Text = "CENTER", DipSelected = true };
            HzAlign.AddView(hzCenterSegment);
            var hzEndSegment = new SegmentButton(Context) { Text = "END" };
            HzAlign.AddView(hzEndSegment);
            var hzStretchSegment = new SegmentButton(Context) { Text = "STRETCH" };
            HzAlign.AddView(hzStretchSegment);
            ((GridLayout.LayoutParams)HzAlign.LayoutParameters).SetGravity(GravityFlags.Center);
            ((GridLayout.LayoutParams)HzAlign.LayoutParameters).SetMargins(10, 10, 10, 10);
            grid.AddView(HzAlign);

            VtAlign = new SegmentedControl(Context)
            {
                LayoutParameters = new GridLayout.LayoutParams(GridLayout.InvokeSpec(4, 1f), GridLayout.InvokeSpec(0, 1f)),
            };
            var vtStartSegment = new SegmentButton(Context) { Text = "START" };
            VtAlign.AddView(vtStartSegment);
            var vtCenterSegment = new SegmentButton(Context) { Text = "CENTER", DipSelected = true };
            VtAlign.AddView(vtCenterSegment);
            var vtEndSegment = new SegmentButton(Context) { Text = "END" };
            VtAlign.AddView(vtEndSegment);
            var vtStretchSegment = new SegmentButton(Context) { Text = "STRETCH" };
            VtAlign.AddView(vtStretchSegment);
            ((GridLayout.LayoutParams)VtAlign.LayoutParameters).SetGravity(GravityFlags.Center);
            ((GridLayout.LayoutParams)VtAlign.LayoutParameters).SetMargins(10, 10, 10, 10);
            grid.AddView(VtAlign);

            var cdPageButton = new Button(Context)
            {
                Text = "C&D Page",
                LayoutParameters = new GridLayout.LayoutParams(GridLayout.InvokeSpec(5, 1f), GridLayout.InvokeSpec(0, 1f)),
            };
            ((GridLayout.LayoutParams)cdPageButton.LayoutParameters).SetGravity(GravityFlags.Center);
            ((GridLayout.LayoutParams)cdPageButton.LayoutParameters).SetMargins(10, 10, 10, 10);
            cdPageButton.Click += async (s, e) => await this.PushAsync(new ContentAndDetailPresenterPage());
            grid.AddView(cdPageButton);

            var label = new TextView(Context)
            {
                Text = "LABEL",
                Gravity = GravityFlags.Bottom | GravityFlags.Right,
                //TextAlignment = TextAlignment.Center,
                LayoutParameters = new GridLayout.LayoutParams(GridLayout.InvokeSpec(6, 1f), GridLayout.InvokeSpec(0, 1f)),
            };
            label.SetPadding(30, 30, 30, 30);
            label.SetBackgroundColor(Color.Orange);
            grid.AddView(label);

            DipContent = grid;
            
        }


        async void OnSegment_Click(object sender, EventArgs e)
        {

            if (sender is SegmentButton segment)
            {
                var content = new TextView(Context)
                {
                    Text = segment.Text,
                    Background = Color.Orange.AsDrawable(),
                    Gravity = GravityFlags.Center
                };
                var popup = new P42.Native.Controls.TargetedPopup(sender as SegmentButton)
                {
                    DipContent = content,
                    DipMargin = (Thickness)50,
                    DipBackgroundColor = Color.Aquamarine,
                    DipBorderColor = Color.Transparent,
                    DipPreferredPointerDirection = segment.Text == "UP"
                    ? P42.Native.Controls.PointerDirection.Up
                    : segment.Text == "DOWN"
                        ? P42.Native.Controls.PointerDirection.Down
                        : segment.Text == "RIGHT"
                            ? P42.Native.Controls.PointerDirection.Right
                            : P42.Native.Controls.PointerDirection.Left
                    //PageOverlayMode = P42.Native.Controls.PageOverlayMode.TouchTransparent
                };
                ((IControl)popup).DipBorderWidth = 2;

                var hzSelected = HzAlign.Children().Where(c => c is SegmentButton).Cast<SegmentButton>().FirstOrDefault(s => s.DipSelected)?.Text;
                popup.DipHorizontalAlignment = hzSelected == "START"
                    ? P42.Native.Controls.Alignment.Start
                    : hzSelected == "CENTER"
                        ? P42.Native.Controls.Alignment.Center
                        : hzSelected == "END"
                            ? P42.Native.Controls.Alignment.End
                            : P42.Native.Controls.Alignment.Stretch;
                var vtSelected = VtAlign.Children().Where(c => c is SegmentButton).Cast<SegmentButton>().FirstOrDefault(s => s.DipSelected)?.Text;
                popup.DipVerticalAlignment = vtSelected == "START"
                    ? P42.Native.Controls.Alignment.Start
                    : vtSelected == "CENTER"
                        ? P42.Native.Controls.Alignment.Center
                        : vtSelected == "END"
                            ? P42.Native.Controls.Alignment.End
                            : P42.Native.Controls.Alignment.Stretch;

                await popup.PushAsync();
            }
        }

        async void OnShowPopupButton_Click(object sender, EventArgs e)
        {
            var content = new TextView(Context)
            {
                Text = "CONTENT",
                Background = Color.Orange.AsDrawable(),
                Gravity = GravityFlags.Center
            };
            var popup = new P42.Native.Controls.TargetedPopup(sender as Button)
            {
                DipContent = content,
                DipBackgroundColor = Color.Aquamarine,
                DipBorderColor = Color.Transparent,
                DipPreferredPointerDirection = P42.Native.Controls.PointerDirection.Up,
            };
            ((IControl)popup).DipBorderWidth = 2;

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
