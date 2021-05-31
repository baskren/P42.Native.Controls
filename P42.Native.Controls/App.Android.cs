using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace P42.Native.Controls
{
    public class App : ViewFlipper
    {
        static App _current;
        public static App Current => _current;

        #region Properties
        public IPage CurrentPage => CurrentView as IPage;
        #endregion


        #region Constructors
        public App(Context context) : base(context) { }

        
        public App(Page page) : this(global::P42.Utils.Droid.Settings.Context, page) { }

        public App(Context context, Page page) : base(context)
        {
            Build(page);
        }

        public App(NavigationPage page) : this(global::P42.Utils.Droid.Settings.Context, page) { }

        public App(Context context, NavigationPage page) : base(context)
        {
            Build(page);
        }

        void Build(View page)
        {
            _current = this;
            AutoStart = false;
            SetFlipInterval(int.MaxValue);
            AddView(page, new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
        }
        
        #endregion

        #region Push / Pop
        static PageAnimationOptions StayPut = new PageAnimationOptions
        {
            Direction = PageAnimationDirection.None,
            ShouldFade = false,
            Duration = TimeSpan.FromMilliseconds(10)
        };

        internal async Task InternalPushModalAsync(IPage pushPage, PageAnimationOptions options)
        {
            if (pushPage != null)
            {
                var fromPage = CurrentPage;
                if (await pushPage.OnAppearing(fromPage))
                {
                    options = options ?? new PageAnimationOptions
                    {
                        Direction = PageAnimationDirection.DefaultVertical,
                        ShouldFade = false
                    };
                    InAnimation = options.ToPushAnimation();
                    StayPut.Duration = options.Duration;
                    OutAnimation = StayPut.ToPushAnimation();
                    //((View)pushPage).Background = Android.Graphics.Color.Transparent.ToColorDrawable();
                    AddView(pushPage as View, new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
                    ShowNext();
                    await Task.Delay(options.Duration);
                    await pushPage.OnAppeared(fromPage);
                }
            }
        }

        internal async Task InternalPopModalAsync(IPage popPage, PageAnimationOptions options, bool dispose)
        {
            if (ChildCount < 2)
                return;
            if (popPage == CurrentView)
            {
                var toPage = GetChildAt(ChildCount - 2) as IPage;
                if (await popPage.OnDisappearing(toPage))
                {
                    options = options ?? new PageAnimationOptions
                    {
                        Direction = PageAnimationDirection.DefaultVertical,
                        ShouldFade = false
                    };
                    StayPut.Duration = options.Duration;
                    InAnimation = StayPut.ToPopAnimation();
                    OutAnimation = options.ToPopAnimation();
                    ShowPrevious();
                    await Task.Delay(options.Duration);
                    RemoveView(popPage as View);
                    await popPage.OnDisappeared(toPage);
                }
            }
            else
            {
                if (await popPage.OnDisappearing(null))
                {
                    RemoveView(popPage as View);
                    await popPage.OnDisappeared(null);
                }
            }

            if (dispose)
                ((View)popPage).Dispose();
        }
        #endregion
    }
}
