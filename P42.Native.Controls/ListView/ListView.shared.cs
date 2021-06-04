using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;
using SmartTraitsDefs;

#if __ANDROID__
using Element = Android.Views.View;
#endif

namespace P42.Native.Controls
{
    [AddSimpleTrait(typeof(TElement))]
    public partial class ListView 
    {

        #region Properties

        Element b_DipHeader;
        public Element DipHeader
        {
            get => b_DipHeader;
            set => SetField(ref b_DipHeader, value);
        }

        Element b_DipFooter;
        public Element DipFooter
        {
            get => b_DipFooter;
            set => SetField(ref b_DipFooter, value);
        }

        bool b_DipIsItemClickEnabled = true;
        public bool DipIsItemClickEnabled
        {
            get => b_DipIsItemClickEnabled;
            set => SetField(ref b_DipIsItemClickEnabled, value);
        }

        internal P42.Utils.ObservableConcurrentCollection<object> b_DipSelectedItems = new P42.Utils.ObservableConcurrentCollection<object>();
        public IEnumerable DipSelectedItems
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
                        var item = DipItemsSource.ElementAt(value);
                        DipSelectItem(item);
                    }
                    catch
                    {
                        DipSelectItem(null);
                    }
                }
            }
        }

        object b_DipSelectedItem;
        public object DipSelectedItem
        {
            get => b_DipSelectedItem;
            set
            {
                if (DipSelectionMode != SelectionMode.None)
                    SetField(ref b_DipSelectedItem, value, () => DipSelectItem(value));
            }
        }

        IEnumerable b_DipItemsSource;
        public IEnumerable DipItemsSource
        {
            get => b_DipItemsSource;
            set => SetField(ref b_DipItemsSource, value);
        }

        Type b_DipItemViewType;
        public Type DipItemViewType
        {
            get => b_DipItemViewType;
            set => SetField(ref b_DipItemViewType, value, UpdateNativeListView);
        }


        IItemTypeSelector b_DipItemViewTypeSelector;
        public IItemTypeSelector DipItemViewTypeSelector
        {
            get => b_DipItemViewTypeSelector;
            set => SetField(ref b_DipItemViewTypeSelector, value, UpdateNativeListView);
        }
        #endregion


        #region Events
        public event ItemClickEventHandler DipItemClick;
        public event SelectionChangedEventHandler DipSelectionChanged;
        #endregion


        void SharedBuild()
        {
            DipHorizontalAlignment = Alignment.Stretch;
            DipVerticalAlignment = Alignment.Stretch;
            b_DipSelectedItems.CollectionChanged += DipOnSelectedItems_CollectionChanged;
        }

        void SharedDispose()
        {
            b_DipSelectedItems.CollectionChanged -= DipOnSelectedItems_CollectionChanged;
        }

        #region Scroll
        public partial Task DipScrollIntoView(object item, ScrollIntoViewAlignment alignment);

        #endregion


        #region Selection

        internal async Task DipOnCellTapped(Cell cell)
        {
            System.Diagnostics.Debug.WriteLine("ListView. CLICK");
            if (DipSelectionMode != SelectionMode.None)
            {
                if (b_DipSelectedItems.Contains(cell.DipDataContext))
                {
                    if (DipSelectionMode != SelectionMode.Radio)
                    {
                        DipDeselectItem(cell.DipDataContext);
                    }
                }
                else
                    DipSelectItem(cell.DipDataContext);
            }
            await Task.Delay(10);
            if (DipIsItemClickEnabled)
                DipItemClick?.Invoke(this, new ItemClickEventArgs(this, cell.DipDataContext, cell));
        }

        private void DipUpdateSelectionMode()
        {
            if (DipSelectionMode == SelectionMode.None)
                b_DipSelectedItems.Clear();
            else if (DipSelectionMode != SelectionMode.Multi)
            {
                if (b_DipSelectedItems.LastOrDefault() is object last && last != null)
                {
                    var items = b_DipSelectedItems.ToArray();
                    foreach (var item in items)
                        if (item != last)
                            b_DipSelectedItems.Remove(item);
                }
            }
        }

        public void DipSelectAll()
        {
            if (DipSelectionMode == SelectionMode.Multi)
            {
                _manuallyCallingSelectionChanged = true;
                var newItems = new List<object>();
                foreach (var item in DipItemsSource)
                {
                    if (!b_DipSelectedItems.Contains(item))
                        newItems.Add(item);
                }
                b_DipSelectedItems.AddRange(newItems);

                _manuallyCallingSelectionChanged = false;
            }
        }

        public void DipSelectItem(object item)
        {

            if (DipSelectionMode != SelectionMode.None && DipSelectedItem != item)
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

                DipSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(this, oldItems,newItems));
                _manuallyCallingSelectionChanged = false;
            }
        }

        public void DipDeselectItem(object item)
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