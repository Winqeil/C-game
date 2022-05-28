using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PlatformerGame
{
    public abstract class Creature
    {
        public enum currentAnimation
        {
            Idle,
            Run,
            Jumping,
            Falling
        }

        public Vector2 position;
        public Rectangle hitbox;

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
