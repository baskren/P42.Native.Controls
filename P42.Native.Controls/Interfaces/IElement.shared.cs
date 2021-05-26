using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace P42.Native.Controls
{
    [SmartTraitsDefs.TraitInterface]
    public interface IElement : INotifiable
    {
        #region FrameworkElement
        
        int RequestedWidth { get; set;}
        double DipRequestedWidth { get; set; }

        int RequestedHeight { get; set;}
        double DipRequestedHeight { get; set; }

        public Alignment HorizontalAlignment { get; set;}

        public Alignment VerticalAlignment { get; set;}

        ThicknessI Margin { get; set;}
        Thickness DipMargin { get; set; }

        int MinWidth { get; set; }
        double DipMinWidth { get; set;}

        int MinHeight { get; set; }
        double DipMinHeight { get; set;}

        int MaxWidth { get; set; }
        double DipMaxWidth { get; set;}

        int MaxHeight { get; set;}
        double DipMaxHeight { get; set; }

        int ActualWidth { get; }
        double DipActualWidth { get; }

        int ActualHeight { get; }
        double DipActualHeight { get; }

        SizeI ActualSize { get; }
        Size DipActualSize { get; }

        object DataContext { get; set; }

        double Opacity { get; set; }

        bool IsVisible { get; set; }
        #endregion


        #region Events
        event EventHandler<Size> SizeChanged;
        #endregion


        #region Methods
        void OnDataContextChanged() { }

        Task WaitForDrawComplete();
        #endregion


    }
}
