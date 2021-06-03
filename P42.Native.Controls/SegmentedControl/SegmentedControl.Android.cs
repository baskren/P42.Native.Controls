using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace P42.Native.Controls
{
    public class SegmentedControl : SegmentedPanel
    {

        static double s_DefaultBorderThickness = DisplayExtensions.DipToPxD(1);
        static Color s_defaultOutlineColor = Color.DarkGray;

        #region Properties


        #region Colors
        Color b_TextColor = SegmentButton.DipDefaultTextColor;
        public virtual Color TextColor
        {
            get => b_TextColor;
            set
            {
                if (SetField(ref b_TextColor, value))
                    UpdateSegments(s => s.DipTextColor = value);
            }
        }

        Color b_SelectedTextColor = SegmentButton.DipDefaultSelectedTextColor;
        public virtual Color SelectedTextColor
        {
            get => b_SelectedTextColor;
            set
            {
                if (SetField(ref b_SelectedTextColor, value))
                    UpdateSegments(s => s.DipSelectedTextColor = value);
            }
        }

        public override Color DipBackgroundColor
        {
            get => b_DipBackgroundColor;
            set
            {
                if (SetField(ref b_DipBackgroundColor, value))
                    UpdateSegments(s => s.DipBackgroundColor = value);
            }
        }

        Color b_SelectedBackgroundColor = SegmentButton.DipDefaultSelectedBackgroundColor;
        public Color SelectedBackgroundColor
        {
            get => b_SelectedBackgroundColor;
            set
            {
                if (SetField(ref b_SelectedBackgroundColor, value))
                    UpdateSegments(s => s.DipSelectedBackgroundColor = value);
            }
        }

        Color b_OutlineColor = s_defaultOutlineColor;
        public Color OutlineColor
        {
            get => b_OutlineColor;
            set => SetField(ref b_OutlineColor, value);
        }
        #endregion

        #region Outline Width / Radius
        public override double NtvCornerRadius
        {
            get => b_NtvCornerRadius;
            set
            {
                if (SetField(ref b_NtvCornerRadius, value))
                    UpdateRadii();
            }
        }
        #endregion

        #region DipPadding
        public override ThicknessI NtvPadding
        {
            get => b_NtvPadding;
            set
            {
                if (SetField(ref b_NtvPadding, value))
                    UpdateSegments(s => s.SetPadding(b_NtvPadding));
            }
        }
        #endregion


        public List<int> SelectedIndexes
        {
            get
            {
                //var result = Segments.Where(s => s.IsSelected).Select(s => s.Index).ToList();
                var result = new List<int>();
                foreach (var child in this.Children())
                {
                    if (child is SegmentButton button &&
                        button.DipSelected)
                        result.Add(button.Index);
                }
                return result;
            }
        }

        #endregion


        #region Fields

        //ShapeDrawable m_Shape = new ShapeDrawable();
        //LayerDrawable m_layer;
        //InsetDrawable m_Inset;
        Paint m_paint = new Paint(PaintFlags.AntiAlias);
        #endregion


        #region Constructors / Initializer
        public SegmentedControl() : this(global::P42.Utils.Droid.Settings.Context) { }

        public SegmentedControl(Context context) : base(context)
        {
            Init();
        }

        public SegmentedControl(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public SegmentedControl(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public SegmentedControl(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public SegmentedControl(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }


        void Init()
        {
            b_DipBackgroundColor = SegmentButton.DipDefaultBackgroundColor;
            b_NtvBorderWidth = s_DefaultBorderThickness;
            b_NtvCornerRadius = SegmentButton.NtvDefaultCornerRadius + s_DefaultBorderThickness;
            b_NtvPadding = DisplayExtensions.DipToPx((Thickness)SegmentButton.NtvDefaultPadding);
            SetWillNotDraw(false);
            UpdateRadii();
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                m_paint.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region Property Change Handlers
        void UpdateRadii()
        {
            UpdateSegments(s => s.NtvCornerRadius = NtvCornerRadius);
        }

        public override void OnViewAdded(View child)
        {
            base.OnViewAdded(child);
            if (child is SegmentButton segment)
            {
                InitiateSegment(segment);
                IndexSegments();
            }
            else
                RemoveView(child);
        }

        public override void OnViewRemoved(View child)
        {
            base.OnViewRemoved(child);
            if (child is SegmentButton)
                IndexSegments();
        }

        void IndexSegments()
        {
            for (int i = 0; i < ChildCount; i++)
            {
                if (GetChildAt(i) is SegmentButton segment)
                {
                    segment.Index = i;
                    if (ChildCount == 1)
                        segment.DipPosition = SegmentPosition.Only;
                    else if (i == 0)
                        segment.DipPosition = SegmentPosition.First;
                    else if (i == ChildCount - 1)
                        segment.DipPosition = SegmentPosition.Last;
                    else
                        segment.DipPosition = SegmentPosition.Middle;
                }
            }
        }

        void InitiateSegment(SegmentButton segment)
        {
            segment.NtvCornerRadius = NtvCornerRadius;
            segment.DipTextColor = TextColor;
            segment.DipSelectedTextColor = SelectedTextColor;
            segment.DipBackgroundColor = DipBackgroundColor;
            segment.DipSelectedBackgroundColor = SelectedBackgroundColor;
            segment.DipOrientation = Orientation;
            segment.SetPadding(NtvPadding);
        }

        internal void OnSegmentClicked(SegmentButton segment)
        {
            if (SelectionMode == SelectionMode.None)
                segment.DipSelected = false;
            else if (SelectionMode == SelectionMode.Radio)
                UpdateSegments(s => s.DipSelected = s == segment);
            else if (SelectionMode == SelectionMode.Single)
            {
                if (segment.DipSelected)
                    segment.DipSelected = false;
                else
                    UpdateSegments(s => s.DipSelected = s == segment);
            }
            else if (SelectionMode == SelectionMode.Multi)
                segment.DipSelected = !segment.DipSelected;
        }

        void UpdateSegments(Action<SegmentButton> action, SegmentButton except = null)
        {
            foreach (var child in this.Children())
            {
                if (child != except && child is SegmentButton button)
                    action?.Invoke(button);
            }
        }
        #endregion


        #region Android Measure / Layout / Draw
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var borderW = (int)(NtvBorderWidth + 0.5);

            var availableWidth = MeasureSpec.GetSize(widthMeasureSpec) - 2 * borderW;
            var availableHeight = MeasureSpec.GetSize(heightMeasureSpec) - 2 * borderW;
            var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
            var vtMode = MeasureSpec.GetMode(heightMeasureSpec);

            base.OnMeasure(
                MeasureSpec.MakeMeasureSpec(availableWidth, hzMode),
                MeasureSpec.MakeMeasureSpec(availableHeight, vtMode));
            SetMeasuredDimension(MeasuredWidth + 2 * borderW, MeasuredHeight + 2 * borderW);
        }

        public override void OnDrawForeground(Canvas canvas)
        {
            base.OnDrawForeground(canvas);
            m_paint.Color = OutlineColor;
            m_paint.SetStyle(Paint.Style.Stroke);
            m_paint.StrokeWidth = (float)NtvBorderWidth;
            var borderInset = (float)NtvBorderWidth / 2.0f;
            float r = (float)NtvCornerRadius - borderInset;
            canvas.DrawRoundRect(
                new Rect(0,0, canvas.Width, canvas.Height).Inflate(-(Thickness)NtvMargin).Inflate(-borderInset).AsRectF(),
                r, r,
                m_paint);

            double cellWidth = canvas.Width - NtvMargin.Horizontal;
            double cellHeight = canvas.Height - NtvMargin.Vertical;

            if (IsHorizontal && canvas.Width > 1)
                cellWidth /= ChildCount;
            else if (!IsHorizontal && canvas.Height > 1)
                cellHeight /= ChildCount;

            var path = new Path();

            if (IsHorizontal)
            {
                double start = NtvMargin.Left;
                var top = (float)(NtvMargin.Top + NtvBorderWidth);
                var bottom = (float)(NtvMargin.Top + cellHeight - NtvBorderWidth);
                for (int i = 0; i < ChildCount - 1; i++)
                {
                    start += cellWidth;
                    var x = (int) (start + 0.5);
                    path.MoveTo(x, top);
                    path.LineTo(x, bottom);
                }
            }
            else
            {
                double start = NtvMargin.Top;
                var left = (float)(NtvMargin.Left + NtvBorderWidth);
                var right = (float)(NtvMargin.Left + cellWidth - NtvBorderWidth);
                for (int i = 0; i < ChildCount -1; i++)
                {
                    start += cellHeight;
                    var y = (int)(start + 0.5);
                    path.MoveTo(left, y);
                    path.LineTo(right, y);
                }
            }

            canvas.DrawPath(path, m_paint);
        }
        #endregion





    }
}
