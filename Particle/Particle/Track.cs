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
        public TrackPoint start;

        const int checkpointlength = 100;
        const int trackwidth = 400;

        public Track()
        {
            int radius = 200;
            Vector2 centre = new Vector2(300, 300);

            start = new TrackPoint(centre + getVector(radius, 0), 0, checkpointlength, trackwidth);
            TrackPoint temp = start;
            int increment = 15;
            for (int i = increment; i < 360; i += increment)
            {
                temp.next = new TrackPoint(centre + getVector(radius, i), -i, checkpointlength, trackwidth);

                temp = temp.next;
            }
            temp.next = start;
        }

        private Vector2 getVector(int radius, int i)
        {
            double x = 0, y = 0;
            if (i <= 180)
                {
                    if (i <= 90)
                    {
                        x = -radius * Math.Sin(toRad(i));
                        y = -radius * Math.Cos(toRad(i));
                    }
                    else
                    {
                        x = -radius * Math.Cos(toRad(i - 90));
                        y = radius * Math.Sin(toRad(i - 90));
                    }
                }
                else
                {
                    if (i <= 270)
                    {
                        x = radius * Math.Sin(toRad(-i));
                        y = - radius * Math.Cos(toRad(-i));
                    }
                    else
                    {
                        x = - radius * Math.Cos(toRad(90 - i));
                        y = - radius * Math.Sin(toRad(90 - i));
                    }
                }
            return new Vector2((float)x, (float)y);
        }

        private double toRad(float degree)
        {
            return degree * Math.PI / 180;
        }

        public bool isOnTrack(Vector2 vec, TrackPoint tp = null)
        {
            return getTrackPosition(vec, tp) != null;
        }

        public TrackPoint getTrackPosition(Vector2 vec, TrackPoint tp = null)
        {
            if (tp == null) { tp = start; }

            TrackPoint startPoint = tp;

            do
            {
                if (tp.contains(vec))
                {
                    return tp;
                }
                tp = tp.next;
            }
            while (tp != startPoint);
            return null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            start.Draw(spriteBatch, start);
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
