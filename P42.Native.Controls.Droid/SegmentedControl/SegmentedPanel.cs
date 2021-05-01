using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace P42.Native.Controls.Droid
{
    public class SegmentedPanel : Android.Views.ViewGroup, INotifyPropertyChanged
    {
        float b_dipSpacing = 1;
        public float DipSpacing
        {
            get => b_dipSpacing;
            set
            {
                if (value != b_dipSpacing)
                {
                    b_dipSpacing = value;
                    Invalidate();
                }
            }
        }

        /*
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
        */

        Orientation b_Orientation = Orientation.Horizontal;
        public Orientation Orientation
        {
            get => b_Orientation;
            set => SetField(ref b_Orientation, value);
        }

        protected bool IsHorizontal => Orientation == Orientation.Horizontal;

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

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            System.Diagnostics.Debug.WriteLine($"SegmentedPanel.OnMeasure(({MeasureSpec.GetSize(widthMeasureSpec)},{MeasureSpec.GetMode(widthMeasureSpec)}),({MeasureSpec.GetSize(heightMeasureSpec)},{MeasureSpec.GetMode(heightMeasureSpec)})");
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
                    var size = P42.Native.Controls.Droid.DisplayExtensions.Size;

                    if (availableWidth < 0)
                        availableWidth = size.Width;
                    if (availableHeight < 0)
                        availableHeight = size.Height;
                }

                var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
                var vtMode = MeasureSpec.GetMode(heightMeasureSpec);
                //System.Diagnostics.Debug.WriteLine($" hzMode:{hzMode} vtMode:{vtMode} ");

                int cellWidth = availableWidth;
                int cellHeight = availableHeight;
                var deviceSpacing = (int)(DisplayExtensions.DipToPx(DipSpacing) + 0.5f);
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

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            System.Diagnostics.Debug.WriteLine($"SegmentedPanel.OnLayout({changed}, {l}, {t}, {r}, {b}");
            if (ChildCount > 0)
            {
                var children = this.Children();

                var availableWidth = r - l - PaddingLeft - PaddingRight;
                var availableHeight = b - t - PaddingTop - PaddingBottom;

                float cellWidth = availableWidth;
                float cellHeight = availableHeight;
                float deviceSpacing = (int)(DisplayExtensions.DipToPx(DipSpacing) + 0.5f);

                if (IsHorizontal && availableWidth > 0)
                    cellWidth = (availableWidth - deviceSpacing * (ChildCount - 1)) / ChildCount;
                else if (!IsHorizontal && availableHeight > 0)
                    cellHeight = (availableHeight - deviceSpacing * (ChildCount - 1)) / ChildCount;

                if (IsHorizontal)
                {
                    float start = PaddingLeft;
                    foreach (var child in children)
                    {
                        child.Layout((int)(start + 0.5f), PaddingTop, (int)(start + cellWidth + 0.5f), PaddingTop + availableHeight);
                        start += cellWidth + deviceSpacing;
                    }
                }
                else
                {
                    float start = PaddingTop;
                    foreach (var child in children)
                    {
                        child.Layout(PaddingLeft, (int)(start + 0.5f), PaddingLeft + availableWidth, (int)(start + cellHeight + 0.5f));
                        start += cellHeight + deviceSpacing;
                    }
                }
            }
        }




        #region Property Change Handler
        protected bool hasDrawn;

        [JsonIgnore]
        public bool HasChanged { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
            => HasChanged = false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            if (propertyName == null)
                throw new Exception("null propertyName in SetField");

            field = value;
            HasChanged = true;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetRedrawField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (SetField(ref field, value, propertyName, callerPath))
            {
                if (hasDrawn) PostInvalidate();
                return true;
            }
            return false;
        }

        protected bool SetLayoutField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (SetField(ref field, value, propertyName, callerPath))
            {
                if (hasDrawn) RequestLayout();
                return true;
            }
            return false;
        }
        #endregion
    }
}
