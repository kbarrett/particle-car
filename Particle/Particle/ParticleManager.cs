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
    class ParticleManager
    {
        List<Particle> particles;
        Track track;

        public ParticleManager(Track track)
	    {
		    particles = new List<Particle>();
            this.track = track;
		    resetParticles();
	    }

        private void resetParticles()
        {
            foreach(TrackPoint tp in track)
            {
                particles.Add(new Particle(tp.loc.X, tp.loc.Y, track));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
	    {
		    foreach(Particle p in particles)
		    {
                p.Draw(spriteBatch);
		    }
	    }

        public void Update(Track track)
        {
            CheckParticlePosition(track);
		    foreach(Particle p in particles)
		    {
			    p.Move(track, particles);
		    }
            foreach (Particle p in particles)
            {
                p.Update(track);
            }
	    }

        private void CheckParticlePosition(Track track)
        {
            List<Particle> throwList = new List<Particle>();
            
            foreach(Particle p in particles)
            {
                if(p.trackPoint == null)
                {
                    throwList.Add(p);
                    continue;
                }
                foreach(Particle q in particles)
                {
                    if (p == q) { continue; }
                    if (p.Collision(q))
                    {
                        throwList.Add(p);
                        throwList.Add(q);
                    }
                }
            }

            foreach (Particle p in throwList)
            {
                particles.Remove(p);
            }
        }
    }
}
