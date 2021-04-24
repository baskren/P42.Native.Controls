using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace P42.Native.Controls.Droid
{
    public class SegmentedControl : SegmentedPanel, INotifyPropertyChanged
    {
        #region Properties

        static int s_defaultOutlineWidth = (int)(ViewExtensions.ConvertFromDipToPx(1) + 0.5f);
        static Color s_defaultOutlineColor = Color.DarkGray;

        public override bool IsHorizontal
        {
            get => base.IsHorizontal;
            set
            {
                if (_isHorizontal != value)
                {
                    base.IsHorizontal = value;
                    UpdateSegments(s => s.IsHorizontal = value);
                }
            }
        }

        Color b_TextColor = SegmentButton.s_defaultTextColor;
        public Color TextColor
        {
            get => b_TextColor;
            set
            {
                if (SetField(ref b_TextColor, value))
                    UpdateSegments(s => s.TextColor = value);
            }
        }

        Color b_BackgroundColor = SegmentButton.s_defaultBackgroundColor;
        public Color BackgroundColor
        {
            get => b_BackgroundColor;
            set
            {
                if (SetField(ref b_BackgroundColor, value))
                    UpdateSegments(s => s.BackgroundColor = value);
            }
        }

        Color b_SelectedTextColor = SegmentButton.s_defaultSelectedTextColor;
        public Color SelectedTextColor
        {
            get => b_SelectedTextColor;
            set
            {
                if (SetField(ref b_SelectedTextColor, value))
                    UpdateSegments(s => s.SelectedTextColor = value);
            }
        }

        Color b_SelectedBackgroundColor = SegmentButton.s_defaultSelectedBackgroundColor;
        public Color SelectedBackgroundColor
        {
            get => b_SelectedBackgroundColor;
            set
            {
                if (SetField(ref b_SelectedBackgroundColor, value))
                    UpdateSegments(s => s.SelectedBackgroundColor = value);
            }
        }

        Color b_OutlineColor = s_defaultOutlineColor;
        public Color OutlineColor
        {
            get => b_OutlineColor;
            set
            {
                if (SetField(ref b_OutlineColor, value))
                    UpdateOutlineColor();
            }
        }


        int b_OutlineWidth = s_defaultOutlineWidth;
        public int OutlineWidth
        {
            get => b_OutlineWidth;
            set
            {
                if (SetField(ref b_OutlineWidth, value))
                    UpdateOutlineWidth();
            }
        }

        float b_OutlineRadius = SegmentButton.s_defaultCornerRadius + s_defaultOutlineWidth;
        public float OutlineRadius
        {
            get => b_OutlineRadius;
            set
            {
                if (SetField(ref b_OutlineRadius, value))
                    UpdateRadii();
            }
        }

        Thickness b_SegmentPadding = SegmentButton.sDefaultPadding;
        public Thickness SegmentPadding
        {
            get => b_SegmentPadding;
            set
            {
                if (SetField(ref b_SegmentPadding, value))
                    UpdateSegments(s => s.SetPadding(b_SegmentPadding));
            }
        }

        #endregion



        #region Fields

        //ShapeDrawable m_Shape = new ShapeDrawable();
        //LayerDrawable m_layer;
        //InsetDrawable m_Inset;
        Paint m_paint = new Paint();
        #endregion

        public SegmentedControl() : base(P42.Utils.Droid.Settings.Context)
        {
            Init();
        }

        public SegmentedControl(Context context) : base(context)
        {
            Init();
        }

        public SegmentedControl(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public SegmentedControl(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public SegmentedControl(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public SegmentedControl(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }


        void Init()
        {
            //m_layer = new LayerDrawable(new Drawable[] { m_Shape });
            //Background = m_layer;
            m_paint.AntiAlias = true;
            m_paint.SetStyle(Paint.Style.Fill);
            SetWillNotDraw(false);

            UpdateOutlineWidth();
            UpdateOutlineColor();
            UpdateRadii();
            SetPadding(PaddingLeft, PaddingTop, PaddingRight, PaddingBottom);
        }

        void UpdateRadii()
        {
            UpdateSegments(s => s.CornerRadius = OutlineRadius - OutlineWidth);
            //var shape = new RoundRectShape(new float[] { OutlineRadius, OutlineRadius, OutlineRadius, OutlineRadius, OutlineRadius, OutlineRadius, OutlineRadius, OutlineRadius }, null, null);
            //m_Shape.Shape = shape;
        }

        void UpdateOutlineColor()
        {
            //InvalidateOutline();  // worth looking into
            //m_Shape.Paint.Color = OutlineColor;
        }

        void UpdateOutlineWidth()
        {
           // m_Shape.Paint.StrokeWidth = OutlineWidth;
        }

        public override void SetPadding(int left, int top, int right, int bottom)
        {
            //var inset = new InsetDrawable(m_Shape, PaddingLeft, PaddingTop, PaddingRight, PaddingBottom);
            //var paddingCheck = inset.GetPadding(new Rect(PaddingLeft, PaddingTop, PaddingRight, PaddingBottom));
            //var item = m_layer.GetDrawable(0);
            //var x = item.GetPadding(new Rect(10, 10, 10, 10));
            
            base.SetPadding(left, top, right, bottom);
        }

        public override void OnViewAdded(View child)
        {
            base.OnViewAdded(child);
            if (child is SegmentButton segment)
            {
                InitiateSegment(segment);
                IndexSegments();
            }
            else
                RemoveView(child);
        }

        public override void OnViewRemoved(View child)
        {
            base.OnViewRemoved(child);
            if (child is SegmentButton)
                IndexSegments();
        }

        void IndexSegments()
        {
            for (int i = 0; i < ChildCount; i++)
            {
                if (GetChildAt(i) is SegmentButton segment)
                {
                    segment.Index = i;
                    if (ChildCount == 1)
                        segment.Position = SegmentPosition.Only;
                    else if (i == 0)
                        segment.Position = SegmentPosition.First;
                    else if (i == ChildCount - 1)
                        segment.Position = SegmentPosition.Last;
                    else
                        segment.Position = SegmentPosition.Middle;
                }
            }
        }

        void InitiateSegment(SegmentButton segment)
        {
            segment.CornerRadius = OutlineRadius - OutlineWidth;
            segment.TextColor = TextColor;
            segment.SelectedTextColor = SelectedTextColor;
            segment.BackgroundColor = BackgroundColor;
            segment.SelectedBackgroundColor = SelectedBackgroundColor;
            segment.IsHorizontal = IsHorizontal;
        }

        internal void OnSegmentClicked(SegmentButton segment)
        {
            if (SelectionMode == SelectionMode.None)
                segment.Selected = false;
            else if (SelectionMode == SelectionMode.Radio)
                UpdateSegments(s => s.Selected = s == segment);
            else if (SelectionMode == SelectionMode.Single)
            {
                if (segment.Selected)
                    segment.Selected = false;
                else
                    UpdateSegments(s => s.Selected = s == segment);
            }
            else if (SelectionMode == SelectionMode.Multi)
                segment.Selected = !segment.Selected;
        }

        void UpdateSegments(Action<SegmentButton> action, SegmentButton except = null)
        {
            foreach (var child in this.Children())
            {
                if (child != except && child is SegmentButton button)
                    action?.Invoke(button);
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            m_paint.Color = OutlineColor;
            canvas.DrawRoundRect(
                new RectF(0 + PaddingLeft, 0 + PaddingTop, canvas.Width - PaddingRight, canvas.Height - PaddingBottom),
                OutlineRadius, OutlineRadius,
                m_paint);
            base.OnDraw(canvas);
        }

        public override void OnDrawForeground(Canvas canvas)
        {
            base.OnDrawForeground(canvas);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l + OutlineWidth, t + OutlineWidth, r - OutlineWidth, b - OutlineWidth);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var availableWidth = MeasureSpec.GetSize(widthMeasureSpec) - 2 * OutlineWidth;
            var availableHeight = MeasureSpec.GetSize(heightMeasureSpec) - 2 * OutlineWidth;
            var hzMode = MeasureSpec.GetMode(widthMeasureSpec);
            var vtMode = MeasureSpec.GetMode(heightMeasureSpec);

            base.OnMeasure(
                MeasureSpec.MakeMeasureSpec(availableWidth, hzMode),
                MeasureSpec.MakeMeasureSpec(availableHeight, vtMode));

            SetMeasuredDimension(MeasuredWidth + 2 * OutlineWidth, MeasuredHeight + 2 * OutlineWidth);

        }

        #region Property Change Handler
        protected List<string> BatchedPropertyChanges = new List<string>();

        int _batchChanges;
        [JsonIgnore]
        public bool BatchChanges
        {
            get => _batchChanges > 0;
            set
            {
                if (value)
                    _batchChanges++;
                else
                    _batchChanges--;
                _batchChanges = Math.Max(0, _batchChanges);
                if (_batchChanges == 0)
                {
                    var propertyNames = new List<string>(BatchedPropertyChanges);
                    BatchedPropertyChanges.Clear();
                    foreach (var propertyName in propertyNames)
                        OnPropertyChanged(propertyName);
                }
            }
        }

        [JsonIgnore]
        public bool HasChanged { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
            => HasChanged = false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(TextColor))
            {
                foreach (var segment in this.Children().Cast<SegmentButton>())
                {

                }
            }

            if (BatchChanges)
                BatchedPropertyChanges.Add(propertyName);
            else
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            if (propertyName == null)
                throw new Exception("OnPropertyChanged: null propertyName in SetField");

            field = value;
            HasChanged = true;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion


    }
}
