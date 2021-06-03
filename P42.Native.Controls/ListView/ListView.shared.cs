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

        internal ObservableCollection<object> b_DipSelectedItems = new ObservableCollection<object>();
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
            set => DipSelectIndex(value);
        }

        object b_DipSelectedItem;
        public object DipSelectedItem
        {
            get => b_DipSelectedItem;
            set => DipSelectItem(value);
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
        }

        #region Scroll
        public partial Task DipScrollIntoView(object item, ScrollIntoViewAlignment alignment);

        #endregion


        #region Selection
        internal partial Task DipOnCellTapped(Cell cell);

        private void DipUpdateSelectionMode()
        {
            if (DipSelectionMode == SelectionMode.None)
                b_DipSelectedItems.Clear();
            else if (DipSelectionMode != SelectionMode.Multi)
            {
                if (b_DipSelectedItems.FirstOrDefault() is object first && first != null)
                {
                    var items = b_DipSelectedItems.ToArray();
                    foreach (var item in items)
                        if (item != first)
                            b_DipSelectedItems.Remove(item);
                }
            }
        }

        public void DipSelectAll()
        {
            if (DipSelectionMode == SelectionMode.Multi)
            {
                foreach (var item in DipItemsSource)
                {
                    if (!b_DipSelectedItems.Contains(item))
                        b_DipSelectedItems.Add(item);
                }
            }
        }

        public void DipDeselectAll()
        {
            b_DipSelectedItems.Clear();
        }

        void DipSelectIndex(int index)
        {
            if (DipSelectionMode != SelectionMode.None)
            {
                if (DipItemsSource is IList collection)
                {
                    DipSelectedItem = collection[index];
                    return;
                }
                int i = 0;
                foreach (var item in DipItemsSource)
                {
                    if (i == index)
                    {
                        DipSelectItem(item);
                        return;
                    }
                    i++;
                }
            }
        }

        void DipSelectItem(object item)
        {
            if (DipSelectionMode != SelectionMode.None)
            {
                if (_repondingToSelectedItemsCollectionChanged)
                    return;
                if (DipSelectionMode == SelectionMode.Single)
                {
                    if (!b_DipSelectedItems.Contains(item))
                    {
                        b_DipSelectedItems.Clear();
                        b_DipSelectedItems.Add(item);
                    }
                }
                else if (DipSelectionMode == SelectionMode.Multi)
                {
                    if (b_DipSelectedItems.Contains(item))
                        b_DipSelectedItems.Remove(item);
                    else
                        b_DipSelectedItems.Add(item);
                }
            }
        }

        bool _repondingToSelectedItemsCollectionChanged;
        private void DipOnSelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _repondingToSelectedItemsCollectionChanged = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    if (e.NewItems?.Any() ?? false)
                        DipSelectedItem = e.NewItems[0];
                    else if (DipSelectedItem != null && (e.OldItems?.Contains(DipSelectedItem) ?? false))
                        DipSelectedItem = null;
                    DipSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(this, e.OldItems, e.NewItems));
                    break;
                case NotifyCollectionChangedAction.Move:
                default:
                    break;
            }
            _repondingToSelectedItemsCollectionChanged = false;
        }
        #endregion
    }
}