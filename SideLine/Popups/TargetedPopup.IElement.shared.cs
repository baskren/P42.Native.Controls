using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Views;

namespace P42.Native.Controls
{
    public partial class TargetedPopup : IElement, INotifiable
    {
        #region FrameworkElement

        int b_RequestedWidth = -1;
        public int RequestedWidth
        {
            get => b_RequestedWidth;
            set => ((INotifiable)this).SetField(ref b_RequestedWidth, value);
        }

        int b_RequestedHeight = -1;
        public int RequestedHeight
        {
            get => b_RequestedHeight;
            set => ((INotifiable)this).SetField(ref b_RequestedHeight, value);
        }

        Alignment b_HorizontalAlignment = Alignment.Center;
        public Alignment HorizontalAlignment
        {
            get => b_HorizontalAlignment;
            set => ((INotifiable)this).SetField(ref b_HorizontalAlignment, value);
        }

        Alignment b_VerticalAlignment = Alignment.Center;
        public Alignment VerticalAlignment
        {
            get => b_VerticalAlignment;
            set => ((INotifiable)this).SetField(ref b_VerticalAlignment, value);
        }

        internal protected ThicknessI b_Margin = (ThicknessI)0;
        public virtual ThicknessI Margin
        {
            get => b_Margin;
            set => ((INotifiable)this).SetLayoutField(ref b_Margin, value);
        }


        internal protected int b_MinWidth = DisplayExtensions.DipToPx(50);
        public virtual int MinWidth
        {
            get => b_MinWidth;
            set => ((INotifiable)this).SetField(ref b_MinWidth, value);
        }


        internal protected int b_MinHeight = DisplayExtensions.DipToPx(50);
        public virtual int MinHeight
        {
            get => b_MinHeight;
            set => ((INotifiable)this).SetField(ref b_MinHeight, value);
        }


        internal protected int b_MaxWidth = DisplayExtensions.PxWidth();
        public virtual int MaxWidth
        {
            get => b_MaxWidth;
            set => ((INotifiable)this).SetField(ref b_MaxWidth, value);
        }

        internal protected int b_MaxHeight = DisplayExtensions.PxHeight();
        public virtual int MaxHeight
        {
            get => b_MaxHeight;
            set => ((INotifiable)this).SetField(ref b_MaxHeight, value);
        }

        internal protected int b_ActualWidth;
        internal protected bool m_ActualWidthSet;
        public virtual int ActualWidth
        {
            get => b_ActualWidth;
            private set => ((INotifiable)this).SetField(ref b_ActualWidth, value, () => m_ActualWidthSet = true);
        }


        internal protected int b_ActualHeight;
        internal protected bool m_ActualHeightSet;
        public virtual int ActualHeight
        {
            get => b_ActualHeight;
            private set => ((INotifiable)this).SetField(ref b_ActualHeight, value, () => m_ActualHeightSet = true);
        }


        internal protected SizeI b_ActualSize;
        public virtual SizeI ActualSize
        {
            get => new SizeI(b_ActualWidth, b_ActualHeight);
            private set
            {
                if (value.Width != b_ActualWidth || value.Height != b_ActualHeight)
                {
                    ActualWidth = value.Width;
                    ActualHeight = value.Height;
                    OnPropertyChanged();
                    SizeChanged?.Invoke(this, ((IElement)this).DipActualSize);
                }
            }
        }

        object b_DataContext;
        public object DataContext
        {
            get => b_DataContext;
            set => ((INotifiable)this).SetField(ref b_DataContext, value, ((IElement)this).OnDataContextChanged);
        }

        public double Opacity
        {
            get => m_Border.Opacity;
            set => m_Border.Opacity = value;
        }

        public bool IsVisible
        {
            get => m_Border.IsVisible;
            set => m_Border.IsVisible = value;
        }
        #endregion


        #region Events
        public event EventHandler<Size> SizeChanged;
        #endregion


        #region INotifiable

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        #endregion


        #region Fields
        bool b_HasDrawn;
        public bool HasDrawn
        {
            get => b_HasDrawn;
            set => ((INotifiable)this).SetField(ref b_HasDrawn, value);
        }

        public bool HasChanged { get; set; }
        #endregion

        #endregion
    }
}
