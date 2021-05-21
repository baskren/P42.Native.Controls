using System;
namespace P42.Native.Controls.Droid
{
    public partial class TargetedPopup
    {
        #region FrameworkElement

        double b_RequestedWidth = -1;
        public double RequestedWidth
        {
            get => b_RequestedWidth;
            set => SetField(ref b_RequestedWidth, value);
        }

        public double DipRequstedWidth
        {
            get => DisplayExtensions.PxToDip(b_RequestedWidth);
            set => RequestedWidth = DisplayExtensions.DipToPx(value);
        }

        double b_RequestedHeight = -1;
        public double RequestedHeight
        {
            get => b_RequestedHeight;
            set => SetField(ref b_RequestedHeight, value);
        }

        public double DipRequestedHeight
        {
            get => DisplayExtensions.PxToDip(b_RequestedHeight);
            set => RequestedHeight = DisplayExtensions.DipToPx(value);
        }

        Alignment b_HorizontalAlignment = Alignment.Center;
        public Alignment HorizontalAlignment
        {
            get => b_HorizontalAlignment;
            set => SetField(ref b_HorizontalAlignment, value);
        }

        Alignment b_VerticalAlignment = Alignment.Center;
        public Alignment VerticalAlignment
        {
            get => b_VerticalAlignment;
            set => SetField(ref b_VerticalAlignment, value);
        }

        Thickness b_Margin = 0;
        public Thickness Margin
        {
            get => b_Margin;
            set => SetField(ref b_Margin, value);
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
            set => SetField(ref b_MinWidth, value);
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
            set => SetField(ref b_MinHeight, value);
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
            set => SetField(ref b_MaxWidth, value);
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
            set => SetField(ref b_MaxHeight, value);
        }

        public double DipMaxHeight
        {
            get => DisplayExtensions.PxToDip(b_MaxHeight);
            set => MaxHeight = DisplayExtensions.DipToPx(value);
        }
        
        int b_ActualWidth;
        bool m_ActualWidthSet;
        public int ActualWidth
        {
            get => b_ActualWidth;
            private set => SetField(ref b_ActualWidth, value, () => m_ActualWidthSet = true);
        }

        public double DipActualWidth => DisplayExtensions.PxToDip(b_ActualWidth);

        int b_ActualHeight;
        bool m_ActualHeightSet;
        public int ActualHeight
        {
            get => b_ActualHeight;
            private set => SetField(ref b_ActualHeight, value, () => m_ActualHeightSet = true);
        }

        public double DipActualHeight => DisplayExtensions.PxToDip(b_ActualHeight);

        #endregion

        #region Events
        public event EventHandler<Size> SizeChanged;
        #endregion


        #region Property Change Handlers
        #endregion

    }
}
