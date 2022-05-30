using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TiledSharp;
using FontStashSharp;
using System.IO;

namespace PlatformerGame
{
    public class MainGame : Game
    {
        public static float screenHeight;
        public static float screenWidth;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FontSystem _fontSystem;
        private TmxMap map;
        private TilemapController tilemapController;
        private Texture2D tileset;
        private List<Rectangle> collisionRct;
        private Rectangle startRct;
        private Player player;
        private Enemy enemy;
        private List<Enemy> enemiesUnkillable;
        private List<Enemy> enemiesKillable;
        private List<Enemy> enemiesTrigger;
        private List<Rectangle> enemyPath;
        private List<Rectangle> triggersRct;
        private List<Coin> coins;
        private List<Player> players;
        private List<Shooting> bullets;
        private Texture2D bulletSprite;
        public int pointsCounter;
        private int bulletDelay;
        private bool IsGameEnded;
        private bool IsTriggered;
        

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsGameEnded = false;
            IsTriggered = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 470;
            _graphics.PreferredBackBufferWidth = 1020;
            _graphics.ApplyChanges();
            screenHeight = _graphics.PreferredBackBufferHeight;
            screenWidth = _graphics.PreferredBackBufferWidth;
            base.Initialize();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _fontSystem = new FontSystem();
            _fontSystem.AddFont(File.ReadAllBytes("Content\\Samson.ttf"));

            map = new TmxMap("Content\\Level1.tmx");
            tileset = Content.Load<Texture2D>("Cave Tileset\\Cave Tileset\\" + map.Tilesets[0].Name.ToString());
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;
            int tilesetTileWidth = tileset.Width / tileWidth;
            tilemapController = new TilemapController(map, tileset, tilesetTileWidth, tileWidth, tileHeight);

            coins = new List<Coin>();
            var coinSprite = Content.Load<Texture2D>("Mini FX, Items & UI\\Mini FX, Items & UI\\Common Pick-ups\\Coin (16 x 16)");
            foreach (var obj in map.ObjectGroups["Coins"].Objects)
            {
                var coin = new Coin(coinSprite, new Vector2((float)obj.X, (float)obj.Y));
                coins.Add(coin);
            }

            collisionRct = new List<Rectangle>();
            foreach (var obj in map.ObjectGroups["Collisions"].Objects)
            {
                if (obj.Name == "")
                {
                    collisionRct.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
                }
            }

            enemyPath = new List<Rectangle>();
            foreach (var obj in map.ObjectGroups["EnemyPath"].Objects)
            {
                enemyPath.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
            }

            triggersRct = new List<Rectangle>();
            foreach (var obj in map.ObjectGroups["Triggers"].Objects)
            {
                triggersRct.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
            }

