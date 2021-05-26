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

        int RequestedHeight { get; set;}

        public Alignment HorizontalAlignment { get; set;}

        public Alignment VerticalAlignment { get; set;}

        ThicknessI Margin { get; set;}

        int MinWidth { get; set;}

        int MinHeight { get; set;}

        int MaxWidth { get; set;}

        int MaxHeight { get; set;}

        int ActualWidth { get; }

        int ActualHeight { get; }

        SizeI ActualSize { get; }

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
