using System;
namespace P42.Native.Controls.Droid
{
    public partial class BubbleBorder
    {
        #region FrameworkElement
        public double DipRequestedWidth
        {
            get => DisplayExtensions.PxToDip(RequestedWidth);
            set => RequestedWidth = DisplayExtensions.DipToPx(value);
        }

        double b_RequestedWidth = -1;
        public double RequestedWidth
        {
            get => b_RequestedWidth;
            set => SetField(ref b_RequestedWidth, value, UpdateLayoutParams);
        }

        public double DipRequestedHeight
        {
            get => DisplayExtensions.PxToDip(RequestedHeight);
            set => RequestedHeight = DisplayExtensions.DipToPx(value);
        }

        double b_RequestedHeight = -1;
        public double RequestedHeight
        {
            get => b_RequestedHeight;
            set => SetField(ref b_RequestedHeight, value, UpdateLayoutParams);
        }

        Alignment b_HorizontalAlignment = Alignment.Center;
        public Alignment HorizontalAlignment
        {
            get => b_HorizontalAlignment;
            set => SetField(ref b_HorizontalAlignment, value, UpdateLayoutParams);
        }

        Alignment b_VerticalAlignment = Alignment.Center;
        public Alignment VerticalAlignment
        {
            get => b_VerticalAlignment;
            set => SetField(ref b_VerticalAlignment, value, UpdateLayoutParams);
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
            set => SetField(ref b_MinWidth, value, UpdateMinWidth);
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
            set => SetField(ref b_MinHeight, value, UpdateMinHeight);
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
            private set => SetField(ref b_ActualWidth, value, () => m_ActualWidthSet = true);
        }

        public double DipActualWidth => DisplayExtensions.PxToDip(b_ActualWidth);

        double b_ActualHeight;
        bool m_ActualHeightSet;
        public double ActualHeight
        {
            get => b_ActualHeight;
            private set => SetField(ref b_ActualHeight, value, () => m_ActualHeightSet = true);
        }

        public double DipActualHeight => DisplayExtensions.PxToDip(b_ActualHeight);

        #endregion


        #region Control Property Change Handlers
        protected virtual void UpdateLayoutParams()
        {
            LayoutParameters = new LayoutParams(
                    HorizontalAlignment == Alignment.Stretch
                        ? Android.Views.ViewGroup.LayoutParams.MatchParent
                        : RequestedWidth < 0
                            ? Android.Views.ViewGroup.LayoutParams.WrapContent
                            : RequestedWidth < MinWidth
                                ? (int)(MinWidth + 0.5)
                                : RequestedWidth > MaxWidth
                                    ? (int)(MaxWidth + 0.5)
                                    : (int)(RequestedWidth + 0.5),
                    VerticalAlignment == Alignment.Start
                        ? Android.Views.ViewGroup.LayoutParams.MatchParent
                        : RequestedHeight < 0
                            ? Android.Views.ViewGroup.LayoutParams.WrapContent
                            : RequestedHeight < MinHeight
                                ? (int)(MinHeight + 0.5)
                                : RequestedHeight > MaxHeight
                                    ? (int)(Height + 0.5)
                                    : (int)(RequestedHeight + 0.5)
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


        #region Events
        public event EventHandler<Size> SizeChanged;
        #endregion

    }
}
