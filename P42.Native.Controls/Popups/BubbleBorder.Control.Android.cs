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

namespace P42.Native.Controls 
{
    public partial class BubbleBorder : IControl
    {
        #region Defaults
        public static ThicknessI DefaultPadding = (ThicknessI)DisplayExtensions.DipToPx(10);
        public static double DefaultBorderWidth = DisplayExtensions.DipToPxD(1);
        public static Color DefaultBorderColor = Color.Black;
        public static double DefaultCornerRadius = DisplayExtensions.DipToPxD(5);
        public static Color DefaultBackgroundColor = Color.Gray;
        #endregion

        #region Control
        internal protected ThicknessI b_Padding = DefaultPadding;
        public virtual ThicknessI Padding
        {
            get => b_Padding;
            set
            {
                if (((INotifiable)this).SetField(ref b_Padding, value))
                    SetPadding((int)(b_Padding.Left + 0.5), (int)(b_Padding.Top + 0.5), (int)(b_Padding.Right + 0.5), (int)(b_Padding.Bottom +0.5));
            }
        }


        internal protected double b_BorderWidth = DefaultBorderWidth;
        public virtual double BorderWidth
        {
            get => b_BorderWidth;
            set => ((INotifiable)this).SetLayoutField(ref b_BorderWidth, value);
        }


        internal protected Color b_BorderColor = DefaultBorderColor;
        public virtual Color BorderColor
        {
            get => b_BorderColor;
            set => ((INotifiable)this).SetRedrawField(ref b_BorderColor, value);
        }

        internal protected double b_CornerRadius = DefaultCornerRadius;
        public virtual double CornerRadius
        {
            get => b_CornerRadius;
            set => ((INotifiable)this).SetRedrawField(ref b_CornerRadius, value);
        }


        internal protected Color b_BackgroundColor = DefaultBackgroundColor;
        public virtual Color BackgroundColor
        {
            get => b_BackgroundColor;
            set => ((INotifiable)this).SetRedrawField(ref b_BackgroundColor, value);
        }


        Alignment b_HorizontalContentAlignment = Alignment.Center;
        public Alignment HorizontalContentAlignment
        {
            get => b_HorizontalContentAlignment;
            set => ((INotifiable)this).SetField(ref b_HorizontalContentAlignment, value);
        }

        Alignment b_VerticalContentAlignment = Alignment.Center;
        public Alignment VerticalContentAlignment
        {
            get => b_VerticalContentAlignment;
            set => ((INotifiable)this).SetField(ref b_VerticalContentAlignment, value);
        }

        #endregion



    }
}
