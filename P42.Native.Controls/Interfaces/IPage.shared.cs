using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;

namespace P42.Native.Controls
{
    public interface IPage : INotifiable
    {
        string Title { get; }

        Context Context { get; }

        Task OnAppeared(IPage fromPage);

        Task<bool> OnAppearing(IPage fromPage);

        Task OnDisappeared(IPage toPage);

        Task<bool> OnDisappearing(IPage toPage);

    }
}
