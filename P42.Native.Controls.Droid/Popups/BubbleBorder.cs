﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Newtonsoft.Json;

namespace P42.Native.Controls.Droid
{
    public class BubbleBorder : Android.Views.ViewGroup, INotifyPropertyChanged
    {

        #region Properties

        Android.Views.View b_Content;
        public Android.Views.View Content
        {
            get => b_Content;
            set
            {
                if (b_Content != value)
                {
                    if (b_Content != null)
                        RemoveView(b_Content);
                    if (value != null)
                        AddView(value);
                    SetField(ref b_Content, value);
                }
            }
        }

        #region Pointer
        /*
        double b_PointerBias;
        public double PointerBias
        {
            get => b_PointerBias;
            set => SetRedrawField(ref b_PointerBias, value);
        }
        */

        double b_PointerLength = DisplayExtensions.DipToPx(10);
        public double PointerLength
        {
            get => b_PointerLength;
            set => SetLayoutField(ref b_PointerLength, value);
        }

        public double DipPointerLength
        {
            get => DisplayExtensions.PxToDip(b_PointerLength);
            set => PointerLength = DisplayExtensions.DipToPx(value);
        }

        double b_PointerTipRadius = DisplayExtensions.DipToPx(2);
        public double PointerTipRadius
        {
            get => b_PointerTipRadius;
            set => SetRedrawField(ref b_PointerTipRadius, value);
        }

        public double DipPointerTipRadius
        {
            get => DisplayExtensions.PxToDip(b_PointerTipRadius);
            set => PointerTipRadius = DisplayExtensions.DipToPx(value);
        }

        double b_PointerAxialPosition = 0.5;
        public double PointerAxialPosition
        {
            get => b_PointerAxialPosition;
            set => SetRedrawField(ref b_PointerAxialPosition, value);
        }

        public double DipPointerAxialPosition
        {
            get => DisplayExtensions.PxToDip(b_PointerAxialPosition);
            set
            {
                if (value <= 1)
                    PointerAxialPosition = value;
                else
                    PointerAxialPosition = DisplayExtensions.DipToPx(value);
            }
        }

        PointerDirection b_PointerDirection = PointerDirection.Any;
        public PointerDirection PointerDirection
        {
            get => b_PointerDirection;
            set
            {
                if (PointerDirection.IsHorizontal() == value.IsHorizontal())
                    SetRedrawField(ref b_PointerDirection, value);
                else
                    SetLayoutField(ref b_PointerDirection, value);
            }
        }

        double b_PointerCornerRadius = DisplayExtensions.DipToPx(2);
        public double PointerCornerRadius
        {
            get => b_PointerCornerRadius;
            set => SetField(ref b_PointerCornerRadius, value);
        }

        public double DipPointerCornerRadius
        {
            get => DisplayExtensions.PxToDip(b_PointerCornerRadius);
            set => PointerCornerRadius = DisplayExtensions.DipToPx(value);
        }
        #endregion


        #region Control
        Thickness b_Padding = DisplayExtensions.DipToPx(10);
        public Thickness Padding
        {
            get => b_Padding;
            set => SetLayoutField(ref b_Padding, value);
        }

        public Thickness DipPadding
        {
            get => DisplayExtensions.PxToDip(b_Padding);
            set => Padding = DisplayExtensions.DipToPx(value);
        }

        double b_BorderWidth = DisplayExtensions.DipToPx(1);
        public double BorderWidth
        {
            get => b_BorderWidth;
            set => SetLayoutField(ref b_BorderWidth, value);
        }

        public double DipBorderWidth
        {
            get => DisplayExtensions.PxToDip(b_BorderWidth);
            set => BorderWidth = DisplayExtensions.DipToPx(value);
        }

        Color b_BorderColor = Color.Black;
        public Color BorderColor
        {
            get => b_BorderColor;
            set => SetRedrawField(ref b_BorderColor, value);
        }

        double b_cornerRadius = DisplayExtensions.DipToPx(5);
        public double CornerRadius
        {
            get => b_cornerRadius;
            set => SetRedrawField(ref b_cornerRadius, value);
        }

        public double DipCornerRadius
        {
            get => DisplayExtensions.PxToDip(b_cornerRadius);
            set => CornerRadius = DisplayExtensions.DipToPx(value);
        }

