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
	
	    private int speed;

        private float lastTurn = 0;
        private float maxTurn = (float)(Math.PI / 3.0);

        public TrackPoint trackPoint;
	
	    public Particle(float x, float y, Track track, int speed = 3)
	    {
            loc = new Vector2(x, y);
            this.speed = speed;
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

        public void Move(Track track, List<Particle> list)
        {
            Vector2 nextLoc = trackPoint.next.loc;
            while(track.isOnTrack(nextLoc))
            {
                Vector2 col = findCollisionPoint(nextLoc);
                if (isLocFull(list, col))
                {
                    nextLoc.Y++;
                    //why??
                }
                else
                {
                    UpdateOrientation(col);
                    loc = col;
                    break;
                }
            }
        }

        public void Update(Track track)
        {
            trackPoint = track.getTrackPosition(loc, trackPoint);
        }

        private bool isLocFull(List<Particle> list, Vector2 loc)
        {
            foreach (Particle p in list)
            {
                if (p.loc == loc && p != this)
                {
                    return true;
                }
            }
            return false;
        }

        private Vector2 findCollisionPoint(Vector2 linePoint1)
        {
            double length = (linePoint1 - loc).Length();
            double theta = Math.Acos((linePoint1.Y - loc.Y) / length);
            int x, y;
            if (linePoint1.X > loc.X)
            {
                if (linePoint1.Y > loc.Y)
                {
                    /*if (theta + maxTurn > lastTurn)
                    {
                        theta = maxTurn;
                    }*/
                    x = (int)(speed * Math.Cos(theta));
                    y = (int)(speed * Math.Sin(theta));
                }
                else //if (linePoint1.Y < loc.Y)
                {
                    /*if (theta - maxTurn > lastTurn)
                    {
                        theta = maxTurn + lastTurn + (Math.PI / 2.0);
                    }*/
                    x = (int)(speed * Math.Cos(theta));
                    y = -(int)(speed * Math.Sin(theta));
                }
            }
            else
            {
                theta = -theta;
                if (linePoint1.Y > loc.Y)
                {
                    /*if (theta + maxTurn > lastTurn)
                    {
                        theta = maxTurn;
                    }*/
                    x = -(int)(speed * Math.Cos(theta));
                    y = (int)(speed * Math.Sin(theta));
                }
                else //if (linePoint1.Y < loc.Y)
                {
                    /*if (theta - maxTurn > lastTurn)
                    {
                        theta = maxTurn + lastTurn + (Math.PI / 2.0);
                    }*/
                    x = -(int)(speed * Math.Cos(theta));
                    y = -(int)(speed * Math.Sin(theta));
                }
            }
            return new Vector2(x, y) + loc;
        }

        private void UpdateOrientation(Vector2 col)
        {
            lastTurn = (float)Math.Acos(Vector2.Dot(Vector2.Normalize(col), Vector2.Normalize(loc)));
        }

        private bool causesCollisions(List<Particle> list, Vector2 newLoc)
        {
            foreach (Particle p in list)
            {
                if (p == this) { continue; }
                if (p.getLocation() == newLoc)
                {
                    return true;
                }
            }
            return false;
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