            enemiesUnkillable = new List<Enemy>(); // враги, которых убить нельзя
            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\9 - Mr._Circuit_Running (32 x 32)"),
                enemyPath[2],
                5.0f
                );
            enemiesUnkillable.Add(enemy);

            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\2 - Martian_Red_Running (32 x 32)"),
                enemyPath[0],
                6.0f
                );
            enemiesUnkillable.Add(enemy);



            enemiesKillable = new List<Enemy>(); // cмертные враги за которые дают очки, в нашем случае - шарики
            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\4 - Ballooney_Flying (32 x 32)"),
                enemyPath[3],
                1.0f
                );
            enemiesKillable.Add(enemy);

            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\4 - Ballooney_Flying (32 x 32)"),
                enemyPath[4],
                1.0f
                );
            enemiesKillable.Add(enemy);

            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\4 - Ballooney_Flying (32 x 32)"),
                enemyPath[5],
                1.0f
                );
            enemiesKillable.Add(enemy);

            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\4 - Ballooney_Flying (32 x 32)"),
                enemyPath[6],
                1.0f
                );
            enemiesKillable.Add(enemy);

            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\4 - Ballooney_Flying (32 x 32)"),
                enemyPath[7],
                1.0f
                );
            enemiesKillable.Add(enemy);

            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\4 - Ballooney_Flying (32 x 32)"),
                enemyPath[8],
                1.0f
                );
            enemiesKillable.Add(enemy);

            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\4 - Ballooney_Flying (32 x 32)"),
                enemyPath[9],
                1.0f
                );
            enemiesKillable.Add(enemy);


            enemiesTrigger = new List<Enemy>(); // враги, который появляются при триггере
            enemy = new Enemy(
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\8 - Roach_Running (32 x 32)"),
                enemyPath[1],
                1.0f
                );
            enemiesTrigger.Add(enemy);

            players = new List<Player>();
            player = new Player(
                new Vector2(startRct.X, startRct.Y),
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\1 - Agent_Mike_Idle (32 x 32)"),
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\1 - Agent_Mike_Running (32 x 32)"),
                Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\1 - Agent_Mike_Jump_&_Falling (32 x 32)")
            );
            players.Add(player);

            bullets = new List<Shooting>();
            bulletSprite = Content.Load<Texture2D>("Sprite Pack 4\\Sprite Pack 4\\1 - Agent_Mike_Bullet (16 x 16)");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // сбор монет

            foreach (var coin in coins.ToArray())
            {
                if (coin.rectangle.Intersects(player.hitbox))
                {
                    pointsCounter++;
                    coins.Remove(coin);
                    break;
                }
            }

            // смерть игрока при пересечении с врагом

            foreach (var enemy in enemiesUnkillable)
            {
                enemy.Update();
                KillPlayer(enemy);
                if (enemy.IsHitPlayer(player.hitbox))
                    IsGameEnded = true;
            }

            foreach (var enemy in enemiesTrigger)
            {
                enemy.Update();
                KillPlayer(enemy);
                if(enemy.IsHitPlayer(player.hitbox))
                    IsGameEnded = true;
            }



            var initialPos = player.position;
            player.Update();

            //цикл для проверки коллизии по Y

            foreach (var rect in collisionRct)
            {
                if(!player.IsJumping)
                    player.IsFalling = true;
                if (rect.Intersects(player.playerFallRect))
                {
                    player.IsFalling = false;
                    break;
                }
            }

            //цикл для проверки коллизии по X

            foreach (var rect in collisionRct)
            {
                if (rect.Intersects(player.hitbox))
                {
                    player.position = initialPos;
                    player.velocity = initialPos;
                    break;
                }
            }

            foreach (var rect in triggersRct) // проверка на триггер
            {
                if (rect.Intersects(player.hitbox))
                {
                    IsTriggered = true;
                    break;
                }
            }

            if (player.IsShooting)
            {

                if (bulletDelay > 20) //время между выстрелами
                {

                    var bulletHitbox = new Rectangle((int)player.position.X + 7, (int)player.position.Y + 16,
                    bulletSprite.Width, bulletSprite.Height);

                    if (player.effects == SpriteEffects.None) // стрельба вправо
                    {

                        bullets.Add(new Shooting(bulletSprite, 4, bulletHitbox));
                    }

                    if (player.effects == SpriteEffects.FlipHorizontally) // стрельба влево
                    {

                        bullets.Add(new Shooting(bulletSprite, -4, bulletHitbox));
                    }

                    bulletDelay = 0;
                }

                else

                {
                    bulletDelay++;
                }
            }

            foreach (var bullet in bullets.ToArray())
            {
                bullet.Update();

                foreach (var rectangle in collisionRct)
                {
                    if (rectangle.Intersects(bullet.hitbox))
                    {
                        bullets.Remove(bullet);
                        break;
                    }
                }

                foreach (var enemy in enemiesKillable.ToArray())
                {
                    if (bullet.hitbox.Intersects(enemy.hitbox))
                    {
                        bullets.Remove(bullet);
                        enemiesKillable.Remove(enemy);
                        pointsCounter++;
                        break;
                    }
                }
            }

            base.Update(gameTime);
        }

        private void KillPlayer(Enemy enemy)
        {
            foreach (var player in players.ToArray())
            {
                if (enemy.IsHitPlayer(player.hitbox))
                {
                    players.Remove(player);
                    IsGameEnded = true;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            tilemapController.Draw(_spriteBatch);
            foreach (var coin in coins)
                coin.Draw(_spriteBatch, gameTime);
            foreach (var enemy in enemiesUnkillable)
                enemy.Draw(_spriteBatch, gameTime);
            foreach (var enemy in enemiesKillable)
                enemy.Draw(_spriteBatch, gameTime);
            foreach (var enemy in enemiesTrigger)
                if (IsTriggered)
                    enemy.Draw(_spriteBatch, gameTime);
            foreach (var player in players)
                player.Draw(_spriteBatch, gameTime);
            foreach (var bullet in bullets)
                bullet.Draw(_spriteBatch);
            SpriteFontBase font20 = _fontSystem.GetFont(20);
            SpriteFontBase font40 = _fontSystem.GetFont(40);
            _spriteBatch.DrawString(font20, "A,D,W - move", new Vector2(53, 175), Color.White);
            _spriteBatch.DrawString(font20, "Enter - fire", new Vector2(53, 195), Color.White);
            _spriteBatch.DrawString(font20, "collect gold", new Vector2(53, 215), Color.White);
            _spriteBatch.DrawString(font20, "Destroy ballons", new Vector2(53, 235), Color.White);
            if (IsGameEnded)
            {
                _spriteBatch.DrawString(font40, "You Lose!", new Vector2(450, 193), Color.Red);
                _spriteBatch.DrawString(font40, "Please restart the game", new Vector2(320, 233), Color.Red);
            }

            if(pointsCounter == 27)
                _spriteBatch.DrawString(font40, "You Win, congratulations!", new Vector2(320, 233), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
