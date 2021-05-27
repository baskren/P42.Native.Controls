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
        Color b_TextColor = SegmentButton.s_DefaultTextColor;
        public virtual Color TextColor
        {
            get => b_TextColor;
            set
            {
                if (SetField(ref b_TextColor, value))
                    UpdateSegments(s => s.TextColor = value);
            }
        }

        Color b_SelectedTextColor = SegmentButton.s_DefaultSelectedTextColor;
        public virtual Color SelectedTextColor
        {
            get => b_SelectedTextColor;
            set
            {
                if (SetField(ref b_SelectedTextColor, value))
                    UpdateSegments(s => s.SelectedTextColor = value);
            }
        }

        public override Color BackgroundColor
        {
            get => b_BackgroundColor;
            set
            {
                if (SetField(ref b_BackgroundColor, value))
                    UpdateSegments(s => s.BackgroundColor = value);
            }
        }

        Color b_SelectedBackgroundColor = SegmentButton.s_DefaultSelectedBackgroundColor;
        public Color SelectedBackgroundColor
        {
            get => b_SelectedBackgroundColor;
            set
            {
                if (SetField(ref b_SelectedBackgroundColor, value))
                    UpdateSegments(s => s.SelectedBackgroundColor = value);
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
        public override double CornerRadius
        {
            get => b_CornerRadius;
            set
            {
                if (SetField(ref b_CornerRadius, value))
                    UpdateRadii();
            }
        }
        #endregion

        #region DipPadding
        public override ThicknessI Padding
        {
            get => b_Padding;
            set
            {
                if (SetField(ref b_Padding, value))
                    UpdateSegments(s => s.SetPadding(b_Padding));
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
                        button.Selected)
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
        public SegmentedControl() : this(P42.Utils.Droid.Settings.Context) { }

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
            b_BackgroundColor = SegmentButton.s_DefaultBackgroundColor;
            b_BorderWidth = s_DefaultBorderThickness;
            b_CornerRadius = SegmentButton.s_DefaultCornerRadius + s_DefaultBorderThickness;
            b_Padding = DisplayExtensions.DipToPx((Thickness)SegmentButton.s_DefaultPadding);
            SetWillNotDraw(false);
            UpdateRadii();
        }
        #endregion


        #region Property Change Handlers
        void UpdateRadii()
        {
            UpdateSegments(s => s.CornerRadius = CornerRadius);
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
                        segment.Position = SegmentPosition.Only;
                    else if (i == 0)
                        segment.Position = SegmentPosition.First;
                    else if (i == ChildCount - 1)
                        segment.Position = SegmentPosition.Last;
                    else
                        segment.Position = SegmentPosition.Middle;
                }
            }
        }

        void InitiateSegment(SegmentButton segment)
        {
            segment.CornerRadius = CornerRadius;
            segment.TextColor = TextColor;
            segment.SelectedTextColor = SelectedTextColor;
            segment.BackgroundColor = BackgroundColor;
            segment.SelectedBackgroundColor = SelectedBackgroundColor;
            segment.Orientation = Orientation;
            segment.Gravity = GravityFlags.Center;
            segment.SetPadding(Padding);
        }

        internal void OnSegmentClicked(SegmentButton segment)
        {
            if (SelectionMode == SelectionMode.None)
                segment.Selected = false;
            else if (SelectionMode == SelectionMode.Radio)
                UpdateSegments(s => s.Selected = s == segment);
            else if (SelectionMode == SelectionMode.Single)
            {
                if (segment.Selected)
                    segment.Selected = false;
                else
                    UpdateSegments(s => s.Selected = s == segment);
            }
            else if (SelectionMode == SelectionMode.Multi)
                segment.Selected = !segment.Selected;
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
            var borderW = (int)(BorderWidth + 0.5);

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
            m_paint.StrokeWidth = (float)BorderWidth;
            var borderInset = (float)BorderWidth / 2.0f;
            float r = (float)CornerRadius - borderInset;
            canvas.DrawRoundRect(
                new Rect(0,0, canvas.Width, canvas.Height).Inflate(-(Thickness)Margin).Inflate(-borderInset).AsRectF(),
                r, r,
                m_paint);

            double cellWidth = canvas.Width - Margin.Horizontal;
            double cellHeight = canvas.Height - Margin.Vertical;

            if (IsHorizontal && canvas.Width > 1)
                cellWidth /= ChildCount;
            else if (!IsHorizontal && canvas.Height > 1)
                cellHeight /= ChildCount;

            var path = new Path();

            if (IsHorizontal)
            {
                double start = Margin.Left;
                var top = (float)(Margin.Top + BorderWidth);
                var bottom = (float)(Margin.Top + cellHeight - BorderWidth);
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
                double start = Margin.Top;
                var left = (float)(Margin.Left + BorderWidth);
                var right = (float)(Margin.Left + cellWidth - BorderWidth);
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
