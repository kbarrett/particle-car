using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
            float speed = 2;
            for (int i = 0; i < 200; ++i)
            {
                particles.Add(new Particle(track.start.loc.X, 300 - i, track, speed));
                //particles.Add(new Particle(track.start.next.next.next.next.next.next.loc.X, 200 - i, track, speed));
                speed += 0.3f;
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

            foreach (Particle particle in particles)
            {
                particle.Move(track, particles);
            }
            foreach (Particle particle in particles)
            {
                particle.Update(track);
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
