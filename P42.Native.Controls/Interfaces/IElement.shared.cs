using System;
using System.Threading.Tasks;

namespace P42.Native.Controls
{
    public interface IElement : INotifiable
    {
        #region FrameworkElement

        public int NtvRequestedWidth { get; set;}
        public double DipRequestedWidth { get; set; }

        public int NtvRequestedHeight { get; set;}
        public double DipRequestedHeight { get; set; }

        public P42.Native.Controls.Alignment DipHorizontalAlignment { get; set;}

        public P42.Native.Controls.Alignment DipVerticalAlignment { get; set;}

        public ThicknessI NtvMargin { get; set;}
        public Thickness DipMargin { get; set; }

        public int NtvMinWidth { get; set; }
        public double DipMinWidth { get; set;}

        public int NtvMinHeight { get; set; }
        public double DipMinHeight { get; set;}

        public int NtvMaxWidth { get; set; }
        public double DipMaxWidth { get; set;}

        public int NtvMaxHeight { get; set;}
        public double DipMaxHeight { get; set; }

        public int NtvActualWidth { get; }
        public double DipActualWidth { get; }

        public int NtvActualHeight { get; }
        public double DipActualHeight { get; }

        public SizeI NtvActualSize { get; }
        public Size DipActualSize { get; }

        public object DipDataContext { get; set; }

        public double DipOpacity { get; set; }

        public bool DipIsVisible { get; set; }
        #endregion


        #region Events
        public event EventHandler<Size> DipSizeChanged;
        #endregion


        #region Methods
        public void DipOnDataContextChanged() { }

        #endregion


#if __ANDROID__
        Android.Views.View NtvBaseView { get; }

        public void DipRedrawElement()
            => NtvBaseView.PostInvalidate();

        public void DipRelayoutElement()
            => NtvBaseView.RequestLayout();
#else
#endif
    }
}
