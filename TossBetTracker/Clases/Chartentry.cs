using System;

namespace TossBetTracker
{
    public class Chartentry
    {
        public float Value { get; }
        public Chartentry(float value)
        {
            this.Value = value;
        }

        public SkiaSharp.SKColor Color { get; set; }
        public string Label { get; set; }
        public string ValueLabel { get; set; }
    }
}

