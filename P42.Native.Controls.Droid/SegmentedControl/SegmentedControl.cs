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

namespace P42.Native.Controls.Droid
{
    public class SegmentedControl : SegmentedPanel, INotifyPropertyChanged
    {

        static int s_defaultBorderThickness = (int)(DisplayExtensions.DipToPx(1) + 0.5f);
        static Color s_defaultOutlineColor = Color.DarkGray;

        #region Properties


        #region Colors
        Color b_TextColor = SegmentButton.s_defaultTextColor;
        public Color TextColor
        {
            get => b_TextColor;
            set
            {
                if (SetField(ref b_TextColor, value))
                    UpdateSegments(s => s.TextColor = value);
            }
        }

        Color b_BackgroundColor = SegmentButton.s_defaultBackgroundColor;
        public Color BackgroundColor
        {
            get => b_BackgroundColor;
            set
            {
                if (SetField(ref b_BackgroundColor, value))
                    UpdateSegments(s => s.BackgroundColor = value);
            }
        }

        Color b_SelectedTextColor = SegmentButton.s_defaultSelectedTextColor;
        public Color SelectedTextColor
        {
            get => b_SelectedTextColor;
            set
            {
                if (SetField(ref b_SelectedTextColor, value))
                    UpdateSegments(s => s.SelectedTextColor = value);
            }
        }

        Color b_SelectedBackgroundColor = SegmentButton.s_defaultSelectedBackgroundColor;
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
        double b_borderWidth = s_defaultBorderThickness;
        public double BorderWidth
        {
            get => b_borderWidth;
            set => SetField(ref b_borderWidth, value);
        }

        public double DipBorderWidth
        {
            get => DisplayExtensions.PxToDip(b_borderWidth);
            set => BorderWidth = DisplayExtensions.DipToPx(value);
        }


        double b_cornerRadius = SegmentButton.s_defaultCornerRadius + s_defaultBorderThickness;
        public double CornerRadius
        {
            get => b_cornerRadius;
            set
            {
                if (SetField(ref b_cornerRadius, value))
                    UpdateRadii();
            }
        }

        public double DipCornerRadius
        {
            get => DisplayExtensions.PxToDip(b_cornerRadius);
            set => CornerRadius = DisplayExtensions.DipToPx(value);
        }
        #endregion

        #region DipPadding
        Thickness b_dipPadding = SegmentButton.sDefaultDipPadding;
        public Thickness DipPadding
        {
            get => b_dipPadding;
            set
            {
                if (SetField(ref b_dipPadding, value))
                    UpdateSegments(s => s.SetDipPadding(b_dipPadding));
            }
        }
        #endregion

        #endregion


        #region Fields

        //ShapeDrawable m_Shape = new ShapeDrawable();
        //LayerDrawable m_layer;
        //InsetDrawable m_Inset;
        Paint m_paint = new Paint();
        #endregion


        #region Constructors / Initializer
        public SegmentedControl() : base(P42.Utils.Droid.Settings.Context)
        {
            Init();
        }

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
            SetWillNotDraw(false);
            m_paint.AntiAlias = true;
            m_paint.SetStyle(Paint.Style.Fill);
            UpdateRadii();
            SetPadding(PaddingLeft, PaddingTop, PaddingRight, PaddingBottom);
            DipSpacing = 0;
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
            segment.IsHorizontal = IsHorizontal;
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

            System.Diagnostics.Debug.WriteLine($"SegmentedControl.OnMeasure(({MeasureSpec.GetSize(widthMeasureSpec)},{MeasureSpec.GetMode(widthMeasureSpec)}),({MeasureSpec.GetSize(heightMeasureSpec)},{MeasureSpec.GetMode(heightMeasureSpec)})");

            var availableWidth = MeasureSpec.GetSize(widthMeasureSpec) - 2 * borderW;
            var availableHeight = MeasureSpec.GetSize(heightMeasureSpec) - 2 * borderW;
            var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
            var vtMode = MeasureSpec.GetMode(heightMeasureSpec);



            base.OnMeasure(
                MeasureSpec.MakeMeasureSpec(availableWidth, hzMode),
                MeasureSpec.MakeMeasureSpec(availableHeight, vtMode));
            SetMeasuredDimension(MeasuredWidth + 2 * borderW, MeasuredHeight + 2 * borderW);
        }

        /*
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            System.Diagnostics.Debug.WriteLine($"SegmentedControl.OnLayout({changed}, {l}, {t}, {r}, {b}");
            var borderW = (int)(BorderWidth + 0.5);
            base.OnLayout(changed, l + borderW, t + borderW, r - borderW, b - borderW);
        }
        */

        /*
        protected override void OnDraw(Canvas canvas)
        {
            
            System.Diagnostics.Debug.WriteLine($"SegmentedControl.OnDraw({canvas.Width}, {canvas.Height})");
            m_paint.Color = OutlineColor;
            float r = (float)CornerRadius;
            canvas.DrawRoundRect(
                new RectF(0 + PaddingLeft, 0 + PaddingTop, canvas.Width - PaddingRight, canvas.Height - PaddingBottom),
                r, r,
                m_paint);
            base.OnDraw(canvas);
            
        }
        */
        public override void OnDrawForeground(Canvas canvas)
        {
            base.OnDrawForeground(canvas);
            m_paint.Color = OutlineColor;
            m_paint.SetStyle(Paint.Style.Stroke);
            m_paint.StrokeWidth = (float)BorderWidth;
            var borderInset = (float)BorderWidth / 2.0f;
            float r = (float)CornerRadius - borderInset;
            canvas.DrawRoundRect(
                new RectF(PaddingLeft + borderInset, PaddingTop + borderInset, canvas.Width - PaddingRight - borderInset, canvas.Height - PaddingBottom - borderInset),
                r, r,
                m_paint);

            System.Diagnostics.Debug.WriteLine($"SegmentedControl.OnDrawForeground: right: {canvas.Width - PaddingRight}  radius:{r}");

            double cellWidth = canvas.Width - PaddingLeft - PaddingRight;
            double cellHeight = canvas.Height - PaddingTop - PaddingBottom;

            if (IsHorizontal && canvas.Width > 1)
                cellWidth /= ChildCount;
            else if (!IsHorizontal && canvas.Height > 1)
                cellHeight /= ChildCount;

            var path = new Path();

            if (IsHorizontal)
            {
                double start = PaddingLeft;
                var top = (float)(PaddingTop + BorderWidth);
                var bottom = (float)(PaddingTop + cellHeight - BorderWidth);
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
                double start = PaddingTop;
                var left = (float)(PaddingLeft + BorderWidth);
                var right = (float)(PaddingLeft + cellWidth - BorderWidth);
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
