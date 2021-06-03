﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Newtonsoft.Json;

namespace P42.Native.Controls
{
    public partial class BubbleBorder : Android.Views.ViewGroup
    {


        #region Fields
        readonly Paint m_paint = new Paint(PaintFlags.AntiAlias);
        #endregion


        #region Constructors
        public BubbleBorder() : this(global::P42.Utils.Droid.Settings.Context) { }

        public BubbleBorder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { Init(); }

        public BubbleBorder(Context context) : base(context) { Init(); }

        public BubbleBorder(Context context, IAttributeSet attrs) : base(context, attrs) { Init(); }

        public BubbleBorder(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(); }

        public BubbleBorder(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { Init(); }

        void Init()
        {
            NtvBaseView = this;
            SetWillNotDraw(false);
            NtvUpdateLayoutParams();
        }
        #endregion


        #region Android Measure / Layout / Draw

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            System.Diagnostics.Debug.WriteLine($"BubbleBorder.OnMeasure(({MeasureSpec.GetSize(widthMeasureSpec)},{MeasureSpec.GetMode(widthMeasureSpec)}),({MeasureSpec.GetSize(heightMeasureSpec)},{MeasureSpec.GetMode(heightMeasureSpec)})");
            if (DipContent is null)
            {
                SetMeasuredDimension((int)NtvMinWidth, (int)NtvMinHeight);
                return;
            }

            var availableWidth = MeasureSpec.GetSize(widthMeasureSpec);
            var availableHeight = MeasureSpec.GetSize(heightMeasureSpec);
            var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
            var vtMode = MeasureSpec.GetMode(heightMeasureSpec);

            
            if (hzMode == Android.Views.MeasureSpecMode.Exactly && vtMode == Android.Views.MeasureSpecMode.Exactly)
            {

                SetMeasuredDimension(availableWidth, availableHeight);
                return;
            }
            
            

            var hzInset = (int)(NtvMargin.Horizontal + 2 * NtvBorderWidth + NtvPadding.Horizontal + (DipPointerDirection.IsHorizontal() ? NtvPointerLength : 0) + 0.5);
            var vtInset = (int)(NtvMargin.Vertical + 2 * NtvBorderWidth + NtvPadding.Vertical + (DipPointerDirection.IsVertical() ? NtvPointerLength : 0) + 0.5);
            
            if (HasShadow)
            {
                hzInset += 2 * NtvShadowRadius;
                vtInset += 2 * NtvShadowRadius;
            }

            var contentWidthAvailable = availableWidth - hzInset;
            var contentHeightAvailable = availableHeight - vtInset;

            var contentWidthSpec = MeasureSpec.MakeMeasureSpec(contentWidthAvailable, hzMode);
            var contentHeightSpec = MeasureSpec.MakeMeasureSpec(contentHeightAvailable, vtMode);

            DipContent.Measure(contentWidthSpec, contentHeightSpec);
            //MeasureChildren(contentWidthSpec, contentHeightSpec);

            /*
            var mWidth = Content.MeasuredWidth + hzInset;
            if (Content.LayoutParameters.Width.GetTypeCode() == TypeCo)
            var mHeigth = Content.MeasuredHeight + vtInset;
            */

            SetMeasuredDimension(DipContent.MeasuredWidth + hzInset, DipContent.MeasuredHeight + vtInset);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            System.Diagnostics.Debug.WriteLine($"BubbleBorder.OnLayout({changed}, {l}, {t}, {r}, {b})  w:{r-l} h:{b-t}");
            if (DipContent != null)
            {
                var borderWidth = 0.0;
                if (DipBorderColor.A > 0 && NtvBorderWidth > 0)
                    borderWidth = (float)NtvBorderWidth;
                System.Diagnostics.Debug.WriteLine($"BubbleBorder.OnLayout: Margin:{NtvMargin} Padding:{NtvPadding} BorderWidth:{NtvBorderWidth} PointerDir:{DipPointerDirection} PointerLen:{NtvPointerLength}");
                var cl = l + NtvMargin.Left + borderWidth + NtvPadding.Left + (DipPointerDirection == PointerDirection.Left ? NtvPointerLength : 0);
                var ct = t + NtvMargin.Top + borderWidth + NtvPadding.Top + (DipPointerDirection == PointerDirection.Up ? NtvPointerLength : 0);
                var cr = r - (NtvMargin.Right + borderWidth + NtvPadding.Right + (DipPointerDirection == PointerDirection.Right ? NtvPointerLength : 0));
                var cb = b - (NtvMargin.Bottom + borderWidth + NtvPadding.Bottom + (DipPointerDirection == PointerDirection.Down ? NtvPointerLength : 0));

                if (HasShadow)
                {
                    cl += NtvShadowRadius - NtvShadowShift.X;
                    ct += NtvShadowRadius - NtvShadowShift.Y;
                    cr -= NtvShadowRadius + NtvShadowShift.X;
                    cb -= NtvShadowRadius + NtvShadowShift.Y;
                }
                System.Diagnostics.Debug.WriteLine($"BubbleBorder.OnLayout Content.Layout({(int)(cl + 0.5)}, {(int)(ct + 0.5)}, {(int)(cr + 0.5)}, {(int)(cb + 0.5)}) w:{cr - cl} h:{cb - ct}");

                    
                var contentWidthSpec = MeasureSpec.MakeMeasureSpec((int)(cr - cl + 0.5), Android.Views.MeasureSpecMode.AtMost);
                var contentHeightSpec = MeasureSpec.MakeMeasureSpec((int)(cb - ct + 0.5), Android.Views.MeasureSpecMode.AtMost);
                DipContent.Measure(contentWidthSpec, contentHeightSpec);
                if (DipVerticalAlignment == Alignment.End)
                {
                    ct = cb - DipContent.MeasuredHeight;
                }
                else if (DipVerticalAlignment == Alignment.Start)
                {
                    cb = ct + DipContent.MeasuredHeight;
                }
                else if (DipVerticalAlignment == Alignment.Center)
                {
                    ct += +(cb - ct) / 2 - (DipContent.MeasuredHeight / 2.0);
                    cb = ct + DipContent.MeasuredHeight;
                }

                if (DipHorizontalAlignment == Alignment.End)
                {
                    cl = cr - DipContent.MeasuredWidth;
                }
                else if (DipHorizontalAlignment == Alignment.Start)
                {
                    cr = cl + DipContent.MeasuredWidth;
                }
                else if (DipHorizontalAlignment == Alignment.Center)
                {
                    cl += (cr - cl) / 2 - (DipContent.MeasuredWidth / 2.0);
                    cr = cl + DipContent.MeasuredWidth;
                }

                System.Diagnostics.Debug.WriteLine($"BubbleBorder.OnLayout Content.Layout({(int)(cl + 0.5)}, {(int)(ct + 0.5)}, {(int)(cr+0.5)}, {(int)(cb+0.5)}) w:{cr-cl} h:{cb-ct}");
                DipContent.Layout((int)(cl+0.5), (int)(ct+0.5), (int)(cr+0.5), (int)(cb+0.5));
            }

        }

