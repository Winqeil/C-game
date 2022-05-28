using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PlatformerGame
{
    public class GameController
    {

        public static bool paused;

        public GameController()
        {
            paused = false;
        }

        public bool IsGameEnded(Rectangle playerHitbox)
        {
            return false;
        }
    }
}
