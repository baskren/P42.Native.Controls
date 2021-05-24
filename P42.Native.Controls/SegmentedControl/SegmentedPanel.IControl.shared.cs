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
    public partial class SegmentedPanel : IControl
    {
        #region Control
        internal protected ThicknessI b_Padding = (ThicknessI)DisplayExtensions.DipToPx(10);
        public virtual ThicknessI Padding
        {
            get => b_Padding;
            set => ((INotifiable)this).SetField(ref b_Padding, value);
        }


        internal protected double b_BorderWidth = DisplayExtensions.DipToPxD(1);
        public virtual double BorderWidth
        {
            get => b_BorderWidth;
            set => ((INotifiable)this).SetLayoutField(ref b_BorderWidth, value);
        }


        internal protected Color b_BorderColor = Color.Black;
        public virtual Color BorderColor
        {
            get => b_BorderColor;
            set => ((INotifiable)this).SetRedrawField(ref b_BorderColor, value);
        }

        internal protected double b_CornerRadius = DisplayExtensions.DipToPxD(5);
        public virtual double CornerRadius
        {
            get => b_CornerRadius;
            set => ((INotifiable)this).SetRedrawField(ref b_CornerRadius, value);
        }


        internal protected Color b_BackgroundColor = Color.Gray.WithAlpha(0.5);
        public virtual Color BackgroundColor
        {
            get => b_BackgroundColor;
            set => ((INotifiable)this).SetRedrawField(ref b_BackgroundColor, value);
        }



        #endregion



    }
}