        protected override void OnDraw(Canvas canvas)
        {
            Path p = GeneratePath(new Size(canvas.Width, canvas.Height));

            if (HasShadow)
            {
                // Create paint for shadow
                m_paint.Color = Color.Black.WithAlpha(0.5);
                m_paint.SetMaskFilter(new BlurMaskFilter(
                    NtvShadowRadius / 2,
                    BlurMaskFilter.Blur.Normal));
                Matrix translateMatrix = new Matrix();
                translateMatrix.SetTranslate(NtvShadowShift.X, NtvShadowShift.Y);
                var shadowPath = new Path(p);
                shadowPath.Transform(translateMatrix);
                canvas.DrawPath(shadowPath, m_paint);
            }

            m_paint.SetMaskFilter(null);
            System.Diagnostics.Debug.WriteLine($"BubbleBorder.OnDraw({canvas.Width}, {canvas.Height})");
            m_paint.Color = DipBackgroundColor;
            m_paint.SetStyle(Paint.Style.Fill);
            canvas.DrawPath(p, m_paint);

            if (NtvBorderWidth > 0 && DipBorderColor.A > 0)
            {
                m_paint.StrokeWidth = (float)NtvBorderWidth;
                m_paint.Color = DipBorderColor;
                m_paint.SetStyle(Paint.Style.Stroke);
                canvas.DrawPath(p, m_paint);
            }

            base.OnDraw(canvas);

            DipHasDrawn = true;
            NtvActualSize = new SizeI(canvas.Width, canvas.Height);
        }