        Color b_BackgroundColor = Color.Gray;
        public Color BackgroundColor
        {
            get => b_BackgroundColor;
            set => SetRedrawField(ref b_BackgroundColor, value);
        }



        #region FrameworkElement
        public double b_dipWidth = -1;
        public double DipWidth
        {
            get => b_dipWidth;
            set
            {
                if (SetField(ref b_dipWidth, value))
                    UpdateLayoutParams();
            }
        }

        public double b_dipHeight = -1;
        public double DipHeight
        {
            get => b_dipHeight;
            set
            {
                if (SetField(ref b_dipHeight, value))
                    UpdateLayoutParams();
            }
        }

        Alignment b_HorizontalAlignment = Alignment.Center;
        public Alignment HorizontalAlignment
        {
            get => b_HorizontalAlignment;
            set
            {
                if (SetField(ref b_HorizontalAlignment, value))
                    UpdateLayoutParams();
            }
        }

        Alignment b_VerticalAlignment = Alignment.Center;
        public Alignment VerticalAlignment
        {
            get => b_VerticalAlignment;
            set
            {
                if (SetField(ref b_VerticalAlignment, value))
                    UpdateLayoutParams();
            }
        }

        Thickness b_Margin = 0;
        public Thickness Margin
        {
            get => b_Margin;
            set => SetLayoutField(ref b_Margin, value);
        }

        public Thickness DipMargin
        {
            get => DisplayExtensions.PxToDip(b_Margin);
            set => Margin = DisplayExtensions.DipToPx(value);
        }

        double b_MinWidth = DisplayExtensions.DipToPx(50);
        public double MinWidth
        {
            get => b_MinWidth;
            set
            {
                if (SetField(ref b_MinWidth, value))
                    UpdateMinWidth();
            }
        }

        public double DipMinWidth
        {
            get => DisplayExtensions.PxToDip(b_MinWidth);
            set => MinWidth = DisplayExtensions.DipToPx(value);
        }

        double b_MinHeight = DisplayExtensions.DipToPx(50);
        public double MinHeight
        {
            get => b_MinHeight;
            set
            {
                if (SetField(ref b_MinHeight, value))
                    UpdateMinHeight();
            }
        }

        public double DipMinHeight
        {
            get => DisplayExtensions.PxToDip(b_MinHeight);
            set => MinHeight = DisplayExtensions.DipToPx(value);
        }

        double b_MaxWidth = DisplayExtensions.Width;
        public double MaxWidth
        {
            get => b_MaxWidth;
            set
            {
                if (SetField(ref b_MaxWidth, value) && hasDrawn && m_ActualWidthSet && ActualWidth > MaxWidth)
                    RequestLayout();
            }
        }

        public double DipMaxWidth
        {
            get => DisplayExtensions.PxToDip(b_MaxWidth);
            set => MaxWidth = DisplayExtensions.DipToPx(value);
        }

        double b_MaxHeight = DisplayExtensions.Height;
        public double MaxHeight
        {
            get => b_MaxHeight;
            set
            {
                if (SetField(ref b_MaxHeight, value) && hasDrawn && m_ActualHeightSet && ActualHeight > MaxHeight)
                    RequestLayout();
            }
        }

        public double DipMaxHeight
        {
            get => DisplayExtensions.PxToDip(b_MaxHeight);
            set => MaxHeight = DisplayExtensions.DipToPx(value);
        }

        double b_ActualWidth;
        bool m_ActualWidthSet;
        public double ActualWidth
        {
            get => b_ActualWidth;
            private set
            {
                if (SetField(ref b_ActualWidth, value))
                    m_ActualWidthSet = true;
            }
        }

        public double DipActualWidth => DisplayExtensions.PxToDip(b_ActualWidth);

        double b_ActualHeight;
        bool m_ActualHeightSet;
        public double ActualHeight
        {
            get => b_ActualHeight;
            private set
            {
                if (SetField(ref b_ActualHeight, value))
                    m_ActualHeightSet = true;
            }
        }

        public double DipActualHeight => DisplayExtensions.PxToDip(b_ActualHeight);



        #endregion

        #endregion

        #endregion


        #region Fields
        readonly Paint m_paint = new Paint(PaintFlags.AntiAlias);
        #endregion


        #region Events
        public event EventHandler<Size> SizeChanged;
        #endregion


        #region Constructors

        public BubbleBorder() : base(P42.Utils.Droid.Settings.Context) { Init(); }

