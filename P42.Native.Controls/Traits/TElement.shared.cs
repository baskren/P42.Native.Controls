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
        public static P42.Native.Controls.Alignment DipDefaultHorizontalAlignment = P42.Native.Controls.Alignment.Center;
        public static P42.Native.Controls.Alignment DipDefaultVerticalAlignment = P42.Native.Controls.Alignment.Center;
        public static ThicknessI NtvDefaultMargin = (ThicknessI)0;
        public static int NtvDefaultMinWidth = DisplayExtensions.DipToPx(50);
        public static int NtvDefaultMinHeight = DisplayExtensions.DipToPx(50);
        public static int NtvDefaultMaxWidth = DisplayExtensions.PxWidth();
        public static int NtvDefaultMaxHeight = DisplayExtensions.PxHeight();
        #endregion

        
        #region Properties
        int b_NtvRequestedWidth = -1;
        public virtual int NtvRequestedWidth
        {
            get => b_NtvRequestedWidth;
            set => SetField(ref b_NtvRequestedWidth, value, NtvUpdateLayoutParams);
        }
        public double DipRequestedWidth
        {
            get => DisplayExtensions.PxToDip(NtvRequestedWidth);
            set => NtvRequestedWidth = value.DipToPx();
        }

        int b_NtvRequestedHeight = -1;
        public virtual int NtvRequestedHeight
        {
            get => b_NtvRequestedHeight;
            set => SetField(ref b_NtvRequestedHeight, value, NtvUpdateLayoutParams);
        }
        public double DipRequestedHeight
        {
            get => DisplayExtensions.PxToDip(NtvRequestedHeight);
            set => NtvRequestedHeight = value.DipToPx();
        }

        P42.Native.Controls.Alignment b_HorizontalAlignment = DipDefaultHorizontalAlignment;
        public virtual P42.Native.Controls.Alignment DipHorizontalAlignment
        {
            get => b_HorizontalAlignment;
            set => SetField(ref b_HorizontalAlignment, value, NtvUpdateLayoutParams);
        }

        P42.Native.Controls.Alignment b_VerticalAlignment = DipDefaultVerticalAlignment;
        public virtual P42.Native.Controls.Alignment DipVerticalAlignment
        {
            get => b_VerticalAlignment;
            set => SetField(ref b_VerticalAlignment, value, NtvUpdateLayoutParams);
        }

        internal protected ThicknessI b_NtvMargin = NtvDefaultMargin;
        public virtual ThicknessI NtvMargin
        {
            get => b_NtvMargin;
            set => SetLayoutField(ref b_NtvMargin, value);
        }
        public virtual Thickness DipMargin
        {
            get => DisplayExtensions.PxToDip(NtvMargin);
            set => NtvMargin = value.DipToPx();
        }


        internal protected int b_NtvMinWidth = NtvDefaultMinWidth;
        public virtual int NtvMinWidth
        {
            get => b_NtvMinWidth;
            set => SetField(ref b_NtvMinWidth, value, NtvUpdateMinWidth);
        }
        public virtual double DipMinWidth
        {
            get => DisplayExtensions.PxToDip(NtvMinWidth);
            set => NtvMinWidth = value.DipToPx();
        }


        internal protected int b_NtvMinHeight = NtvDefaultMinHeight;
        public virtual int NtvMinHeight
        {
            get => b_NtvMinHeight;
            set => SetField(ref b_NtvMinHeight, value, NtvUpdateMinHeight);
        }
        public virtual double DipMinHeight
        {
            get => DisplayExtensions.PxToDip(NtvMinHeight);
            set => NtvMinHeight = value.DipToPx();
        }


        internal protected int b_NtvMaxWidth = NtvDefaultMaxWidth;
        public virtual int NtvMaxWidth
        {
            get => b_NtvMaxWidth;
            set
            {
                if (SetField(ref b_NtvMaxWidth, value) && DipHasDrawn && m_NtvActualWidthSet && NtvActualWidth > NtvMaxWidth)
                    NtvBaseView.RequestLayout();
            }
        }
        public virtual double DipMaxWidth
        {
            get => DisplayExtensions.PxToDip(NtvMaxWidth);
            set => NtvMaxWidth = value.DipToPx();
        }

        internal protected int b_NtvMaxHeight = NtvDefaultMaxHeight;
        public virtual int NtvMaxHeight
        {
            get => b_NtvMaxHeight;
            set
            {
                if (SetField(ref b_NtvMaxHeight, value) && DipHasDrawn && m_NtvActualHeightSet && NtvActualHeight > NtvMaxHeight)
                    NtvBaseView.RequestLayout();
            }
        }
        public virtual double DipMaxHeight
        {
            get => DisplayExtensions.PxToDip(NtvMaxHeight);
            set => NtvMaxHeight = value.DipToPx();
        }

        internal protected int b_NtvActualWidth;
        internal protected bool m_NtvActualWidthSet;
        public virtual int NtvActualWidth
        {
            get => b_NtvActualWidth;
            private set => SetField(ref b_NtvActualWidth, value, () => m_NtvActualWidthSet = true);
        }
        public double DipActualWidth => NtvActualWidth.PxToDip();


        internal protected int b_NtvActualHeight;
        internal protected bool m_NtvActualHeightSet;
        public virtual int NtvActualHeight
        {
            get => b_NtvActualHeight;
            private set => SetField(ref b_NtvActualHeight, value, () => m_NtvActualHeightSet = true);
        }
        public virtual double DipActualHeight => NtvActualHeight.PxToDip();

        internal protected SizeI b_NtvActualSize;
        public virtual SizeI NtvActualSize
        {
            get => new SizeI(b_NtvActualWidth, b_NtvActualHeight);
            private set
            {
                if (value.Width != b_NtvActualWidth || value.Height != b_NtvActualHeight)
                {
                    NtvActualWidth = value.Width;
                    NtvActualHeight = value.Height;
                    ((IElement)this).OnPropertyChanged();
                    DipSizeChanged?.Invoke(this, DipActualSize);
                }
            }
        }
        public virtual Size DipActualSize => NtvActualSize.PxToDip();

        object b_DipDataContext;
        public object DipDataContext
        {
            get => b_DipDataContext;
            set => SetField(ref b_DipDataContext, value,((IElement)this).DipOnDataContextChanged);
        }

