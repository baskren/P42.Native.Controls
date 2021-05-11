using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using P42.Native.Controls.Droid;
using static Android.Views.ViewGroup;
using Android.Graphics.Drawables;
using Android.Widget;
using static Android.Views.View;
//using Android.Widget;

namespace Demo.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            P42.Utils.Droid.Settings.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            var mainPage = new MainPage();
            SetContentView(new App(new NavigationPage(mainPage)));

            /*
            SetContentView(Resource.Layout.activity_main);
            var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            var layout = FindViewById<Android.Widget.LinearLayout>(Resource.Id.relativeLayout1);

            P42.Native.Controls.Droid.Platform.Init(this);

            
            var text1 = new Android.Widget.TextView(this)
            {
                Text = "TEXT 1",
                Background = new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Yellow),
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
            };
            text1.SetPadding(0, 0, 0, 0);
            text1.SetMinHeight(10);
            text1.SetMinWidth(10);
            layout.AddView(text1);

            var bubbleText = new TextView(this)
            {
                Text = "CONTENT",
                Gravity = GravityFlags.Bottom | GravityFlags.Right,
                Background = new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Pink),
                LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent),
                Visibility = ViewStates.Visible
            };
            bubbleText.SetIncludeFontPadding(false);
            var bubble = new BubbleBorder
            {
                PointerDirection = P42.Native.Controls.PointerDirection.None,
                DipPadding = 10,
                DipMargin = 10,
                HorizontalAlignment = P42.Native.Controls.Alignment.Stretch,
                DipRequestedHeight = 80,
                Content = bubbleText
            };
            layout.AddView(bubble);
            

            var segment1 = new SegmentButton("LEFT");
            var segment2 = new SegmentButton("CENTER");
            var segment3 = new SegmentButton("right");

            var segmentedControl = new SegmentedControl
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent),
                //DipPadding = 5,
                DipMargin = 10,
                //DipMargin = 10,
                //OutlineColor = Android.Graphics.Color.Black.WithAlpha(0.25)
                //BackgroundColor = Android.Graphics.Color.Transparent,
                //TextColor = Android.Graphics.Color.Pink
            };
            segmentedControl.AddView(segment1);
            segmentedControl.AddView(segment2);
            segmentedControl.AddView(segment3);
            segmentedControl.Background = new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Pink);
            layout.AddView(segmentedControl);





            //var button = new Android.Widget.Button(this)
            var text2 = new Android.Widget.TextView(this)
            {
                Text = "TEXT 2",
                Background = new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Yellow),
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
            };
            text2.SetPadding(0, 0, 0, 0);
            text2.SetMinHeight(10);
            text2.SetMinWidth(10);
            layout.AddView(text2);

            */
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            /*
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
            */
            var content = new FrameLayout(this)
            {
                Background = new ColorDrawable(Android.Graphics.Color.Pink),
            };
            content.SetPadding(10, 10, 10, 10);
            content.AddView(new TextView(this)
            {
                Text = "CONTENT",
            });
            ShowPopup(content, view);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        void ShowPopup(View content, View parent)
        {
            var windowSize = P42.Native.Controls.Droid.DisplayExtensions.Size;

            var shape = new ShapeDrawable();
            shape.Paint.Color = Android.Graphics.Color.DarkGray.WithAlpha(0.2);
            var view = new View(this)
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
                Background = shape
            };
            var pageOverlay = new PopupWindow(view, (int)windowSize.Width, (int)windowSize.Height, true);
            pageOverlay.ShowAtLocation(parent, GravityFlags.Top | GravityFlags.Left, 0, 0);
            pageOverlay.SetTouchInterceptor(new PopupTouchListener(pageOverlay));

            content.Measure(MeasureSpec.MakeMeasureSpec(1000, MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(1000, MeasureSpecMode.Unspecified));
            LayoutInflater inflater = (LayoutInflater)GetSystemService(LayoutInflaterService);
            int width = LayoutParams.WrapContent;
            int height = LayoutParams.WrapContent;
            bool focusable = true;
            var popupWindow = new PopupWindow(content, width, height, focusable);


            popupWindow.ShowAtLocation(parent, GravityFlags.Top | GravityFlags.Left, (int)((windowSize.Width - content.MeasuredWidth)/2 + 0.5), (int)((windowSize.Height - content.MeasuredHeight)/2 + 0.5));
            popupWindow.SetTouchInterceptor(new PopupTouchListener(popupWindow));
        }

        class PopupTouchListener : Java.Lang.Object, IOnTouchListener
        {
            PopupWindow PopupWindow;

            public PopupTouchListener(PopupWindow popupWindow)
                => PopupWindow = popupWindow;

            public bool OnTouch(View v, MotionEvent e)
            {
                var x = e.GetX();
                var y = e.GetY();
                System.Diagnostics.Debug.WriteLine($"PopupTouchListener.OnTouch {e.Action} [{x}, {y}]");
                /*
                if (v is ViewGroup viewGroup)
                {
                    var child = viewGroup.GetChildAt(0); // content
                    //System.Diagnostics.Debug.WriteLine($"PopupTouchListener.OnTouch child: [{child.Left}, {child.Top}, {child.Right}, {child.Bottom}]");
                    if (x >= 0 && x <= child.Width && y >= 0 && y <= child.Height)
                        return true;
                }
                */
                PopupWindow.Dismiss();
                return true;
            }
        }

    }

}
