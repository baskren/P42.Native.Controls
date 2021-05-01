using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using P42.Native.Controls;

namespace P42.Native.Controls.Droid
{
    public class TargetedPopup : P42.Utils.NotifiablePropertyObject
    {
        #region Properties

        Android.Views.View b_Content;
        public Android.Views.View Content
        {
            get => b_Content;
            set => SetField(ref b_Content, value);
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

        double b_PointerCornerRadius;
        public double PointerCornerRadius
        {
            get => b_PointerCornerRadius;
            set => SetField(ref b_PointerCornerRadius, value);
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

        double b_PointerLength;
        public double PointerLength
        {
            get => b_PointerLength;
            set => SetField(ref b_PointerLength, value);
        }

        double b_PointerTipRadius;
        public double PointerTipRadius
        {
            get => b_PointerTipRadius;
            set => SetField(ref b_PointerTipRadius, value);
        }

        double b_PointerMargin;
        public double PointerMargin
        {
            get => b_PointerMargin;
            set => SetField(ref b_PointerMargin, value);
        }
        #endregion

        #region Page Overlay Properties
        PageOverlayMode b_PageOverlayMode;
        public PageOverlayMode PageOverlayMode
        {
            get => b_PageOverlayMode;
            set => SetField(ref b_PageOverlayMode, value);
        }

        System.Drawing.Color b_PageOverlayColor;
        public System.Drawing.Color PageOverlayColor
        {
            get => b_PageOverlayColor;
            set => SetField(ref b_PageOverlayColor, value);
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

        #region View Properties
        Thickness b_Margin;
        public Thickness Margin
        {
            get => b_Margin;
            set => SetField(ref b_Margin, value);
        }

        Alignment b_HorizontalAlignment;
        public Alignment HorizontalAlignment
        {
            get => b_HorizontalAlignment;
            set => SetField(ref b_HorizontalAlignment, value);
        }

        Alignment b_VerticalAlignment;
        public Alignment VerticalAlignment
        {
            get => b_VerticalAlignment;
            set => SetField(ref b_VerticalAlignment, value);
        }
        #endregion

        #endregion


        #region Events
        public event EventHandler Pushed;
        public event EventHandler<PopupPoppedEventArgs> Popped;
        #endregion



        #region Fields
        #endregion


        #region Construction / Disposal
        public TargetedPopup(Android.Views.View target = null)
        {
            Target = target;
        }
        #endregion


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

        async Task InnerPushAsyc()
        {
            PushPopState = PushPopState.Pushing;
            _popCompletionSource = null;

            PoppedCause = PopupPoppedCause.BackgroundTouch;
            PoppedTrigger = null;

            await OnPushBeginAsync();

            // requiired to render popup the first time.
            _border.Opacity = 0.0;
            _popupOpenedCompletionSource = new TaskCompletionSource<bool>();

            _popup.IsOpen = true;
            await Task.Delay(5);
            UpdateMarginAndAlignment();
            _popup.InvalidateMeasure();


            if (IsAnimated)
            {
                Action<double> action = percent => _border.Opacity = Opacity * percent;
                var animator = new P42.Utils.Uno.ActionAnimator(0.11, 0.95, TimeSpan.FromMilliseconds(300), action);
                await animator.RunAsync();
            }

            _border.Bind(BubbleBorder.OpacityProperty, this, nameof(Opacity));

            if (PopAfter > default(TimeSpan))
            {
                P42.Utils.Timer.StartTimer(PopAfter, async () =>
                {
                    await PopAsync(PopupPoppedCause.Timeout, "Timeout");
                    return false;
                });
            }

            await OnPushEndAsync();

            PushPopState = PushPopState.Pushed;
            Pushed?.Invoke(this, EventArgs.Empty);
            _pushCompletionSource?.TrySetResult(true);

        }

        public virtual async Task PopAsync(PopupPoppedCause cause = PopupPoppedCause.MethodCalled, [CallerMemberName] object trigger = null)
        {
            if (PushPopState == PushPopState.Popping || PushPopState == PushPopState.Popped)
                return;

            if (PushPopState == PushPopState.Pushing)
            {
                if (_pushCompletionSource is null)
                    await WaitForPush();
                else
                    return;
            }
            _pushCompletionSource = null;

            PushPopState = PushPopState.Popping;
            _pushCompletionSource = null;

            _border.SizeChanged -= OnBorderSizeChanged;

            PoppedCause = cause;
            PoppedTrigger = trigger;
            await OnPopBeginAsync();

            if (IsAnimated)
            {
                Action<double> action = percent => _border.Opacity = Opacity * percent;
                var animator = new P42.Utils.Uno.ActionAnimator(0.95, 0.11, TimeSpan.FromMilliseconds(300), action);
                await animator.RunAsync();
            }

            _popup.IsOpen = false;

            CompletePop(PoppedCause, PoppedTrigger);
        }

        void CompletePop(PopupPoppedCause poppedCause, object poppedTrigger)
        {
            var result = new PopupPoppedEventArgs(PoppedCause, PoppedTrigger);
            PushPopState = PushPopState.Popped;
            _border.Bind(BubbleBorder.OpacityProperty, this, nameof(Opacity));
            _popCompletionSource?.TrySetResult(result);
            Popped?.Invoke(this, result);
            P42.Utils.Uno.GC.Collect();
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


        #region Layout
        /*
        protected override Size MeasureOverride(Size availableSize)
        {
            if (IsEmpty)
                return AppWindow.Size();

            _border.Measure(AppWindow.Size());
            return AppWindow.Size();
        }
        */
        void UpdateMarginAndAlignment()
        {
            if (_border is null)
                return;

            var windowSize = Droid.DisplayExtensions.Size;
            if (windowSize.Width < 1 || windowSize.Height < 1)
                return;
            var windowWidth = windowSize.Width - Margin.Horizontal;
            var windowHeight = windowSize.Height - Margin.Vertical;
            var cleanSize = MeasureBorder(new Size(windowWidth, windowHeight));

            if (PreferredPointerDirection == PointerDirection.None || Target is null)
            {
                //System.Diagnostics.Debug.WriteLine(GetType() + ".UpdateMarginAndAlignment PreferredPointerDirection == PointerDirection.None");
                CleanMarginAndAlignment(HorizontalAlignment, VerticalAlignment, windowSize, cleanSize);
                return;
            }

            var target = TargetBounds();

            //System.Diagnostics.Debug.WriteLine(GetType() + ".UpdateBorderMarginAndAlignment targetBounds:["+targetBounds+"]");
            var availableSpace = AvailableSpace(target);
            var stats = BestFit(availableSpace, cleanSize);

            if (stats.PointerDirection == PointerDirection.None)
            {
                CleanMarginAndAlignment(Alignment.Center, Alignment.Center, windowSize, cleanSize);
                return;
            }

            ActualPointerDirection = stats.PointerDirection;
            var margin = Margin;
            var hzAlign = HorizontalAlignment;
            var vtAlign = VerticalAlignment;

            if (stats.PointerDirection.IsHorizontal())
            {
                if (stats.PointerDirection == PointerDirection.Left)
                {
                    margin.Left = target.Right;
                    if (HorizontalAlignment != HorizontalAlignment.Stretch)
                        hzAlign = HorizontalAlignment.Left;
                }
                else if (stats.PointerDirection == PointerDirection.Right)
                {
                    margin.Right = windowSize.Width - target.Left;
                    if (HorizontalAlignment != HorizontalAlignment.Stretch)
                        hzAlign = HorizontalAlignment.Right;
                }

                if (VerticalAlignment == VerticalAlignment.Top)
                {
                    margin.Top = Math.Max(Margin.Top, target.Top);
                }
                else if (VerticalAlignment == VerticalAlignment.Center)
                {
                    margin.Top = Math.Max(Margin.Top, (target.Top + target.Bottom) / 2.0 - stats.BorderSize.Height / 2.0);
                    vtAlign = VerticalAlignment.Top;
                }
                else if (VerticalAlignment == VerticalAlignment.Bottom)
                {
                    margin.Bottom = Math.Max(Margin.Bottom, windowSize.Height - target.Bottom);
                }

                if (margin.Top + stats.BorderSize.Height > windowSize.Height - Margin.Bottom)
                    margin.Top = windowSize.Height - Margin.Bottom - stats.BorderSize.Height;

                if (VerticalAlignment == VerticalAlignment.Bottom)
                    _border.PointerAxialPosition = (target.Top - (windowSize.Height - margin.Bottom - cleanSize.Height)) + target.Bottom - (target.Top + target.Bottom) / 2.0;
                else
                    _border.PointerAxialPosition = (target.Top - margin.Top) + target.Bottom - (target.Top + target.Bottom) / 2.0;
            }
            else
            {
                if (stats.PointerDirection == PointerDirection.Up)
                {
                    margin.Top = target.Bottom;
                    if (VerticalAlignment != VerticalAlignment.Stretch)
                        vtAlign = VerticalAlignment.Top;
                }
                else if (stats.PointerDirection == PointerDirection.Down)
                {
                    margin.Bottom = windowSize.Height - target.Top;
                    if (VerticalAlignment != VerticalAlignment.Stretch)
                        vtAlign = VerticalAlignment.Bottom;
                }

                if (HorizontalAlignment == HorizontalAlignment.Left)
                    margin.Left = Math.Max(Margin.Left, target.Left);
                else if (HorizontalAlignment == HorizontalAlignment.Center)
                    margin.Left = Math.Max(Margin.Left, (target.Left + target.Right) / 2.0 - stats.BorderSize.Width / 2.0);
                else if (HorizontalAlignment == HorizontalAlignment.Right)
                    margin.Right = Math.Max(Margin.Right, windowSize.Width - target.Right);

                if (margin.Left + stats.BorderSize.Width > windowSize.Width - Margin.Right)
                    margin.Left = windowSize.Width - Margin.Right - stats.BorderSize.Width;

                if (HorizontalAlignment == HorizontalAlignment.Right)
                    _border.PointerAxialPosition = (target.Left - (windowSize.Width - margin.Right - cleanSize.Width)) + (target.Right - (target.Left + target.Right) / 2.0);
                else
                    _border.PointerAxialPosition = (target.Left - margin.Left) + (target.Right - (target.Left + target.Right) / 2.0);
            }

            ActualPointerDirection = _border.PointerDirection = stats.PointerDirection;
            SetMarginAndAlignment(margin, hzAlign, vtAlign, windowSize, cleanSize);
        }

        void CleanMarginAndAlignment(Alignment hzAlign, Alignment vtAlign, Size windowSize, Size cleanSize)
        {
            ActualPointerDirection = PointerDirection.None;

            if (_border is null)
                return;

            _border.PointerDirection = ActualPointerDirection;
            SetMarginAndAlignment(Margin, hzAlign, vtAlign, windowSize, cleanSize);
        }

        void SetMarginAndAlignment(Thickness margin, Alignment hzAlign, Alignment vtAlign, Size windowSize, Size cleanSize)
        {
            var frame = CalculateFrame(margin, hzAlign, vtAlign, windowSize, cleanSize);

            _popup.Margin = new Thickness(0);
            _popup.HorizontalOffset = frame.Left;
            _popup.VerticalOffset = frame.Top + P42.Utils.Uno.AppWindow.StatusBarHeight(this);

            _border.Margin = new Thickness(0);
            _border.Width = frame.Width;
            _border.Height = frame.Height;

            _border.HorizontalAlignment = hzAlign == HorizontalAlignment.Stretch && !this.HasPrescribedWidth()
                ? HorizontalAlignment.Stretch
                : HorizontalAlignment.Left;
            _border.VerticalAlignment = vtAlign == VerticalAlignment.Stretch && !this.HasPrescribedHeight()
                ? VerticalAlignment.Stretch
                : VerticalAlignment.Top;

            System.Diagnostics.Debug.WriteLine("TargetedPopup.CleanMarginAndAlignment frame: " + frame);
        }

        Rect CalculateFrame(Thickness margin, HorizontalAlignment hzAlign, VerticalAlignment vtAlign, Size windowSize, Size borderSize)
        {
            var hzPointer = ActualPointerDirection.IsHorizontal() ? PointerLength : 0;
            var vtPointer = ActualPointerDirection.IsVertical() ? PointerLength : 0;
            var left = margin.Left;
            var top = margin.Top;
            if (this.HasPrescribedWidth())
                borderSize.Width = Width;
            if (this.HasPrescribedHeight())
                borderSize.Height = Height;
            var right = Math.Min(left + borderSize.Width + hzPointer, windowSize.Width - margin.Right);
            var bottom = Math.Min(top + borderSize.Height + vtPointer, windowSize.Height - margin.Bottom);

            if (hzAlign == HorizontalAlignment.Center)
            {
                left = Math.Max((windowSize.Width - borderSize.Width) / 2.0, left);
                right = Math.Min(left + borderSize.Width, windowSize.Width - margin.Right);
            }
            else if (hzAlign == HorizontalAlignment.Right)
            {
                left = Math.Max(windowSize.Width - margin.Right - hzPointer - borderSize.Width, left);
                right = windowSize.Width - margin.Right;
            }
            else if (hzAlign == HorizontalAlignment.Stretch && !this.HasPrescribedWidth())
            {
                right = windowSize.Width - margin.Right;
            }

            if (vtAlign == VerticalAlignment.Center)
            {
                top = Math.Max((windowSize.Height - borderSize.Height) / 2.0, top);
                bottom = Math.Min(top + borderSize.Height, windowSize.Height - margin.Bottom);
            }
            else if (vtAlign == VerticalAlignment.Bottom)
            {
                top = Math.Max(windowSize.Height - margin.Bottom - vtPointer - borderSize.Height, top);
                bottom = windowSize.Height - margin.Bottom;
            }
            else if (vtAlign == VerticalAlignment.Stretch && !this.HasPrescribedHeight())
            {
                bottom = windowSize.Height - margin.Bottom;
            }

            return new Rect(left, top, right - left, bottom - top);
        }

        DirectionStats BestFit(Thickness availableSpace, Size cleanSize)
        {
            // given the amount of free space, determine if the border will fit 
            var windowSize = AppWindow.Size(this);
            var windowSpaceW = Math.Max(0, windowSize.Width - Margin.Horizontal);
            var windowSpaceH = Math.Max(0, windowSize.Height - Margin.Vertical);
            var windowSpace = new Size(windowSpaceW, windowSpaceH);

            var freeSpaceW = Math.Max(0, windowSpace.Width - cleanSize.Width);
            var freeSpaceH = Math.Max(0, windowSpace.Height - cleanSize.Height);

            var cleanStat = new DirectionStats
            {
                PointerDirection = PointerDirection.None,
                BorderSize = cleanSize,
                FreeSpace = new Size(freeSpaceW, freeSpaceH)
            };

            #region Check if clean border fits in preferred pointer quadrants
            // see if the existing measurement data works
            if (GetBestDirectionStat(GetRectangleBorderStatsForDirection(PreferredPointerDirection, cleanStat, availableSpace)) is DirectionStats stats0)
                return stats0;
            #endregion

            #region Check if border + content could fit in any of the preferred pointer quadrants
            // at this point in time valid and invalid fits are in the stats list
            if (GetBestDirectionStat(GetMeasuredStatsForDirection(PreferredPointerDirection, cleanStat, availableSpace, windowSpace)) is DirectionStats stats1)
                return stats1;
            #endregion

            // the stats list only contains invalid fallback fits ... but perhaps not all fallback fits have yet been tried
            var uncheckedFallbackPointerDirection = (FallbackPointerDirection ^ PreferredPointerDirection) | FallbackPointerDirection;

            #region Check if clean border fits in unchecked fallback pointer quadrants
            if (GetBestDirectionStat(GetRectangleBorderStatsForDirection(uncheckedFallbackPointerDirection, cleanStat, availableSpace)) is DirectionStats stats2)
                return stats2;
            #endregion

            #region Check if border + content could fit in any of the unchecked fallback pointer quadrants
            if (GetBestDirectionStat(GetMeasuredStatsForDirection(uncheckedFallbackPointerDirection, cleanStat, availableSpace, windowSpace)) is DirectionStats stats3)
                return stats3;
            #endregion

            return cleanStat;
        }

        DirectionStats? GetBestDirectionStat(List<DirectionStats> stats)
        {
            if (stats.Count == 1)
                return stats[0];
            if (stats.Count > 1)
            {
                var max = stats[0];
                foreach (var s in stats)
                {
                    if (s.MinAvail == max.MinAvail && s.MaxAvail > max.MaxAvail)
                        max = s;
                    else if (s.MinAvail > max.MinAvail)
                        max = s;
                }
                return max;
            }
            return null;
        }

        Rect TargetBounds()
        {
            var targetBounds = Target is null ? Rect.Empty : Target.GetBounds();

            double targetLeft = (Target is null ? TargetPoint.X : targetBounds.Left) - PointerMargin;
            double targetRight = (Target is null ? TargetPoint.X : targetBounds.Right) + PointerMargin;
            double targetTop = (Target is null ? TargetPoint.Y : targetBounds.Top) - PointerMargin;
            double targetBottom = (Target is null ? TargetPoint.Y : targetBounds.Bottom) + PointerMargin;

            return new Rect(targetLeft, targetTop, targetRight - targetLeft, targetBottom - targetTop);
        }

        Thickness AvailableSpace(Rect target)
        {
            var windowBounds = AppWindow.Size(this);
            if (Target != null || (TargetPoint.X > 0 || TargetPoint.Y > 0))
            {
                if (target.Right > 0 && target.Left < windowBounds.Width && target.Bottom > 0 && target.Top < windowBounds.Height)
                {
                    var availL = target.Left - Margin.Left;
                    var availR = windowBounds.Width - Margin.Right - target.Right;
                    var availT = target.Top - Margin.Top;
                    var availB = windowBounds.Height - target.Bottom - Margin.Bottom;

                    var maxWidth = MaxWidth;
                    if (Width > 0 && Width < maxWidth)
                        maxWidth = Width;
                    if (maxWidth > 0 && HorizontalAlignment != HorizontalAlignment.Stretch)
                    {
                        availL = Math.Min(availL, maxWidth);
                        availR = Math.Min(availR, maxWidth);
                    }

                    var maxHeight = MaxHeight;
                    if (Height > 0 && Height < maxHeight)
                        maxHeight = Height;
                    if (maxHeight > 0 && VerticalAlignment != VerticalAlignment.Stretch)
                    {
                        availT = Math.Min(availT, maxHeight);
                        availB = Math.Min(availB, maxHeight);
                    }

                    return new Thickness(availL, availT, availR, availB);
                }
            }

            if (PointToOffScreenElements)
                return new Thickness(windowBounds.Width - Margin.Horizontal, windowBounds.Height - Margin.Vertical, windowBounds.Width - Margin.Horizontal, windowBounds.Height - Margin.Vertical);

            return new Thickness(-1, -1, -1, -1);
        }

        List<DirectionStats> GetRectangleBorderStatsForDirection(PointerDirection pointerDirection, DirectionStats cleanStat, Thickness availableSpace)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + ".GetRectangleBorderStatsForDirection cleanStat:["+cleanStat+"]");
            var stats = new List<DirectionStats>();
            if (pointerDirection.LeftAllowed() && (availableSpace.Right - cleanStat.BorderSize.Width) >= PointerLength)
            {
                var stat = cleanStat;
                stat.PointerDirection = PointerDirection.Left;
                stat.BorderSize.Width += PointerLength;
                var free = availableSpace.Right - stat.BorderSize.Width;
                if (free >= 0)
                {
                    stat.FreeSpace.Width = free;
                    stats.Add(stat);
                }
            }

            if (pointerDirection.RightAllowed() && (availableSpace.Left - cleanStat.BorderSize.Width) >= PointerLength)
            {
                var stat = cleanStat;
                stat.PointerDirection = PointerDirection.Right;
                stat.BorderSize.Width += PointerLength;
                var free = availableSpace.Left - stat.BorderSize.Width;
                if (free >= 0)
                {
                    stat.FreeSpace.Width = free;
                    stats.Add(stat);
                }
            }

            if (pointerDirection.UpAllowed() && (availableSpace.Bottom - cleanStat.BorderSize.Height) >= PointerLength)
            {
                var stat = cleanStat;
                stat.PointerDirection = PointerDirection.Up;
                stat.BorderSize.Height += PointerLength;
                var free = availableSpace.Bottom - stat.BorderSize.Height;
                if (free >= 0)
                {
                    stat.FreeSpace.Height = free;
                    stats.Add(stat);
                }
            }

            if (pointerDirection.DownAllowed() && (availableSpace.Top - cleanStat.BorderSize.Height) >= PointerLength)
            {
                var stat = cleanStat;
                stat.PointerDirection = PointerDirection.Down;
                stat.BorderSize.Height += PointerLength;
                var free = availableSpace.Top - stat.BorderSize.Height;
                if (free >= 0)
                {
                    stat.FreeSpace.Height = free;
                    stats.Add(stat);
                }
            }

            return stats;
        }

        List<DirectionStats> GetMeasuredStatsForDirection(PointerDirection pointerDirection, DirectionStats cleanStat, Thickness availableSpace, Size windowSpace)
        {
            var stats = new List<DirectionStats>();
            if (pointerDirection.LeftAllowed())
            {
                if (availableSpace.Right > 0)
                {
                    var size = new Size(availableSpace.Right, windowSpace.Height);
                    var borderSize = MeasureBorder(size, cleanStat.BorderSize);
                    var stat = cleanStat;
                    stat.PointerDirection = PointerDirection.Left;
                    stat.BorderSize = borderSize;
                    var free = availableSpace.Right - borderSize.Width;
                    if (free >= 0)
                    {
                        stat.FreeSpace.Width = free;
                        stats.Add(stat);
                    }
                }
            }

            if (pointerDirection.RightAllowed())
            {
                if (availableSpace.Left > 0)
                {
                    var size = new Size(availableSpace.Left, windowSpace.Height);
                    var border = MeasureBorder(size, cleanStat.BorderSize);
                    var stat = cleanStat;
                    stat.PointerDirection = PointerDirection.Right;
                    stat.BorderSize = border;
                    var free = availableSpace.Left - border.Width;
                    if (free >= 0)
                    {
                        stat.FreeSpace.Width = free;
                        stats.Add(stat);
                    }
                }
            }

            if (pointerDirection.UpAllowed())
            {
                if (availableSpace.Bottom > 0)
                {
                    var size = new Size(windowSpace.Width, availableSpace.Bottom);
                    var border = MeasureBorder(size, cleanStat.BorderSize);
                    var stat = cleanStat;
                    stat.PointerDirection = PointerDirection.Up;
                    stat.BorderSize = border;
                    var free = availableSpace.Bottom - border.Height;
                    if (free >= 0)
                    {
                        stat.FreeSpace.Height = free;
                        stats.Add(stat);
                    }
                }
            }

            if (pointerDirection.DownAllowed())
            {
                if (availableSpace.Top > 0)
                {
                    var size = new Size(windowSpace.Width, availableSpace.Top);
                    var border = MeasureBorder(size, cleanStat.BorderSize);
                    var stat = cleanStat;
                    stat.PointerDirection = PointerDirection.Down;
                    stat.BorderSize = border;
                    var free = availableSpace.Top - border.Height;
                    if (free >= 0)
                    {
                        stat.FreeSpace.Height = free;
                        stats.Add(stat);
                    }
                }
            }

            return stats;
        }

        Size MeasureBorder(Size available, Size failSize = default)
        {
            System.Diagnostics.Debug.WriteLine("\n");
            System.Diagnostics.Debug.WriteLine($"TargetedPopup.MeasureBorder({available})");
            var width = available.Width;
            var height = available.Height;
            if (this.HasPrescribedWidth())
                width = Math.Min(Width, width);
            if (this.HasPrescribedHeight())
                height = Math.Min(Height, height);

            if (this.HasPrescribedWidth() && this.HasPrescribedHeight())
                return new Size(width, height);

            if (IsEmpty)
                return new Size(
                    this.HasPrescribedWidth()
                        ? width : 50 + Padding.Horizontal(),
                    this.HasPrescribedHeight()
                        ? height : 50 + Padding.Vertical()
                    );

            var hasBorder = (BorderThickness.Average() > 0) && BorderBrush is SolidColorBrush brush && brush.Color.A > 0;
            var border = BorderThickness.Average() * (hasBorder ? 1 : 0) * 2;
            var availableWidth = width - Padding.Horizontal() - border - 1;
            var availableHeight = height - Padding.Vertical() - border - 1;
            System.Diagnostics.Debug.WriteLine($"TargetedPopup.MeasureBorder border:[{border}] Padding:[{Padding}]  availableWidth:[" + availableWidth + "] availableHeight:[" + availableHeight + "]");
            if (availableWidth > 0 && availableHeight > 0 && _contentPresenter.Content != null)
            {
                _contentPresenter.Measure(new Size(availableWidth, availableHeight));
                var result = _contentPresenter.DesiredSize;
                System.Diagnostics.Debug.WriteLine("TargetedPopup.MeasureBorder  _contentPresenter.DesiredSize:[" + _contentPresenter.DesiredSize + "]");
                result.Width += Padding.Horizontal() + border;
                result.Height += Padding.Vertical() + border;



                var resultSize = new Size(
                    this.HasPrescribedWidth()
                        ? width : result.Width,
                    this.HasPrescribedHeight()
                        ? height : result.Height
                    );

                System.Diagnostics.Debug.WriteLine($"TargetedPopup.MeasureBorder resultSize: {resultSize}");
                return resultSize;
            }

            return failSize;
        }

        #endregion

    }
}
