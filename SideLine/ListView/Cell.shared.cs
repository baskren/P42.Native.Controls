using System;

#if __ANDROID__
using Color = Android.Graphics.Color;
using UIElement = Android.Views.View;
#endif

namespace P42.Native.Controls
{
    public partial class Cell : P42.Native.Controls.IElement
    {

        #region  Properties
        public bool IsSelected
            => DataContext is null
            ? false
            : ListView?.b_SelectedItems.Contains(DataContext) ?? false;

        bool b_IsEnabled;
        public bool IsEnabled
        {
            get => b_IsEnabled;
            set => ((INotifiable)this).SetField(ref b_IsEnabled, value, OnIsEnabledChanged);
        }

        int b_Index;
        public int Index
        {
            get => b_Index;
            internal set => ((INotifiable)this).SetField(ref b_Index, value, UpdateSelection);
        }

        Color b_SelectedColor = Color.Blue;
        public Color SelectedColor
        {
            get => b_SelectedColor;
            internal set => ((INotifiable)this).SetField(ref b_SelectedColor, value, UpdateSelection);
        }

        #endregion


        #region Fields
        ListView ListView;
        #endregion


        #region Construction / Disposal
        void BuildCommon(ListView listView)
        {
            ListView = listView;
            ListView.b_SelectedItems.CollectionChanged += OnSelectedItems_CollectionChanged;
            SizeChanged += OnSizeChanged;
        }

        void DisposeCommon()
        {
            SizeChanged -= OnSizeChanged;
            ListView.b_SelectedItems.CollectionChanged -= OnSelectedItems_CollectionChanged;
            ListView = null;
        }
        #endregion

        #region Event Handlers
        partial void UpdateSelection();

        partial void OnIsEnabledChanged();

        public virtual void OnDataContextChanged()
        {
            UpdateSelection();
            foreach (var child in this.Children())
                if (child is IElement element)
                    element.DataContext = DataContext;
        }

        private void OnSelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateSelection();
        }

        private void OnSizeChanged(object sender, Size e)
        {
            if (ListView is ListView listView)
            {
                if (ActualHeight > -1 && DataContext != null)
                {
                    if (Index < listView.PxCellHeights.Count)
                    {
                        listView.PxCellHeights[Index] = ActualHeight;
                        return;
                    }
                    while (Index > listView.PxCellHeights.Count)
                        listView.PxCellHeights.Add(0);
                    listView.PxCellHeights.Add(ActualHeight);
                }
            }
        }
        #endregion
    }
}