using System;

namespace P42.Native.Controls
{
    public interface IFrameworkElement : INotifiable
    {
        #region FrameworkElement
        int RequestedWidth { get; set;}
        public double DipRequestedWidth
        {
            get => DisplayExtensions.PxToDip(RequestedWidth);
            set => RequestedWidth = value.DipToPx();
        }

        int RequestedHeight { get; set;}
        public double DipRequestedHeight
        {
            get => DisplayExtensions.PxToDip(RequestedHeight);
            set => RequestedHeight = value.DipToPx();
        }

        public Alignment HorizontalAlignment { get; set;}

        public Alignment VerticalAlignment { get; set;}


        ThicknessI Margin { get; set;}
        public virtual Thickness DipMargin
        {
            get => DisplayExtensions.PxToDip(Margin);
            set => Margin = value.DipToPx();
        }


        int MinWidth { get; set;}
        public virtual double DipMinWidth
        {
            get => DisplayExtensions.PxToDip(MinWidth);
            set => MinWidth = value.DipToPx();
        }


        int MinHeight { get; set;}
        public virtual double DipMinHeight
        {
            get => DisplayExtensions.PxToDip(MinHeight);
            set => MinHeight = value.DipToPx();
        }


        int MaxWidth { get; set;}
        public virtual double DipMaxWidth
        {
            get => DisplayExtensions.PxToDip(MaxWidth);
            set => MaxWidth = value.DipToPx();
        }

        int MaxHeight { get; set;}
        public virtual double DipMaxHeight
        {
            get => DisplayExtensions.PxToDip(MaxHeight);
            set => MaxHeight = value.DipToPx();
        }

        int ActualWidth { get; }
        public double DipActualWidth => ActualWidth.PxToDip();

        int ActualHeight { get; }
        public virtual double DipActualHeight => ActualHeight.PxToDip();

        SizeI ActualSize { get; }
        public virtual Size DipActualSize => ActualSize.PxToDip();

        object DataContext { get; set; }
        #endregion


        #region Events
        public event EventHandler<Size> SizeChanged;
        #endregion


        #region Methods
        public void OnDataContextChanged()
        {
        }
        #endregion
    }
}
