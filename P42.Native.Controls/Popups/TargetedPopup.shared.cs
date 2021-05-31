using System;
using System.Threading.Tasks;
using SmartTraitsDefs;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    [AddSimpleTrait(typeof(TControl))]
    public partial class TargetedPopup  
    {
        #region Properties

        Android.Views.View b_Content;
        public Android.Views.View Content
        {
            get => b_Content;
            set
            {
                if (SetField(ref b_Content, value))
                    m_Border.Content = value;
            }
        }

        TimeSpan b_PopAfter;
        public TimeSpan PopAfter
        {
            get => b_PopAfter;
            set => SetField(ref b_PopAfter, value);
        }

        public bool IsEmpty => Content is null;

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

        Android.Graphics.Point b_TargetPoint;
        public Android.Graphics.Point TargetPoint
        {
            get => b_TargetPoint;
            set => SetField(ref b_TargetPoint, value);
        }
        #endregion

        #region Pointer Properties
        double b_PointerBias;
        public double PointerBias
        {
            get => b_PointerBias;
            set => SetField(ref b_PointerBias, value);
        }

        double b_PointerCornerRadius = BubbleBorder.DefaultPointerCornerRadius;
        public double PointerCornerRadius
        {
            get => b_PointerCornerRadius;
            set
            {
                if (SetField(ref b_PointerCornerRadius, value))
                    m_Border.PointerCornerRadius = PointerCornerRadius;
            }
        }

        PointerDirection b_ActualPointerDirection;
        public PointerDirection ActualPointerDirection
        {
            get => b_ActualPointerDirection;
            private set => SetField(ref b_ActualPointerDirection, value);
        }

        PointerDirection b_PreferredPointerDirection;
        public PointerDirection PreferredPointerDirection
        {
            get => b_PreferredPointerDirection;
            set => SetField(ref b_PreferredPointerDirection, value);
        }

        PointerDirection b_FallbackPointerDirection;
        public PointerDirection FallbackPointerDirection
        {
            get => b_FallbackPointerDirection;
            set => SetField(ref b_FallbackPointerDirection, value);
        }

        int b_PointerLength = BubbleBorder.DefaultPointerLength;
        public int PointerLength
        {
            get => b_PointerLength;
            set => SetField(ref b_PointerLength, value);
        }

        double b_PointerTipRadius = BubbleBorder.DefaultPointerTipRadius;
        public double PointerTipRadius
        {
            get => b_PointerTipRadius;
            set
            {
                if (SetField(ref b_PointerTipRadius, value))
                    m_Border.PointerTipRadius = PointerTipRadius;
            }
        }

        int b_PointerMargin;
        public int PointerMargin
        {
            get => b_PointerMargin;
            set => SetField(ref b_PointerMargin, value);
        }
        #endregion

        #region Page Overlay Properties
        PageOverlayMode b_PageOverlayMode = PageOverlayMode.TouchDismiss;
        public PageOverlayMode PageOverlayMode
        {
            get => b_PageOverlayMode;
            set => SetField(ref b_PageOverlayMode, value);
        }

        Color b_PageOverlayColor = Color.Red.WithAlpha(0.5);
        public Color PageOverlayColor
        {
            get => b_PageOverlayColor;
            set
            {
                if (SetField(ref b_PageOverlayColor, value))
                    //m_OverlayShape.Paint.Color = PageOverlayColor;
                    m_Overlay.Background = PageOverlayColor.AsDrawable();
            }
        }
        #endregion

        #region Push/Pop
        TimeSpan b_AnimationDuration;
        public TimeSpan AnimationDuration
        {
            get => b_AnimationDuration;
            set => SetField(ref b_AnimationDuration, value);
        }

        PopupPoppedCause b_PoppedCause;
        public PopupPoppedCause PoppedCause
        {
            get => b_PoppedCause;
            set => SetField(ref b_PoppedCause, value);
        }

        object b_PoppedTrigger;
        public object PoppedTrigger
        {
            get => b_PoppedTrigger;
            set => SetField(ref b_PoppedTrigger, value);
        }

        PushPopState b_PushPopState;
        public PushPopState PushPopState
        {
            get => b_PushPopState;
            set => SetField(ref b_PushPopState, value);
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
            if (PushPopState == PushPopState.Pushed || PushPopState == PushPopState.Popping)
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
            if (PushPopState == PushPopState.Pushed || PushPopState == PushPopState.Pushing)
                return;

            if (PushPopState == PushPopState.Popping)
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
            var result = new PopupPoppedEventArgs(PoppedCause, PoppedTrigger);
            PushPopState = PushPopState.Popped;
            //_border.Bind(BubbleBorder.OpacityProperty, this, nameof(Opacity));
            _popCompletionSource?.TrySetResult(result);
            Popped?.Invoke(this, result);
            //P42.Utils.Uno.GC.Collect();
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