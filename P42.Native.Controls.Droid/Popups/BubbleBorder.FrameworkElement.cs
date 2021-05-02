using System;
namespace P42.Native.Controls.Droid
{
    public partial class BubbleBorder
    {
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

        #region Events
        public event EventHandler<Size> SizeChanged;
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

    }
}
