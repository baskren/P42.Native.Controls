using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace P42.Native.Controls
{
    public partial class SegmentButton : TextView, INotifiable
    {
        #region Static Implmenetation
        readonly static ColorStateList s_DefaultColorStateList = new ColorStateList(new int[][] { }, new int[] { });
        internal static ThicknessI s_DefaultPadding = DisplayExtensions.DipToPx(new Thickness(5,2));
        internal static Color s_DefaultBackgroundColor = Color.White;
        internal static Color s_DefaultSelectedBackgroundColor = Color.DarkGray;
        internal static Color s_DefaultTextColor = Color.Gray;
        internal static Color s_DefaultSelectedTextColor = Color.White;
        internal static float s_DefaultCornerRadius = DisplayExtensions.DipToPx(5);

        static SegmentButton()
        {
            var tv = new TextView(P42.Utils.Droid.Settings.Context);
            var colors = tv.TextColors;
            s_DefaultTextColor = colors.DefaultColor.ToColor();
        }
        #endregion


        #region Events
        public event EventHandler<bool> SelectedChanged;
        #endregion


        #region Properties

        #region Index / Selection
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

        Android.Widget.Orientation b_Orientation;
        public Android.Widget.Orientation Orientation
        {
            get => b_Orientation;
            set
            {
                if (((INotifiable)this).SetField(ref b_Orientation, value))
                    UpdateRadii();
            }
        }

        internal bool IsHorizontal => b_Orientation == Android.Widget.Orientation.Horizontal;

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
        #endregion


        #region Corner Radius
        public double DipCornerRadius
        {
            get => DisplayExtensions.PxToDip(b_CornerRadius);
            set => CornerRadius = DisplayExtensions.DipToPx(value);
        }

        double b_CornerRadius = s_DefaultCornerRadius;
        internal double CornerRadius
        {
            get => b_CornerRadius;
            set
            {
                if (b_CornerRadius != value)
                {
                    b_CornerRadius = value;
                    UpdateRadii();
                }
            }
        }
        #endregion


        #region Color Properties
        Color b_TextColor = s_DefaultTextColor;
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

        Color b_SelectedTextColor = s_DefaultSelectedTextColor;
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

        Color b_BackgroundColor = s_DefaultBackgroundColor;
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

        Color b_selectedBackgroundColor = s_DefaultSelectedBackgroundColor;
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
            this.SetPadding(s_DefaultPadding);
            Click += OnClicked;
            TextAlignment = Android.Views.TextAlignment.Center;
            m_Background.SetShape(ShapeType.Rectangle);
            m_Background.SetColor(BackgroundColor);
            Background = new RippleDrawable(s_DefaultColorStateList, m_Background, null);
        }
        #endregion


        #region Event Handlers
        private void OnClicked(object sender, EventArgs e)
        {
            if (Parent is SegmentedControl control)
                control.OnSegmentClicked(this);
        }

        void UpdateRadii()
        {
            float tl = 0, tr = 0, br = 0, bl = 0;

            if (b_Position == SegmentPosition.Only)
            {
                tl = tr = br = bl = (float)CornerRadius;
            }
            else if (b_Position == SegmentPosition.First)
            {
                if (IsHorizontal)
                    tl = bl = (float)CornerRadius;
                else
                    tl = tr = (float)CornerRadius;
            }
            else if (b_Position == SegmentPosition.Last)
            {
                if (IsHorizontal)
                    tr = br = (float)CornerRadius;
                else
                    bl = br = (float)CornerRadius;
            }
            m_Background.SetCornerRadii(new float[] { tl, tl, tr, tr, br, br, bl, bl }); // TL, TR, BR, BL
        }

        #endregion


        #region INotifiable

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        #endregion


        #region Fields
        public bool HasDrawn { get; set; }
        public bool HasChanged { get; set; }
        #endregion


        #region Methods
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public void RedrawElement() => PostInvalidate();

        public void RelayoutElement() => RequestLayout();
        #endregion

        #endregion

    }
}
