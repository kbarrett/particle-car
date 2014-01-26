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
            int trackId = 0;
            int speed = 1;
            foreach (TrackPoint tp in track)
            {
                if (trackId % 2 == 0)
                {
                    ++speed;
                }
                int number = (trackId % 4 == 0) ? 0 : 1;
                for (int i = 0; i < number; ++i)
                {
                    particles.Add(new Particle(tp, speed));
                }
                ++trackId;
                if (trackId > 90)
                {
                    return;
                }
            }
        }

        public void Update(Track track)
        {
            foreach (Particle p in particles)
            {
                p.Move(track);
            }
        }
    }
}
