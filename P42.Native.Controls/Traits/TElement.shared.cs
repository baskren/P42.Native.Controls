using System;
using System.Runtime.CompilerServices;
using Android.Views;
using SmartTraitsDefs;

namespace P42.Native.Controls
{
    [SimpleTrait]
    partial class TElement : TNotifiable, IElement
    {


        #region Defaults
        public static Alignment DefaultHorizontalAlignment = Alignment.Center;
        public static Alignment DefaultVerticalAlignment = Alignment.Center;
        public static ThicknessI DefaultMargin = (ThicknessI)0;
        public static int DefaultMinWidth = DisplayExtensions.DipToPx(50);
        public static int DefaultMinHeight = DisplayExtensions.DipToPx(50);
        public static int DefaultMaxWidth = DisplayExtensions.PxWidth();
        public static int DefaultMaxHeight = DisplayExtensions.PxHeight();
        #endregion

        
        #region Properties
        int b_RequestedWidth = -1;
        public virtual int RequestedWidth
        {
            get => b_RequestedWidth;
            set => SetField(ref b_RequestedWidth, value, UpdateLayoutParams);
        }
        public double DipRequestedWidth
        {
            get => DisplayExtensions.PxToDip(RequestedWidth);
            set => RequestedWidth = value.DipToPx();
        }

        int b_RequestedHeight = -1;
        public virtual int RequestedHeight
        {
            get => b_RequestedHeight;
            set => SetField(ref b_RequestedHeight, value, UpdateLayoutParams);
        }
        public double DipRequestedHeight
        {
            get => DisplayExtensions.PxToDip(RequestedHeight);
            set => RequestedHeight = value.DipToPx();
        }

        Alignment b_HorizontalAlignment = DefaultHorizontalAlignment;
        public virtual Alignment HorizontalAlignment
        {
            get => b_HorizontalAlignment;
            set => SetField(ref b_HorizontalAlignment, value, UpdateLayoutParams);
        }

        Alignment b_VerticalAlignment = DefaultVerticalAlignment;
        public virtual Alignment VerticalAlignment
        {
            get => b_VerticalAlignment;
            set => SetField(ref b_VerticalAlignment, value, UpdateLayoutParams);
        }

        internal protected ThicknessI b_Margin = DefaultMargin;
        public virtual ThicknessI Margin
        {
            get => b_Margin;
            set => SetLayoutField(ref b_Margin, value);
        }
        public virtual Thickness DipMargin
        {
            get => DisplayExtensions.PxToDip(Margin);
            set => Margin = value.DipToPx();
        }


        internal protected int b_MinWidth = DefaultMinWidth;
        public virtual int MinWidth
        {
            get => b_MinWidth;
            set => SetField(ref b_MinWidth, value, UpdateMinWidth);
        }
        public virtual double DipMinWidth
        {
            get => DisplayExtensions.PxToDip(MinWidth);
            set => MinWidth = value.DipToPx();
        }


        internal protected int b_MinHeight = DefaultMinHeight;
        public virtual int MinHeight
        {
            get => b_MinHeight;
            set => SetField(ref b_MinHeight, value, UpdateMinHeight);
        }
        public virtual double DipMinHeight
        {
            get => DisplayExtensions.PxToDip(MinHeight);
            set => MinHeight = value.DipToPx();
        }


        internal protected int b_MaxWidth = DefaultMaxWidth;
        public virtual int MaxWidth
        {
            get => b_MaxWidth;
            set
            {
                if (SetField(ref b_MaxWidth, value) && HasDrawn && m_ActualWidthSet && ActualWidth > MaxWidth)
                    BaseView.RequestLayout();
            }
        }
        public virtual double DipMaxWidth
        {
            get => DisplayExtensions.PxToDip(MaxWidth);
            set => MaxWidth = value.DipToPx();
        }

