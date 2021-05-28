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
                BorderColor = BorderColor,
                BackgroundColor = BackgroundColor
            };
            m_Border.PropertyChanged += OnBorderPropertyChanged;
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
            if (e.PropertyName == nameof(HasDrawn))
                HasDrawn = m_Border.HasDrawn;
            else if (e.PropertyName == nameof(ActualSize))
                ActualSize = m_Border.ActualSize;
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
            //m_BorderPopup = new PopupWindow(m_Border, DisplayExtensions.Width, DisplayExtensions.Height, true);
            //m_BorderPopup.ShowAtLocation(App.Current, GravityFlags.Top | GravityFlags.Left, 0, 0);
            m_BorderPopup.Touchable = true;
            m_BorderPopup.Focusable = false;
            m_BorderPopup.OutsideTouchable = false;
            //m_BorderPopup.TouchIntercepted += M_BorderPopup_TouchIntercepted;
            m_BorderPopup.TouchModal = false;
            m_BorderPopup.SetTouchInterceptor(null);
            //m_BorderPopup.ClippingEnabled = false;
            //m_BorderPopup.SetBackgroundDrawable(PageOverlayColor.AsDrawable());
            /*
            m_BorderPopup.SetTouchInterceptor(new OverlayTouchListener((view, point) =>
            {
                // return true if touch is within border
                //return point.X >=0 && point.X < view.Width && point.Y >= 0 && point.Y < view.Height;
                System.Diagnostics.Debug.WriteLine($"TargetedPopup.INTERCEPT");

                // return: dismiss popup?
                return true;
            }));
            */
            //_popup.IsOpen = true;
            //await Task.Delay(5);
            //_popup.InvalidateMeasure();
            m_BorderPopup.ShowAtLocation(App.Current, GravityFlags.Top | GravityFlags.Left, (int)(m_PopupFrame.Left + 0.5), (int)(m_PopupFrame.Top + 0.5));

            if (IsAnimated)
            {
                Action<double> action = percent => m_Border.Alpha = (float)percent;
                var animator = new global::P42.Utils.ActionAnimator(0.11, 0.95, TimeSpan.FromMilliseconds(300), action);
                await animator.RunAsync();
            }

            //_border.Bind(BubbleBorder.OpacityProperty, this, nameof(Opacity));
            m_Border.Alpha = 1;

            await ((IElement)m_Border).WaitForDrawComplete();
            HasDrawn = true;

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

            //m_Border.SizeChanged -= OnBorderSizeChanged;

            PoppedCause = cause;
            PoppedTrigger = trigger;
            await OnPopBeginAsync();

            if (IsAnimated)
            {
                Action<double> action = percent => m_Border.Alpha = (float)percent;
                var animator = new global::P42.Utils.ActionAnimator(0.95, 0.11, TimeSpan.FromMilliseconds(300), action);
                await animator.RunAsync();
            }

            //_popup.IsOpen = false;

            m_BorderPopup.Dismiss();
            m_OverlayPopup.Dismiss();
            m_BorderPopup.Dispose();
            m_OverlayPopup.Dispose();

            CompletePop(PoppedCause, PoppedTrigger);
        }
        #endregion


        #region Layout
        void UpdateMarginAndAlignment()
        {
            if (Content is null)
                return;

            var windowSize = DisplayExtensions.PxSize();
            if (windowSize.Width < 1 || windowSize.Height < 1)
                return;
            //windowSize.Height -= DisplayExtensions.StatusBarHeight();

            var windowWidth = windowSize.Width - Margin.Horizontal;
            var windowHeight = windowSize.Height - Margin.Vertical;
            var cleanSize = MeasureBorder(new SizeI(windowWidth, windowHeight));

            if (PreferredPointerDirection == PointerDirection.None || Target is null)
            {
                //System.Diagnostics.Debug.WriteLine(GetType() + ".UpdateMarginAndAlignment PreferredPointerDirection == PointerDirection.None");
                CleanMarginAndAlignment(HorizontalAlignment, VerticalAlignment, windowSize, cleanSize);
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
            var margin = Margin;
            var hzAlign = HorizontalAlignment;
            var vtAlign = VerticalAlignment;

            if (stats.PointerDirection.IsHorizontal())
            {
                if (stats.PointerDirection == PointerDirection.Left)
                {
                    margin.Left = target.Right;
                    if (HorizontalAlignment != Alignment.Stretch)
                        hzAlign = Alignment.Start;
                }
                else if (stats.PointerDirection == PointerDirection.Right)
                {
                    margin.Right = windowSize.Width - target.Left;
                    if (HorizontalAlignment != Alignment.Stretch)
                        hzAlign = Alignment.End;
                }

                if (VerticalAlignment == Alignment.Start)
                {
                    margin.Top = Math.Max(Margin.Top, target.Top);
                }
                else if (VerticalAlignment == Alignment.Center)
                {
                    margin.Top = Math.Max(Margin.Top, (int)(0.5 + target.VerticalCenter - stats.BorderSize.Height / 2.0));
                    vtAlign = Alignment.Start;
                }
                else if (VerticalAlignment == Alignment.End)
                {
                    margin.Bottom = Math.Max(Margin.Bottom, windowSize.Height  - target.Bottom);
                }

                if (margin.Top + stats.BorderSize.Height > windowSize.Height  - Margin.Bottom)
                    margin.Top = windowSize.Height - Margin.Bottom - stats.BorderSize.Height;

                if (VerticalAlignment == Alignment.End)
                    m_Border.PointerAxialPosition = (target.Top - (windowSize.Height - margin.Bottom - cleanSize.Height)) + target.Bottom - (target.Top + target.Bottom) / 2.0;
                else
                    m_Border.PointerAxialPosition = (target.Top - margin.Top) + target.Bottom - (target.Top + target.Bottom) / 2.0;
            }
            else
            {
                if (stats.PointerDirection == PointerDirection.Up)
                {
                    margin.Top = target.Bottom;
                    if (VerticalAlignment != Alignment.Stretch)
                        vtAlign = Alignment.Start;
                }
                else if (stats.PointerDirection == PointerDirection.Down)
                {
                    margin.Bottom = windowSize.Height - target.Top;
                    if (VerticalAlignment != Alignment.Stretch)
                        vtAlign = Alignment.End;
                }

                if (HorizontalAlignment == Alignment.Start)
                {
                    margin.Left = Math.Max(Margin.Left, target.Left);
                }
                else if (HorizontalAlignment == Alignment.Center)
                {
                    margin.Left = Math.Max(Margin.Left, (int)(0.5 + target.HoriztonalCenter - stats.BorderSize.Width / 2.0));
                    hzAlign = Alignment.Start;
                }
                else if (HorizontalAlignment == Alignment.End)
                {
                    margin.Right = Math.Max(Margin.Right, windowSize.Width - target.Right);
                }

                if (margin.Left + stats.BorderSize.Width > windowSize.Width - Margin.Right)
                    margin.Left = windowSize.Width - Margin.Right - stats.BorderSize.Width;

                if (HorizontalAlignment == Alignment.End)
                    m_Border.PointerAxialPosition = (target.Left - (windowSize.Width - margin.Right - cleanSize.Width)) + (target.Right - (target.Left + target.Right) / 2.0);
                else
                    m_Border.PointerAxialPosition = (target.Left - margin.Left) + (target.Right - (target.Left + target.Right) / 2.0);
            }

            ActualPointerDirection = m_Border.PointerDirection = stats.PointerDirection;
            SetMarginAndAlignment(margin, hzAlign, vtAlign, windowSize, cleanSize);
        }

        void CleanMarginAndAlignment(Alignment hzAlign, Alignment vtAlign, SizeI windowSize, SizeI borderSize)
        {
            ActualPointerDirection = PointerDirection.None;

            if (m_Border is null)
                return;

            m_Border.PointerDirection = ActualPointerDirection;
            SetMarginAndAlignment(Margin, hzAlign, vtAlign, windowSize, borderSize);
        }

        void SetMarginAndAlignment(ThicknessI margin, Alignment hzAlign, Alignment vtAlign, SizeI windowSize, SizeI borderSize)
        {
            var frame = CalculateFrame(margin, hzAlign, vtAlign, windowSize, borderSize);

            //_popup.Margin = new Thickness(0);
            //m_PopupOffset = new Point(frame.Left, frame.Top + DisplayExtensions.StatusBarHeight());
            m_PopupFrame = frame;
            m_PopupFrame.Top += DisplayExtensions.StatusBarHeight();

            m_Border.Margin = new ThicknessI(0);
            m_Border.RequestedWidth = frame.Width;
            m_Border.RequestedHeight = frame.Height;

            m_Border.HorizontalAlignment = hzAlign == Alignment.Stretch && !this.HasPrescribedWidth()
                ? Alignment.Stretch
                : Alignment.Start;
            m_Border.VerticalAlignment = vtAlign == Alignment.Stretch && !this.HasPrescribedHeight()
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
                borderSize.Width = RequestedWidth;
            if (this.HasPrescribedHeight())
                borderSize.Height = RequestedHeight;
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
            var windowSpaceW = Math.Max(0, windowSize.Width - Margin.Horizontal);
            var windowSpaceH = Math.Max(0, windowSize.Height - Margin.Vertical);
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
                    var availL = target.Left - Margin.Left;
                    var availR = windowBounds.Width - Margin.Right - target.Right;
                    var availT = target.Top - Margin.Top;
                    var availB = windowBounds.Height - target.Bottom - Margin.Bottom;

                    var maxWidth = MaxWidth;
                    if (RequestedWidth > 0 && RequestedWidth < maxWidth)
                        maxWidth = RequestedWidth;
                    if (maxWidth > 0 && HorizontalAlignment != Alignment.Stretch)
                    {
                        availL = Math.Min(availL, maxWidth);
                        availR = Math.Min(availR, maxWidth);
                    }

                    var maxHeight = MaxHeight;
                    if (RequestedHeight > 0 && RequestedHeight < maxHeight)
                        maxHeight = RequestedHeight;
                    if (maxHeight > 0 && VerticalAlignment != Alignment.Stretch)
                    {
                        availT = Math.Min(availT, maxHeight);
                        availB = Math.Min(availB, maxHeight);
                    }

                    return new ThicknessI(availL, availT, availR, availB);
                }
            }

            //if (PointToOffScreenElements)
            //    return new Thickness(windowBounds.Width - Margin.Horizontal, windowBounds.Height - Margin.Vertical, windowBounds.Width - Margin.Horizontal, windowBounds.Height - Margin.Vertical);

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
                width = Math.Min(RequestedWidth, width);
            if (this.HasPrescribedHeight())
                height = Math.Min(RequestedHeight, height);

            if (this.HasPrescribedWidth() && this.HasPrescribedHeight())
                return new SizeI(width, height);

            if (IsEmpty)
                return new SizeI(
                    this.HasPrescribedWidth()
                        ? width : MinWidth + Padding.Horizontal,
                    this.HasPrescribedHeight()
                        ? height : MinHeight + Padding.Vertical
                    );

            var hasBorder = (BorderWidth > 0) && BorderColor.A > 0;
            var border = (int)(BorderWidth * (hasBorder ? 1 : 0) * 2 + 0.5);
            var availableWidth = width - Padding.Horizontal - border - 1;
            var availableHeight = height - Padding.Vertical - border - 1;
            System.Diagnostics.Debug.WriteLine($"TargetedPopup.MeasureBorder border:[{border}] Padding:[{Padding}]  availableWidth:[" + availableWidth + "] availableHeight:[" + availableHeight + "]");
            if (availableWidth > 0 && availableHeight > 0 && Content != null)
            {
                var hzSpec = Android.Views.ViewGroup.MeasureSpec.MakeMeasureSpec((int)(availableWidth + 0.5), HorizontalAlignment.AsMeasureSpecMode());
                var vtSpec = Android.Views.ViewGroup.MeasureSpec.MakeMeasureSpec((int)(availableHeight + 0.5), VerticalAlignment.AsMeasureSpecMode());
                Content.Measure(hzSpec, vtSpec);
                System.Diagnostics.Debug.WriteLine($"TargetedPopup.MeasureBorder Content.Size:{Content.MeasuredWidth}, {Content.MeasuredHeight} Padding:{Padding}");
                //var result = _contentPresenter.DesiredSize;
                //System.Diagnostics.Debug.WriteLine("TargetedPopup.MeasureBorder  _contentPresenter.DesiredSize:[" + _contentPresenter.DesiredSize + "]");
                var resultWidth = Math.Min(Math.Max(Content.MeasuredWidth + Padding.Horizontal + border, MinWidth), MaxWidth);
                var resultHeight = Math.Min(Math.Max(Content.MeasuredHeight + Padding.Vertical + border, MinHeight), MaxHeight);



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


        bool HasPrescribedHeight() => RequestedHeight >= 0;

        bool HasPrescribedWidth() => RequestedWidth >= 0;

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

        #region DINotifiable




        #region Methods
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Padding))
                m_Border.Padding = Padding;
            else if (propertyName == nameof(BorderWidth))
                m_Border.BorderWidth = BorderWidth;
            else if (propertyName == nameof(BorderColor))
                m_Border.BorderColor = BorderColor;
            else if (propertyName == nameof(CornerRadius))
                m_Border.CornerRadius = CornerRadius;
            else if (propertyName == nameof(BackgroundColor))
                m_Border.BackgroundColor = BackgroundColor;
            else if (propertyName == nameof(HasDrawn) && HasDrawn)
                HasDrawnTaskCompletionSource?.TrySetResult(true);
        }

        public void RedrawElement() => m_Border.PostInvalidate();

        public void RelayoutElement() => m_Border.RequestLayout();
        
        #endregion

        #endregion




    }





}
