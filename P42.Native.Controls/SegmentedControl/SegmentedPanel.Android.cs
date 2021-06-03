using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace P42.Native.Controls
{
    public partial class SegmentedPanel : ViewGroup
    {
        #region Properties

        #region Spacing
        internal protected double b_Spacing = 0;
        public virtual double Spacing
        {
            get => b_Spacing;
            set => SetRedrawField(ref b_Spacing, value);
        }

        public virtual double DipSpacing
        {
            get => DisplayExtensions.PxToDip(Spacing);
            set => Spacing = DisplayExtensions.DipToPxD(value);
        }
        #endregion


        #region Orientation
        internal protected Orientation b_Orientation = Orientation.Horizontal;
        public virtual Orientation Orientation
        {
            get => b_Orientation;
            set => SetField(ref b_Orientation, value);
        }

        protected bool IsHorizontal => Orientation == Orientation.Horizontal;
        #endregion


        #region ExceedsAvailableSpace
        internal protected bool b_ExceedsAvailableSpace;
        public virtual bool ExceedsAvailableSpace
        {
            get => b_ExceedsAvailableSpace;
            private set => SetField(ref b_ExceedsAvailableSpace, value);
        }
        #endregion


        #region SelectionMode
        internal protected SelectionMode b_SelectionMode = SelectionMode.Single;
        public virtual SelectionMode SelectionMode
        {
            get => b_SelectionMode;
            set
            {
                if (b_SelectionMode != value)
                {
                    b_SelectionMode = value;
                    if (b_SelectionMode == SelectionMode.None)
                    {
                        foreach (var child in this.Children())
                        {
                            if (child is Button button)
                                button.Selected = false;
                        }
                    }
                }
            }
        }
        #endregion


        #endregion



        #region Constructors
        public SegmentedPanel() : base(global::P42.Utils.Droid.Settings.Context)
        {
            Build();
        }

        public SegmentedPanel(Context context) : base(context)
        {
            Build();
        }

        public SegmentedPanel(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Build();
        }

        public SegmentedPanel(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Build();
        }

        public SegmentedPanel(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Build();
        }

        public SegmentedPanel(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Build();
        }

        void Build()
        {
            NtvBaseView = this;
            SetWillNotDraw(false);
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                var kids = this.Children();
                RemoveAllViews();
                foreach (var kid in kids)
                    kid.Dispose();
                Background?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region Android Measure / Layout / Draw
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (ChildCount > 0)
            {
                var children = this.Children();
                foreach (var child in children)
                    child.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

                var availableWidth = MeasureSpec.GetSize(widthMeasureSpec) - NtvMargin.Horizontal;
                var availableHeight = MeasureSpec.GetSize(heightMeasureSpec) - NtvMargin.Vertical;

                //System.Diagnostics.Debug.WriteLine($"availW:{availableWidth} availH:{availableHeight}");

                if (availableWidth < 0 || availableHeight < 0)
                {
                    var size = DisplayExtensions.PxSize();

                    if (availableWidth < 0)
                        availableWidth = size.Width;
                    if (availableHeight < 0)
                        availableHeight = size.Height;
                }

                var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
                var vtMode = MeasureSpec.GetMode(heightMeasureSpec);
                //System.Diagnostics.Debug.WriteLine($" hzMode:{hzMode} vtMode:{vtMode} ");

                if (NtvRequestedWidth > 0 && NtvRequestedWidth <= availableWidth)
                {
                    availableWidth = NtvRequestedWidth;
                    hzMode = MeasureSpecMode.Exactly;
                }
                if (NtvRequestedHeight > 0 && NtvRequestedHeight <= availableHeight)
                {
                    availableHeight = NtvRequestedHeight;
                    vtMode = MeasureSpecMode.Exactly;
                }

                double cellWidth = availableWidth;
                double cellHeight = availableHeight;
                if (IsHorizontal && availableWidth > 0)
                    cellWidth = (int)Math.Ceiling((double)(availableWidth - Spacing * (ChildCount - 1)) / ChildCount);
                else if (!IsHorizontal && availableHeight > 0)
                    cellHeight = (int)Math.Ceiling((double)(availableHeight - Spacing * (ChildCount - 1)) / ChildCount);

                // Having to add Padding to the cell size, below, is yet another example of how the Android developers should have spent more time
                // thinking about their code and less time piling up sexual harrasment lawsuits.  
                MeasureChildren(MeasureSpec.MakeMeasureSpec((int)(cellWidth + PaddingLeft + PaddingRight), hzMode), MeasureSpec.MakeMeasureSpec((int)(cellHeight + PaddingLeft + PaddingRight), vtMode));

                var maxCellWidth = 0;
                var maxCellHeight = 0;
                bool tooSmall = false;
                foreach (var child in children)
                {
                    tooSmall |= child.MeasuredState > 0;
                    maxCellWidth = Math.Max(child.MeasuredWidth, maxCellWidth);
                    maxCellHeight = Math.Max(child.MeasuredHeight, maxCellHeight);
                }
                //System.Diagnostics.Debug.WriteLine($" maxW:{maxCellWidth} maxH:{maxCellHeight}");

                var calcWidth = IsHorizontal
                    ? maxCellWidth * ChildCount + Spacing * (ChildCount - 1)
                    : maxCellWidth;
                var calcHeight = IsHorizontal
                    ? maxCellHeight
                    : maxCellHeight * ChildCount + Spacing * (ChildCount - 1);

                var measuredWidth = calcWidth;
                if (hzMode == MeasureSpecMode.AtMost)
                    measuredWidth = (int)(Math.Min(calcWidth, availableWidth)) | (calcWidth > availableWidth ? MeasuredStateTooSmall : 0);
                else if (hzMode == MeasureSpecMode.Exactly)
                    measuredWidth = availableWidth | (calcWidth > availableWidth ? MeasuredStateTooSmall : 0);

                var measuredHeight = calcHeight;
                if (vtMode == MeasureSpecMode.AtMost)
                    measuredHeight = (int)(Math.Min(calcHeight, availableHeight)) | (calcHeight > availableHeight ? MeasuredStateTooSmall : 0);
                else if (vtMode == MeasureSpecMode.Exactly)
                    measuredHeight = availableHeight | (calcHeight > availableHeight ? MeasuredStateTooSmall : 0);

                //System.Diagnostics.Debug.WriteLine($"measuredW:{measuredWidth} measuredH:{measuredHeight}");
                //System.Diagnostics.Debug.WriteLine(" ");
                //SetMeasuredDimension(measuredWidth + PaddingLeft + PaddingRight, measuredHeight + PaddingTop + PaddingBottom);
                //SetMeasuredDimension(measuredWidth + PaddingLeft + PaddingRight, measuredHeight + PaddingTop + PaddingBottom);
                SetMeasuredDimension((int)(measuredWidth + NtvMargin.Horizontal + PaddingLeft), (int)(measuredHeight + NtvMargin.Vertical + PaddingTop));
                ExceedsAvailableSpace = tooSmall || calcWidth > availableWidth || calcHeight > availableHeight;
            }
            else
            {
                SetMeasuredDimension(0, 0);
                ExceedsAvailableSpace = false;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (ChildCount > 0)
            {
                var children = this.Children();

                var availableWidth = r - l - NtvMargin.Horizontal;
                var availableHeight = b - t - NtvMargin.Vertical;

                double cellWidth = availableWidth;
                double cellHeight = availableHeight;

                if (IsHorizontal && availableWidth > 0)
                    cellWidth = (availableWidth - Spacing * (ChildCount - 1)) / ChildCount;
                else if (!IsHorizontal && availableHeight > 0)
                    cellHeight = (availableHeight - Spacing * (ChildCount - 1)) / ChildCount;

                if (IsHorizontal)
                {
                    double start = NtvMargin.Left;
                    var top = NtvMargin.Top;
                    var bottom = NtvMargin.Top + availableHeight ;
                    foreach (var child in children)
                    {
                        if (child.MeasuredWidth != cellWidth || child.MeasuredHeight != cellHeight)
                            child.Measure(MeasureSpec.MakeMeasureSpec((int)(cellWidth + 0.5), MeasureSpecMode.Exactly), MeasureSpec.MakeMeasureSpec((int)(cellHeight+0.5), MeasureSpecMode.Exactly));
                        child.Layout((int)(start + 0.5), top, (int)(start + cellWidth + 0.5), bottom);
                        start += cellWidth + Spacing;
                    }
                }
                else
                {
                    double start = NtvMargin.Top;
                    var left = (int)(NtvMargin.Left + 0.5);
                    var right = (int)(NtvMargin.Left + availableWidth + 0.5);
                    foreach (var child in children)
                    {
                        if (child.MeasuredWidth != cellWidth || child.MeasuredHeight != cellHeight)
                            child.Measure(MeasureSpec.MakeMeasureSpec((int)(cellWidth + 0.5), MeasureSpecMode.Exactly), MeasureSpec.MakeMeasureSpec((int)(cellHeight + 0.5), MeasureSpecMode.Exactly));
                        child.Layout(left, (int)(start + 0.5), right, (int)(start + cellHeight + 0.5f));
                        start += cellHeight + Spacing;
                    }
                }
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            DipHasDrawn = true;
            NtvActualSize = new SizeI(canvas.Width, canvas.Height);
        }

        #endregion

    }
}
