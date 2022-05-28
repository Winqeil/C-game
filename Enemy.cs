using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PlatformerGame
{
    public class Enemy : Creature
    {
        private Animation enemyAnimation;
        private float speed;
        private Rectangle path;

        public Enemy(Texture2D enemySpriteSheet, Rectangle path, float speed)
        {
            this.speed = speed;
            enemyAnimation = new Animation(enemySpriteSheet);
            this.path = path;
            position = new Vector2(path.X, path.Y);
            hitbox = new Rectangle(path.X, path.Y, 16, 16);
        }

        public override void Update()
        {
            //проверка на хитбокс внутри пути

            if (!path.Contains(hitbox))
            {
                speed = -speed;
            }

            position.X += speed;

            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
        }

        public bool IsHitPlayer(Rectangle playerRect)
        {
            return hitbox.Intersects(playerRect);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            enemyAnimation.Draw(spriteBatch, position, gameTime, 100);
        }
    }
}
