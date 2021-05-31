using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace P42.Native.Controls
{
    public partial class NavPageWrapper : ViewGroup
    {
        #region Properties

        internal readonly View pageView;

        #endregion


        #region Fields
        //GridLayout m_NavBar;
        TextView m_BackButton;
        TextView m_TitleTextView;
        Android.Graphics.Drawables.ShapeDrawable m_BorderDrawable;
        bool _disposed;
        Paint m_Paint = new Paint(PaintFlags.AntiAlias)
        {
            Color = Color.Black,
            StrokeWidth = (float)DisplayExtensions.DipToPxD(1),
            
        };
        #endregion


        #region Construction
        public NavPageWrapper(IPage page) : base(page.Context)
        {
            SetBackgroundColor(Color.White);
            SetWillNotDraw(false);
            m_Paint.SetStyle(Paint.Style.Stroke);
            Page = page;
            pageView = page as View;
            Page.PropertyChanged += OnPage_PropertyChanged;
            AddView(pageView, new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
          
            UpdateTitle();
            UpdateBackButton();

            Background = Android.Graphics.Color.Red.ToColorDrawable();

        }

        /*
        public NavPageWrapper(Context context) : base(context)
        {
            Build();
        }

        public NavPageWrapper(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Build();
        }

        public NavPageWrapper(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Build();
        }

        public NavPageWrapper(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Build();
        }

        public NavPageWrapper(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Build();
        }

        void Build()
        {
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (Page is Page page)
                    page.PropertyChanged -= OnPage_PropertyChanged;
                m_TitleTextView?.Dispose();
                m_BackButton?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region Property Change Handlers
        private void OnPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Controls.Page.Title))
                UpdateTitle();
            else if (e.PropertyName == nameof(Controls.Page.BackButtonTitle))
                UpdateBackButton();
            else if (e.PropertyName == nameof(Controls.Page.HasBackButton))
                UpdateBackButton();
        }

        void UpdateBackButton()
        {
            if (Page is Page page && page.HasNavigationBar && page.HasBackButton)
            {
                if (m_BackButton is null)
                {
                    m_BackButton = new TextView(Context);
                    m_BackButton.SetTextColor(Color.Blue);
                    m_BackButton.Click += OnBackButtonClicked;
                    AddView(m_BackButton);
                }
                m_BackButton.Text = "< " + page?.BackButtonTitle;
            }
        }

        void UpdateTitle()
        {
            if (Page is Page page && page.HasNavigationBar && !string.IsNullOrEmpty(Title))
            {
                if (m_TitleTextView is null)
                {
                    m_TitleTextView = new TextView(Context)
                    {
                        TextSize = 20,
                    };
                    m_TitleTextView.SetTypeface(null, TypefaceStyle.Bold);
                    AddView(m_TitleTextView);
                }
                m_TitleTextView.Text = Title;
                m_TitleTextView.Visibility = ViewStates.Visible;
                
            }
        }
        #endregion


        #region Event Handlers
        protected async virtual void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Page.PopAsync();
        }
        #endregion


        #region Android Measure / Layout / Draw

        int NavBarHeight => (int)Math.Min(Math.Max(m_BackButton?.MeasuredHeight ?? 0, m_TitleTextView?.MeasuredHeight ?? 0),DisplayExtensions.DipToPx(30));

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            //base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            var availableWidth = MeasureSpec.GetSize(widthMeasureSpec);
            var availableHeight = MeasureSpec.GetSize(heightMeasureSpec);
            var pageAvailableHeight = availableHeight;
            var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
            var vtMode = MeasureSpec.GetMode(heightMeasureSpec);


            if (Parent is NavigationPage navPage && Page is Page page && page.HasNavigationBar)
            {
                m_TitleTextView?.Measure(MeasureSpec.MakeMeasureSpec(availableWidth, MeasureSpecMode.AtMost), MeasureSpec.MakeMeasureSpec(availableHeight, MeasureSpecMode.AtMost));
                m_BackButton?.Measure(MeasureSpec.MakeMeasureSpec(availableWidth, MeasureSpecMode.AtMost), MeasureSpec.MakeMeasureSpec(availableHeight, MeasureSpecMode.AtMost));
                pageAvailableHeight -= NavBarHeight;// + (int)(m_Paint.StrokeWidth + 0.5);
            }

            pageView?.Measure(MeasureSpec.MakeMeasureSpec(availableWidth, hzMode), MeasureSpec.MakeMeasureSpec(pageAvailableHeight, vtMode));
            SetMeasuredDimension(availableWidth, availableHeight);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var navBarHt = 0;
            if (Parent is NavigationPage navPage && Page is Page page && page.HasNavigationBar)
            {
                navBarHt = NavBarHeight;
                var t1 = t + (navBarHt - m_BackButton.MeasuredHeight) / 2;
                if (page.HasBackButton && navPage.GetChildAt(0) != this)
                {
                    var p = DisplayExtensions.DipToPx(5);
                    m_BackButton.Layout(l + p, t1, l + p + m_BackButton.MeasuredWidth, t1 + m_BackButton.MeasuredHeight);
                }
                if (m_TitleTextView is View ttv)
                {
                    var l1 = l + ((r - l) - ttv.MeasuredWidth) / 2;
                    t1 = t + (navBarHt - ttv.MeasuredHeight) / 2;
                    ttv.Layout(l1, t1, l1 + ttv.MeasuredWidth, t1 + ttv.MeasuredHeight);
                }
            }
            pageView?.Layout(l, t + navBarHt + (int)(m_Paint.StrokeWidth + 0.5), r, b);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (Parent is NavigationPage navPage && Page is Page page && page.HasNavigationBar)
            {
                var path = new Path();
                path.MoveTo(0, NavBarHeight);
                path.LineTo(canvas.Width, NavBarHeight);
                canvas.DrawPath(path, m_Paint);
            }
        }
        #endregion


    }
}
