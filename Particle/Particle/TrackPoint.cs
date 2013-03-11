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
        static int current = 0;
        int identifier;
        public TrackPoint next;
        public Vector2 loc;
        private float orientation;
        Rectangle rect;

        public TrackPoint(Vector2 loc, float orientation, int width, int height)
        {
            this.orientation = (float)(Math.PI * orientation / 180);
            this.loc = loc;
            rect = new Rectangle((int)(loc.X - width / 2.0), (int)(loc.Y - height / 2.0), width, height);

            identifier = current++;
        }

        public float getOrientation()
        {
            return orientation;
        }

        public bool contains(Vector2 p)
        {
            Vector2 rotatedP = rotate(p);
            return rect.Contains(new Point((int)rotatedP.X, (int)rotatedP.Y)) ;
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

            spriteBatch.Draw(Game1.background, rotate(new Vector2(rect.Left, rect.Bottom), 1), Color.Red);
            spriteBatch.Draw(Game1.background, rotate(new Vector2(rect.Right, rect.Bottom), 1), Color.Blue);
            spriteBatch.Draw(Game1.background, rotate(new Vector2(rect.Left, rect.Top), 1), Color.Yellow);
            spriteBatch.Draw(Game1.background, rotate(new Vector2(rect.Right, rect.Top), 1), Color.Green);
            spriteBatch.Draw(Game1.background, loc, Color.White);
            next.Draw(spriteBatch, tp);

        }
    }
}
