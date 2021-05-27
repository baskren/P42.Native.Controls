using System;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace P42.Native.Controls
{
    public partial class NavigationPage : ViewFlipper
    {

        #region Properties

        public System.Collections.Generic.IEnumerable<IPage> Children => this.Children().Cast<NavPageWrapper>().Select(c => c.Page);
        #endregion


        #region Constructors
        public NavigationPage(IPage page) : this(P42.Utils.Droid.Settings.Context, page) { }

        public NavigationPage(Context context, IPage iPage) : base(context)
        {
            Build();
            if (iPage is View page)
                AddView(new NavPageWrapper(iPage), new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
        }

        public NavigationPage(Context context) : base(context)
        {
            Build();
        }

        public NavigationPage(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Build();
        }

        protected NavigationPage(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Build();
        }

        void Build()
        {
            AutoStart = false;
            SetFlipInterval(int.MaxValue);
            LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
        }
        #endregion




        #region Push / Pop
        static PageAnimationOptions StayPut = new PageAnimationOptions
        {
            Direction = PageAnimationDirection.None,
            ShouldFade = false,
            Duration = TimeSpan.FromMilliseconds(10)
        };

        internal async Task InternalPushAsync(IPage iPushPage, PageAnimationOptions options)
        {
            var fromPage = CurrentPage;
            if (iPushPage is Page pushPage && await iPushPage.OnAppearing(fromPage))
            {
                options = options ?? new PageAnimationOptions
                {
                    Direction = PageAnimationDirection.Default,
                    ShouldFade = false
                };
                InAnimation = options.ToPushAnimation();
                StayPut.Duration = options.Duration;
                OutAnimation = StayPut.ToPushAnimation();
                AddView(new NavPageWrapper(pushPage), new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
                ShowNext();
                await Task.Delay(options.Duration);
                await iPushPage.OnAppeared(fromPage);
            }
        }

        internal async Task InternalPopAsync(IPage iPopPage, PageAnimationOptions options, bool dispose)
        {
            if (ChildCount < 2)
                return;
            if (iPopPage is View popPage && Children.Contains(iPopPage))
            {
                var wrapper = popPage.Parent as NavPageWrapper;
                if (popPage == CurrentPage)
                {
                    var toPage = ((NavPageWrapper)GetChildAt(ChildCount-2)).Page;
                    if (await iPopPage.OnDisappearing(toPage))
                    {
                        options = options ?? new PageAnimationOptions
                        {
                            Direction = PageAnimationDirection.Default,
                            ShouldFade = false
                        };
                        StayPut.Duration = options.Duration;
                        InAnimation = StayPut.ToPopAnimation();
                        OutAnimation = options.ToPopAnimation();
                        ShowPrevious();
                        await Task.Delay(options.Duration);
                        wrapper.RemoveAllViews();
                        RemoveView(wrapper);
                        //wrapper.Dispose();
                        await iPopPage.OnDisappeared(toPage);
                    }
                }
                else
                {
                    if (await iPopPage.OnDisappearing(null))
                    {
                        RemoveView(wrapper);
                        wrapper.RemoveAllViews();
                        wrapper.Dispose();
                        await iPopPage.OnDisappeared(null);
                    }
                }

                if (dispose)
                    popPage.Dispose();


            }
        }
        #endregion


    }
}
