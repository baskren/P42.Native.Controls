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
    public partial class Page : ViewGroup
    {


        #region Construction
        public Page() : this(global::P42.Utils.Droid.Settings.Context) { }

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
            SetBackgroundColor(Color.White);
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
            SetMeasuredDimension(availableWidth, availableHeight);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            Content?.Layout((int)(Padding.Left + 0.5), (int)(Padding.Top + 0.5) , Content.MeasuredWidth, Content.MeasuredHeight);
        }

        #endregion


        #region Property Change Handlers
        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Content))
                RemoveView(Content);
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Content))
                AddView(Content, LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
        }
        #endregion
    }
}
