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
    public partial class SegmentedPanel
    {
        #region FrameworkElement
        internal protected double b_DipWidth = -1;
        public virtual double DipWidth
        {
            get => b_DipWidth;
            set
            {
                if (SetField(ref b_DipWidth, value))
                    UpdateLayoutParams();
            }
        }

        internal protected double b_DipHeight = -1;
        public virtual double DipHeight
        {
            get => b_DipHeight;
            set
            {
                if (SetField(ref b_DipHeight, value))
                    UpdateLayoutParams();
            }
        }

        internal protected Alignment b_HorizontalAlignment = Alignment.Center;
        public virtual Alignment HorizontalAlignment
        {
            get => b_HorizontalAlignment;
            set
            {
                if (SetField(ref b_HorizontalAlignment, value))
                    UpdateLayoutParams();
            }
        }

        internal protected Alignment b_VerticalAlignment = Alignment.Center;
        public virtual Alignment VerticalAlignment
        {
            get => b_VerticalAlignment;
            set
            {
                if (SetField(ref b_VerticalAlignment, value))
                    UpdateLayoutParams();
            }
        }

        internal protected Thickness b_Margin = 0;
        public virtual Thickness Margin
        {
            get => b_Margin;
            set => SetLayoutField(ref b_Margin, value);
        }

        public virtual Thickness DipMargin
        {
            get => DisplayExtensions.PxToDip(b_Margin);
            set => Margin = DisplayExtensions.DipToPx(value);
        }

        internal protected double b_MinWidth = DisplayExtensions.DipToPx(50);
        public virtual double MinWidth
        {
            get => b_MinWidth;
            set
            {
                if (SetField(ref b_MinWidth, value))
                    UpdateMinWidth();
            }
        }

        public virtual double DipMinWidth
        {
            get => DisplayExtensions.PxToDip(b_MinWidth);
            set => MinWidth = DisplayExtensions.DipToPx(value);
        }

        internal protected double b_MinHeight = DisplayExtensions.DipToPx(50);
        public virtual double MinHeight
        {
            get => b_MinHeight;
            set
            {
                if (SetField(ref b_MinHeight, value))
                    UpdateMinHeight();
            }
        }

        public virtual double DipMinHeight
        {
            get => DisplayExtensions.PxToDip(b_MinHeight);
            set => MinHeight = DisplayExtensions.DipToPx(value);
        }

        internal protected double b_MaxWidth = DisplayExtensions.Width;
        public virtual double MaxWidth
        {
            get => b_MaxWidth;
            set
            {
                if (SetField(ref b_MaxWidth, value) && hasDrawn && m_ActualWidthSet && ActualWidth > MaxWidth)
                    RequestLayout();
            }
        }

        public virtual double DipMaxWidth
        {
            get => DisplayExtensions.PxToDip(b_MaxWidth);
            set => MaxWidth = DisplayExtensions.DipToPx(value);
        }

        internal protected double b_MaxHeight = DisplayExtensions.Height;
        public virtual double MaxHeight
        {
            get => b_MaxHeight;
            set
            {
                if (SetField(ref b_MaxHeight, value) && hasDrawn && m_ActualHeightSet && ActualHeight > MaxHeight)
                    RequestLayout();
            }
        }

        public virtual double DipMaxHeight
        {
            get => DisplayExtensions.PxToDip(b_MaxHeight);
            set => MaxHeight = DisplayExtensions.DipToPx(value);
        }

        internal protected double b_ActualWidth;
        internal protected bool m_ActualWidthSet;
        public virtual double ActualWidth
        {
            get => b_ActualWidth;
            private set
            {
                if (SetField(ref b_ActualWidth, value))
                    m_ActualWidthSet = true;
            }
        }

        public double DipActualWidth => DisplayExtensions.PxToDip(b_ActualWidth);

        internal protected double b_ActualHeight;
        internal protected bool m_ActualHeightSet;
        public virtual double ActualHeight
        {
            get => b_ActualHeight;
            private set
            {
                if (SetField(ref b_ActualHeight, value))
                    m_ActualHeightSet = true;
            }
        }

        public virtual double DipActualHeight => DisplayExtensions.PxToDip(b_ActualHeight);

        #endregion


        #region Control Property Change Handlers
        protected virtual void UpdateLayoutParams()
        {
            //LayoutParameters = new LayoutParams(Android.Views.ViewGroup.LayoutParams.WrapContent, Android.Views.ViewGroup.LayoutParams.WrapContent);

            LayoutParameters = new LayoutParams(
                    HorizontalAlignment == Alignment.Stretch
                        ? Android.Views.ViewGroup.LayoutParams.MatchParent
                        : DipWidth < 0
                            ? Android.Views.ViewGroup.LayoutParams.WrapContent
                            : DipWidth < DipMinWidth
                                ? (int)(MinWidth + 0.5)
                                : DipWidth > DipMaxWidth
                                    ? (int)(MaxWidth + 0.5)
                                    : (int)(DisplayExtensions.DipToPx(DipWidth) + 0.5),
                    VerticalAlignment == Alignment.Start
                        ? Android.Views.ViewGroup.LayoutParams.MatchParent
                        : DipHeight < 0
                            ? Android.Views.ViewGroup.LayoutParams.WrapContent
                            : DipHeight < DipMinHeight
                                ? (int)(MinHeight + 0.5)
                                : DipHeight > DipMaxHeight
                                    ? (int)(MaxHeight + 0.5)
                                    : (int)(DisplayExtensions.DipToPx(DipHeight) + 0.5)
                );
            if (hasDrawn)
                RequestLayout();

        }

        protected virtual void UpdateMinWidth()
        {
            SetMinimumWidth((int)(b_MinWidth + 0.5));
            UpdateLayoutParams();
        }

        protected virtual void UpdateMinHeight()
        {
            SetMinimumHeight((int)(b_MinHeight + 0.5));
            UpdateLayoutParams();
        }
        #endregion


    }
}
