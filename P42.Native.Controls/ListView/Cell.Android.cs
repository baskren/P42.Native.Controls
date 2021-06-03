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

            NtvBaseView = this;
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Click -= OnClicked;
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
            System.Diagnostics.Debug.WriteLine($"Cell[{DipIndex}] Tapped");
            await (ListView?.DipOnCellTapped(this) ?? Task.CompletedTask);
        }

        #endregion


        #region Partial Methods

        partial void UpdateSelection()
        {
            SetBackgroundColor(DipIsSelected ? DipSelectedColor : Color.Transparent);
        }

        partial void OnIsEnabledChanged()
        {
            Enabled = DipIsEnabled;
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

            //System.Diagnostics.Debug.WriteLine($"Cell.OnMeasure : height = {height}");

            SetMeasuredDimension(width, height);
        }

        int yOffset = -1;
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            yOffset = t;
            //System.Diagnostics.Debug.WriteLine($"Cell.OnLayout({l}, {t}, {r}, {b})");
            foreach (var child in this.Children())
                child.Layout(0, 0, r-l, b-t);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            DipHasDrawn = true;
            NtvActualSize = new SizeI(canvas.Width, canvas.Height);

            //System.Diagnostics.Debug.WriteLine($"Cell OnDraw: ActualSize: " + NtvActualSize );

            if (yOffset + canvas.Height >= ListView.NtvActualHeight)
            {
                ListView.OnDrawCellsComplete();
            }
        }
        #endregion




    }
}