        public BubbleBorder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { Init(); }

        public BubbleBorder(Context context) : base(context) { Init(); }

        public BubbleBorder(Context context, IAttributeSet attrs) : base(context, attrs) { Init(); }

        public BubbleBorder(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(); }

        public BubbleBorder(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { Init(); }

        void Init()
        {
            SetWillNotDraw(false);
            UpdateLayoutParams();
        }
        #endregion



        #region Measure / Layout / Draw

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            System.Diagnostics.Debug.WriteLine($"BubbleBorder.OnMeasure(({MeasureSpec.GetSize(widthMeasureSpec)},{MeasureSpec.GetMode(widthMeasureSpec)}),({MeasureSpec.GetSize(heightMeasureSpec)},{MeasureSpec.GetMode(heightMeasureSpec)})");
            if (Content is null)
            {
                SetMeasuredDimension((int)MinWidth, (int)MinHeight);
                return;
            }

            var availableWidth = MeasureSpec.GetSize(widthMeasureSpec);
            var availableHeight = MeasureSpec.GetSize(heightMeasureSpec);
            var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
            var vtMode = MeasureSpec.GetMode(heightMeasureSpec);

            /*
            if (hzMode == Android.Views.MeasureSpecMode.Exactly && vtMode == Android.Views.MeasureSpecMode.Exactly)
            {

                SetMeasuredDimension(availableWidth, availableHeight);
                return;
            }
            */
            

            var hzInset = (int)(2 * BorderWidth + Padding.Horizontal + (PointerDirection.IsHorizontal() ? PointerLength : 0) + 0.5);
            var vtInset = (int)(2 * BorderWidth + Padding.Vertical + (PointerDirection.IsVertical() ? PointerLength : 0) + 0.5);
            var contentWidthAvailable = availableWidth - hzInset;
            var contentHeightAvailable = availableHeight - vtInset;

            var contentWidthSpec = MeasureSpec.MakeMeasureSpec(contentWidthAvailable, hzMode);
            var contentHeightSpec = MeasureSpec.MakeMeasureSpec(contentHeightAvailable, vtMode);

            Content.Measure(contentWidthSpec, contentHeightSpec);
            SetMeasuredDimension(Content.MeasuredWidth + hzInset, Content.MeasuredHeight + vtInset);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            System.Diagnostics.Debug.WriteLine($"BubbleBorder.OnLayout({changed}, {l}, {t}, {r}, {b}");
            if (Content != null)
            {
                l += (int)(BorderWidth + Padding.Left + (PointerDirection == PointerDirection.Right ? PointerLength : 0) + 0.5);
                t += (int)(BorderWidth + Padding.Top + (PointerDirection == PointerDirection.Up ? PointerLength : 0) + 0.5);
                r -= (int)(BorderWidth + Padding.Right + (PointerDirection == PointerDirection.Left ? PointerLength : 0) + 0.5);
                b -= (int)(BorderWidth + Padding.Bottom + (PointerDirection == PointerDirection.Down ? PointerLength : 0) + 0.5);
                Content.Layout(l, r, t, b);
            }

        }

        protected override void OnDraw(Canvas canvas)
        {
            System.Diagnostics.Debug.WriteLine($"BubbleBorder.OnDraw({canvas.Width}, {canvas.Height})");
            Path p = GeneratePath(new Size(canvas.Width, canvas.Height));
            m_paint.Color = BackgroundColor;
            m_paint.SetStyle(Paint.Style.Fill);
            canvas.DrawPath(p, m_paint);

            m_paint.StrokeWidth = (float)BorderWidth;
            m_paint.Color = BorderColor;
            m_paint.SetStyle(Paint.Style.Stroke);
            canvas.DrawPath(p, m_paint);

            base.OnDraw(canvas);
            hasDrawn = true;
        }
        #endregion


