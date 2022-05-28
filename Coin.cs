using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PlatformerGame
{
    public class Coin
    {
        public Rectangle rectangle;
        private Animation anim;

        public Coin(Texture2D sprite, Vector2 position)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, 16, 16);
            anim = new Animation(sprite, 16, 16);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            anim.Draw(spriteBatch, new Vector2(rectangle.X, rectangle.Y), gameTime);
        }
    }
}
