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
        string b_DipTitle = string.Empty;
        public string DipTitle
        {
            get => b_DipTitle;
            set => SetField(ref b_DipTitle, value);
        }

        bool b_DipHasBackButton = true;
        public bool DipHasBackButton
        {
            get => b_DipHasBackButton;
            set => SetField(ref b_DipHasBackButton, value);
        }

        string b_DipBackButtonTitle = "back";
        public string DipBackButtonTitle
        {
            get => b_DipBackButtonTitle;
            set => SetField(ref b_DipBackButtonTitle, value);
        }


        bool b_DipHasNavigationBar = true;
        public bool DipHasNavigationBar
        {
            get => b_DipHasNavigationBar;
            set => SetField(ref b_DipHasNavigationBar, value);
        }

        ThicknessI b_NtvPadding;
        public ThicknessI NtvPadding
        {
            get => b_NtvPadding;
            set => SetField(ref b_NtvPadding, value);
        }

        public Thickness DipPadding
        {
            get => NtvPadding.PxToDip();
            set => NtvPadding = value.DipToPx();
        }

        View b_DipContent;
        public View DipContent
        {
            get => b_DipContent;
            set => SetField(ref b_DipContent, value);
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


        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                DipContent?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
