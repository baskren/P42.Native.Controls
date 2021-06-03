using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
//using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace P42.Native.Controls
{
    public partial class SegmentButton : TextView
    {
        #region Static Implmenetation
        readonly static Android.Content.Res.ColorStateList s_DefaultColorStateList = new Android.Content.Res.ColorStateList(new int[][] { }, new int[] { });
        internal static ThicknessI NtvDefaultPadding = DisplayExtensions.DipToPx(new Thickness(5,2));
        internal static Color DipDefaultBackgroundColor = Color.White;
        internal static Color DipDefaultSelectedBackgroundColor = Color.DarkGray;
        internal static Color DipDefaultTextColor = Color.Gray;
        internal static Color DipDefaultSelectedTextColor = Color.White;
        internal static float NtvDefaultCornerRadius = DisplayExtensions.DipToPx(5);

        static SegmentButton()
        {
            var tv = new TextView(global::P42.Utils.Droid.Settings.Context);
            var colors = tv.TextColors;
            DipDefaultTextColor = colors.DefaultColor.ToColor();
        }
        #endregion



        #region Properties

        #region Index / Selection
        public bool DipSelected
        {
            get => base.Selected;
            set 
            {
                if (base.Selected != value)
                {
                    base.Selected = value;
                    SelectedChanged?.Invoke(this, base.Selected);
                    m_Background.SetColor(base.Selected ? DipSelectedBackgroundColor : DipBackgroundColor);
                    SetTextColor(base.Selected ? DipSelectedTextColor : DipTextColor);
                }
            }
        }



        #endregion


        #region Corner Radius
        public double DipCornerRadius
        {
            get => DisplayExtensions.PxToDip(b_NtvCornerRadius);
            set => NtvCornerRadius = DisplayExtensions.DipToPx(value);
        }

        double b_NtvCornerRadius = NtvDefaultCornerRadius;
        internal double NtvCornerRadius
        {
            get => b_NtvCornerRadius;
            set
            {
                if (b_NtvCornerRadius != value)
                {
                    b_NtvCornerRadius = value;
                    UpdateRadii();
                }
            }
        }
        #endregion


        #region Color Properties
        Color b_DipTextColor = DipDefaultTextColor;
        public Color DipTextColor
        {
            get => b_DipTextColor;
            set
            {
                if (b_DipTextColor != value)
                {
                    b_DipTextColor = value;
                    if (!DipSelected)
                        SetTextColor(b_DipTextColor);
                }
            }
        }

        Color b_DipSelectedTextColor = DipDefaultSelectedTextColor;
        public Color DipSelectedTextColor
        {
            get => b_DipSelectedTextColor;
            set
            {
                if (b_DipSelectedTextColor != value)
                {
                    b_DipSelectedTextColor = value;
                    if (DipSelected)
                        SetTextColor(b_DipSelectedTextColor);
                }
            }
        }

        Color b_DipBackgroundColor = DipDefaultBackgroundColor;
        internal Color DipBackgroundColor
        {
            get => b_DipBackgroundColor;
            set
            {
                if (b_DipBackgroundColor != value)
                {
                    b_DipBackgroundColor = value;
                    if (!DipSelected)
                        m_Background.SetColor(b_DipBackgroundColor);
                }
            }
        }

        Color b_DipSelectedBackgroundColor = DipDefaultSelectedBackgroundColor;
        internal Color DipSelectedBackgroundColor
        {
            get => b_DipSelectedBackgroundColor;
            set
            {
                if (b_DipSelectedBackgroundColor != value)
                {
                    b_DipSelectedBackgroundColor = value;
                    if (DipSelected)
                        m_Background.SetColor(b_DipSelectedBackgroundColor);
                }
            }
        }
        #endregion

        #endregion


        #region Fields
        GradientDrawable m_Background = new GradientDrawable();
        #endregion


        #region Constructors / Initialization
        public SegmentButton(Context context, string text) : this(context)
        {
            Text = text;
        }

        public SegmentButton(Context context) : base(context)
        {
            Init();
        }

        public SegmentButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public SegmentButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public SegmentButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        protected SegmentButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        void Init()
        {
            //this.SetPadding(s_DefaultPadding);
            Click += OnClicked;
            m_Background.SetShape(ShapeType.Rectangle);
            m_Background.SetColor(DipBackgroundColor);
            Background = new RippleDrawable(s_DefaultColorStateList, m_Background, null);
            Gravity = Android.Views.GravityFlags.Center;
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                Click -= OnClicked;
                Background?.Dispose();
                m_Background?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region Event Handlers
        private void OnClicked(object sender, EventArgs e)
        {
            if (Parent is SegmentedControl control)
                control.OnSegmentClicked(this);
        }

        partial void UpdateRadii()
        {
            float tl = 0, tr = 0, br = 0, bl = 0;

            if (b_DipPosition == SegmentPosition.Only)
            {
                tl = tr = br = bl = (float)NtvCornerRadius;
            }
            else if (b_DipPosition == SegmentPosition.First)
            {
                if (DipIsHorizontal)
                    tl = bl = (float)NtvCornerRadius;
                else
                    tl = tr = (float)NtvCornerRadius;
            }
            else if (b_DipPosition == SegmentPosition.Last)
            {
                if (DipIsHorizontal)
                    tr = br = (float)NtvCornerRadius;
                else
                    bl = br = (float)NtvCornerRadius;
            }
            m_Background.SetCornerRadii(new float[] { tl, tl, tr, tr, br, br, bl, bl }); // TL, TR, BR, BL
        }

        #endregion
    }
}
