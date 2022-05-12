// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using System.Drawing;

namespace Game
{
    public static class Interpolation
    {
        public static double Lerp(double start, double final, double amount) => start + (final - start) * amount;

        /// <summary>
        /// Interpolates between 2 values (start and final) using a given base and exponent.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="final">The end value.</param>
        /// <param name="base">The base of the exponential. The valid range is [0, 1], where smaller values mean that the final value is achieved more quickly, and values closer to 1 results in slow convergence to the final value.</param>
        /// <param name="exponent">The exponent of the exponential. An exponent of 0 results in the start values, whereas larger exponents make the result converge to the final value.</param>
        /// <returns></returns>
        public static double Damp(double start, double final, double @base, double exponent) {
            if (@base < 0 || @base > 1)
                throw new ArgumentOutOfRangeException($"{nameof(@base)} has to lie in [0,1], but is {@base}.", nameof(@base));
            if (exponent < 0)
                throw new ArgumentOutOfRangeException($"{nameof(exponent)} has to be bigger than 0, but is {exponent}.", nameof(exponent));

            return Lerp(start, final, 1 - Math.Pow(@base, exponent));
        }

        public static byte ValueAt(double time, byte val1, byte val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (byte)Math.Round(ValueAt(time, (double)val1, val2, startTime, endTime, easing));

        public static sbyte ValueAt(double time, sbyte val1, sbyte val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (sbyte)Math.Round(ValueAt(time, (double)val1, val2, startTime, endTime, easing));

        public static short ValueAt(double time, short val1, short val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (short)Math.Round(ValueAt(time, (double)val1, val2, startTime, endTime, easing));

        public static ushort ValueAt(double time, ushort val1, ushort val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (ushort)Math.Round(ValueAt(time, (double)val1, val2, startTime, endTime, easing));

        public static int ValueAt(double time, int val1, int val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (int)Math.Round(ValueAt(time, (double)val1, val2, startTime, endTime, easing));

        public static uint ValueAt(double time, uint val1, uint val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (uint)Math.Round(ValueAt(time, (double)val1, val2, startTime, endTime, easing));

        public static long ValueAt(double time, long val1, long val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (long)Math.Round(ValueAt(time, (double)val1, val2, startTime, endTime, easing));

        public static ulong ValueAt(double time, ulong val1, ulong val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (ulong)Math.Round(ValueAt(time, (double)val1, val2, startTime, endTime, easing));

        public static float ValueAt(double time, float val1, float val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (float)ValueAt(time, (double)val1, val2, startTime, endTime, easing);

        public static decimal ValueAt(double time, decimal val1, decimal val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) =>
            (decimal)ValueAt(time, (double)val1, (double)val2, startTime, endTime, easing);

        public static double ValueAt(double time, double val1, double val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) {
            if (val1 == val2)
                return val1;

            double current = time - startTime;
            double duration = endTime - startTime;

            if (current == 0)
                return val1;
            if (duration == 0)
                return val2;

            double t = ApplyEasing(easing, current / duration);
            return val1 + t * (val2 - val1);
        }

        public static RectangleF ValueAt(double time, RectangleF val1, RectangleF val2, double startTime, double endTime, EasingTypes easing = EasingTypes.None) {
            float current = (float)(time - startTime);
            float duration = (float)(endTime - startTime);

            if (duration == 0 || current == 0)
                return val1;

            float t = (float)ApplyEasing(easing, current / duration);

            return new RectangleF(
                val1.X + t * (val2.X - val1.X),
                val1.Y + t * (val2.Y - val1.Y),
                val1.Width + t * (val2.Width - val1.Width),
                val1.Height + t * (val2.X - val1.Height));
        }

        public static double ApplyEasing(EasingTypes easing, double time) {
            const double elastic_const = 2 * Math.PI / .3;
            const double elastic_const2 = .3 / 4;

            const double back_const = 1.70158;
            const double back_const2 = back_const * 1.525;

            const double bounce_const = 1 / 2.75;

            switch (easing) {
                default:
                    return time;

                case EasingTypes.In:
                case EasingTypes.InQuad:
                    return time * time;
                case EasingTypes.Out:
                case EasingTypes.OutQuad:
                    return time * (2 - time);
                case EasingTypes.InOutQuad:
                    if (time < .5) return time * time * 2;
                    return --time * time * -2 + 1;

                case EasingTypes.InCubic:
                    return time * time * time;
                case EasingTypes.OutCubic:
                    return --time * time * time + 1;
                case EasingTypes.InOutCubic:
                    if (time < .5) return time * time * time * 4;
                    return --time * time * time * 4 + 1;

                case EasingTypes.InQuart:
                    return time * time * time * time;
                case EasingTypes.OutQuart:
                    return 1 - --time * time * time * time;
                case EasingTypes.InOutQuart:
                    if (time < .5) return time * time * time * time * 8;
                    return --time * time * time * time * -8 + 1;

                case EasingTypes.InQuint:
                    return time * time * time * time * time;
                case EasingTypes.OutQuint:
                    return --time * time * time * time * time + 1;
                case EasingTypes.InOutQuint:
                    if (time < .5) return time * time * time * time * time * 16;
                    return --time * time * time * time * time * 16 + 1;

                case EasingTypes.InSine:
                    return 1 - Math.Cos(time * Math.PI * .5);
                case EasingTypes.OutSine:
                    return Math.Sin(time * Math.PI * .5);
                case EasingTypes.InOutSine:
                    return .5 - .5 * Math.Cos(Math.PI * time);

                case EasingTypes.InExpo:
                    return Math.Pow(2, 10 * (time - 1));
                case EasingTypes.OutExpo:
                    return -Math.Pow(2, -10 * time) + 1;
                case EasingTypes.InOutExpo:
                    if (time < .5) return .5 * Math.Pow(2, 20 * time - 10);
                    return 1 - .5 * Math.Pow(2, -20 * time + 10);

                case EasingTypes.InCirc:
                    return 1 - Math.Sqrt(1 - time * time);
                case EasingTypes.OutCirc:
                    return Math.Sqrt(1 - --time * time);
                case EasingTypes.InOutCirc:
                    if ((time *= 2) < 1) return .5 - .5 * Math.Sqrt(1 - time * time);
                    return .5 * Math.Sqrt(1 - (time -= 2) * time) + .5;

                case EasingTypes.InElastic:
                    return -Math.Pow(2, -10 + 10 * time) * Math.Sin((1 - elastic_const2 - time) * elastic_const);
                case EasingTypes.OutElastic:
                    return Math.Pow(2, -10 * time) * Math.Sin((time - elastic_const2) * elastic_const) + 1;
                case EasingTypes.OutElasticHalf:
                    return Math.Pow(2, -10 * time) * Math.Sin((.5 * time - elastic_const2) * elastic_const) + 1;
                case EasingTypes.OutElasticQuarter:
                    return Math.Pow(2, -10 * time) * Math.Sin((.25 * time - elastic_const2) * elastic_const) + 1;
                case EasingTypes.InOutElastic:
                    if ((time *= 2) < 1)
                        return -.5 * Math.Pow(2, -10 + 10 * time) * Math.Sin((1 - elastic_const2 * 1.5 - time) * elastic_const / 1.5);
                    return .5 * Math.Pow(2, -10 * --time) * Math.Sin((time - elastic_const2 * 1.5) * elastic_const / 1.5) + 1;

                case EasingTypes.InBack:
                    return time * time * ((back_const + 1) * time - back_const);
                case EasingTypes.OutBack:
                    return --time * time * ((back_const + 1) * time + back_const) + 1;
                case EasingTypes.InOutBack:
                    if ((time *= 2) < 1) return .5 * time * time * ((back_const2 + 1) * time - back_const2);
                    return .5 * ((time -= 2) * time * ((back_const2 + 1) * time + back_const2) + 2);

                case EasingTypes.InBounce:
                    time = 1 - time;
                    if (time < bounce_const)
                        return 1 - 7.5625 * time * time;
                    if (time < 2 * bounce_const)
                        return 1 - (7.5625 * (time -= 1.5 * bounce_const) * time + .75);
                    if (time < 2.5 * bounce_const)
                        return 1 - (7.5625 * (time -= 2.25 * bounce_const) * time + .9375);
                    return 1 - (7.5625 * (time -= 2.625 * bounce_const) * time + .984375);
                case EasingTypes.OutBounce:
                    if (time < bounce_const)
                        return 7.5625 * time * time;
                    if (time < 2 * bounce_const)
                        return 7.5625 * (time -= 1.5 * bounce_const) * time + .75;
                    if (time < 2.5 * bounce_const)
                        return 7.5625 * (time -= 2.25 * bounce_const) * time + .9375;
                    return 7.5625 * (time -= 2.625 * bounce_const) * time + .984375;
                case EasingTypes.InOutBounce:
                    if (time < .5) return .5 - .5 * ApplyEasing(EasingTypes.OutBounce, 1 - time * 2);
                    return ApplyEasing(EasingTypes.OutBounce, (time - .5) * 2) * .5 + .5;

                case EasingTypes.OutPow10:
                    return --time * Math.Pow(time, 10) + 1;
            }
        }
    }
}