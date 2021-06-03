using System;
using System.Collections;

//using ElementType = P42.Native.Controls.ListView;

#if __ANDROID__
using UIElement = Android.Views.View;
#endif


namespace P42.Native.Controls
{
    public static class ListViewMarkupExtensions
    {
        public static ElementType Header<ElementType>(this ElementType element, UIElement value) where ElementType : ListView
        { element.DipHeader = value; return element; }

        public static ElementType Footer<ElementType>(this ElementType element, UIElement value) where ElementType : ListView
        { element.DipFooter = value; return element; }

        public static ElementType IsItemClickEnabled<ElementType>(this ElementType element, bool value) where ElementType : ListView
        { element.DipIsItemClickEnabled = value; return element; }

        public static ElementType SelectedItems<ElementType>(this ElementType element, IEnumerable value) where ElementType : ListView
        { element.DipSelectedItems = value; return element; }

        public static ElementType SelectionMode<ElementType>(this ElementType element, SelectionMode value) where ElementType : ListView
        { element.DipSelectionMode = value; return element; }

        public static ElementType SelectedIndex<ElementType>(this ElementType element, int value) where ElementType : ListView
        { element.DipSelectedIndex = value; return element; }

        public static ElementType SelectedItem<ElementType>(this ElementType element, object value) where ElementType : ListView
        { element.DipSelectedItem = value; return element; }

        public static ElementType ItemsSource<ElementType>(this ElementType element, IEnumerable value) where ElementType : ListView
        { element.DipItemsSource = value; return element; }

        public static ElementType ItemViewType<ElementType>(this ElementType element, Type value) where ElementType : ListView
        { element.DipItemViewType = value; return element; }

        public static ElementType ItemViewTypeSelector<ElementType>(this ElementType element, IItemTypeSelector value) where ElementType : ListView
        { element.DipItemViewTypeSelector = value; return element; }

        public static ElementType ItemClick<ElementType>(this ElementType element, ItemClickEventHandler value) where ElementType : ListView
        { element.DipItemClick += value; return element; }

        public static ElementType SelectionChanged<ElementType>(this ElementType element, SelectionChangedEventHandler value) where ElementType : ListView
        { element.DipSelectionChanged += value; return element; }

    }
}