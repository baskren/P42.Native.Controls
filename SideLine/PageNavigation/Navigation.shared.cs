using System;
using System.Threading.Tasks;
using Android.Views;

namespace P42.Native.Controls
{
    public static class Navigation
    {
        public static async Task PushAsync(this Page currentPage, Page newPage, PageAnimationOptions options = null)
        {
            if (currentPage is View view)
            {
                if (view.FindAncestor<NavigationPage>() is NavigationPage navPage)
                    await navPage.InternalPushAsync(newPage, options);
                else
                    throw new Exception("Current page is not a descendent of a NavigationPage");
            }
        }

        public static async Task PopAsync(this IPage currentPage, PageAnimationOptions options = null, bool dispose = false)
        {
            if (currentPage is View view)
            {
                if (view.FindAncestor<NavigationPage>() is NavigationPage navPage)
                    await navPage.InternalPopAsync(currentPage, options, dispose);
                else
                    throw new Exception("Current page is not a descendent of a NavigationPage");
            }
        }

        public static async Task PushModalAsync(this Page currentPage, Page newPage, PageAnimationOptions options = null)
            => await App.Current.InternalPushModalAsync(newPage, options);

        public static async Task PushModalAsync(Page newPage, PageAnimationOptions options = null)
            => await App.Current.InternalPushModalAsync(newPage, options);

        public static async Task PopModalAsync(this IPage currentPage, PageAnimationOptions options = null, bool dispose = false)
            => await App.Current.InternalPopModalAsync(currentPage, options, dispose);

    }
}
