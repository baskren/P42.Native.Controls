using System;
using P42.Utils;

namespace P42.Native.Controls
{

    public enum PageAnimationDirection
    {
        None,
        Default,
        DefaultHorizontal,
        DefaultVertical,
        RightToLeft,
        BottomToTop,
        LeftToRight,
        TopToBottom
    }

    /// <summary>
    /// Options to customize the page animation
    /// </summary>
    public class PageAnimationOptions
    {
        /// <summary>
        /// Direction of page animation
        /// </summary>
        public PageAnimationDirection Direction { get; set; } = PageAnimationDirection.RightToLeft;

        /// <summary>
        /// Should opacity fade with animation?
        /// </summary>
        public bool ShouldFade { get; set; } = false;

        /// <summary>
        /// Animation duration
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Animation easing function
        /// </summary>
        public Easing EasingFunction { get; set; } = Easing.ExponentialOut;
    }

}
