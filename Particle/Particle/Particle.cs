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
    class Particle
    {
        Vector2 loc;

        private float speed;

        private float lastTurn = 0;
        private float maxTurn = (float)(Math.PI / 3.0);

        public TrackPoint trackPoint;

        private static Random random = new Random();
        private float confidence;
	
	    public Particle(float x, float y, Track track, float speed = 3, float confidence = 0.5f)
	    {
            loc = new Vector2(x, y);
            this.speed = speed;
            this.confidence = confidence;
            trackPoint = track.getTrackPosition(loc);
            if (trackPoint != null)
            {
                lastTurn = (float)(Math.PI / 2.0) + trackPoint.getOrientation();
            }
	    }

        public Vector2 getLocation()
        {
            return loc;
        }

        public float getSpeed()
        {
            return speed;
        }

        public void Move(Track track, List<Particle> list)
        {
            Vector2 nextLoc = trackPoint.next.loc;
            while(track.isOnTrack(nextLoc))
            {
                Vector2 col = findCollisionPoint(nextLoc);
                if (causesCollisions(list, col))
                {
                    float size = nextLoc.Length();
                    double angle = (Math.PI / 2.0) - trackPoint.next.getOrientation();
                    float x = (float)((size + speed) * Math.Sin(angle));
                    float y = (float)((size + speed) * Math.Cos(angle));
                    nextLoc.X = x;
                    nextLoc.Y = y;
                }
                else
                {
                    loc = col;
                    return;
                }
            }
        }

        public void Update(Track track)
        {
            trackPoint = track.getTrackPosition(loc, trackPoint);
        }

        private bool causesCollisions(List<Particle> list, Vector2 loc)
        {
            foreach (Particle p in list)
            {
                if (p == this)
                {
                    continue;
                }
                if (Math.Abs(p.loc.X - loc.X) < p.getSpeed() && Math.Abs(p.loc.Y - this.loc.Y) < p.getSpeed())
                {
                    if (random.NextDouble() > confidence)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        private Vector2 findCollisionPoint(Vector2 linePoint1)
        {
            return (Vector2.Normalize(linePoint1 - loc) * speed) + loc;
        }

        private void UpdateOrientation(Vector2 col)
        {
            lastTurn = (float)Math.Acos(Vector2.Dot(Vector2.Normalize(col), Vector2.Normalize(loc)));
        }

	    public void Move(float xMov, float yMov)
	    {
		    loc += new Vector2(xMov, yMov);
	    }

        public void Draw(SpriteBatch spriteBatch)
	    {
		    spriteBatch.Draw(Game1.background, loc, Color.IndianRed);
	    }

        public bool Collision(Particle p)
        {
            return p.getLocation().Equals(getLocation());
        }

    }
}
