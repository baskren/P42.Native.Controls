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
    public partial class ContentAndDetailPresenter
    {
        #region Control
        internal protected Thickness b_Padding = DisplayExtensions.DipToPx(10);
        public virtual Thickness Padding
        {
            get => b_Padding;
            set
            {
                if (SetField(ref b_Padding, value))
                    SetPadding((int)(b_Padding.Left + 0.5), (int)(b_Padding.Top + 0.5), (int)(b_Padding.Right + 0.5), (int)(b_Padding.Bottom +0.5));
            }
        }

        public virtual Thickness DipPadding
        {
            get => DisplayExtensions.PxToDip(b_Padding);
            set => Padding = DisplayExtensions.DipToPx(value);
        }

        internal protected double b_BorderWidth = DisplayExtensions.DipToPx(1);
        public virtual double BorderWidth
        {
            get => b_BorderWidth;
            set => SetLayoutField(ref b_BorderWidth, value);
        }

        public virtual double DipBorderWidth
        {
            get => DisplayExtensions.PxToDip(b_BorderWidth);
            set => BorderWidth = DisplayExtensions.DipToPx(value);
        }

        internal protected Color b_BorderColor = Color.Black;
        public virtual Color BorderColor
        {
            get => b_BorderColor;
            set => SetRedrawField(ref b_BorderColor, value);
        }

        internal protected double b_CornerRadius = DisplayExtensions.DipToPx(5);
        public virtual double CornerRadius
        {
            get => b_CornerRadius;
            set => SetRedrawField(ref b_CornerRadius, value);
        }

        public virtual double DipCornerRadius
        {
            get => DisplayExtensions.PxToDip(b_CornerRadius);
            set => CornerRadius = DisplayExtensions.DipToPx(value);
        }

        internal protected Color b_BackgroundColor = Color.Gray.WithAlpha(0.5);
        public virtual Color BackgroundColor
        {
            get => b_BackgroundColor;
            set => SetRedrawField(ref b_BackgroundColor, value);
        }



        #endregion



    }
}
