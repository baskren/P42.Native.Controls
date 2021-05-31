using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace P42.Native.Controls
{
    public delegate void ItemClickEventHandler(object sender, ItemClickEventArgs e);

    public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);

    public class ItemClickEventArgs
    {
        public object OriginalSource { get; private set; }

        public object ClickedItem { get; private set; }

        public Cell CellElement { get; private set; }

        internal ItemClickEventArgs(object simpleListView, object clickedItem, Cell cellElement)
        {
            OriginalSource = simpleListView;
            ClickedItem = clickedItem;
            CellElement = cellElement;
        }
    }

    public class SelectionChangedEventArgs
    {
        public object OriginalSource { get; private set; }

        public IList RemovedItems
        {
            get;
            private set;
        }

        public IList AddedItems
        {
            get;
            private set;
        }

        public SelectionChangedEventArgs(object simpleListView, IList removedItems, IList addedItems)
        {
            OriginalSource = simpleListView;
            RemovedItems = removedItems;
            AddedItems = addedItems;
        }

    }
}
