using System;
using SmartTraitsDefs;

#if __ANDROID__
using Color = Android.Graphics.Color;
using UIElement = Android.Views.View;
#endif


namespace P42.Native.Controls
{
    [AddSimpleTrait(typeof(TElement))]
    public partial class Cell
    {

        #region  Properties
        public bool DipIsSelected
            => DipDataContext is null
            ? false
            : ListView?.b_DipSelectedItems.Contains(DipDataContext) ?? false;

        bool b_DipIsEnabled;
        public bool DipIsEnabled
        {
            get => b_DipIsEnabled;
            set => SetField(ref b_DipIsEnabled, value, OnIsEnabledChanged);
        }

        int b_DipIndex;
        public int DipIndex
        {
            get => b_DipIndex;
            internal set => SetField(ref b_DipIndex, value, UpdateSelection);
        }

        Color b_DipSelectedColor = Color.Blue;
        public Color DipSelectedColor
        {
            get => b_DipSelectedColor;
            internal set => SetField(ref b_DipSelectedColor, value, UpdateSelection);
        }

        #endregion


        #region Fields
        ListView ListView;
        #endregion


        #region Construction / Disposal
        void BuildCommon(ListView listView)
        {
            ListView = listView;
            ListView.b_DipSelectedItems.CollectionChanged += OnSelectedItems_CollectionChanged;
            DipSizeChanged += OnSizeChanged;
        }

        void DisposeCommon()
        {
            DipSizeChanged -= OnSizeChanged;
            ListView.b_DipSelectedItems.CollectionChanged -= OnSelectedItems_CollectionChanged;
            ListView = null;
        }
        #endregion


        #region Event Handlers
        partial void UpdateSelection();

        partial void OnIsEnabledChanged();

        public virtual void DipOnDataContextChanged()
        {
            UpdateSelection();
            foreach (var child in this.Children())
                if (child is IElement element)
                    element.DipDataContext = DipDataContext;
        }

        private void OnSelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateSelection();
        }

        private void OnSizeChanged(object sender, Size e)
        {
            if (ListView is ListView listView)
            {
                if (NtvActualHeight > -1 && DipDataContext != null)
                {
                    if (DipIndex < listView.PxCellHeights.Count)
                    {
                        listView.PxCellHeights[DipIndex] = NtvActualHeight;
                        return;
                    }
                    while (DipIndex > listView.PxCellHeights.Count)
                        listView.PxCellHeights.Add(0);
                    listView.PxCellHeights.Add(NtvActualHeight);
                }
            }
        }
        #endregion
    }
}