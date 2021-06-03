using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Android.Views;
using Android.Runtime;
using Android.Content;
using Android.Util;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace P42.Native.Controls
{

    class ListViewAdapter : Android.Widget.BaseAdapter<object>
    {
        IEnumerable Items;
        Type ItemViewType => ListView?.DipItemViewType;
        IItemTypeSelector TemplateSelector => ListView?.DipItemViewTypeSelector;
        ListView ListView;
        List<Cell> ActiveCells = new List<Cell>();

        public ListViewAdapter(ListView listView)
        {
            ListView = listView;
            SetItems(ListView.DipItemsSource);
        }

        bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                foreach (var cell in ActiveCells)
                    cell.Dispose();
                ActiveCells.Clear();
                ListView = null;
                Items = null;
            }
            base.Dispose(disposing);
        }

        public void SetItems(IEnumerable items)
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
            {
                var index = 1;
                foreach (var type in TemplateSelector.ViewTypes)
                {
                    if (type == viewType)
                        return index;
                    index++;
                }
            }
            return 0;
        }

        // ALWAYS SET INDEX BEFORE DATACONTEXT
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Cell cell = convertView as Cell;
            if (cell is null)
            {
                Type cellType = null;
                if (TemplateSelector?.GetViewType(this[position]) is Type t1 && typeof(Cell).IsAssignableFrom(t1))
                    cellType = t1;
                else if (ItemViewType is Type t2 && typeof(Cell).IsAssignableFrom(t2))
                    cellType = t2;
                if (cellType != null)
                    ActiveCells.Add(cell = (Cell)Activator.CreateInstance(cellType, new object[] { ListView }));
            }

            if (cell is null)
                ActiveCells.Add(cell = new TextCell(ListView));

            cell.DipIndex = position;
            cell.DipDataContext = this[position];
            //cell.Background = new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Orange);
            cell.Invalidate();
            return cell;
        }

    }


    partial class TextCell : Cell
    {
        Label Label;

        public TextCell(ListView listView) : base(listView)
        {
            Label = new Label(Context);
            AddView(Label);
        }

        public override void DipOnDataContextChanged()
        {
            base.DipOnDataContextChanged();
            Label.Text = DipDataContext.ToString();
        }
    }

}
