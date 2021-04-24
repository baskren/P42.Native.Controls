using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace P42.Native.Controls.Droid
{
    public class SegmentedPanel : Android.Views.ViewGroup
    {
        float _spacing = 1;
        public float Spacing
        {
            get => _spacing;
            set
            {
                if (value != _spacing)
                {
                    _spacing = value;
                    Invalidate();
                }
            }
        }

        protected bool _isHorizontal = true;
        public virtual bool IsHorizontal
        {
            get => _isHorizontal;
            set
            {
                if (_isHorizontal != value)
                {
                    _isHorizontal = value;
                    Invalidate();
                }
            }
        }

        public bool ExceedsAvailableSpace { get; private set; }

        SelectionMode b_selectionMode = SelectionMode.Single;
        public SelectionMode SelectionMode
        {
            get => b_selectionMode;
            set
            {
                if (b_selectionMode != value)
                {
                    b_selectionMode = value;
                    if (b_selectionMode == SelectionMode.None)
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

        public SegmentedPanel(Context context) : base(context)
        {
        }

        public SegmentedPanel(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public SegmentedPanel(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public SegmentedPanel(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public SegmentedPanel(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (ChildCount > 0)
            {
                var children = this.Children();

                var availableWidth = r - l - PaddingLeft - PaddingRight;
                var availableHeight = b - t - PaddingTop - PaddingBottom;

                float cellWidth = availableWidth;
                float cellHeight = availableHeight;
                float deviceSpacing = (int)(ViewExtensions.ConvertFromDipToPx(Spacing) + 0.5f);

                if (IsHorizontal && availableWidth > 0)
                    cellWidth = (availableWidth - deviceSpacing * (ChildCount - 1)) / ChildCount;
                else if (!IsHorizontal && availableHeight > 0)
                    cellHeight = (availableHeight - deviceSpacing * (ChildCount - 1)) / ChildCount;

                if (IsHorizontal)
                {
                    float start = l + PaddingLeft;
                    foreach (var child in children)
                    {
                        child.Layout((int)(start+0.5f), t + PaddingTop, (int)(start + cellWidth + 0.5f), b - PaddingBottom);
                        start += cellWidth + deviceSpacing;
                    }
                }
                else
                {
                    float start = t + PaddingTop;
                    foreach (var child in children)
                    {
                        child.Layout(l + PaddingLeft, (int)(start + 0.5f), r - PaddingRight, (int)(start + cellHeight + 0.5f));
                        start += cellHeight + deviceSpacing;
                    }
                }
            }
        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (ChildCount > 0)
            {
                var children = this.Children();
                foreach (var child in children)
                    child.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

                var availableWidth = MeasureSpec.GetSize(widthMeasureSpec) - PaddingLeft - PaddingRight;
                var availableHeight = MeasureSpec.GetSize(heightMeasureSpec) - PaddingTop - PaddingBottom;

                //System.Diagnostics.Debug.WriteLine($"availW:{availableWidth} availH:{availableHeight}");

                if (availableWidth < 0 || availableHeight < 0)
                {
                    using var service = Context.ApplicationContext.GetSystemService(Context.WindowService);
                    using var windowManager = service?.JavaCast<IWindowManager>();
                    var display = windowManager?.DefaultDisplay;
                    using var displayMetrics = new DisplayMetrics();
                    display?.GetRealMetrics(displayMetrics);

                    if (availableWidth < 0)
                        availableWidth = displayMetrics?.WidthPixels ?? 0;
                    if (availableHeight < 0)
                        availableHeight = displayMetrics?.HeightPixels ?? 0;
                }

                var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
                var vtMode = MeasureSpec.GetMode(heightMeasureSpec);
                //System.Diagnostics.Debug.WriteLine($" hzMode:{hzMode} vtMode:{vtMode} ");

                int cellWidth = availableWidth;
                int cellHeight = availableHeight;
                var deviceSpacing = (int)(ViewExtensions.ConvertFromDipToPx(Spacing) + 0.5f);
                if (IsHorizontal && availableWidth > 0)
                    cellWidth = (int)Math.Ceiling((double)(availableWidth - deviceSpacing * (ChildCount - 1)) / ChildCount);
                else if (!IsHorizontal && availableHeight > 0)
                    cellHeight = (int)Math.Ceiling((double)(availableHeight - deviceSpacing * (ChildCount - 1)) / ChildCount);

                //System.Diagnostics.Debug.WriteLine($" cellW:{cellWidth} cellH:{cellHeight}");
                // Having to add Padding to the cell size, below, is yet another example of how the Android developers should have spent more time
                // thinking about their code and less time piling up sexual harrasment lawsuits.  Bunch of drunk monkeys claiming to be software engineers.
                MeasureChildren(MeasureSpec.MakeMeasureSpec(cellWidth + PaddingLeft + PaddingRight, hzMode), MeasureSpec.MakeMeasureSpec(cellHeight + PaddingLeft + PaddingRight, vtMode));

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
                    ? maxCellWidth * ChildCount + deviceSpacing * (ChildCount - 1)
                    : maxCellWidth;
                var calcHeight = IsHorizontal
                    ? maxCellHeight
                    : maxCellHeight * ChildCount + deviceSpacing * (ChildCount - 1);

                var measuredWidth = calcWidth;
                if (hzMode == MeasureSpecMode.AtMost)
                {
                    measuredWidth = Math.Min(calcWidth, availableWidth) | (calcWidth > availableWidth ? MeasuredStateTooSmall : 0);
                }
                else if (hzMode == MeasureSpecMode.Exactly)
                {
                    measuredWidth = availableWidth | (calcWidth > availableWidth ? MeasuredStateTooSmall : 0);
                }

                var measuredHeight = calcHeight;
                if (vtMode == MeasureSpecMode.AtMost)
                {
                    measuredHeight = Math.Min(calcHeight, availableHeight) | (calcHeight > availableHeight ? MeasuredStateTooSmall : 0);
                }
                else if (vtMode == MeasureSpecMode.Exactly)
                {
                    measuredHeight = availableHeight | (calcHeight > availableHeight ? MeasuredStateTooSmall : 0);
                }

                //System.Diagnostics.Debug.WriteLine($"measuredW:{measuredWidth} measuredH:{measuredHeight}");
                //System.Diagnostics.Debug.WriteLine(" ");
                //SetMeasuredDimension(measuredWidth + PaddingLeft + PaddingRight, measuredHeight + PaddingTop + PaddingBottom);
                SetMeasuredDimension(measuredWidth + PaddingLeft + PaddingRight, measuredHeight + PaddingTop + PaddingBottom);
                ExceedsAvailableSpace = tooSmall || calcWidth > availableWidth || calcHeight > availableHeight;
            }
            else
            {
                SetMeasuredDimension(0, 0);
                ExceedsAvailableSpace = false;
            }
        }

    }
}
