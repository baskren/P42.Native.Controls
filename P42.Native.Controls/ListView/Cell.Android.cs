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

        #region INotifiable

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        #endregion


        #region Fields
        public bool HasDrawn { get; set; }
        public bool HasChanged { get; set; }
        #endregion


        #region Android Measure / Layout / Draw
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (Child is View view)
                view.Layout(l, t, r, b);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            HasDrawn = true;
            ActualSize = new SizeI(canvas.Width, canvas.Height);
        }
        #endregion


        #region Methods
        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Child))
            {
                if (Child is View view)
                    RemoveView(view);
            }
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Child))
            {
                if (Child is View view)
                    AddView(view);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RedrawElement() => PostInvalidate();

        public void RelayoutElement() => RequestLayout();
        #endregion

        #endregion

    }
}
