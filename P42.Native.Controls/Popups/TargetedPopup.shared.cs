using System;
using System.Threading.Tasks;
using SmartTraitsDefs;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    [AddSimpleTrait(typeof(TControl))]
    public partial class TargetedPopup : IDisposable
    {
        #region Properties

        Android.Views.View b_DipContent;
        public Android.Views.View DipContent
        {
            get => b_DipContent;
            set
            {
                if (SetField(ref b_DipContent, value))
                    m_Border.DipContent = value;
            }
        }

        int b_NtvShadowRadius = (10.0).DipToPx();
        public int NtvShadowRadius
        {
            get => b_NtvShadowRadius;
            set => SetField(ref b_NtvShadowRadius, value);
        }
        public double DipShadowRadius
        {
            get => NtvShadowRadius.PxToDip();
            set => NtvShadowRadius = value.DipToPx();
        }

        bool b_HasShadow = true;
        public bool DipHasShadow
        {
            get => b_HasShadow;
            set => SetField(ref b_HasShadow, value);
        }


        TimeSpan b_PopAfter;
        public TimeSpan PopAfter
        {
            get => b_PopAfter;
            set => SetField(ref b_PopAfter, value);
        }

        public bool IsEmpty => DipContent is null;

        bool b_IsAnimated;
        public bool IsAnimated
        {
            get => b_IsAnimated;
            set => SetField(ref b_IsAnimated, value);
        }

        #region Target Properties
        Android.Views.View b_Target;
        public Android.Views.View Target
        {
            get => b_Target;
            set => SetField(ref b_Target, value);
        }

        PointI b_NtvTargetPoint;
        public PointI NtvTargetPoint
        {
            get => b_NtvTargetPoint;
            set => SetField(ref b_NtvTargetPoint, value);
        }
        public Point DipTargetPoint
        {
            get => NtvTargetPoint.PxToDip();
            set => NtvTargetPoint = value.DipToPx();
        }
        #endregion

        #region Pointer Properties
        /*
        double b_PointerBias;
        public double PointerBias
        {
            get => b_PointerBias;
            set => SetField(ref b_PointerBias, value);
        }
        */
        double b_NtvPointerCornerRadius = BubbleBorder.NtvDefaultPointerCornerRadius;
        public double NtvPointerCornerRadius
        {
            get => b_NtvPointerCornerRadius;
            set
            {
                if (SetField(ref b_NtvPointerCornerRadius, value))
                    m_Border.NtvPointerCornerRadius = NtvPointerCornerRadius;
            }
        }
        public double DipPointerCornerRadius
        {
            get => NtvPointerCornerRadius.PxToDip();
            set => NtvPointerCornerRadius = value.DipToPx();
        }

        PointerDirection b_DipActualPointerDirection;
        public PointerDirection DipActualPointerDirection
        {
            get => b_DipActualPointerDirection;
            private set => SetField(ref b_DipActualPointerDirection, value);
        }

        PointerDirection b_DipPreferredPointerDirection;
        public PointerDirection DipPreferredPointerDirection
        {
            get => b_DipPreferredPointerDirection;
            set => SetField(ref b_DipPreferredPointerDirection, value);
        }

        PointerDirection b_DipFallbackPointerDirection;
        public PointerDirection DipFallbackPointerDirection
        {
            get => b_DipFallbackPointerDirection;
            set => SetField(ref b_DipFallbackPointerDirection, value);
        }

        int b_NtvPointerLength = BubbleBorder.NtvDefaultPointerLength;
        public int NtvPointerLength
        {
            get => b_NtvPointerLength;
            set => SetField(ref b_NtvPointerLength, value);
        }
        public double DipPointerLength
        {
            get => NtvPointerLength.PxToDip();
            set => NtvPointerLength = value.DipToPx();
        }

        double b_NtvPointerTipRadius = BubbleBorder.NtvDefaultPointerTipRadius;
        public double NtvPointerTipRadius
        {
            get => b_NtvPointerTipRadius;
            set
            {
                if (SetField(ref b_NtvPointerTipRadius, value))
                    m_Border.NtvPointerTipRadius = NtvPointerTipRadius;
            }
        }
        public double DipPointerTipRadius
        {
            get => NtvPointerTipRadius.PxToDip();
            set => NtvPointerTipRadius = value.DipToPx();
        }

        int b_NtvPointerMargin;
        public int NtvPointerMargin
        {
            get => b_NtvPointerMargin;
            set => SetField(ref b_NtvPointerMargin, value);
        }
        public double DipPointerMargin
        {
            get => NtvPointerMargin.PxToDip();
            set => NtvPointerMargin = value.DipToPx();
        }
        #endregion

        #region Page Overlay Properties
        PageOverlayMode b_DipPageOverlayMode = PageOverlayMode.TouchDismiss;
        public PageOverlayMode DipPageOverlayMode
        {
            get => b_DipPageOverlayMode;
            set => SetField(ref b_DipPageOverlayMode, value);
        }

        Color b_DipPageOverlayColor = Color.Gray.WithAlpha(0.25);
        public Color DipPageOverlayColor
        {
            get => b_DipPageOverlayColor;
            set
            {
                if (SetField(ref b_DipPageOverlayColor, value))
                    //m_OverlayShape.Paint.Color = PageOverlayColor;
                    m_Overlay.Background = DipPageOverlayColor.AsDrawable();
            }
        }
        #endregion

        #region Push/Pop
        TimeSpan b_DipAnimationDuration;
        public TimeSpan DipAnimationDuration
        {
            get => b_DipAnimationDuration;
            set => SetField(ref b_DipAnimationDuration, value);
        }

        PopupPoppedCause b_PoppedCause;
        public PopupPoppedCause DipPoppedCause
        {
            get => b_PoppedCause;
            set => SetField(ref b_PoppedCause, value);
        }

        object b_DipPoppedTrigger;
        public object DipPoppedTrigger
        {
            get => b_DipPoppedTrigger;
            set => SetField(ref b_DipPoppedTrigger, value);
        }

        PushPopState b_DipPushPopState;
        public PushPopState DipPushPopState
        {
            get => b_DipPushPopState;
            set => SetField(ref b_DipPushPopState, value);
        }

        bool b_DipDisposeOnPop = true;
        public bool DipDisposeOnPop
        {
            get => b_DipDisposeOnPop;
            set => SetField(ref b_DipDisposeOnPop, value);
        }

        #endregion

        #region Control

        #endregion

        #endregion


        #region Events
        public event EventHandler Pushed;
        public event EventHandler<PopupPoppedEventArgs> Popped;
        #endregion


        #region Methods

        #region Push / Pop
        private void OnPopupClosed(object sender, object e)
        {
            if (DipPushPopState == PushPopState.Pushed || DipPushPopState == PushPopState.Popping)
            {
                CompletePop(PopupPoppedCause.BackgroundTouch, null);
            }
        }

        TaskCompletionSource<bool> _popupOpenedCompletionSource;
        private void OnPopupOpened(object sender, object e)
        {
            _popupOpenedCompletionSource?.TrySetResult(true);
        }

        public virtual async Task PushAsync()
        {
            if (DipPushPopState == PushPopState.Pushed || DipPushPopState == PushPopState.Pushing)
                return;

            if (DipPushPopState == PushPopState.Popping)
            {
                if (_popCompletionSource is null)
                {
                    await WaitForPoppedAsync();
                }
                else
                    return;
            }
            await InnerPushAsyc();
        }

        void CompletePop(PopupPoppedCause poppedCause, object poppedTrigger)
        {
            var result = new PopupPoppedEventArgs(DipPoppedCause, DipPoppedTrigger);
            DipPushPopState = PushPopState.Popped;
            //_border.Bind(BubbleBorder.OpacityProperty, this, nameof(Opacity));
            _popCompletionSource?.TrySetResult(result);
            Popped?.Invoke(this, result);
            //P42.Utils.Uno.GC.Collect();

            if (DipDisposeOnPop)
                Dispose();
        }

        TaskCompletionSource<PopupPoppedEventArgs> _popCompletionSource;
        public async Task<PopupPoppedEventArgs> WaitForPoppedAsync()
        {
            _popCompletionSource = _popCompletionSource ?? new TaskCompletionSource<PopupPoppedEventArgs>();
            return await _popCompletionSource.Task;
        }

        TaskCompletionSource<bool> _pushCompletionSource;
        async Task<bool> WaitForPush()
        {
            _pushCompletionSource = _pushCompletionSource ?? new TaskCompletionSource<bool>();
            return await _pushCompletionSource.Task;
        }

        #endregion

        #region Protected Push / Pop Methods
        /// <summary>
        /// Invoked at start on appearing animation
        /// </summary>
        /// <returns></returns>
        protected virtual async Task OnPushBeginAsync()
        {
            await (_popupOpenedCompletionSource?.Task ?? Task.CompletedTask);
        }

        /// <summary>
        /// Invoked at end of appearing animation
        /// </summary>
        /// <returns></returns>
        protected virtual async Task OnPushEndAsync()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Invoked at start of disappearing animation
        /// </summary>
        /// <returns></returns>
        protected virtual async Task OnPopBeginAsync()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Invoked at end of disappearing animation
        /// </summary>
        /// <returns></returns>
        protected virtual async Task OnPopEndAsync()
        {
            await Task.CompletedTask;
        }
        #endregion

        #endregion
    }
}