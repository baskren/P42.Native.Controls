using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace P42.Native.Controls.Droid
{
    public partial class ContentAndDetailPresenter : Android.Views.ViewGroup
    {
        #region Properties

        #region Content
        View b_Content;
        public View Content
        {
            get => b_Content;
            set
            {
                if (b_Content != value)
                {
                    if (b_Content != null)
                        RemoveView(b_Content);
                    SetField(ref b_Content, value);
                    if (b_Content != null)
                        AddView(b_Content);
                }
            }
        }
        #endregion


        #region Footer
        View b_Footer;
        public View Footer
        {
            get => b_Footer;
            set
            {
                if (b_Footer != value)
                    RemoveView(b_Footer);
                SetField(ref b_Footer, value);
                if (b_Footer != value)
                    AddView(b_Footer);
            }
        }
        #endregion


        #region Detail
        View b_Detail;
        public View Detail
        {
            get => b_Detail;
            set => SetField(ref b_Detail, value);
        }

        Color b_DetailBackgroundColor;
        public Color DetailBackgroundColor
        {
            get => b_DetailBackgroundColor;
            set => SetField(ref b_DetailBackgroundColor, value, () =>
            {
                //m_detailDrawer.Background = value.AsDrawable();
                m_targetedPopup.BackgroundColor = value;
            });
        }

        double b_DetailAspectRatio;
        public double DetailAspectRatio
        {
            get => b_DetailAspectRatio;
            set => SetField(ref b_DetailAspectRatio, value);
        }

        View b_Target;
        public View Target
        {
            get => b_Target;
            set => SetField(ref b_Target, value);
        }

        public double DipPopupWidth
        {
            get => DisplayExtensions.PxToDip(PopupWidth);
            set => PopupWidth = (int)(DisplayExtensions.DipToPx(value) + 0.5);
        }

        int b_PopupWidth = -1;
        public int PopupWidth
        {
            get => b_PopupWidth;
            set => SetField(ref b_PopupWidth, value);
        }

        public double DipPopupHeight
        {
            get => DisplayExtensions.PxToDip(PopupHeight);
            set => PopupHeight = (int)(DisplayExtensions.DipToPx(value) + 0.5);
        }

        int b_PopupHeight;
        public int PopupHeight
        {
            get => b_PopupHeight;
            set => SetField(ref b_PopupHeight, value);
        }

        Alignment b_PopupHorizontalAlignment;
        public Alignment PopupHorizontalAlignment
        {
            get => b_PopupHorizontalAlignment;
            set => SetField(ref b_PopupHorizontalAlignment, value);
        }

        Alignment b_PopupVerticalAlignment;
        public Alignment PopupVerticalAlignment
        {
            get => b_PopupVerticalAlignment;
            set => SetField(ref b_PopupVerticalAlignment, value);
        }

        #endregion


        #region General Layout
        public bool IsDrawer => IsInDrawerMode(ViewEstimatedSize);

        public bool IsInDrawerMode(Size size)
        {
            // until Uno.Android.ListView issue are addressed, we're not going here!
            /*
            var popupSize = new Size(PopupWidth, PopupHeight);
            var aspect = AspectRatio(size);
            // landscape
            if (aspect > DetailAspectRatio * 1.5 && popupSize.Width <= ActualWidth)
            {
                var drawerSize = new Size(ActualHeight * DetailAspectRatio, ActualHeight);
                if (drawerSize.Width <= ActualWidth * 0.5 && drawerSize.Height < popupSize.Height + popupMargin * 2 && drawerSize.Width < popupSize.Width + popupMargin * 2)
                {
                    return true;
                }
            }

            // portrait
            if (aspect < (DetailAspectRatio * 0.66) && popupSize.Width <= ActualWidth * 1.5)
            {
                var drawerSize = new Size(ActualWidth, ActualWidth / DetailAspectRatio);
                if (drawerSize.Height <= ActualHeight * 0.5 && drawerSize.Height < popupSize.Height + popupMargin * 2 && drawerSize.Width < popupSize.Width + popupMargin * 2)
                {
                    return true;
                }
            }
            */
            return false;
        }

        double AspectRatio(Size size)
        {
            if (size.Height > 0)
                return size.Width / size.Height;
            return 0;
        }

        Size ViewEstimatedSize
        {
            get
            {
                var width = ActualWidth;
                if (width <= 0)
                    width = MeasuredWidth;
                if (width <= 0)
                    width = DisplayExtensions.Width;
                var height = ActualHeight;
                if (height <= 0)
                    height = MeasuredHeight;
                if (height <= 0)
                    height = DisplayExtensions.Height;
                return new Size(width, height);
            }
        }

        public PushPopState DetailPushPopState { get; private set; } = PushPopState.Popped;

        bool b_IsAnimated;
        public bool IsAnimated
        {
            get => b_IsAnimated;
            set => SetField(ref b_IsAnimated, value);
        }
        #endregion


        #region Light Dismiss Properties

        PageOverlayMode b_PageOverlayMode;
        public PageOverlayMode PageOverlayMode
        {
            get => b_PageOverlayMode;
            set => SetField(ref b_PageOverlayMode, value);
        }

        Color b_PageOverlayColor;
        public Color PageOverlayColor
        {
            get => b_PageOverlayColor;
            set => SetField(ref b_PageOverlayColor, value, ()=>m_pageOverlay.SetBackgroundColor(value));
        }

        #endregion


        #endregion


        #region Events
        public event EventHandler<DismissPointerPressedEventArgs> DismissPointerPressed;
        #endregion


        #region Fields
        //ViewGroup m_detailDrawer;
        View m_pageOverlay;
        TargetedPopup m_targetedPopup;
        #endregion


        #region Constructors
        public ContentAndDetailPresenter(Context context) : base(context)
        {
            Build();
        }

        public ContentAndDetailPresenter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Build();
        }

        public ContentAndDetailPresenter(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Build();
        }

        public ContentAndDetailPresenter(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Build();
        }

        public ContentAndDetailPresenter(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Build();
        }

        void Build()
        {
            m_pageOverlay = new View(Context)
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
            };
            m_pageOverlay.Click += OnPageOverlay_Click;
        }

        #endregion


        #region Android Measure / Layout / Draw
        double PercentOpen = 0.0;
        int _l = 0, _t = 0;

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            // PercentOpen only important for Landscape Drawer - but currently used in Portrait Drawer!

            if (ChildCount > 0)
            {
                var children = this.Children();
                foreach (var child in children)
                    child.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

                var availableWidth = MeasureSpec.GetSize(widthMeasureSpec) - (int)(Margin.Horizontal + 0.5);
                var availableHeight = MeasureSpec.GetSize(heightMeasureSpec) - (int)(Margin.Vertical + 0.5);

                //System.Diagnostics.Debug.WriteLine($"availW:{availableWidth} availH:{availableHeight}");

                if (availableWidth < 0 || availableHeight < 0)
                {
                    var size = P42.Native.Controls.Droid.DisplayExtensions.Size;

                    if (availableWidth < 0)
                        availableWidth = (int)(size.Width + 0.5);
                    if (availableHeight < 0)
                        availableHeight = (int)(size.Height + 0.5);
                }

                var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
                var vtMode = MeasureSpec.GetMode(heightMeasureSpec);
                //System.Diagnostics.Debug.WriteLine($" hzMode:{hzMode} vtMode:{vtMode} ");

                Footer?.Measure(
                    MeasureSpec.MakeMeasureSpec(availableWidth, MeasureSpecMode.Exactly),
                    MeasureSpec.MakeMeasureSpec(availableHeight, MeasureSpecMode.AtMost));
                if (IsInDrawerMode(new Size(availableWidth, availableHeight)))
                {
                    if (m_targetedPopup != null)
                        m_targetedPopup.Content = null;

                    var aspect = availableWidth / availableHeight;
                    if (aspect > 1) // landscape
                    {
                        var drawerWidth = (int)(availableHeight / DetailAspectRatio + 0.5);
                        Detail?.Measure(
                            MeasureSpec.MakeMeasureSpec(drawerWidth, MeasureSpecMode.Exactly),
                            MeasureSpec.MakeMeasureSpec(availableHeight, MeasureSpecMode.Exactly));
                        var remainingWidth = availableWidth - (int)(PercentOpen * (Detail?.MeasuredWidth ?? 0) + 0.5);
                        var remainingHeight = availableHeight - (Footer?.MeasuredHeight ?? 0);
                        Content?.Measure(
                            MeasureSpec.MakeMeasureSpec(remainingWidth, MeasureSpecMode.Exactly),
                            MeasureSpec.MakeMeasureSpec(remainingHeight, MeasureSpecMode.Exactly));
                    }
                    else
                    {
                        var drawerHeight = (int)(availableWidth * DetailAspectRatio + 0.5);
                        Detail?.Measure(
                            MeasureSpec.MakeMeasureSpec(availableWidth, MeasureSpecMode.Exactly),
                            MeasureSpec.MakeMeasureSpec(drawerHeight, MeasureSpecMode.Exactly));
                        var remainingHeight = availableHeight - Math.Max(
                                (int)(PercentOpen * (Detail?.MeasuredHeight ?? 0) + 0.5),
                                (Footer?.MeasuredHeight ?? 0));
                        Content?.Measure(
                            MeasureSpec.MakeMeasureSpec(availableWidth, MeasureSpecMode.Exactly),
                            MeasureSpec.MakeMeasureSpec(remainingHeight, MeasureSpecMode.Exactly));
                    }
                }
                else
                {
                    m_targetedPopup = m_targetedPopup ?? new TargetedPopup();
                    if (m_targetedPopup.Content is null && Detail is View view)
                    {
                        view.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                        m_targetedPopup.Content = view;
                    }
                    var remainingHeight = availableHeight - (Footer?.MeasuredHeight ?? 0);
                    Content?.Measure(
                        MeasureSpec.MakeMeasureSpec(availableWidth, MeasureSpecMode.Exactly),
                        MeasureSpec.MakeMeasureSpec(remainingHeight, MeasureSpecMode.Exactly));
                }

                SetMeasuredDimension((int)(availableWidth + Margin.Horizontal + 0.5), (int)(availableHeight + Margin.Vertical + 0.5));
            }
            else
            {
                SetMeasuredDimension(0, 0);
            }
        }

        void LayoutDetailAndOverlay(double percentOpen)
            => LayoutDetailAndOverlay(percentOpen, _l, _t, _l + ActualWidth, _t + ActualHeight);

        void LayoutPageOverlay(double percentOpen, int l, int t, int r, int b)
        {
            if (percentOpen > 0 &&
                (PageOverlayMode != PageOverlayMode.TouchTransparent ||
                PageOverlayColor.A > 0))
            {
                m_pageOverlay.Alpha = (float)percentOpen;
                m_pageOverlay.Layout(l, t, r, b);
                // https://stackoverflow.com/questions/19676712/pass-touches-to-the-view-under
                m_pageOverlay.Clickable = PageOverlayMode != PageOverlayMode.TouchTransparent;
                m_pageOverlay.Focusable = PageOverlayMode != PageOverlayMode.TouchTransparent;
            }
        }

        void LayoutDetailAndOverlay(double percentOpen, int l, int t, int r, int b)
        {
            if (ChildCount < 1)
                return;
            var width = r - l;
            var height = b - t;
            if (width < 1 || height < 1)
                return;

            Footer?.Layout(l, b - Footer.MeasuredHeight, r, b);

            if (IsInDrawerMode(new Size(width, height)))
            {
                var aspect = width / height;
                if (aspect > 1) // landscape
                {
                    var drawerWidth = (int)(width / DetailAspectRatio + 0.5);
                    var drawerStart = r - (int)(percentOpen * drawerWidth + 0.5);
                    var remainingHeight = height - (Footer?.MeasuredHeight ?? 0);
                    Content?.Layout(l, t, drawerStart, t + remainingHeight);
                    LayoutPageOverlay(percentOpen, l, t, r, b);
                    Detail?.Layout(drawerStart, t, drawerStart + drawerWidth, b);
                }
                else
                {
                    var drawerHeight = (int)(width * DetailAspectRatio + 0.5);
                    var drawerStart = b - (int)(percentOpen * drawerHeight + 0.5);
                    var remainingHeight = height - Math.Max(
                            (int)(PercentOpen * (Detail?.MeasuredHeight ?? 0) + 0.5),
                            (Footer?.MeasuredHeight ?? 0));
                    Content?.Layout(l, t, r, t + remainingHeight);
                    LayoutPageOverlay(percentOpen, l, t, r, b);
                    Detail?.Layout(l, drawerStart, r, drawerStart + drawerHeight);
                }
                Detail?.SetBackgroundColor(DetailBackgroundColor);
            }
            else
            {
                var remainingHeight = height - (Footer?.MeasuredHeight ?? 0);
                Content?.Layout(l, t, r, t + remainingHeight);
                LayoutPageOverlay(percentOpen, l, t, r, b);
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            _l = l;
            _t = t;
            LayoutDetailAndOverlay(PercentOpen, l, t, r, b);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            ActualWidth = canvas.Width;
            ActualHeight = canvas.Height;
            SizeChanged?.Invoke(this, new Size(ActualWidth, ActualHeight));

            hasDrawn = true;
        }
        #endregion


        #region Push / Pop
        public async Task PushDetailAsync()
        {
            if (DetailPushPopState == PushPopState.Pushing || DetailPushPopState == PushPopState.Pushed)
                return;

            if (DetailPushPopState == PushPopState.Popping)
            {
                if (_popCompletionSource is null)
                    await WaitForPop();
                else
                    return;
            }

            DetailPushPopState = PushPopState.Pushing;
            _popCompletionSource = null;

            var size = new Size(ActualWidth, ActualHeight);

            if (IsInDrawerMode(size))
            {
                if (IsAnimated)
                {
                    LayoutDetailAndOverlay(0.11);
                    Action<double> action = percent => LayoutDetailAndOverlay(percent);
                    var animator = new P42.Utils.ActionAnimator(0.11, 0.95, TimeSpan.FromMilliseconds(300), action);
                    await animator.RunAsync();
                }
                LayoutDetailAndOverlay(1);
            }
            else
                await m_targetedPopup.PushAsync();

            DetailPushPopState = PushPopState.Pushed;
            _pushCompletionSource?.SetResult(true);
        }

        public async Task PopDetailAsync()
        {

            if (DetailPushPopState == PushPopState.Popping || DetailPushPopState == PushPopState.Popped)
                return;

            if (DetailPushPopState == PushPopState.Pushing)
            {
                if (_pushCompletionSource is null)
                    await WaitForPush();
                else
                    return;
            }

            DetailPushPopState = PushPopState.Popping;
            _pushCompletionSource = null;

            var size = new Size(ActualWidth, ActualHeight);

            if (IsInDrawerMode(size))
            {
                if (IsAnimated)
                {
                    Action<double> action = percent => LayoutDetailAndOverlay(percent);
                    var animator = new P42.Utils.ActionAnimator(0.89, 0.11, TimeSpan.FromMilliseconds(300), action);
                    await animator.RunAsync();
                }
                LayoutDetailAndOverlay(0);
            }
            else
                await m_targetedPopup.PopAsync();

            DetailPushPopState = PushPopState.Popped;
            _popCompletionSource?.SetResult(true);
        }


        async void OnPageOverlay_Click(object sender, EventArgs e)
        {
            if (PageOverlayMode == PageOverlayMode.TouchDismiss)
            {
                var args = new DismissPointerPressedEventArgs();
                DismissPointerPressed?.Invoke(this, args);
                if (!args.CancelDismiss)
                    await PopDetailAsync();
            }
        }

        /*
        async void OnTargetedPopupPopped(object sender, PopupPoppedEventArgs e)
        {
            //_targetedPopup.DismissPointerPressed -= OnTargetedPopupDismissPointerPressed;
            //_targetedPopup.Popped -= OnTargetedPopupPopped;
            //DetailPushPopState = PushPopState.Popped;
            await PopDetailAsync();
        }

        void OnTargetedPopupDismissPointerPressed(object sender, DismissPointerPressedEventArgs e)
        {
            if (IsLightDismissEnabled)
                DismissPointerPressed?.Invoke(this, e);
            else
                e.CancelDismiss = true;
        }
        /*
        async void OnDismissPointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (IsLightDismissEnabled)
            {
                var dismissEventArgs = new DismissPointerPressedEventArgs();
                DismissPointerPressed?.Invoke(this, dismissEventArgs);
                if (!dismissEventArgs.CancelDismiss)
                    await PopDetailAsync();
            }
        }
        */
        TaskCompletionSource<bool> _popCompletionSource;
        public async Task<bool> WaitForPop()
        {
            _popCompletionSource = _popCompletionSource ?? new TaskCompletionSource<bool>();
            return await _popCompletionSource.Task;
        }


        TaskCompletionSource<bool> _pushCompletionSource;
        async Task<bool> WaitForPush()
        {
            _pushCompletionSource = _pushCompletionSource ?? new TaskCompletionSource<bool>();
            return await _pushCompletionSource.Task;
        }

        #endregion

    }
}
