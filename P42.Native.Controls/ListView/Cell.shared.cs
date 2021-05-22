using System;

#if __ANDROID__
using Color = Android.Graphics.Color;
using UIElement = Android.Views.View;
#endif

namespace P42.Native.Controls
{
    public partial class Cell : P42.Native.Controls.IFrameworkElement
    {

        #region  Properties
        public bool IsSelected
            => DataContext is null
            ? false
            : ListView?.SelectedItems.Contains(DataContext) ?? false;

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

        Color b_SelectedColor;
        public Color SelectedColor
        {
            get => b_SelectedColor;
            internal set => ((INotifiable)this).SetField(ref b_SelectedColor, value, UpdateSelection);
        }

        IFrameworkElement b_Child;
        public IFrameworkElement Child
        {
            get => b_Child;
            internal set => ((INotifiable)this).SetField(ref b_Child, value);
        }
        #endregion


        #region Fields
        ListView ListView;
        #endregion


        #region Construction / Disposal
        void BuildCommon(ListView listView)
        {
            ListView = listView;
            ListView.SelectedItems.CollectionChanged += OnSelectedItems_CollectionChanged;
            SizeChanged += OnSizeChanged;
        }

        void DisposeCommon()
        {
            SizeChanged -= OnSizeChanged;
            ListView.SelectedItems.CollectionChanged -= OnSelectedItems_CollectionChanged;
            ListView = null;
        }
        #endregion

        #region Event Handlers
        partial void UpdateSelection();

        partial void OnIsEnabledChanged();

        public virtual void OnDataContextChanged()
        {
            if (Child is IFrameworkElement element)
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
                    if (Index < listView.NativeCellHeights.Count)
                    {
                        listView.NativeCellHeights[Index] = ActualHeight;
                        return;
                    }
                    while (Index > listView.NativeCellHeights.Count)
                        listView.NativeCellHeights.Add(0);
                    listView.NativeCellHeights.Add(ActualHeight);
                }
            }
        }
        #endregion
    }
}