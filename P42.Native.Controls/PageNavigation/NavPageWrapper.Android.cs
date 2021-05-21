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
    public partial class NavPageWrapper : ViewGroup, INotifiable
    {
        #region Properties

        string Title => Page?.Title;

        internal readonly IPage Page;
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
            if (e.PropertyName == nameof(P42.Native.Controls.Page.Title))
                UpdateTitle();
            else if (e.PropertyName == nameof(P42.Native.Controls.Page.BackButtonTitle))
                UpdateBackButton();
            else if (e.PropertyName == nameof(P42.Native.Controls.Page.HasBackButton))
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

        int NavBarHeight => (int)Math.Max(m_BackButton?.MeasuredHeight + (m_Paint.StrokeWidth + 0.5) ?? 0, m_TitleTextView?.MeasuredHeight + 1 ?? 0);

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            //base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            var availableWidth = MeasureSpec.GetSize(widthMeasureSpec);
            var availableHeight = MeasureSpec.GetSize(heightMeasureSpec);
            var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
            var vtMode = MeasureSpec.GetMode(heightMeasureSpec);


            if (Page is Page page && page.HasNavigationBar)
            {
                m_TitleTextView?.Measure(MeasureSpec.MakeMeasureSpec(availableWidth, MeasureSpecMode.AtMost), MeasureSpec.MakeMeasureSpec(availableHeight, MeasureSpecMode.AtMost));
                m_BackButton?.Measure(MeasureSpec.MakeMeasureSpec(availableWidth, MeasureSpecMode.AtMost), MeasureSpec.MakeMeasureSpec(availableHeight, MeasureSpecMode.AtMost));
                availableHeight -= NavBarHeight + (int)(m_Paint.StrokeWidth + 0.5);
            }

            pageView?.Measure(MeasureSpec.MakeMeasureSpec(availableWidth, hzMode), MeasureSpec.MakeMeasureSpec(availableHeight, vtMode));
            SetMeasuredDimension(availableWidth, availableHeight);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var nbh = 0;
            if (Page is Page page && page.HasNavigationBar)
            {
                nbh = NavBarHeight;
                var t1 = t + (nbh - m_BackButton.MeasuredHeight) / 2;
                if (page.HasBackButton && Parent is NavigationPage navPage && navPage.GetChildAt(0) != this)
                {
                    var p = (int)(DisplayExtensions.DipToPx(5)+0.5);
                    m_BackButton.Layout(l + p, t1, l + p + m_BackButton.MeasuredWidth, t1 + m_BackButton.MeasuredHeight);
                }
                var l1 = l + ((r - l) - m_TitleTextView.MeasuredWidth) / 2;
                t1 = t + (nbh - m_TitleTextView.MeasuredHeight) / 2;
                m_TitleTextView.Layout(l1, t1, l1 + m_TitleTextView.MeasuredWidth, t1 + m_TitleTextView.MeasuredHeight);
            }
            pageView?.Layout(l, t + nbh, r, b);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var nbh = NavBarHeight;
            if (Page is Page page && page.HasNavigationBar && nbh > 0)
            {
                var path = new Path();
                path.MoveTo(0, nbh);
                path.LineTo(canvas.Width, nbh);
                canvas.DrawPath(path, m_Paint);
            }
        }
        #endregion


        #region INotifiable

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        #endregion


        #region Fields
        public bool HasDrawn { get; set; }
        public bool HasChanged { get; set; }
        #endregion


        #region Methods
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public void RedrawElement() => PostInvalidate();

        public void RelayoutElement() => RequestLayout();
        #endregion

        #endregion


    }
}
