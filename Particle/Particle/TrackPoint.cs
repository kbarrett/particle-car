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
    class TrackPoint
    {
        public List<Particle> occupying = new List<Particle>();
        public TrackPoint next;
        public Vector2 loc;
        private float orientation;
        private int maxThroughput;
        private int defaultThroughput = 3;

        public TrackPoint(Vector2 loc, float orientation, int maxThroughput = 3)
        {
            this.orientation = (float)(Math.PI * orientation / 180);
            this.loc = loc;
            this.maxThroughput = maxThroughput;
        }

        public float getOrientation()
        {
            return orientation;
        }
        public void setDefaultThroughput(int maxThroughput)
        {
            defaultThroughput = maxThroughput;
            this.maxThroughput = maxThroughput;
        }
        public void setThroughput(int throughput)
        {
            this.maxThroughput = throughput;
        }
        public void resetThroughput()
        {
            this.maxThroughput = defaultThroughput;
        }

        public bool addParticle(Particle p)
        {
            if (occupying.Count == 0)
            {
                occupying.Add(p);
                return true;
            }
            if (occupying.Count < maxThroughput && p.speed >= occupying[occupying.Count - 1].speed)
            {
                occupying.Add(p);
                return true;
            }
            return false;
        }
        public void removeParticle(Particle p)
        {
            occupying.Remove(p);
        }

        private Vector2 rotate(Vector2 point, int clock = -1)
        {
            point -= loc;
            point = Vector2.Transform(point, Matrix.CreateRotationZ(clock * orientation));
            point += loc;
            return point;
        }

        private bool drawn = false;
        public void Draw(SpriteBatch spriteBatch, TrackPoint tp)
        {
            if (tp == this)
            {
                if (!drawn)
                {
                    drawn = true;
                }
                else
                {
                    drawn = false;
                    return;
                }
            }
            next.Draw(spriteBatch, tp);

            foreach (Particle p in occupying)
            {
                Vector2 shift = new Vector2(0, 1);
                shift.Normalize();
                shift *= -10 * occupying.IndexOf(p);
                Color colour = maxThroughput < 3 ? Color.Red : Color.IndianRed;
                p.Draw(spriteBatch, rotate(loc + shift, 1), colour);
            }

        }
    }
}
