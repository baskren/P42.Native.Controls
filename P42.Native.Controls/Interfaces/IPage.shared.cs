using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;

namespace P42.Native.Controls
{
    public interface IPage : INotifiable
    {
        public string Title { get; }

        public Context Context { get; }

        public Task OnAppeared(IPage fromPage);

        public Task<bool> OnAppearing(IPage fromPage);

        public Task OnDisappeared(IPage toPage);

        public Task<bool> OnDisappearing(IPage toPage);

    }
}
