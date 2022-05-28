using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame
{
    public class Shooting
    {
        private Texture2D bulletSprite;
        private float speedshot;
        public Rectangle hitbox;

        public Shooting(Texture2D bulletSprite, float speedshot, Rectangle hitbox)
        {
            this.bulletSprite = bulletSprite;
            this.speedshot = speedshot;
            this.hitbox = hitbox;
        }

        public void Update()
        {
            hitbox.X += (int)speedshot;
        }

        public bool IsBulletHit(Rectangle rectangle)
        {
            return hitbox.Intersects(rectangle);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletSprite, hitbox, Color.White);
        }
    }
}
