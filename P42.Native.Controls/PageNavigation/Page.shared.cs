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
using SmartTraitsDefs;

namespace P42.Native.Controls
{
    [AddSimpleTrait(typeof(TNotifiable))]
    public partial class Page : IPage
    {
        #region Properties
        string b_Title = string.Empty;
        public string Title
        {
            get => b_Title;
            set => SetField(ref b_Title, value);
        }

        bool b_HasBackButton = true;
        public bool HasBackButton
        {
            get => b_HasBackButton;
            set => SetField(ref b_HasBackButton, value);
        }

        string b_BackButtonTitle = "back";
        public string BackButtonTitle
        {
            get => b_BackButtonTitle;
            set => SetField(ref b_BackButtonTitle, value);
        }


        bool b_HasNavigationBar = true;
        public bool HasNavigationBar
        {
            get => b_HasNavigationBar;
            set => SetField(ref b_HasNavigationBar, value);
        }

        Thickness b_Padding;
        public Thickness Padding
        {
            get => b_Padding;
            set => SetField(ref b_Padding, value);
        }


        View b_Content;
        public View Content
        {
            get => b_Content;
            set => SetField(ref b_Content, value);
        }
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
