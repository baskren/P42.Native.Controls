using System;
using System.Runtime.CompilerServices;
using Android.Views;

namespace P42.Native.Controls
{
    public partial class Label : IFrameworkElement, INotifiable
    {
        #region FrameworkElement

        int b_RequestedWidth = -1;
        public int RequestedWidth
        {
            get => b_RequestedWidth;
            set => ((INotifiable)this).SetField(ref b_RequestedWidth, value, UpdateLayoutParams);
        }

        int b_RequestedHeight = -1;
        public int RequestedHeight
        {
            get => b_RequestedHeight;
            set => ((INotifiable)this).SetField(ref b_RequestedHeight, value, UpdateLayoutParams);
        }

        Alignment b_HorizontalAlignment = Alignment.Center;
        public Alignment HorizontalAlignment
        {
            get => b_HorizontalAlignment;
            set => ((INotifiable)this).SetField(ref b_HorizontalAlignment, value, UpdateLayoutParams);
        }

        Alignment b_VerticalAlignment = Alignment.Center;
        public Alignment VerticalAlignment
        {
            get => b_VerticalAlignment;
            set => ((INotifiable)this).SetField(ref b_VerticalAlignment, value, UpdateLayoutParams);
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
            set => ((INotifiable)this).SetField(ref b_MinWidth, value, UpdateMinWidth);
        }


        internal protected int b_MinHeight = DisplayExtensions.DipToPx(50);
        public virtual int MinHeight
        {
            get => b_MinHeight;
            set => ((INotifiable)this).SetField(ref b_MinHeight, value, UpdateMinHeight);
        }


        internal protected int b_MaxWidth = DisplayExtensions.PxWidth();
        public virtual int MaxWidth
        {
            get => b_MaxWidth;
            set
            {
                if (((INotifiable)this).SetField(ref b_MaxWidth, value) && HasDrawn && m_ActualWidthSet && ActualWidth > MaxWidth)
                    RequestLayout();
            }
        }

        internal protected int b_MaxHeight = DisplayExtensions.PxHeight();
        public virtual int MaxHeight
        {
            get => b_MaxHeight;
            set
            {
                if (((INotifiable)this).SetField(ref b_MaxHeight, value) && HasDrawn && m_ActualHeightSet && ActualHeight > MaxHeight)
                    RequestLayout();
            }
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
                    SizeChanged?.Invoke(this, ((IFrameworkElement)this).DipActualSize);
                }
            }
        }

        object b_DataContext;
        public object DataContext
        {
            get => b_DataContext;
            set => ((INotifiable)this).SetField(ref b_DataContext, value,((IFrameworkElement)this).OnDataContextChanged);
        }
        #endregion


        #region Events
        public event EventHandler<Size> SizeChanged;
        #endregion


        #region Support Methods
        void UpdateLayoutParams()
        {
            LayoutParameters = new LayoutParams(
                    HorizontalAlignment == Alignment.Stretch
                        ? LayoutParams.MatchParent
                        : RequestedWidth < 0
                            ? LayoutParams.WrapContent
                            : RequestedWidth < MinWidth
                                ? MinWidth
                                : RequestedWidth > MaxWidth
                                    ? MaxWidth
                                    : RequestedWidth,
                    VerticalAlignment == Alignment.Start
                        ? LayoutParams.MatchParent
                        : RequestedHeight < 0
                            ? LayoutParams.WrapContent
                            : RequestedHeight < MinHeight
                                ? MinHeight
                                : RequestedHeight > MaxHeight
                                    ? MaxHeight
                                    : RequestedHeight
                );
            if (HasDrawn)
                RequestLayout();
        }

        void UpdateMinWidth()
        {
            SetMinimumWidth(MinWidth);
            UpdateLayoutParams();
        }

        void UpdateMinHeight()
        {
            SetMinimumHeight(MinHeight);
            UpdateLayoutParams();
        }
        #endregion


    }
}
