using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;

#if __ANDROID__
using UIElement = Android.Views.View;
#endif

namespace P42.Native.Controls
{
    public partial class ListView : IFrameworkElement
    {

        #region Properties

        UIElement b_Header;
        public UIElement Header
        {
            get => b_Header;
            set => ((INotifiable)this).SetField(ref b_Header, value);
        }

        UIElement b_Footer;
        public UIElement Footer
        {
            get => b_Footer;
            set => ((INotifiable)this).SetField(ref b_Footer, value);
        }

        bool b_IsItemClickEnabled;
        public bool IsItemClickEnabled
        {
            get => b_IsItemClickEnabled;
            set => ((INotifiable)this).SetField(ref b_IsItemClickEnabled, value);
        }

        ObservableCollection<object> b_SelectedItems = new ObservableCollection<object>();
        public ObservableCollection<object> SelectedItems
        {
            get => b_SelectedItems;
            //set => SetField(ref b_SelectedItems, value);
        }

        SelectionMode b_SelectionMode;
        public SelectionMode SelectionMode
        {
            get => b_SelectionMode;
            set => ((INotifiable)this).SetField(ref b_SelectionMode, value);
        }

        int b_SelectedIndex;
        public int SelectedIndex
        {
            get => b_SelectedIndex;
            set => ((INotifiable)this).SetField(ref b_SelectedIndex, value);
        }

        object b_SelectedItem;
        public object SelectedItem
        {
            get => b_SelectedItem;
            set => ((INotifiable)this).SetField(ref b_SelectedItem, value);
        }

        ObservableCollection<object> b_ItemsSource;
        public ObservableCollection<object> ItemsSource
        {
            get => b_ItemsSource;
            set => ((INotifiable)this).SetField(ref b_ItemsSource, value);
        }

        Type b_ItemViewType;
        public Type ItemViewType
        {
            get => b_ItemViewType;
            set => ((INotifiable)this).SetField(ref b_ItemViewType, value);
        }


        IItemViewFactory b_ItemViewFactory;
        public IItemViewFactory ItemViewFactory
        {
            get => b_ItemViewFactory;
            set => ((INotifiable)this).SetField(ref b_ItemViewFactory, value);
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

        internal async Task OnCellTapped(Cell cell)
        {

        }
    }
}