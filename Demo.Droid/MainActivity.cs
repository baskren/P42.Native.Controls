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
//using Android.Widget;

namespace Demo.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            var layout = FindViewById<Android.Widget.RelativeLayout>(Resource.Id.relativeLayout1);

            P42.Native.Controls.Droid.Platform.Init(this);

            
            var segment1 = new SegmentButton("BUTTON 1");
            var segment2 = new SegmentButton("BUTTON 2");
            var segment3 = new SegmentButton("BUTTON 3");

            var segmentedControl = new SegmentedControl
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
            };
            segmentedControl.AddView(segment1);
            segmentedControl.AddView(segment2);
            segmentedControl.AddView(segment3);
            segmentedControl.SetPadding(10, 10, 10, 10);
            //segmentedControl.Background = new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Pink);
            layout.AddView(segmentedControl);


            /*

            //var button = new Android.Widget.Button(this)
            var button = new Android.Widget.TextView(this)
            {
                Text = "BUTTON X",
                
            };
            button.SetPadding(0,0,0,0);
            button.SetMinHeight(10);
            button.SetMinWidth(10);

            layout.AddView(button);
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
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