        internal protected int b_MaxHeight = DefaultMaxHeight;
        public virtual int MaxHeight
        {
            get => b_MaxHeight;
            set
            {
                if (SetField(ref b_MaxHeight, value) && HasDrawn && m_ActualHeightSet && ActualHeight > MaxHeight)
                    BaseView.RequestLayout();
            }
        }
        public virtual double DipMaxHeight
        {
            get => DisplayExtensions.PxToDip(MaxHeight);
            set => MaxHeight = value.DipToPx();
        }

        internal protected int b_ActualWidth;
        internal protected bool m_ActualWidthSet;
        public virtual int ActualWidth
        {
            get => b_ActualWidth;
            private set => SetField(ref b_ActualWidth, value, () => m_ActualWidthSet = true);
        }
        public double DipActualWidth => ActualWidth.PxToDip();


        internal protected int b_ActualHeight;
        internal protected bool m_ActualHeightSet;
        public virtual int ActualHeight
        {
            get => b_ActualHeight;
            private set => SetField(ref b_ActualHeight, value, () => m_ActualHeightSet = true);
        }
        public virtual double DipActualHeight => ActualHeight.PxToDip();

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
                    ((IElement)this).OnPropertyChanged();
                    SizeChanged?.Invoke(this, DipActualSize);
                }
            }
        }
        public virtual Size DipActualSize => ActualSize.PxToDip();

        object b_DataContext;
        public object DataContext
        {
            get => b_DataContext;
            set => SetField(ref b_DataContext, value,((IElement)this).OnDataContextChanged);
        }

#if __ANDROID__

        public virtual double Opacity
        {
            get => BaseView.Alpha;
            set => BaseView.Alpha = (float)value;
        }

        public virtual bool IsVisible
        {
            get => BaseView.Visibility == ViewStates.Visible;
            set => BaseView.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
        }

#else

        public virtual double Opacity { get; set; }
        public virtual bool IsVisible { get; set; }

#endif

        #endregion

        
        #region Events
        public event EventHandler<Size> SizeChanged;
        #endregion


        #region Methods
        
        public virtual void OnDataContextChanged() { }

        public bool SetRedrawField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
            => SetField(ref field, value, () => { if (HasDrawn) ((IElement)this).RedrawElement(); }, propertyName, callerPath);

        public bool SetLayoutField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
            => SetField(ref field, value, () => { if (HasDrawn) ((IElement)this).RelayoutElement(); }, propertyName, callerPath);



#if __ANDROID__
        public Android.Views.View BaseView { get; protected set; }

        void UpdateLayoutParams()
        {
            BaseView.LayoutParameters = new ViewGroup.LayoutParams(
                    HorizontalAlignment == Alignment.Stretch
                        ? ViewGroup.LayoutParams.MatchParent
                        : RequestedWidth < 0
                            ? ViewGroup.LayoutParams.WrapContent
                            : RequestedWidth < MinWidth
                                ? MinWidth
                                : RequestedWidth > MaxWidth
                                    ? MaxWidth
                                    : RequestedWidth,
                    VerticalAlignment == Alignment.Start
                        ? ViewGroup.LayoutParams.MatchParent
                        : RequestedHeight < 0
                            ? ViewGroup.LayoutParams.WrapContent
                            : RequestedHeight < MinHeight
                                ? MinHeight
                                : RequestedHeight > MaxHeight
                                    ? MaxHeight
                                    : RequestedHeight
                );
            if (HasDrawn)
                BaseView.RequestLayout();
        }

        void UpdateMinWidth()
        {
            BaseView.SetMinimumWidth(MinWidth);
            UpdateLayoutParams();
        }

        void UpdateMinHeight()
        {
            BaseView.SetMinimumHeight(MinHeight);
            UpdateLayoutParams();
        }


#else

        void UpdateLayoutParams() {}
        void UpdateMinWidth(){}
        void UpdateMinHeight() {}
        public void RedrawElement();
        public void RelayoutElement();

#endif

        #endregion

    }
}


