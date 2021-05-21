using System;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views.Animations;

namespace P42.Native.Controls
{
    public static class PageAnimationExtensions
    {
        public static AnimationSet ToPushAnimation(this PageAnimationOptions options)
        {
            Animation translate = null;
            switch (options.Direction)
            {
                case PageAnimationDirection.None:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f,     // fromX, toX
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f      // fromY, toY
                        );
                    break;
                case PageAnimationDirection.LeftToRight:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, -1.0f, Dimension.RelativeToParent, 0.0f,     // fromX, toX
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f      // fromY, toY
                        );
                    break;
                case PageAnimationDirection.Default:
                case PageAnimationDirection.DefaultHorizontal:
                case PageAnimationDirection.RightToLeft:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, 1.0f, Dimension.RelativeToParent, 0.0f,     // fromX, toX
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f      // fromY, toY
                        );
                    break;
                case PageAnimationDirection.TopToBottom:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f,     // fromX, toX
                        Dimension.RelativeToParent, -1.0f, Dimension.RelativeToParent, 0.0f      // fromY, toY
                        );
                    break;
                case PageAnimationDirection.DefaultVertical:
                case PageAnimationDirection.BottomToTop:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f,     // fromX, toX
                        Dimension.RelativeToParent, 1.0f, Dimension.RelativeToParent, 0.0f      // fromY, toY
                        );
                    break;
            }
            translate.Duration = options.Duration.Milliseconds;
            //translate.Interpolator = new EasingInterpolator(options.EasingFunction);
            //translate.Interpolator = new BounceInterpolator();

            Animation alpha = null;
            if (options.ShouldFade)
            {
                alpha = new AlphaAnimation(0, 1)
                {
                    Duration = options.Duration.Milliseconds,
                    Interpolator = new LinearInterpolator()
                };
            }

            if (alpha is null && translate is null)
                return null;

            AnimationSet result = new AnimationSet(true);
            if (translate != null)
                result.AddAnimation(translate);
            if (alpha != null)
                result.AddAnimation(alpha);

            result.Duration = options.Duration.Milliseconds;
            result.Interpolator = new EasingInterpolator(options.EasingFunction);

            return result;
        }

        public static AnimationSet ToPopAnimation(this PageAnimationOptions options)
        {
            Animation translate = null;
            switch (options.Direction)
            {
                case PageAnimationDirection.None:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f,     // fromX, toX
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f      // fromY, toY
                        );
                    break;
                case PageAnimationDirection.Default:
                case PageAnimationDirection.DefaultHorizontal:
                case PageAnimationDirection.LeftToRight:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 1.0f,     // fromX, toX
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f      // fromY, toY
                        );
                    break;
                case PageAnimationDirection.RightToLeft:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, -1.0f,     // fromX, toX
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent,  0.0f      // fromY, toY
                        );
                    break;
                case PageAnimationDirection.DefaultVertical:
                case PageAnimationDirection.TopToBottom:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f,     // fromX, toX
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 1.0f      // fromY, toY
                        );
                    break;
                case PageAnimationDirection.BottomToTop:
                    translate = new TranslateAnimation(
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, 0.0f,     // fromX, toX
                        Dimension.RelativeToParent, 0.0f, Dimension.RelativeToParent, -1.0f      // fromY, toY
                        );
                    break;
            }
            translate.Duration = options.Duration.Milliseconds;
            //translate.Interpolator = new EasingInterpolator(options.EasingFunction);
            //translate.Interpolator = new BounceInterpolator();

            Animation alpha = null;
            if (options.ShouldFade)
            {
                alpha = new AlphaAnimation(1, 0)
                {
                    Duration = options.Duration.Milliseconds,
                    Interpolator = new LinearInterpolator()
                };
            }

            if (alpha is null && translate is null)
                return null;

            AnimationSet result = new AnimationSet(true);
            if (translate != null)
                result.AddAnimation(translate);
            if (alpha != null)
                result.AddAnimation(alpha);

            result.Duration = options.Duration.Milliseconds;
            result.Interpolator = new EasingInterpolator(options.EasingFunction);

            return result;
        }

    }


    public class EasingInterpolator : Java.Lang.Object, IInterpolator
    {
        P42.Utils.Easing Easing;
        public EasingInterpolator(P42.Utils.Easing easing)
        {
            Easing = easing;
        }

        public float GetInterpolation(float input)
            => (float)Easing.Ease(input);
        
    }
}
