using Android.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.ObjectModel;
using Android.Runtime;
using Android.Util;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;

namespace P42.Native.Controls
{
    public partial class ListView : ViewGroup
    {
        #region Fields
        internal ObservableCollection<int> NativeCellHeights = new ObservableCollection<int>();
        Android.Views.View _headerView;
        Android.Views.View _footerView;
        Android.Widget.ListView _nativeListView;
        SimpleAdapter _adapter;
        #endregion


        #region Constructors
        public ListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ListView(Context context) : base(context)
        {
        }

        public ListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public ListView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        void Build()
        {
            SharedBuild();

        }
        #endregion




        #region INotifiable

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        #endregion


        #region Fields
        public bool HasDrawn { get; set; }
        public bool HasChanged { get; set; }
        #endregion


        #region Methods
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public void RedrawElement() => PostInvalidate();

        public void RelayoutElement() => RequestLayout();
        #endregion

        #endregion

    }



}
