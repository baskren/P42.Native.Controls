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
    [AddTrait(typeof(TControl))]
    [AddTrait(typeof(TElement))]
    [AddTrait(typeof(TNotifiable))]
    public partial class ListView : IElement
    {

        #region Properties

        Element b_Header;
        public Element Header
        {
            get => b_Header;
            set => SetField(ref b_Header, value);
        }

        Element b_Footer;
        public Element Footer
        {
            get => b_Footer;
            set => SetField(ref b_Footer, value);
        }

        bool b_IsItemClickEnabled;
        public bool IsItemClickEnabled
        {
            get => b_IsItemClickEnabled;
            set => SetField(ref b_IsItemClickEnabled, value);
        }

        internal ObservableCollection<object> b_SelectedItems = new ObservableCollection<object>();
        public IEnumerable SelectedItems
        {
            get => b_SelectedItems;
            set
            {
                var deleteItems = b_SelectedItems.ToList();
                foreach (var item in value)
                {
                    if (!b_SelectedItems.Contains(item))
                        b_SelectedItems.Add(item);
                    deleteItems.Remove(item);
                }
                foreach (var item in deleteItems)
                    b_SelectedItems.Remove(item);
            }
        }

        SelectionMode b_SelectionMode;
        public SelectionMode SelectionMode
        {
            get => b_SelectionMode;
            set => SetField(ref b_SelectionMode, value, UpdateSelectionMode);
        }

        int b_SelectedIndex;
        public int SelectedIndex
        {
            get => b_SelectedIndex;
            set => SelectIndex(value);
        }

        object b_SelectedItem;
        public object SelectedItem
        {
            get => b_SelectedItem;
            set => SelectItem(value);
        }

        IEnumerable b_ItemsSource;
        public IEnumerable ItemsSource
        {
            get => b_ItemsSource;
            set => SetField(ref b_ItemsSource, value);
        }

        Type b_ItemViewType;
        public Type ItemViewType
        {
            get => b_ItemViewType;
            set => SetField(ref b_ItemViewType, value, UpdateNativeListView);
        }


        IItemTypeSelector b_ItemViewTypeSelector;
        public IItemTypeSelector ItemViewTypeSelector
        {
            get => b_ItemViewTypeSelector;
            set => SetField(ref b_ItemViewTypeSelector, value, UpdateNativeListView);
        }
        #endregion


        #region Events
        public event ItemClickEventHandler ItemClick;
        public event SelectionChangedEventHandler SelectionChanged;
        #endregion


        void SharedBuild()
        {
            HorizontalAlignment = Alignment.Stretch;
            VerticalAlignment = Alignment.Stretch;
        }

        #region Scroll
        public partial Task ScrollIntoView(object item, ScrollIntoViewAlignment alignment);

        #endregion

        #region Selection
        internal partial Task OnCellTapped(Cell cell);

        private void UpdateSelectionMode()
        {
            if (SelectionMode == SelectionMode.None)
                b_SelectedItems.Clear();
            else if (SelectionMode != SelectionMode.Multi)
            {
                if (b_SelectedItems.FirstOrDefault() is object first && first != null)
                {
                    var items = b_SelectedItems.ToArray();
                    foreach (var item in items)
                        if (item != first)
                            b_SelectedItems.Remove(item);
                }
            }
        }

        public void SelectAll()
        {
            if (SelectionMode == SelectionMode.Multi)
            {
                foreach (var item in ItemsSource)
                {
                    if (!b_SelectedItems.Contains(item))
                        b_SelectedItems.Add(item);
                }
            }
        }

        public void DeselectAll()
        {
            b_SelectedItems.Clear();
        }

        void SelectIndex(int index)
        {
            if (SelectionMode != SelectionMode.None)
            {
                if (ItemsSource is IList collection)
                {
                    SelectedItem = collection[index];
                    return;
                }
                int i = 0;
                foreach (var item in ItemsSource)
                {
                    if (i == index)
                    {
                        SelectItem(item);
                        return;
                    }
                    i++;
                }
            }
        }

        void SelectItem(object item)
        {
            if (SelectionMode != SelectionMode.None)
            {
                if (_repondingToSelectedItemsCollectionChanged)
                    return;
                if (SelectionMode == SelectionMode.Single)
                {
                    if (!b_SelectedItems.Contains(item))
                    {
                        b_SelectedItems.Clear();
                        b_SelectedItems.Add(item);
                    }
                }
                else if (SelectionMode == SelectionMode.Multi)
                {
                    if (b_SelectedItems.Contains(item))
                        b_SelectedItems.Remove(item);
                    else
                        b_SelectedItems.Add(item);
                }
            }
        }

        bool _repondingToSelectedItemsCollectionChanged;
        private void OnSelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _repondingToSelectedItemsCollectionChanged = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    if (e.NewItems?.Any() ?? false)
                        SelectedItem = e.NewItems[0];
                    else if (SelectedItem != null && (e.OldItems?.Contains(SelectedItem) ?? false))
                        SelectedItem = null;
                    SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(this, e.OldItems, e.NewItems));
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