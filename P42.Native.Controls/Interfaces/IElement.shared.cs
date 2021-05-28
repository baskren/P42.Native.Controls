using System;
using System.Threading.Tasks;
using SmartTraitsDefs;

namespace P42.Native.Controls
{
    [TraitInterface]
    public interface IElement : DINotifiable
    {
        #region FrameworkElement

        public int RequestedWidth { get; set;}
        public double DipRequestedWidth { get; set; }

        public int RequestedHeight { get; set;}
        public double DipRequestedHeight { get; set; }

        public Alignment HorizontalAlignment { get; set;}

        public Alignment VerticalAlignment { get; set;}

        public ThicknessI Margin { get; set;}
        public Thickness DipMargin { get; set; }

        public int MinWidth { get; set; }
        public double DipMinWidth { get; set;}

        public int MinHeight { get; set; }
        public double DipMinHeight { get; set;}

        public int MaxWidth { get; set; }
        public double DipMaxWidth { get; set;}

        public int MaxHeight { get; set;}
        public double DipMaxHeight { get; set; }

        public int ActualWidth { get; }
        public double DipActualWidth { get; }

        public int ActualHeight { get; }
        public double DipActualHeight { get; }

        public SizeI ActualSize { get; }
        public Size DipActualSize { get; }

        public object DataContext { get; set; }

        public double Opacity { get; set; }

        public bool IsVisible { get; set; }
        #endregion


        #region Events
        public event EventHandler<Size> SizeChanged;
        #endregion


        #region Methods
        public void OnDataContextChanged() { }

        public Task WaitForDrawComplete();

        #endregion


#if __ANDROID__
        Android.Views.View BaseView { get; }

        public void RedrawElement()
            => BaseView.PostInvalidate();

        public void RelayoutElement()
            => BaseView.RequestLayout();
#else
#endif
    }
}
