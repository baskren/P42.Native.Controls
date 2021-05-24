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
        { element.Header = value; return element; }

        public static ElementType Footer<ElementType>(this ElementType element, UIElement value) where ElementType : ListView
        { element.Footer = value; return element; }

        public static ElementType IsItemClickEnabled<ElementType>(this ElementType element, bool value) where ElementType : ListView
        { element.IsItemClickEnabled = value; return element; }

        public static ElementType SelectedItems<ElementType>(this ElementType element, IEnumerable value) where ElementType : ListView
        { element.SelectedItems = value; return element; }

        public static ElementType SelectionMode<ElementType>(this ElementType element, SelectionMode value) where ElementType : ListView
        { element.SelectionMode = value; return element; }

        public static ElementType SelectedIndex<ElementType>(this ElementType element, int value) where ElementType : ListView
        { element.SelectedIndex = value; return element; }

        public static ElementType SelectedItem<ElementType>(this ElementType element, object value) where ElementType : ListView
        { element.SelectedItem = value; return element; }

        public static ElementType ItemsSource<ElementType>(this ElementType element, IEnumerable value) where ElementType : ListView
        { element.ItemsSource = value; return element; }

        public static ElementType ItemViewType<ElementType>(this ElementType element, Type value) where ElementType : ListView
        { element.ItemViewType = value; return element; }

        public static ElementType ItemViewTypeSelector<ElementType>(this ElementType element, IItemTypeSelector value) where ElementType : ListView
        { element.ItemViewTypeSelector = value; return element; }

        public static ElementType ItemClick<ElementType>(this ElementType element, ItemClickEventHandler value) where ElementType : ListView
        { element.ItemClick += value; return element; }

        public static ElementType SelectionChanged<ElementType>(this ElementType element, SelectionChangedEventHandler value) where ElementType : ListView
        { element.SelectionChanged += value; return element; }

    }
}