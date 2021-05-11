using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace P42.Native.Controls.Droid
{
    public partial class Page : ViewGroup, IPage
    {
        #region Properties
        string b_Title = string.Empty;
        public string Title
        {
            get => b_Title;
            set => SetField(ref b_Title, value);
        }

        bool b_HasBackButton = true;
        public bool HasBackButton
        {
            get => b_HasBackButton;
            set => SetField(ref b_HasBackButton, value);
        }

        string b_BackButtonTitle = "back";
        public string BackButtonTitle
        {
            get => b_BackButtonTitle;
            set => SetField(ref b_BackButtonTitle, value);
        }


        bool b_HasNavigationBar = true;
        public bool HasNavigationBar
        {
            get => b_HasNavigationBar;
            set => SetField(ref b_HasNavigationBar, value);
        }

        Thickness b_Padding;
        public Thickness Padding
        {
            get => b_Padding;
            set => SetField(ref b_Padding, value);
        }


        View b_Content;
        public View Content
        {
            get => b_Content;
            set
            {
                if (b_Content != null)
                    RemoveView(b_Content);
                if (value != null)
                {
                    value.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                    AddView(value);
                }
                SetField(ref b_Content, value);
            }
        }
        #endregion

        #region Fields
        #endregion


        #region Construction
        public Page() : base (P42.Utils.Droid.Settings.Context)
        {
            Build();
        }

        public Page(Context context) : base(context)
        {
            Build();
        }

        public Page(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Build();
        }

        public Page(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Build();
        }

        public Page(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Build();
        }

        public Page(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Build();
        }

        void Build()
        {
        }

        #endregion



        #region Android Measure / Layout / Draw
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var availableWidth = (int)(MeasureSpec.GetSize(widthMeasureSpec) - Padding.Horizontal + 0.5);
            var availableHeight = (int)(MeasureSpec.GetSize(heightMeasureSpec) - Padding.Vertical + 0.5);
            var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
            var vtMode = MeasureSpec.GetMode(heightMeasureSpec);
            Content.Measure(MeasureSpec.MakeMeasureSpec(availableWidth, hzMode), MeasureSpec.MakeMeasureSpec(availableHeight, vtMode));
            //System.Diagnostics.Debug.WriteLine($"Page.OnMeasure ");
            SetMeasuredDimension(DisplayExtensions.Width, DisplayExtensions.Height);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            Content?.Layout((int)(Padding.Left + 0.5), (int)(Padding.Top + 0.5) , Content.MeasuredWidth, Content.MeasuredHeight);
        }
        
        #endregion


        #region IPage Methods

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        async Task<bool> IPage.OnAppearing(IPage fromPage) => await OnAppearing(fromPage);

        protected async Task<bool> OnAppearing(IPage fromPage) => true;

        async Task IPage.OnAppeared(IPage fromPage) => await OnAppeared(fromPage);

        protected async Task OnAppeared(IPage fromPage) { }

        async Task<bool> IPage.OnDisappearing(IPage toPage) => await OnDisappearing(toPage);

        protected async Task<bool> OnDisappearing(IPage toPage) => true;

        async Task IPage.OnDisappeared(IPage toPage) => await OnDisappeared(toPage);

        protected async Task OnDisappeared(IPage toPage) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        #endregion

    }
}
