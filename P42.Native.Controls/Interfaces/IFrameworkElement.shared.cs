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
            set => RequestedWidth = DisplayExtensions.DipToPx(value);
        }

        int RequestedHeight { get; set;}
        public double DipRequestedHeight
        {
            get => DisplayExtensions.PxToDip(RequestedHeight);
            set => RequestedHeight = DisplayExtensions.DipToPx(value);
        }

        public Alignment HorizontalAlignment { get; set;}

        public Alignment VerticalAlignment { get; set;}


        ThicknessI Margin { get; set;}
        public virtual Thickness DipMargin
        {
            get => DisplayExtensions.PxToDip(Margin);
            set => Margin = DisplayExtensions.DipToPx(value);
        }


        int MinWidth { get; set;}
        public virtual double DipMinWidth
        {
            get => DisplayExtensions.PxToDip(MinWidth);
            set => MinWidth = DisplayExtensions.DipToPx(value);
        }


        int MinHeight { get; set;}
        public virtual double DipMinHeight
        {
            get => DisplayExtensions.PxToDip(MinHeight);
            set => MinHeight = DisplayExtensions.DipToPx(value);
        }



        int MaxWidth { get; set;}
        public virtual double DipMaxWidth
        {
            get => DisplayExtensions.PxToDip(MaxWidth);
            set => MaxWidth = DisplayExtensions.DipToPx(value);
        }

        int MaxHeight { get; set;}
        public virtual double DipMaxHeight
        {
            get => DisplayExtensions.PxToDip(MaxHeight);
            set => MaxHeight = DisplayExtensions.DipToPx(value);
        }

        int ActualWidth { get; }
        public double DipActualWidth => DisplayExtensions.PxToDip(ActualWidth);

        int ActualHeight { get; }
        public virtual double DipActualHeight => DisplayExtensions.PxToDip(ActualHeight);

        SizeI ActualSize { get; }
        public virtual Size DipActualSize => DisplayExtensions.PxToDip(ActualSize);
        #endregion


        #region Events
        public event EventHandler<Size> SizeChanged;
        #endregion



    }
}
