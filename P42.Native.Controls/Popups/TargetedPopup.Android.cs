using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using P42.Native.Controls;
using static Android.Views.ViewGroup;

namespace P42.Native.Controls
{
    public partial class TargetedPopup : IDisposable
    {

        #region Fields
        PopupWindow m_OverlayPopup;
        readonly View m_Overlay;

        PopupWindow m_BorderPopup;
        readonly BubbleBorder m_Border;

        PointI m_PopupOffset = new PointI();
        RectI m_PopupFrame = new RectI();
        private bool _disposed;
        #endregion


        #region Construction / Disposal
        public TargetedPopup(View target = null) : this(global::P42.Utils.Droid.Settings.Context, target) { }

        public TargetedPopup(Context context, Android.Views.View target = null)
        {
            Target = target;

            m_Overlay = new View(context)
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
                Background = PageOverlayColor.AsDrawable()
            };

            m_Border = new BubbleBorder(context)
            {
                DipBorderColor = DipBorderColor,
                DipBackgroundColor = DipBackgroundColor
            };
            m_Border.PropertyChanged += OnBorderPropertyChanged;

            NtvBaseView = m_Border;

            m_Border.HasShadow = HasShadow;
            m_Border.NtvShadowRadius = ShadowRadius;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            { 
                m_Border.PropertyChanged -= OnBorderPropertyChanged;
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Event Handlers
        private void OnBorderPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DipHasDrawn))
                DipHasDrawn = m_Border.DipHasDrawn;
            else if (e.PropertyName == nameof(NtvActualSize))
                NtvActualSize = m_Border.NtvActualSize;
        }

        void M_BorderPopup_TouchIntercepted(object sender, View.TouchEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"TargetedPopup.INTERCEPT");
            e.Handled = false;
        }
        #endregion


        #region Push / Pop
        async Task InnerPushAsyc()
        {
            PushPopState = PushPopState.Pushing;
            _popCompletionSource = null;

            PoppedCause = PopupPoppedCause.BackgroundTouch;
            PoppedTrigger = null;

            await OnPushBeginAsync();

            // requiired to render popup the first time.
            m_Border.Alpha = 0;
            _popupOpenedCompletionSource = new TaskCompletionSource<bool>();

            //UpdateMarginAndAlignment();
            
            if (PageOverlayMode != PageOverlayMode.TouchTransparent || b_PageOverlayColor.A > 0 )
            {
                m_OverlayPopup = new PopupWindow(m_Overlay, DisplayExtensions.PxWidth(), DisplayExtensions.PxHeight(), true);
                if (PageOverlayMode == PageOverlayMode.TouchTransparent)
                {
                    m_OverlayPopup.Touchable = false;
                    m_OverlayPopup.TouchModal = false;
                }
                else
                    m_OverlayPopup.SetTouchInterceptor(new OverlayTouchListener((view, point) =>
                    {
                        if (PageOverlayMode == PageOverlayMode.TouchDismiss)
                        {
                            System.Diagnostics.Debug.WriteLine($"TargetedPopup: Overlay INTERCEPT");
                            PopAsync(PopupPoppedCause.BackgroundTouch, null);
                            return true;
                        }
                        return b_PageOverlayMode != PageOverlayMode.TouchTransparent;
                    }));
                m_OverlayPopup.ShowAtLocation(App.Current, GravityFlags.Top | GravityFlags.Left, 0, 0);
            }

            UpdateMarginAndAlignment();

            m_BorderPopup = new PopupWindow(m_Border, (int)(m_PopupFrame.Width + 0.5), (int)(m_PopupFrame.Height + 0.5), true);
            m_BorderPopup.Elevation = 100f;

            m_BorderPopup.Touchable = true;
            m_BorderPopup.Focusable = false;
            m_BorderPopup.OutsideTouchable = false;

            m_BorderPopup.TouchModal = false;
            m_BorderPopup.SetTouchInterceptor(null);

            m_BorderPopup.ShowAtLocation(App.Current, GravityFlags.Top | GravityFlags.Left, (int)(m_PopupFrame.Left + 0.5), (int)(m_PopupFrame.Top + 0.5));

            if (IsAnimated)
            {
                Action<double> action = percent => m_Border.Alpha = (float)percent;
                var animator = new global::P42.Utils.ActionAnimator(0.11, 0.95, TimeSpan.FromMilliseconds(300), action);
                await animator.RunAsync();
            }

            m_Border.Alpha = 1;

            await ((IElement)m_Border).DipWaitForDrawComplete();
            DipHasDrawn = true;

            if (PopAfter > default(TimeSpan))
            {
                global::P42.Utils.Timer.StartTimer(PopAfter, async () =>
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

            PoppedCause = cause;
            PoppedTrigger = trigger;
            await OnPopBeginAsync();

            if (IsAnimated)
            {
                Action<double> action = percent => m_Border.Alpha = (float)percent;
                var animator = new global::P42.Utils.ActionAnimator(0.95, 0.11, TimeSpan.FromMilliseconds(300), action);
                await animator.RunAsync();
            }

            m_BorderPopup.Dismiss();
            m_OverlayPopup.Dismiss();

            if (!DisposeOnPop)
                m_Border.DipContent = null;

            m_BorderPopup.Dispose();
            m_OverlayPopup.Dispose();

            CompletePop(PoppedCause, PoppedTrigger);
        }
        #endregion


        #region Layout
        void UpdateMarginAndAlignment()
        {
            if (DipContent is null)
                return;

            var windowSize = DisplayExtensions.PxSize();
            if (windowSize.Width < 1 || windowSize.Height < 1)
                return;
            windowSize.Height -= DisplayExtensions.StatusBarHeight();

            var windowWidth = windowSize.Width - NtvMargin.Horizontal;
            var windowHeight = windowSize.Height - NtvMargin.Vertical;
            var cleanSize = MeasureBorder(new SizeI(windowWidth, windowHeight));

            if (PreferredPointerDirection == PointerDirection.None || Target is null)
            {
                //System.Diagnostics.Debug.WriteLine(GetType() + ".UpdateMarginAndAlignment PreferredPointerDirection == PointerDirection.None");
                CleanMarginAndAlignment(DipHorizontalAlignment, DipVerticalAlignment, windowSize, cleanSize);
                return;
            }

            var target = TargetBounds();
            System.Diagnostics.Debug.WriteLine(GetType() + ".UpdateBorderMarginAndAlignment targetBounds:["+target+"]");

            var availableSpace = AvailableSpace(target);
            var stats = BestFit(availableSpace, cleanSize);

            if (stats.PointerDirection == PointerDirection.None)
            {
                CleanMarginAndAlignment(Alignment.Center, Alignment.Center, windowSize, cleanSize);
                return;
            }

            ActualPointerDirection = stats.PointerDirection;
            var margin = NtvMargin;
            var hzAlign = DipHorizontalAlignment;
            var vtAlign = DipVerticalAlignment;

            if (stats.PointerDirection.IsHorizontal())
            {
                if (stats.PointerDirection == PointerDirection.Left)
                {
                    margin.Left = target.Right;
                    if (DipHorizontalAlignment != Alignment.Stretch)
                        hzAlign = Alignment.Start;
                }
                else if (stats.PointerDirection == PointerDirection.Right)
                {
                    margin.Right = windowSize.Width - target.Left;
                    if (DipHorizontalAlignment != Alignment.Stretch)
                        hzAlign = Alignment.End;
                }

                if (DipVerticalAlignment == Alignment.Start)
                {
                    margin.Top = Math.Max(NtvMargin.Top, target.Top);
                }
                else if (DipVerticalAlignment == Alignment.Center)
                {
                    margin.Top = Math.Max(NtvMargin.Top, (int)(0.5 + target.VerticalCenter - stats.BorderSize.Height / 2.0));
                    vtAlign = Alignment.Start;
                }
                else if (DipVerticalAlignment == Alignment.End)
                {
                    margin.Bottom = Math.Max(NtvMargin.Bottom, windowSize.Height  - target.Bottom);
                }

                if (margin.Top + stats.BorderSize.Height > windowSize.Height  - NtvMargin.Bottom)
                    margin.Top = windowSize.Height - NtvMargin.Bottom - stats.BorderSize.Height;

                if (DipVerticalAlignment == Alignment.End)
                    m_Border.NtvPointerAxialPosition = (target.Top - (windowSize.Height - margin.Bottom - cleanSize.Height)) + target.Bottom - (target.Top + target.Bottom) / 2.0;
                else
                    m_Border.NtvPointerAxialPosition = (target.Top - margin.Top) + target.Bottom - (target.Top + target.Bottom) / 2.0;
            }
            else
            {
                if (stats.PointerDirection == PointerDirection.Up)
                {
                    margin.Top = target.Bottom;
                    if (DipVerticalAlignment != Alignment.Stretch)
                        vtAlign = Alignment.Start;
                }
                else if (stats.PointerDirection == PointerDirection.Down)
                {
                    margin.Bottom = windowSize.Height - target.Top;
                    if (DipVerticalAlignment != Alignment.Stretch)
                        vtAlign = Alignment.End;
                }

                if (DipHorizontalAlignment == Alignment.Start)
                {
                    margin.Left = Math.Max(NtvMargin.Left, target.Left);
                }
                else if (DipHorizontalAlignment == Alignment.Center)
                {
                    margin.Left = Math.Max(NtvMargin.Left, (int)(0.5 + target.HoriztonalCenter - stats.BorderSize.Width / 2.0));
                    hzAlign = Alignment.Start;
                }
                else if (DipHorizontalAlignment == Alignment.End)
                {
                    margin.Right = Math.Max(NtvMargin.Right, windowSize.Width - target.Right);
                }

                if (margin.Left + stats.BorderSize.Width > windowSize.Width - NtvMargin.Right)
                    margin.Left = windowSize.Width - NtvMargin.Right - stats.BorderSize.Width;

                if (DipHorizontalAlignment == Alignment.End)
                    m_Border.NtvPointerAxialPosition = (target.Left - (windowSize.Width - margin.Right - cleanSize.Width)) + (target.Right - (target.Left + target.Right) / 2.0);
                else
                    m_Border.NtvPointerAxialPosition = (target.Left - margin.Left) + (target.Right - (target.Left + target.Right) / 2.0);
            }

            ActualPointerDirection = m_Border.DipPointerDirection = stats.PointerDirection;
            SetMarginAndAlignment(margin, hzAlign, vtAlign, windowSize, cleanSize);
        }

        void CleanMarginAndAlignment(Alignment hzAlign, Alignment vtAlign, SizeI windowSize, SizeI borderSize)
        {
            ActualPointerDirection = PointerDirection.None;

            if (m_Border is null)
                return;

            m_Border.DipPointerDirection = ActualPointerDirection;
            SetMarginAndAlignment(NtvMargin, hzAlign, vtAlign, windowSize, borderSize);
        }

        void SetMarginAndAlignment(ThicknessI margin, Alignment hzAlign, Alignment vtAlign, SizeI windowSize, SizeI borderSize)
        {
            var frame = CalculateFrame(margin, hzAlign, vtAlign, windowSize, borderSize);

            //_popup.NtvMargin = new Thickness(0);
            //m_PopupOffset = new Point(frame.Left, frame.Top + DisplayExtensions.StatusBarHeight());
            m_PopupFrame = frame;
            m_PopupFrame.Top += DisplayExtensions.StatusBarHeight();

            m_Border.NtvMargin = new ThicknessI(0);
            m_Border.NtvRequestedWidth = frame.Width;
            m_Border.NtvRequestedHeight = frame.Height;

            m_Border.DipHorizontalAlignment = hzAlign == Alignment.Stretch && !this.HasPrescribedWidth()
                ? Alignment.Stretch
                : Alignment.Start;
            m_Border.DipVerticalAlignment = vtAlign == Alignment.Stretch && !this.HasPrescribedHeight()
                ? Alignment.Stretch
                : Alignment.Start;

            System.Diagnostics.Debug.WriteLine("TargetedPopup.CleanMarginAndAlignment frame: " + frame);
        }

        RectI CalculateFrame(ThicknessI margin, Alignment hzAlign, Alignment vtAlign, SizeI windowSize, SizeI borderSize)
        {
            var hzPointer = ActualPointerDirection.IsHorizontal() ? PointerLength : 0;
            var vtPointer = ActualPointerDirection.IsVertical() ? PointerLength : 0;
            var left = margin.Left;
            var top = margin.Top;
            if (this.HasPrescribedWidth())
                borderSize.Width = NtvRequestedWidth;
            if (this.HasPrescribedHeight())
                borderSize.Height = NtvRequestedHeight;
            var right = Math.Min(left + borderSize.Width + hzPointer, windowSize.Width - margin.Right);
            var bottom = Math.Min(top + borderSize.Height + vtPointer, windowSize.Height - margin.Bottom);

            if (hzAlign == Alignment.Center)
            {
                left = Math.Max((int)((windowSize.Width - borderSize.Width) / 2.0 + 0.5), left);
                right = Math.Min(left + borderSize.Width, windowSize.Width - margin.Right);
            }
            else if (hzAlign == Alignment.End)
            {
                left = Math.Max(windowSize.Width - margin.Right - hzPointer - borderSize.Width, left);
                right = windowSize.Width - margin.Right;
            }
            else if (hzAlign == Alignment.Stretch && !this.HasPrescribedWidth())
            {
                right = windowSize.Width - margin.Right;
            }

            if (vtAlign == Alignment.Center)
            {
                top = Math.Max((int)((windowSize.Height - borderSize.Height) / 2.0 + 0.5), top);
                bottom = Math.Min(top + borderSize.Height, windowSize.Height - margin.Bottom);
            }
            else if (vtAlign == Alignment.End)
            {
                top = Math.Max(windowSize.Height - margin.Bottom - vtPointer - borderSize.Height, top);
                bottom = windowSize.Height - margin.Bottom;
            }
            else if (vtAlign == Alignment.Stretch && !this.HasPrescribedHeight())
            {
                bottom = windowSize.Height - margin.Bottom;
            }
            return new RectI(left, top, right - left, bottom - top);
        }

        DirectionStats BestFit(ThicknessI availableSpace, SizeI cleanSize)
        {
            // given the amount of free space, determine if the border will fit 
            var windowSize = DisplayExtensions.PxSize();
            var windowSpaceW = Math.Max(0, windowSize.Width - NtvMargin.Horizontal);
            var windowSpaceH = Math.Max(0, windowSize.Height - NtvMargin.Vertical);
            var windowSpace = new SizeI(windowSpaceW, windowSpaceH);

            var freeSpaceW = Math.Max(0, windowSpace.Width - cleanSize.Width);
            var freeSpaceH = Math.Max(0, windowSpace.Height - cleanSize.Height);

            var cleanStat = new DirectionStats
            {
                PointerDirection = PointerDirection.None,
                BorderSize = cleanSize,
                FreeSpace = new SizeI(freeSpaceW, freeSpaceH)
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

        RectI TargetBounds()
        {
            var targetBounds = Target is null ? RectI.Empty : Target.GetBoundsRelativeTo(App.Current);

            var targetLeft = (Target is null ? TargetPoint.X : targetBounds.Left) - PointerMargin;
            var targetRight = (Target is null ? TargetPoint.X : targetBounds.Right) + PointerMargin;
            var targetTop = (Target is null ? TargetPoint.Y : targetBounds.Top) - PointerMargin;
            var targetBottom = (Target is null ? TargetPoint.Y : targetBounds.Bottom) + PointerMargin;

            return new RectI(targetLeft, targetTop, targetRight - targetLeft, targetBottom - targetTop);
        }

        ThicknessI AvailableSpace(RectI target)
        {
            var windowBounds = DisplayExtensions.PxSize();
            if (Target != null || (TargetPoint.X > 0 || TargetPoint.Y > 0))
            {
                if (target.Right > 0 && target.Left < windowBounds.Width && target.Bottom > 0 && target.Top < windowBounds.Height)
                {
                    var availL = target.Left - NtvMargin.Left;
                    var availR = windowBounds.Width - NtvMargin.Right - target.Right;
                    var availT = target.Top - NtvMargin.Top;
                    var availB = windowBounds.Height - target.Bottom - NtvMargin.Bottom;

                    var maxWidth = NtvMaxWidth;
                    if (NtvRequestedWidth > 0 && NtvRequestedWidth < maxWidth)
                        maxWidth = NtvRequestedWidth;
                    if (maxWidth > 0 && DipHorizontalAlignment != Alignment.Stretch)
                    {
                        availL = Math.Min(availL, maxWidth);
                        availR = Math.Min(availR, maxWidth);
                    }

                    var maxHeight = NtvMaxHeight;
                    if (NtvRequestedHeight > 0 && NtvRequestedHeight < maxHeight)
                        maxHeight = NtvRequestedHeight;
                    if (maxHeight > 0 && DipVerticalAlignment != Alignment.Stretch)
                    {
                        availT = Math.Min(availT, maxHeight);
                        availB = Math.Min(availB, maxHeight);
                    }

                    return new ThicknessI(availL, availT, availR, availB);
                }
            }

            //if (PointToOffScreenElements)
            //    return new Thickness(windowBounds.Width - NtvMargin.Horizontal, windowBounds.Height - NtvMargin.Vertical, windowBounds.Width - NtvMargin.Horizontal, windowBounds.Height - NtvMargin.Vertical);

            return new ThicknessI(-1, -1, -1, -1);
        }

        List<DirectionStats> GetRectangleBorderStatsForDirection(PointerDirection pointerDirection, DirectionStats cleanStat, ThicknessI availableSpace)
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

        List<DirectionStats> GetMeasuredStatsForDirection(PointerDirection pointerDirection, DirectionStats cleanStat, ThicknessI availableSpace, SizeI windowSpace)
        {
            var stats = new List<DirectionStats>();
            if (pointerDirection.LeftAllowed())
            {
                if (availableSpace.Right > 0)
                {
                    var size = new SizeI(availableSpace.Right, windowSpace.Height);
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
                    var size = new SizeI(availableSpace.Left, windowSpace.Height);
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
                    var size = new SizeI(windowSpace.Width, availableSpace.Bottom);
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
                    var size = new SizeI(windowSpace.Width, availableSpace.Top);
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

        SizeI MeasureBorder(SizeI available, SizeI failSize = default)
        {
            System.Diagnostics.Debug.WriteLine("\n");
            System.Diagnostics.Debug.WriteLine($"TargetedPopup.MeasureBorder({available})");
            var width = available.Width;
            var height = available.Height;
            if (this.HasPrescribedWidth())
                width = Math.Min(NtvRequestedWidth, width);
            if (this.HasPrescribedHeight())
                height = Math.Min(NtvRequestedHeight, height);

            if (this.HasPrescribedWidth() && this.HasPrescribedHeight())
                return new SizeI(width, height);

            if (IsEmpty)
                return new SizeI(
                    this.HasPrescribedWidth()
                        ? width : NtvMinWidth + NtvPadding.Horizontal,
                    this.HasPrescribedHeight()
                        ? height : NtvMinHeight + NtvPadding.Vertical
                    );

            var hasBorder = (NtvBorderWidth > 0) && DipBorderColor.A > 0;
            var border = (int)(NtvBorderWidth * (hasBorder ? 1 : 0) * 2 + 0.5);
            var availableWidth = width - NtvPadding.Horizontal - border - 1;
            var availableHeight = height - NtvPadding.Vertical - border - 1;
            System.Diagnostics.Debug.WriteLine($"TargetedPopup.MeasureBorder border:[{border}] NtvPadding:[{NtvPadding}]  availableWidth:[" + availableWidth + "] availableHeight:[" + availableHeight + "]");
            if (availableWidth > 0 && availableHeight > 0 && DipContent != null)
            {
                var hzSpec = Android.Views.ViewGroup.MeasureSpec.MakeMeasureSpec((int)(availableWidth + 0.5), DipHorizontalAlignment.AsMeasureSpecMode());
                var vtSpec = Android.Views.ViewGroup.MeasureSpec.MakeMeasureSpec((int)(availableHeight + 0.5), DipVerticalAlignment.AsMeasureSpecMode());
                DipContent.Measure(hzSpec, vtSpec);
                System.Diagnostics.Debug.WriteLine($"TargetedPopup.MeasureBorder Content.Size:{DipContent.MeasuredWidth}, {DipContent.MeasuredHeight} NtvPadding:{NtvPadding}");
                //var result = _contentPresenter.DesiredSize;
                //System.Diagnostics.Debug.WriteLine("TargetedPopup.MeasureBorder  _contentPresenter.DesiredSize:[" + _contentPresenter.DesiredSize + "]");
                var resultWidth = Math.Min(Math.Max(DipContent.MeasuredWidth + NtvPadding.Horizontal + border, NtvMinWidth), NtvMaxWidth);
                var resultHeight = Math.Min(Math.Max(DipContent.MeasuredHeight + NtvPadding.Vertical + border, NtvMinHeight), NtvMaxHeight);



                var resultSize = new SizeI(
                    this.HasPrescribedWidth()
                        ? width : resultWidth,
                    this.HasPrescribedHeight()
                        ? height : resultHeight
                    );

                System.Diagnostics.Debug.WriteLine($"TargetedPopup.MeasureBorder resultSize: {resultSize}");
                return resultSize;
            }

            return failSize;
        }

        #endregion


        bool HasPrescribedHeight() => NtvRequestedHeight >= 0;

        bool HasPrescribedWidth() => NtvRequestedWidth >= 0;

        class OverlayTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            Func<View, Point, bool> Func;

            public OverlayTouchListener(Func<View, Point, bool> func)
                => Func = func;

            public bool OnTouch(View v, MotionEvent e)
            {
                var x = e.GetX();
                var y = e.GetY();
                System.Diagnostics.Debug.WriteLine($"PopupTouchListener.OnTouch {e.Action} [{x}, {y}]");
                return Func?.Invoke(v, new Point(x, y)) ?? false;
            }
        }

        #region INotifiable




        #region Methods
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(NtvPadding))
                m_Border.NtvPadding = NtvPadding;
            else if (propertyName == nameof(NtvBorderWidth))
                m_Border.NtvBorderWidth = NtvBorderWidth;
            else if (propertyName == nameof(DipBorderColor))
                m_Border.DipBorderColor = DipBorderColor;
            else if (propertyName == nameof(NtvCornerRadius))
                m_Border.NtvCornerRadius = NtvCornerRadius;
            else if (propertyName == nameof(DipBackgroundColor))
                m_Border.DipBackgroundColor = DipBackgroundColor;
            else if (propertyName == nameof(DipHasDrawn) && DipHasDrawn)
                DipHasDrawnTaskCompletionSource?.TrySetResult(true);
            else if (propertyName == nameof(HasShadow))
                m_Border.HasShadow = HasShadow;
        }

        public void RedrawElement() => m_Border.PostInvalidate();

        public void RelayoutElement() => m_Border.RequestLayout();
        
        #endregion

        #endregion




    }





}
