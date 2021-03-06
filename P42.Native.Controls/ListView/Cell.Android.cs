using System;
using Android.Views;
using Android.Graphics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace P42.Native.Controls
{
    public partial class Cell : ViewGroup, IDisposable
    {

        #region Construction / Disposal

        public Cell(ListView listView) : base(listView.Context)
        {
            BuildCommon(listView);

            Click += OnClicked;

            SetWillNotDraw(false);

            SetBackgroundColor(Color.Gold);

            LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                DisposeCommon();

                var children = this.Children();
                RemoveAllViews();
                foreach (var child in children)
                    child.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion


        #region Event Handlers
        async void OnClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Cell[{Index}] Tapped");
            await (ListView?.OnCellTapped(this) ?? Task.CompletedTask);
        }

        #endregion


        #region Partial Methods

        partial void UpdateSelection()
        {
            SetBackgroundColor(IsSelected ? SelectedColor : Color.Transparent);
        }

        partial void OnIsEnabledChanged()
        {
            Enabled = IsEnabled;
        }
        #endregion


        #region Android Measure / Layout / Draw
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {

            MeasureChildren(widthMeasureSpec, heightMeasureSpec);

            var height = 0;
            var width = 0;

            foreach (var child in this.Children())
            {
                width = Math.Max(child.MeasuredWidth, width) ;
                height = Math.Max(child.MeasuredHeight, height);
            }

            System.Diagnostics.Debug.WriteLine($"Cell.OnMeasure : height = {height}");

            SetMeasuredDimension(width, height);
        }

        int yOffset = -1;
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            yOffset = t;
            System.Diagnostics.Debug.WriteLine($"Cell.OnLayout({l}, {t}, {r}, {b})");
            foreach (var child in this.Children())
                child.Layout(0, 0, r-l, b-t);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            HasDrawn = true;
            ActualSize = new SizeI(canvas.Width, canvas.Height);

            System.Diagnostics.Debug.WriteLine($"Cell OnDraw: ActualSize: " + ActualSize );

            if (yOffset + canvas.Height >= ListView.ActualHeight)
            {
                ListView.OnDrawCellsComplete();
            }
        }


        TaskCompletionSource<bool> HasDrawnTaskCompletionSource;
        public async Task WaitForDrawComplete()
        {
            if (HasDrawn)
                return;
            HasDrawnTaskCompletionSource = HasDrawnTaskCompletionSource ?? new TaskCompletionSource<bool>();
            await HasDrawnTaskCompletionSource.Task;
        }

        #endregion


        #region INotifiable

        #region Methods
        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(RequestedWidth) ||
                propertyName == nameof(RequestedHeight) ||
                propertyName == nameof(HorizontalAlignment) ||
                propertyName == nameof(VerticalAlignment))
                UpdateLayoutParams();
            else if (propertyName == nameof(MinWidth))
                UpdateMinWidth();
            else if (propertyName == nameof(MinHeight))
                UpdateMinHeight();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RedrawElement() => PostInvalidate();

        public void RelayoutElement() => RequestLayout();

        #endregion

        #endregion


        #region Support Methods
        void UpdateLayoutParams()
        {
            /*
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
            */
            if (HasDrawn)
                RequestLayout();
        }

        void UpdateMinWidth()
        {
            //SetMinimumWidth(MinWidth);
            UpdateLayoutParams();
        }

        void UpdateMinHeight()
        {
            //SetMinimumHeight(MinHeight);
            UpdateLayoutParams();
        }


        #endregion



    }
}
