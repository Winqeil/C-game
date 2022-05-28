using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PlatformerGame
{
    public class Player : Creature
    {

        public Vector2 velocity;
        public float playerSpeed;
        public Animation[] playerAnimation;
        public currentAnimation animationController;
        public SpriteEffects effects;
        public float fallSpeed;
        public float jumpSpeed;
        public float startY;
        public bool IsFalling;
        public bool IsJumping;
        public bool IsShooting;
        public Rectangle playerFallRect;

        public Player(Vector2 position, Texture2D idleSprite, Texture2D runSprite, Texture2D jumpingSprite)
        {
            velocity = new Vector2();
            this.position = position;
            playerSpeed = 2.0f;
            fallSpeed = 3.0f;
            jumpSpeed = -12.0f;
            IsFalling = true;
            playerAnimation = new Animation[3];
            playerAnimation[0] = new Animation(idleSprite);
            playerAnimation[1] = new Animation(runSprite);
            playerAnimation[2] = new Animation(jumpingSprite);
            hitbox = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            playerFallRect = new Rectangle((int)position.X, (int)position.Y + 32, 32, (int)fallSpeed);
        }
        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();

            IsShooting = keyboard.IsKeyDown(Keys.Enter);

            animationController = currentAnimation.Idle;
            position = velocity;

            if (IsFalling)
            {
                velocity.Y += fallSpeed;
                animationController = currentAnimation.Falling;
            }

            startY = position.Y;
            Move(keyboard);
            Jump(keyboard);

            position = velocity;
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            playerFallRect.X = (int)position.X;
            playerFallRect.Y = (int)velocity.Y + 32; //прибавляем 32 для того, чтобы персонаж не проваливался сквозь тайлы
            
        }

        public void RunAnimation()
        {
            animationController = currentAnimation.Run;
        }

        private void Move(KeyboardState keyboard)
        {

            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= playerSpeed;
                RunAnimation();
                effects = SpriteEffects.FlipHorizontally;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += playerSpeed;
                RunAnimation();
                effects = SpriteEffects.None;
            }
        }

        private void Jump(KeyboardState keyboard)
        {
            if (IsJumping)
            {
                velocity.Y += jumpSpeed;
                jumpSpeed += 1;
                Move(keyboard);
                animationController = currentAnimation.Jumping;

                if (velocity.Y >= startY)
                {
                    velocity.Y = startY;
                    IsJumping = false;
                }
            }

            else
            {
                if (keyboard.IsKeyDown(Keys.W) && !IsFalling)
                {
                    IsJumping = true;
                    IsFalling = false;
                    jumpSpeed = -12.0f;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (animationController)
            {
                case currentAnimation.Idle:
                    playerAnimation[0].Draw(spriteBatch, position, gameTime, 500, effects);
                    break;
                case currentAnimation.Run:
                    playerAnimation[1].Draw(spriteBatch, position, gameTime, 100, effects);
                    break;
                case currentAnimation.Jumping:
                    playerAnimation[2].Draw(spriteBatch, position, gameTime, 500, effects);
                    break;
                case currentAnimation.Falling:
                    playerAnimation[2].Draw(spriteBatch, position, gameTime, 500, effects);
                    break;
            }
        }
    }
}
