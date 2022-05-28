using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PlatformerGame
{
    public class Animation
    {

        Texture2D spritesheet;
        int frames;
        int rows;
        int count;
        float timeSinceLastFrame;
        float width;
        float height;

        public Animation(Texture2D spritesheet, float width = 32, float height = 32)
        {
            this.spritesheet = spritesheet;
            frames = (int)(spritesheet.Width / width);
            rows = count = 0;
            timeSinceLastFrame = 0;
            this.width = width;
            this.height = height;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, float milisecPerFrames = 500, SpriteEffects effect = SpriteEffects.None)
        {
            if (count < frames)
            {
                var srcRect = new Rectangle((int)width * count, rows, (int)width, (int)height);
                spriteBatch.Draw(spritesheet, position, srcRect, Color.White, 0f, new Vector2(), 1f, effect, 1);
                timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timeSinceLastFrame > milisecPerFrames)
                {
                    timeSinceLastFrame -= milisecPerFrames;
                    count++;
                    if (count == frames)
                        count = 0;
                }
            }
        }
    }
}
