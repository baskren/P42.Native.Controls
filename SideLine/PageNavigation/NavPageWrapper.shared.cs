using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace P42.Native.Controls
{
    public partial class NavPageWrapper : INotifiable
    {
        string Title => Page?.Title;

        internal readonly IPage Page;

    }
}