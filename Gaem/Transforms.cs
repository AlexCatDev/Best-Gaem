using System;
namespace Gaem
{
    public class TransformFloat : Transform<float>
    {
        public override float CurrentProgress {
            get {
                return Interpolation.ValueAt(ElapsedTime, StartValue, EndValue, StartTime, EndTime, Easing);
            }
        }
    }
}