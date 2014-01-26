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
        public int maxSpeed;
        public int speed;
        private int turns = -1;
        private static int maxTurns = 5;

        public TrackPoint trackPoint;
	
	    public Particle(TrackPoint startingPoint, int speed = 1)
	    {
            this.maxSpeed = speed;
            this.speed = 1;
            startingPoint.addParticle(this);
            this.trackPoint = startingPoint;
            maxTurns = Math.Max(speed, maxTurns);
	    }

        public void Move(Track track)
        {
            if (turns == speed || turns == -1)
            {
                TrackPoint nextLoc = trackPoint.next;
                if (nextLoc.addParticle(this))
                {
                    trackPoint.removeParticle(this);
                    trackPoint = nextLoc;
                    if (speed < maxSpeed)
                    {
                        ++speed;
                    }
                }
                else
                {
                    speed = Math.Max(0, speed - 1);
                }
                turns = maxTurns;
            }
            else
            {
                --turns;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color)
	    {
            spriteBatch.Draw(Game1.background, location, color);
	    }

    }
}