        #endregion


        #region Path Generation
        Path GeneratePath(Size canvasSize)
        {
            var width = (float)canvasSize.Width;
            var height = (float)canvasSize.Height;

            if (width < 1 || height < 1)
                return new Path();

            var strokeColor = DipBorderColor;
            var borderWidth = 0.0f;
            if (strokeColor.A > 0 && NtvBorderWidth > 0)
                borderWidth = (float)NtvBorderWidth;

            var pointerLength = DipPointerDirection == PointerDirection.None ? 0 : (float)NtvPointerLength;

            var left = (float)(NtvMargin.Left + borderWidth / 2);
            var right = (float)(width  - NtvMargin.Right - borderWidth / 2);
            var top = (float)(NtvMargin.Top + borderWidth / 2);
            var bottom = (float)(height - NtvMargin.Bottom - borderWidth / 2);

            if (HasShadow)
            {
                left += NtvShadowRadius;
                top += NtvShadowRadius;
                right -= NtvShadowRadius;
                bottom -= NtvShadowRadius;
                width = right - left;
                height = bottom - top;
            }

            width -= (DipPointerDirection.IsHorizontal() ? pointerLength : 0);
            height -= (DipPointerDirection.IsVertical() ? pointerLength : 0);

            var cornerRadius = (float)NtvCornerRadius;

            if (cornerRadius * 2 > width)
                cornerRadius = width / 2.0f;
            if (cornerRadius * 2 > height)
                cornerRadius = height / 2.0f;


            var filetRadius = (float)NtvPointerCornerRadius;
            var tipRadius = (float)NtvPointerTipRadius;

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
            var pointerPosition = (float)NtvPointerAxialPosition;
            if (pointerPosition <= 1.0)
                pointerPosition = (float)(DipPointerDirection == PointerDirection.Down || DipPointerDirection == PointerDirection.Up
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
            else if (DipPointerDirection.IsHorizontal())
            {
                var start = left;
                var end = right;
                if (DipPointerDirection == PointerDirection.Right)
                {
                    dir = -1;
                    start = right;
                    end = left;
                }
                var baseX = start + dir * pointerLength;

                var tipY = Math.Min(pointerPosition, (float)(bottom - NtvPointerTipRadius * sqrt3d2));
                tipY = Math.Max(tipY, top + (float)NtvPointerTipRadius * sqrt3d2);
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
                    //result.ToggleInverseFillType();
                }
            }
            else
            {
                var start = top;
                var end = bottom;
                if (DipPointerDirection == PointerDirection.Down)
                {
                    dir = -1;
                    start = bottom;
                    end = top;
                }
                var tipX = Math.Min(pointerPosition, (float)(right - NtvPointerTipRadius * sqrt3d2));     // 1
                tipX = Math.Max(tipX, left + (float)NtvPointerTipRadius * sqrt3d2);                               // 1
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
                    //result.ToggleInverseFillType();
                }
            }
            return result;
        }
        #endregion




        #region INotifiable

        #region Methods
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(DipContent) && DipContent != null)
                RemoveView(b_DipContent);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(DipContent) && DipContent != null)
                AddView(b_DipContent);
        }

        #endregion

        #endregion


    }
}
