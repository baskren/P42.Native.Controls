using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace P42.Native.Controls.Droid
{
    public class ListView : RecyclerView
    {
        public ListView() : this(P42.Utils.Droid.Settings.Context) { }

        public ListView(Context context) : base(context)
        {
            Build();
        }

        public ListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Build();
        }

        public ListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Build();
        }

        protected ListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Build();
        }

        void Build()
        {

        }



    }
}