#if __ANDROID__

        public virtual double DipOpacity
        {
            get => NtvBaseView.Alpha;
            set => NtvBaseView.Alpha = (float)value;
        }

        public virtual bool DipIsVisible
        {
            get => NtvBaseView.Visibility == ViewStates.Visible;
            set => NtvBaseView.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
        }

#else

        public virtual double DipOpacity { get; set; }
        public virtual bool DipIsVisible { get; set; }

#endif

        #endregion

        
        #region Events
        public event EventHandler<Size> DipSizeChanged;
        #endregion


        #region Methods
        
        public bool SetRedrawField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
            => SetField(ref field, value, () => { if (DipHasDrawn) ((IElement)this).DipRedrawElement(); }, propertyName, callerPath);

        public bool SetLayoutField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
            => SetField(ref field, value, () => { if (DipHasDrawn) ((IElement)this).DipRelayoutElement(); }, propertyName, callerPath);



#if __ANDROID__
        public Android.Views.View NtvBaseView { get; protected set; }

        void NtvUpdateLayoutParams()
        {
            NtvBaseView.LayoutParameters = new ViewGroup.LayoutParams(
                    DipHorizontalAlignment == P42.Native.Controls.Alignment.Stretch
                        ? ViewGroup.LayoutParams.MatchParent
                        : NtvRequestedWidth < 0
                            ? ViewGroup.LayoutParams.WrapContent
                            : NtvRequestedWidth < NtvMinWidth
                                ? NtvMinWidth
                                : NtvRequestedWidth > NtvMaxWidth
                                    ? NtvMaxWidth
                                    : NtvRequestedWidth,
                    DipVerticalAlignment == P42.Native.Controls.Alignment.Stretch
                        ? ViewGroup.LayoutParams.MatchParent
                        : NtvRequestedHeight < 0
                            ? ViewGroup.LayoutParams.WrapContent
                            : NtvRequestedHeight < NtvMinHeight
                                ? NtvMinHeight
                                : NtvRequestedHeight > NtvMaxHeight
                                    ? NtvMaxHeight
                                    : NtvRequestedHeight
                );
            if (DipHasDrawn)
                NtvBaseView.RequestLayout();
        }

        void NtvUpdateMinWidth()
        {
            NtvBaseView.SetMinimumWidth(NtvMinWidth);
            NtvUpdateLayoutParams();
        }

        void NtvUpdateMinHeight()
        {
            NtvBaseView.SetMinimumHeight(NtvMinHeight);
            NtvUpdateLayoutParams();
        }


#else

        void NtvUpdateLayoutParams() {}
        void NtvUpdateMinWidth(){}
        void NtvUpdateMinHeight() {}
        public void DipRedrawElement();
        public void DipRelayoutElement();

#endif

        #endregion

    }
}


