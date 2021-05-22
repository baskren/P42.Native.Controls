using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Android.Views;
using Android.Runtime;
using Android.Content;
using Android.Util;

namespace P42.Native.Controls
{

    class ListViewAdapter : Android.Widget.BaseAdapter<object>
    {
        IEnumerable<object> Items;
        Type ItemViewType => ListView.ItemViewType;
        IItemViewFactory TemplateSelector => ListView.ItemViewFactory;
        ListView ListView;
        //List<DataTemplate> Templates = new List<DataTemplate>();

        public ListViewAdapter(ListView listView)
        {
            ListView = listView;
            SetItems(ListView.ItemsSource);
        }

        public void SetItems(IEnumerable<object> items)
        {
            if (items != Items)
            {
                if (Items is INotifyCollectionChanged oldCollection)
                    oldCollection.CollectionChanged -= OnCollectionChaged;
                Items = items;
                if (Items is INotifyCollectionChanged newCollection)
                    newCollection.CollectionChanged += OnCollectionChaged;
                NotifyDataSetChanged();
            }
        }

        private void OnCollectionChaged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged();
        }



        public override object this[int position]
        {
            get
            {
                if (position > -1 && position <= Items.Count())
                    return Items?.ElementAt(position);
                return null;
            }
        }

        public override int Count
            => Items?.Count() ?? 0;

        public override long GetItemId(int position) => position;

        public override int ViewTypeCount
        {
            get
            {
                if (ItemViewType != null || TemplateSelector is null)
                    return 1;
                return TemplateSelector.ViewTypes.Count() + 1;
            }
        }

        public override int GetItemViewType(int position)
        {
            if (ItemViewType != null || TemplateSelector is null)
                return 0;
            if (TemplateSelector?.GetViewType(this[position]) is Type viewType)
                return TemplateSelector.ViewTypes.IndexOf(viewType) + 1;
            return 0;
        }

        // ALWAYS SET INDEX BEFORE DATACONTEXT
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView is CellWrapper wrapper)
            {
                wrapper.Index = position;
                wrapper.DataContext = this[position];
                return convertView;
            }

            if (ItemTemplate?.LoadContent() is FrameworkElement newElement)
            {
                return new CellWrapper(ListView)
                {
                    Child = newElement,
                    Index = position,
                    DataContext = this[position]
                };
            }

            if (TemplateSelector?.SelectTemplate(this[position]) is DataTemplate template)
            {
                if (template.LoadContent() is FrameworkElement newSelectedElement)
                    return new CellWrapper(ListView)
                    {
                        Child = newSelectedElement,
                        Index = position,
                        DataContext = this[position]
                    };
            }

            return new CellWrapper(ListView)
            {
                Child = new Cell(),
                Index = position,
                DataContext = this[position],
            };
        }

    }


    partial class TextCell : Cell
    {


        public TextCell(ListView listView) : base(listView)
        {

        }

        public override void OnDataContextChanged()
        {
            base.OnDataContextChanged();
        }
    }

}
