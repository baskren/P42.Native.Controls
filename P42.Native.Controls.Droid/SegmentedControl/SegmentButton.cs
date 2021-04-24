using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace P42.Native.Controls.Droid
{
    public class SegmentButton : TextView
    {
        static ColorStateList sDefaultColorStateList = new ColorStateList(new int[][] { }, new int[] { });
        internal static Thickness sDefaultPadding = new Thickness(ViewExtensions.ConvertFromDipToPx(5), ViewExtensions.ConvertFromDipToPx(2));
        internal static Color s_defaultBackgroundColor = Color.White;
        internal static Color s_defaultSelectedBackgroundColor = Color.DarkGray;
        internal static Color s_defaultTextColor = Color.Gray;
        internal static Color s_defaultSelectedTextColor = Color.White;
        internal static float s_defaultCornerRadius = ViewExtensions.ConvertFromDipToPx(5);

        static SegmentButton()
        {
            var tv = new TextView(P42.Utils.Droid.Settings.Context);
            var colors = tv.TextColors;
            s_defaultTextColor = colors.DefaultColor.ToColor();
            //  android.R.attr.textColorSecondary
        }

        public event EventHandler<bool> SelectedChanged;

        public override bool Selected
        {
            get => base.Selected;
            set
            {
                if (base.Selected != value)
                {
                    base.Selected = value;
                    SelectedChanged?.Invoke(this, base.Selected);
                    m_Background.SetColor(base.Selected ? SelectedBackgroundColor : BackgroundColor);
                    SetTextColor(base.Selected ? SelectedTextColor : TextColor);
                }
            }
        }
        
        Color b_BackgroundColor = s_defaultBackgroundColor;
        internal Color BackgroundColor
        {
            get => b_BackgroundColor;
            set
            {
                if (b_BackgroundColor != value)
                {
                    b_BackgroundColor = value;
                    if (!Selected)
                        m_Background.SetColor(b_BackgroundColor);
                }
            }
        }

        Color b_selectedBackgroundColor = s_defaultSelectedBackgroundColor;
        internal Color SelectedBackgroundColor
        {
            get => b_selectedBackgroundColor;
            set
            {
                if (b_selectedBackgroundColor != value)
                {
                    b_selectedBackgroundColor = value;
                    if (Selected)
                        m_Background.SetColor(b_selectedBackgroundColor);
                }
            }
        }

        float b_cornerRadius = s_defaultCornerRadius;
        internal float CornerRadius
        {
            get => b_cornerRadius;
            set
            {
                if (b_cornerRadius != value)
                {
                    b_cornerRadius = value;
                    UpdateRadii();
                }
            }
        }

        bool b_isHorizontal = true;
        internal bool IsHorizontal
        {
            get => b_isHorizontal;
            set
            {
                if (b_isHorizontal != value)
                {
                    b_isHorizontal = value;
                    UpdateRadii();
                }
            }
        }


        SegmentPosition b_Position;
        internal SegmentPosition Position
        {
            get => b_Position;
            set
            {
                if (b_Position != value)
                {
                    b_Position = value;
                    UpdateRadii();
                }
            }
        }

        int b_index = -1;
        public int Index
        {
            get => b_index;
            internal set
            {
                if (b_index != value)
                {
                    b_index = value;
                    UpdateRadii();
                }
            }
        }

        Color b_TextColor = s_defaultTextColor;
        public Color TextColor
        {
            get => b_TextColor;
            set
            {
                if (b_TextColor != value)
                {
                    b_TextColor = value;
                    if (!Selected)
                        SetTextColor(b_TextColor);
                }
            }
        }

        Color b_SelectedTextColor = s_defaultSelectedTextColor;
        public Color SelectedTextColor
        {
            get => b_SelectedTextColor;
            set
            {
                if (b_SelectedTextColor != value)
                {
                    b_SelectedTextColor = value;
                    if (Selected)
                        SetTextColor(b_SelectedTextColor);
                }
            }
        }




        GradientDrawable m_Background = new GradientDrawable();

        public SegmentButton(string text) : this(P42.Utils.Droid.Settings.Context)
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
            this.SetPadding(sDefaultPadding);
            Click += OnClicked;
            //SetPadding(1, 1, 1, 1);
            //SetMinHeight(20);
            //SetMinWidth(20);
            SetIncludeFontPadding(false);
            TextAlignment = Android.Views.TextAlignment.Center;
            m_Background.SetShape(ShapeType.Rectangle);
            m_Background.SetColor(BackgroundColor);
            Background = new RippleDrawable(sDefaultColorStateList, m_Background, null);
        }

        private void OnClicked(object sender, EventArgs e)
        {
            if (Parent is SegmentedControl control)
                control.OnSegmentClicked(this);
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
        }

        void UpdateRadii()
        {
            float tl = 0, tr = 0, br = 0, bl = 0;

            if (b_Position == SegmentPosition.Only)
            {
                tl = tr = br = bl = CornerRadius;
            }
            else if (b_Position == SegmentPosition.First)
            {
                if (IsHorizontal)
                    tl = bl = CornerRadius;
                else
                    tl = tr = CornerRadius;
            }
            else if (b_Position == SegmentPosition.Last)
            {
                if (IsHorizontal)
                    tr = br = CornerRadius;
                else
                    bl = br = CornerRadius;
            }
            m_Background.SetCornerRadii(new float[] { tl, tl, tr, tr, br, br, bl, bl }); // TL, TR, BR, BL
        }
        /*
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var availableWidth = MeasureSpec.GetSize(widthMeasureSpec);
            var availableHeight = MeasureSpec.GetSize(heightMeasureSpec);

            System.Diagnostics.Debug.WriteLine($"   Button availW:{availableWidth} availH:{availableHeight}");
            System.Diagnostics.Debug.WriteLine($"   Button pL:{PaddingLeft} pT:{PaddingTop} pR:{PaddingRight} pB:{PaddingBottom}");
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            System.Diagnostics.Debug.WriteLine($"   Button measureW:{MeasuredWidth} measuredH:{MeasuredHeight} ");
        }
        */
    }
}