        #region Path Generation
        Path GeneratePath(Size canvasSize)
        {
            var width = (float)canvasSize.Width;
            var height = (float)canvasSize.Height;

            if (width < 1 || height < 1)
                return new Path();

            var strokeColor = BorderColor;
            var borderWidth = 0.0f;
            if (strokeColor.A > 0 && BorderWidth > 0)
                borderWidth = (float)BorderWidth;

            var pointerLength = PointerDirection == PointerDirection.None ? 0 : (float)PointerLength;

            var left = 0.0f + borderWidth / 2;
            var right = (float)(width - Margin.Left - Margin.Right - borderWidth / 2);
            var top = 0.0f + borderWidth / 2;
            var bottom = (float)(height - Margin.Top - Margin.Bottom - borderWidth / 2);

            width -= (PointerDirection.IsHorizontal() ? pointerLength : 0);
            height -= (PointerDirection.IsVertical() ? pointerLength : 0);

            var cornerRadius = (float)CornerRadius;

            if (cornerRadius * 2 > width)
                cornerRadius = width / 2.0f;
            if (cornerRadius * 2 > height)
                cornerRadius = height / 2.0f;


            var filetRadius = (float)PointerCornerRadius;
            var tipRadius = (float)PointerTipRadius;

            if (filetRadius / 2.0 + tipRadius / 2.0 > pointerLength)
            {
                filetRadius = 2 * (pointerLength - tipRadius / 2.0f);
                if (filetRadius < 0)
                {
                    filetRadius = 0;
                    tipRadius = 2 * pointerLength;
                }
            }

            if (pointerLength - filetRadius / 2.0 < tipRadius / 2.0)
                tipRadius = 2 * (pointerLength - filetRadius / 2.0f);

            var result = new Path();
            var pointerPosition = (float)PointerAxialPosition;
            if (pointerPosition <= 1.0)
                pointerPosition = (float)(PointerDirection == PointerDirection.Down || PointerDirection == PointerDirection.Up
                    ? left + (right - left) * pointerPosition
                    : top + (bottom - top) * pointerPosition);


            const float sqrt3 = (float)1.732050807568877;
            const float sqrt3d2 = (float)0.86602540378444;

            var tipCornerHalfWidth = tipRadius * sqrt3d2;
            var pointerToCornerIntercept = (float)Math.Sqrt((2 * cornerRadius * Math.Sin(Math.PI / 12.0)) * (2 * cornerRadius * Math.Sin(Math.PI / 12.0)) - (cornerRadius * cornerRadius / 4.0));

            var pointerAtLimitSansTipHalfWidth = (float)(pointerToCornerIntercept + cornerRadius / (2.0 * sqrt3) + (pointerLength - tipRadius / 2.0) / sqrt3);
            var pointerAtLimitHalfWidth = pointerAtLimitSansTipHalfWidth + tipRadius * sqrt3d2;

            var pointerSansFiletHalfWidth = (float)(tipCornerHalfWidth + (pointerLength - filetRadius / 2.0 - tipRadius / 2.0) / sqrt3);
            var pointerFiletWidth = filetRadius * sqrt3d2;
            var pointerAndFiletHalfWidth = pointerSansFiletHalfWidth + pointerFiletWidth;

            var dir = 1;

            if (pointerLength <= 1)
            {
                result.MoveTo(right - cornerRadius, top);
                result.ArcTo(new RectF(right - 2 * cornerRadius, top, right, top + 2 * cornerRadius), 270, 90, false);
                result.ArcTo(new RectF(right - 2 * cornerRadius, bottom - 2 * cornerRadius, right, bottom), 0, 90, false);
                result.ArcTo(new RectF(left, bottom - 2 * cornerRadius, left + 2 * cornerRadius, bottom), 90, 90, false);
                result.ArcTo(new RectF(left, top, left + 2 * cornerRadius, top + 2 * cornerRadius), 180, 90, false);
                result.Close();
            }
            else if (PointerDirection.IsHorizontal())
            {
                var start = left;
                var end = right;
                if (PointerDirection == PointerDirection.Right)
                {
                    dir = -1;
                    start = right;
                    end = left;
                }
                var baseX = start + dir * pointerLength;

                var tipY = Math.Min(pointerPosition, (float)(bottom - PointerTipRadius * sqrt3d2));
                tipY = Math.Max(tipY, top + (float)PointerTipRadius * sqrt3d2);
                if (height <= 2 * pointerAtLimitHalfWidth)
                    tipY = (float)((top + bottom) / 2.0);
                result.MoveTo(start + dir * (pointerLength + cornerRadius), top);
                result.ArcWithCenterTo(
                    end - dir * cornerRadius,
                    top + cornerRadius,
                    cornerRadius, 270, dir * 90);
                result.ArcWithCenterTo(
                    end - dir * cornerRadius,
                    bottom - cornerRadius,
                    cornerRadius, 90 - dir * 90, dir * 90);
                result.LineTo(start + dir * (pointerLength + cornerRadius), bottom);

                // bottom half
                if (tipY > bottom - pointerAndFiletHalfWidth - cornerRadius)
                {
                    result.LineTo(start + dir * (pointerLength + cornerRadius), bottom);
                    var endRatio = (float)((height - tipY) / (pointerAndFiletHalfWidth + cornerRadius));
                    result.CubicTo(
                        start + dir * (pointerLength + cornerRadius - endRatio * 4 * cornerRadius / 3.0f),
                        bottom,
                        start + dir * (pointerLength - filetRadius / 2.0f + filetRadius * sqrt3d2),
                        Math.Min(tipY + pointerSansFiletHalfWidth + filetRadius / 2.0f, bottom),
                        start + dir * (pointerLength - filetRadius / 2.0f),
                        Math.Min(tipY + pointerSansFiletHalfWidth, bottom));
                }
                else
                {
                    //result.ArcTo(baseX, bottom, baseX, top, cornerRadius);
                    result.ArcWithCenterTo(
                        start + dir * (pointerLength + cornerRadius),
                        bottom - cornerRadius,
                        cornerRadius, 90, dir * 90
                        );
                    result.ArcWithCenterTo(
                        start + dir * (pointerLength - filetRadius),
                        Math.Max(tipY + pointerAndFiletHalfWidth, top + 2 * pointerAndFiletHalfWidth),
                        filetRadius, 90 - 90 * dir, dir * -60);
                }

                //tip
                result.ArcWithCenterTo(
                    start + dir * tipRadius,
                    Math.Max(Math.Min(tipY, bottom), top),
                    tipRadius, 90 + dir * 30, dir * 2 * 60);

                // top half
                if (tipY < top + pointerAndFiletHalfWidth + cornerRadius)
                {
                    var startRatio = tipY / (pointerAndFiletHalfWidth + cornerRadius);
                    result.CubicTo(
                        start + dir * (pointerLength - filetRadius / 2.0f + filetRadius * sqrt3d2),
                        Math.Max(tipY - pointerSansFiletHalfWidth - filetRadius / 2.0f, top),
                        start + dir * (pointerLength + cornerRadius - startRatio * 4 * cornerRadius / 3.0f),
                        top,
                        start + dir * (pointerLength + cornerRadius),
                        top);
                }
                else
                {
                    result.ArcWithCenterTo(
                        start + dir * (pointerLength - filetRadius),
                        Math.Min(tipY - pointerAndFiletHalfWidth, bottom - pointerAndFiletHalfWidth * 2),
                        filetRadius, 90 - dir * 30, dir * -60);
                    result.ArcWithCenterTo(
                        start + dir * (pointerLength + cornerRadius),
                        top + cornerRadius,
                        cornerRadius, 90 + dir * 90, dir * 90);
                }

                if (dir > 0)
                {
                    //var reverse = new Path();
                    //reverse.AddPathReverse(result);
                    //return reverse;
                    result.ToggleInverseFillType();
                }
            }
            else
            {
                var start = top;
                var end = bottom;
                if (PointerDirection == PointerDirection.Down)
                {
                    dir = -1;
                    start = bottom;
                    end = top;
                }
                var tipX = Math.Min(pointerPosition, (float)(right - PointerTipRadius * sqrt3d2));     // 1
                tipX = Math.Max(tipX, left + (float)PointerTipRadius * sqrt3d2);                               // 1
                if (width <= 2 * pointerAtLimitHalfWidth)
                    tipX = (float)((left + right) / 2.0);   // 8
                result.MoveTo(left, start + dir * (pointerLength + cornerRadius));
                result.ArcWithCenterTo(
                    left + cornerRadius,
                    end - dir * cornerRadius,
                    cornerRadius, 180, dir * -90
                    );
                result.ArcWithCenterTo(
                    right - cornerRadius,
                    end - dir * cornerRadius,
                    cornerRadius, 180 - dir * 90, dir * -90);
                result.LineTo(right, start + dir * (pointerLength + cornerRadius));                             //2

                // right half
                if (tipX > right - pointerAndFiletHalfWidth - cornerRadius)
                {
                    var endRatio = (float)((right - tipX) / (pointerAndFiletHalfWidth + cornerRadius));
                    result.CubicTo(
                        right,
                        start + dir * (pointerLength + cornerRadius - endRatio * 4 * cornerRadius / 3.0f),
                        Math.Min(tipX + pointerSansFiletHalfWidth + filetRadius / 2.0f, right),
                        start + dir * (pointerLength - filetRadius / 2.0f + filetRadius * sqrt3d2),   //3
                        Math.Min(tipX + pointerSansFiletHalfWidth, right),
                        start + dir * (pointerLength - filetRadius / 2.0f)                                                 // 3
                        );
                }
                else
                {
                    result.ArcWithCenterTo(
                        right - cornerRadius,
                        start + dir * (pointerLength + cornerRadius),
                        cornerRadius, 0, dir * -90
                        );
                    result.ArcWithCenterTo(
                        Math.Max(tipX + pointerAndFiletHalfWidth, left + 2 * pointerAndFiletHalfWidth), // 5
                        start + dir * (pointerLength - filetRadius),
                        filetRadius, dir * 90, dir * 60
                        );
                }
                //tip
                result.ArcWithCenterTo(
                    Math.Max(Math.Min(tipX, right), left), // 7
                    start + dir * tipRadius,
                    tipRadius,
                    dir * -30,
                    dir * -2 * 60
                    );


                // left half
                if (tipX < left + pointerAndFiletHalfWidth + cornerRadius)  // 6
                {
                    var startRatio = tipX / (pointerAndFiletHalfWidth + cornerRadius);
                    result.CubicTo(
                            Math.Max(tipX - pointerSansFiletHalfWidth - filetRadius / 2.0f, left), //6
                            start + dir * (pointerLength - filetRadius / 2.0f + filetRadius * sqrt3d2),
                            left,
                            start + dir * (pointerLength + cornerRadius - startRatio * 4 * cornerRadius / 3.0f),
                            left,
                            start + dir * (pointerLength + cornerRadius)
                        );
                }
                else
                {
                    result.ArcWithCenterTo(
                         Math.Min(tipX - pointerAndFiletHalfWidth, right - pointerAndFiletHalfWidth * 2),  // 4
                         start + dir * (pointerLength - filetRadius),
                         filetRadius, dir * 30, dir * 60);
                    result.ArcWithCenterTo(
                        left + cornerRadius,
                        start + dir * (pointerLength + cornerRadius),
                        cornerRadius, dir * -90, dir * -90);
                }
                if (dir < 0)
                {
                    //var reverse = new Path();
                    //reverse.AddPathReverse(result);
                    //return reverse;
                    result.ToggleInverseFillType();
                }
            }
            return result;
        }
        #endregion

