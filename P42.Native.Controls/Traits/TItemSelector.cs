using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using SmartTraitsDefs;

#if __ANDROID__
using UIElement = Android.Views.View;
#endif

namespace P42.Native.Controls
{
    [SimpleTrait]
    public partial class TItemSelector<T,U> : TNotifiable where T : class where U : UIElement
    {
        #region Properties
        bool b_DipIsItemClickEnabled = true;
        public bool DipIsItemClickEnabled
        {
            get => b_DipIsItemClickEnabled;
            set => SetField(ref b_DipIsItemClickEnabled, value);
        }

        internal P42.Utils.ObservableConcurrentCollection<T> b_DipSelectedItems = new P42.Utils.ObservableConcurrentCollection<T>();
        public IEnumerable<T> DipSelectedItems
        {
            get => b_DipSelectedItems;
            set
            {
                var deleteItems = b_DipSelectedItems.ToList();
                foreach (var item in value)
                {
                        if (!b_DipSelectedItems.Contains(item))
                            b_DipSelectedItems.Add(item);
                        deleteItems.Remove(item);
                }
                foreach (var item in deleteItems)
                    b_DipSelectedItems.Remove(item);
            }
        }

        SelectionMode b_DipSelectionMode;
        public SelectionMode DipSelectionMode
        {
            get => b_DipSelectionMode;
            set => SetField(ref b_DipSelectionMode, value, DipUpdateSelectionMode);
        }

        int b_DipSelectedIndex;
        public int DipSelectedIndex
        {
            get => b_DipSelectedIndex;
            set
            {
                if (DipSelectionMode != SelectionMode.None)
                {
                    try
                    {
                        var item = DipItemsSource.ElementAt<T>(value);
                        DipSelectItem(item);
                    }
                    catch
                    {
                        DipSelectItem(null);
                    }
                }
            }
        }

        T b_DipSelectedItem;
        public T DipSelectedItem
        {
            get => b_DipSelectedItem;
            set
            {
                if (DipSelectionMode != SelectionMode.None)
                    SetField(ref b_DipSelectedItem, value, () => DipSelectItem(value));
            }
        }

        IEnumerable<T> b_DipItemsSource;
        public IEnumerable<T> DipItemsSource
        {
            get => b_DipItemsSource;
            set => SetField(ref b_DipItemsSource, value);
        }


        #endregion


        #region Events
        public event ItemClickEventHandler<U> DipItemClick;
        public event SelectionChangedEventHandler DipSelectionChanged;
        #endregion


        #region Construct / Dispose
        void TItemSelectorConstruct()
        {
            b_DipSelectedItems.CollectionChanged += DipOnSelectedItems_CollectionChanged;
        }

        void TItemSelectorDispose()
        {
            b_DipSelectedItems.CollectionChanged -= DipOnSelectedItems_CollectionChanged;

        }
        #endregion

        #region Methods
        internal async Task DipOnItemTapped(T item, U cell)
        {
            System.Diagnostics.Debug.WriteLine("ListView. CLICK");
            if (DipSelectionMode != SelectionMode.None)
            {
                if (b_DipSelectedItems.Contains(item))
                {
                    if (DipSelectionMode != SelectionMode.Radio)
                    {
                        DipDeselectItem(item);
                    }
                }
                else
                    DipSelectItem(item);
            }
            await Task.Delay(10);
            if (DipIsItemClickEnabled)
                DipItemClick?.Invoke(this, new ItemClickEventArgs<U>(this, item, cell));
        }

        private void DipUpdateSelectionMode()
        {
            if (DipSelectionMode == SelectionMode.None)
                b_DipSelectedItems.Clear();
            else if (DipSelectionMode != SelectionMode.Multi)
            {
                if (b_DipSelectedItems.Any() && b_DipSelectedItems.LastOrDefault() is T last)
                {
                    var items = b_DipSelectedItems.ToArray();
                    foreach (var item in items)
                        if (!item.Equals(last))
                        //if (item != last)
                            b_DipSelectedItems.Remove(item);
                }
            }
        }

        public void DipSelectAll()
        {
            if (DipSelectionMode == SelectionMode.Multi)
            {
                _manuallyCallingSelectionChanged = true;
                var newItems = new List<T>();
                foreach (var item in DipItemsSource)
                {
                    if (b_DipSelectedItems.Contains(item))
                        newItems.Add(item);
                }
                b_DipSelectedItems.AddRange(newItems);

                _manuallyCallingSelectionChanged = false;
            }
        }

        public void DipSelectItem(T item)
        {

            if (DipSelectionMode != SelectionMode.None && !DipSelectedItem.Equals(item))
            {
                if (item is null)
                {
                    b_DipSelectedItems.Clear();
                    DipSelectedItem = null;
                    DipSelectedIndex = -1;
                    return;
                }

                _manuallyCallingSelectionChanged = true;
                List<object> oldItems = null;

                if (DipSelectionMode != SelectionMode.Multi)
                {
                    oldItems = new List<object>(b_DipSelectedItems);
                    b_DipSelectedItems.Clear();
                }
                else if (b_DipSelectedItems.IndexOf(item) is int currentIndex && currentIndex > -1)
                {
                    b_DipSelectedItems.Move(currentIndex, b_DipSelectedItems.Count - 1);
                }

                b_DipSelectedItems.Add(item);
                var newItems = new List<object> { item };
                DipSelectedItem = item;
                DipSelectedIndex = DipItemsSource.IndexOf(item);

                DipSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(this, oldItems, newItems));
                _manuallyCallingSelectionChanged = false;
            }
        }

        public void DipDeselectItem(T item)
        {
            if (b_DipSelectedItems.Contains(item))
                b_DipSelectedItems.Remove(item);

            if (DipSelectedItem == item)
            {
                if (b_DipSelectedItems.Any())
                {
                    DipSelectedItem = b_DipSelectedItems.Last();
                    b_DipSelectedIndex = DipItemsSource.IndexOf(DipSelectedItem);
                }
                else
                {
                    DipSelectedIndex = -1;
                    DipSelectedItem = null;
                }
            }
        }

        bool _manuallyCallingSelectionChanged;
        private void DipOnSelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_manuallyCallingSelectionChanged)
                DipSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(this, e.OldItems, e.NewItems));
        }
        #endregion
    }
}
