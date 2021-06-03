using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SmartTraitsDefs;

namespace P42.Native.Controls
{
    [AddSimpleTrait(typeof(TNotifiable))]
    public partial class NavigationPage : IPage
    {
        #region Properties
        string b_DipTitle = string.Empty;
        public string DipTitle
        {
            get => b_DipTitle;
            set => b_DipTitle = value;
        }

        public IPage DipCurrentPage => ((NavPageWrapper)CurrentView).DipPage;
        #endregion


        #region IPage Methods

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        async Task<bool> IPage.OnAppearing(IPage fromPage) => await OnAppearing(fromPage);

        protected async Task<bool> OnAppearing(IPage fromPage) => true;

        async Task IPage.OnAppeared(IPage fromPage) => await OnAppeared(fromPage);

        protected async Task OnAppeared(IPage fromPage) { }

        async Task<bool> IPage.OnDisappearing(IPage toPage) => await OnDisappearing(toPage);

        protected async Task<bool> OnDisappearing(IPage toPage) => true;

        async Task IPage.OnDisappeared(IPage toPage) => await OnDisappeared(toPage);

        protected async Task OnDisappeared(IPage toPage) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        #endregion

    }
}