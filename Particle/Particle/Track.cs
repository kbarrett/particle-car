using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Particle
{
    class Track
    {
        int timeRemainingForRestriction = 0;
        TrackPoint start;
        public Track()
        {
            int radius = 200;
            Vector2 centre = new Vector2(300, 300);

            start = new TrackPoint(centre + new Vector2(0, -radius), 0);
            TrackPoint temp = start;
            int increment = 2;
            int sideLength = 60;
            int currentSide = 0;
            int fudgeFactor = 4;
            for (int i = increment; i <= 360; i += increment)
            {
                double x = 0, y = 0;
                if (i <= 90)
                {
                    x = -radius * Math.Sin(toRad(i));
                    y = -radius * Math.Cos(toRad(i));
                }
                else if (i<180)
                {
                    x = -radius * Math.Cos(toRad(i - 90));
                    y = radius * Math.Sin(toRad(i - 90));
                }
                else if (i == 180)
                {
                    if (currentSide < sideLength)
                    {
                        x = fudgeFactor * increment * currentSide;
                        y = radius;
                        i -= increment;
                        ++currentSide;
                    }
                }
                else if (i <= 270)
                {
                    x = radius * Math.Sin(toRad(-i)) + fudgeFactor * increment * currentSide;
                    y = -radius * Math.Cos(toRad(-i));
                }
                else if (i < 360)
                {
                    x = -radius * Math.Cos(toRad(90 - i)) + fudgeFactor * increment * currentSide;
                    y = -radius * Math.Sin(toRad(90 - i));
                }
                else if (i == 360)
                {
                    if (currentSide > 0)
                    {
                        x = fudgeFactor * increment * currentSide;
                        y = -radius;
                        i -= increment;
                        --currentSide;
                    }
                }
                temp.next = new TrackPoint(centre + new Vector2((float)x, (float)y), -i);
                temp = temp.next;
            }
            temp.next = start;
        }

        private double toRad(float degree)
        {
            return degree * Math.PI / 180;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            start.Draw(spriteBatch, start);
        }

        public void Update() 
        {
            if (timeRemainingForRestriction > 0)
            {
                --timeRemainingForRestriction;
                return;
            }
            Random random = new Random();
            if (random.Next(50) == 0)
            {
                int startPoint = random.Next(360);
                int length = random.Next(100);
                TrackPoint tp = start;
                for (int i = 0; i < startPoint; ++i)
                {
                    tp = tp.next;
                }
                for (int i = 0; i < length; ++i)
                {
                    tp.setThroughput(1);
                    tp = tp.next;
                }
                timeRemainingForRestriction = random.Next(500);
            }
            else
            {
                TrackPoint tp = start;
                do
                {
                    tp.resetThroughput();
                    tp = tp.next;
                }
                while (!tp.Equals(start));
            }
        }

        public TPEnumerator GetEnumerator()
        {
            return new TPEnumerator(start);
        }

        public class TPEnumerator : IEnumerator<TrackPoint>
        {
            private TrackPoint start;
            public TrackPoint Current { get; set; }

            object System.Collections.IEnumerator.Current { get { return Current; }}

            public TPEnumerator(TrackPoint start)
            {
                Current = start;
                this.start = start;
            }
            public bool MoveNext()
            {
                Current = ((TrackPoint)Current).next;
                if (Current == start)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            public void Dispose()
            {
            }
            public void Reset()
            {
                Current = start;
            }
        }
    }
}