        #region Property Change Handlers
        void UpdateLayoutParams()
        {
            //LayoutParameters = new LayoutParams(Android.Views.ViewGroup.LayoutParams.WrapContent, Android.Views.ViewGroup.LayoutParams.WrapContent);
            
            LayoutParameters = new LayoutParams(
                    HorizontalAlignment == Alignment.Stretch
                        ? Android.Views.ViewGroup.LayoutParams.MatchParent
                        : DipWidth < 0
                            ? Android.Views.ViewGroup.LayoutParams.WrapContent
                            : DipWidth < DipMinWidth
                                ? (int)(MinWidth+0.5)
                                : DipWidth > DipMaxWidth
                                    ? (int)(MaxWidth+0.5)
                                    : (int)(DisplayExtensions.DipToPx(DipWidth)+0.5),
                    VerticalAlignment == Alignment.Start
                        ? Android.Views.ViewGroup.LayoutParams.MatchParent
                        : DipHeight < 0
                            ? Android.Views.ViewGroup.LayoutParams.WrapContent
                            : DipHeight < DipMinHeight
                                ? (int)(MinHeight+0.5)
                                : DipHeight > DipMaxHeight
                                    ? (int)(MaxHeight+0.5)
                                    : (int)(DisplayExtensions.DipToPx(DipHeight)+0.5)
                );
            if (hasDrawn)
                RequestLayout();
            
        }

        void UpdateMinWidth()
        {
            SetMinimumWidth((int)(b_MinWidth + 0.5));
            UpdateLayoutParams();
        }

        void UpdateMinHeight()
        {
            SetMinimumHeight((int)(b_MinHeight + 0.5));
            UpdateLayoutParams();
        }
        #endregion

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
