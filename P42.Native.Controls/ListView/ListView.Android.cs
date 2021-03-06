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
        public ListView() : this(P42.Utils.Droid.Settings.Context) { }

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
                Header?.Dispose();
                Footer?.Dispose();
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
            HasDrawn = false;
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
            if (Footer is View oldFooter)
                _nativeListView.RemoveFooterView(oldFooter);
        }

        void RemoveHeader()
        {
            if (Header is View oldHeader)
                _nativeListView.RemoveHeaderView(oldHeader);
        }

        void AddHeader()
        {
            if (Header is View header)
                _nativeListView.AddHeaderView(header);
        }

        void AddFooter()
        {
            if (Footer is View footer)
                _nativeListView.AddFooterView(footer);
        }
        #endregion


        #region Selection

        internal async partial Task OnCellTapped(Cell cell)
        {
            System.Diagnostics.Debug.WriteLine("ListView. CLICK");
            SelectItem(cell.DataContext);
            await Task.Delay(10);
            if (IsItemClickEnabled)
                ItemClick?.Invoke(this, new ItemClickEventArgs(this, cell.DataContext, cell));
        }
        #endregion


        #region Scrolling
        public async partial Task ScrollIntoView(object item, ScrollIntoViewAlignment alignment)
        {
            if (ItemsSource.IndexOf(item) is int index && index > -1)
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
            ActualSize = new SizeI(canvas.Width, canvas.Height);
            System.Diagnostics.Debug.WriteLine($"ListView.OnDraw EXIT : {ActualSize}");
        }

        internal void OnDrawCellsComplete()
        {
            if (_swappingOutNativeListView)
                return;
            HasDrawn = true;
        }

        TaskCompletionSource<bool> HasDrawnTaskCompletionSource;
        public async Task WaitForDrawComplete()
        {
            if (HasDrawn)
                return;
            HasDrawnTaskCompletionSource = HasDrawnTaskCompletionSource ?? new TaskCompletionSource<bool>();
            await HasDrawnTaskCompletionSource.Task;
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
        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Header))
                RemoveHeader();
            else if (propertyName == nameof(Footer))
                RemoveFooter();
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
            else if (propertyName == nameof(Header))
                AddHeader();
            else if (propertyName == nameof(Footer))
                AddFooter();
            else if (propertyName == nameof(SelectedItem))
                SelectItem(SelectedItem);
            else if (propertyName == nameof(ItemsSource))
                _adapter.SetItems(ItemsSource);
            else if (propertyName == nameof(HasDrawn) && HasDrawn)
                HasDrawnTaskCompletionSource?.TrySetResult(true);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RedrawElement() => PostInvalidate();

        public void RelayoutElement() => RequestLayout();
        #endregion

        #endregion


        #region Support Methods
        void UpdateLayoutParams()
        {
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
            if (HasDrawn)
                RequestLayout();
        }

        void UpdateMinWidth()
        {
            SetMinimumWidth(MinWidth);
            UpdateLayoutParams();
        }

        void UpdateMinHeight()
        {
            SetMinimumHeight(MinHeight);
            UpdateLayoutParams();
        }
        #endregion


    }



}
