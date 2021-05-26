
using SmartTraitsDefs;
#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    [Trait]
    abstract partial class TControl : TElement, IControl
    {
        #region Control
        internal protected ThicknessI b_Padding = (ThicknessI)DisplayExtensions.DipToPx(10);
        public virtual ThicknessI Padding
        {
            get => b_Padding;
            set => SetField(ref b_Padding, value, UpdatePadding);
        }
        public virtual Thickness DipPadding
        {
            get => DisplayExtensions.PxToDip(Padding);
            set => Padding = value.DipToPx();
        }


        internal protected double b_BorderWidth = DisplayExtensions.DipToPxD(1);
        public virtual double BorderWidth
        {
            get => b_BorderWidth;
            set => SetLayoutField(ref b_BorderWidth, value);
        }
        public virtual double DipBorderWidth
        {
            get => DisplayExtensions.PxToDip(BorderWidth);
            set => BorderWidth = value.DipToPxD();
        }



        internal protected Color b_BorderColor = Color.Black;
        public virtual Color BorderColor
        {
            get => b_BorderColor;
            set => SetRedrawField(ref b_BorderColor, value);
        }

        internal protected double b_CornerRadius = DisplayExtensions.DipToPxD(5);
        public virtual double CornerRadius
        {
            get => b_CornerRadius;
            set => SetRedrawField(ref b_CornerRadius, value);
        }
        public virtual double DipCornerRadius
        {
            get => DisplayExtensions.PxToDip(CornerRadius);
            set => CornerRadius = value.DipToPxD();
        }


        internal protected Color b_BackgroundColor = Color.Gray.WithAlpha(0.5);
        public virtual Color BackgroundColor
        {
            get => b_BackgroundColor;
            set => SetRedrawField(ref b_BackgroundColor, value);
        }



        #endregion


#if __ANDROID__

        void UpdatePadding()
        {
            BaseView.SetPadding((int)(b_Padding.Left + 0.5), (int)(b_Padding.Top + 0.5), (int)(b_Padding.Right + 0.5), (int)(b_Padding.Bottom + 0.5));
        }

        [SmartTraitsDefs.TraitIgnore]
        Android.Views.View BaseView { get; set; }
#else

        void UpdatePadding() {}

#endif


    }
}
