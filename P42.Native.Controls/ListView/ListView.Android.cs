using Android.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.ObjectModel;
using Android.Runtime;
using Android.Util;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.Graphics;

namespace P42.Native.Controls
{
    public partial class ListView : ViewGroup
    {
        #region Fields
        internal ObservableCollection<int> PxCellHeights = new ObservableCollection<int>();
        Android.Widget.ListView _nativeListView;
        ListViewAdapter _adapter;
        int _waitingForIndex = -1;
        ScrollIntoViewAlignment _waitingAlignment;
        #endregion


        #region Constructors
        public ListView() : this(global::P42.Utils.Droid.Settings.Context) { }

        public ListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Build();
        }

        public ListView(Context context) : base(context)
        {
            Build();
        }

        public ListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Build();
        }

        public ListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Build();
        }

        public ListView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Build();
        }

        void Build()
        {
            NtvBaseView = this;
            SharedBuild();
            SetWillNotDraw(false);
            UpdateNativeListView();
            PxCellHeights.CollectionChanged += OnNativeCellHeights_CollectionChanged;
        }


        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                PxCellHeights.CollectionChanged -= OnNativeCellHeights_CollectionChanged;
                RemoveNativeListView();
                DipHeader?.Dispose();
                DipFooter?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region Rebuild ListView/Adapter/Header/Footer upon ItemViewType and ItemViewTypeSelector change
        bool _swappingOutNativeListView;
        void UpdateNativeListView()
        {
            _swappingOutNativeListView = true;
            RemoveNativeListView();

            _adapter = new ListViewAdapter(this);
            _nativeListView = new Android.Widget.ListView(Context)
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
                //Divider = null,
                Adapter = _adapter,
            };

            AddHeader();
            AddFooter();
            DipHasDrawn = false;
            _swappingOutNativeListView = false;
            AddView(_nativeListView);
        }

        void RemoveNativeListView()
        {
            if (_nativeListView != null)
            {
                _adapter?.NotifyDataSetInvalidated();
                _nativeListView.Adapter = null;
                _adapter?.Dispose();

                RemoveFooter();
                RemoveHeader();
                RemoveView(_nativeListView);
                _nativeListView.Dispose();
                PxCellHeights.Clear();
            }
        }

        void RemoveFooter()
        {
            if (DipFooter is View oldFooter)
                _nativeListView.RemoveFooterView(oldFooter);
        }

        void RemoveHeader()
        {
            if (DipHeader is View oldHeader)
                _nativeListView.RemoveHeaderView(oldHeader);
        }

        void AddHeader()
        {
            if (DipHeader is View header)
                _nativeListView.AddHeaderView(header);
        }

        void AddFooter()
        {
            if (DipFooter is View footer)
                _nativeListView.AddFooterView(footer);
        }
        #endregion


        #region Selection

        internal async partial Task DipOnCellTapped(Cell cell)
        {
            System.Diagnostics.Debug.WriteLine("ListView. CLICK");
            DipSelectItem(cell.DipDataContext);
            await Task.Delay(10);
            if (DipIsItemClickEnabled)
                DipItemClick?.Invoke(this, new ItemClickEventArgs(this, cell.DipDataContext, cell));
        }
        #endregion


        #region Scrolling
        public async partial Task DipScrollIntoView(object item, ScrollIntoViewAlignment alignment)
        {
            if (DipItemsSource.IndexOf(item) is int index && index > -1)
            {
                if (alignment == ScrollIntoViewAlignment.Default)
                {
                    _nativeListView.SmoothScrollToPosition(index);
                    return;
                }
                else if (alignment == ScrollIntoViewAlignment.Leading)
                {
                    _nativeListView.SmoothScrollToPositionFromTop(index, 0);
                    return;
                }
                if (index < PxCellHeights.Count)
                {
                    var cellHeight = PxCellHeights[index];
                    InternalScrollTo(index, alignment, cellHeight);
                }
                else
                {
                    var estCellHeight = (int)(PxCellHeights.Average() + 0.5);
                    _waitingForIndex = index;
                    _waitingAlignment = alignment;
                    InternalScrollTo(index, alignment, estCellHeight);
                }
                await Task.Delay(10);
            }
        }

        void InternalScrollTo(int index, ScrollIntoViewAlignment alignment, int cellHeight)
        {
            var viewHeight = _nativeListView.Height;
            if (alignment == ScrollIntoViewAlignment.Trailing)
                _nativeListView.SmoothScrollToPositionFromTop(index, viewHeight - cellHeight);
            else if (alignment == ScrollIntoViewAlignment.Center)
                _nativeListView.SmoothScrollToPositionFromTop(index, (viewHeight - cellHeight) / 2);
            else if (alignment == ScrollIntoViewAlignment.Default ||
                alignment == ScrollIntoViewAlignment.Leading)
                _nativeListView.SmoothScrollToPosition(index);
        }

        private void OnNativeCellHeights_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewStartingIndex > -1 && _waitingForIndex >= e.NewStartingIndex && _waitingForIndex < e.NewStartingIndex + e.NewItems.Count)
            {
                var index = _waitingForIndex;
                _waitingForIndex = -1;
                var cellHeight = PxCellHeights[index];
                InternalScrollTo(index, _waitingAlignment, cellHeight);
                _waitingAlignment = ScrollIntoViewAlignment.Default;
            }
        }
        #endregion


        #region Android Measure / Layout / Draw
        
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (_swappingOutNativeListView)
                return;
            _nativeListView.Measure(widthMeasureSpec, heightMeasureSpec);
            SetMeasuredDimension(_nativeListView.MeasuredWidth, _nativeListView.MeasuredHeight);
        }
        
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (_swappingOutNativeListView)
                return;
            _nativeListView.Layout(l, t, r, b);
            System.Diagnostics.Debug.WriteLine($"ListView.OnLayout({l}, {t}, {r}, {b}) EXIT : ");
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (_swappingOutNativeListView)
                return;

            base.OnDraw(canvas);

            //HasDrawn = true;
            NtvActualSize = new SizeI(canvas.Width, canvas.Height);
            System.Diagnostics.Debug.WriteLine($"ListView.OnDraw EXIT : {NtvActualSize}");
        }

        internal void OnDrawCellsComplete()
        {
            if (_swappingOutNativeListView)
                return;
            DipHasDrawn = true;
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            System.Diagnostics.Debug.WriteLine($"ListView.OnAttachedToWindow()");
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            System.Diagnostics.Debug.WriteLine($"ListView.OnDetachedFromWindow()");
        }
        #endregion



        #region INotifiable

        #region Methods
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(DipHeader))
                RemoveHeader();
            else if (propertyName == nameof(DipFooter))
                RemoveFooter();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(DipHeader))
                AddHeader();
            else if (propertyName == nameof(DipFooter))
                AddFooter();
            else if (propertyName == nameof(DipSelectedItem))
                DipSelectItem(DipSelectedItem);
            else if (propertyName == nameof(DipItemsSource))
                _adapter.SetItems(DipItemsSource);
        }
        #endregion

        #endregion


    }



}
