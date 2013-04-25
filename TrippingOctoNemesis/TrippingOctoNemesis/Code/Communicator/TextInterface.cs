using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;

namespace TrippingOctoNemesis.Communicator
{
    public class TextInterface : DrawableGameComponent
    {
        List<Speech> Text = new List<Speech>();

        TimeSpan LastTExtRefresh;
        Font font = new Font("");

        static readonly TimeSpan DelayPerChar = TimeSpan.FromSeconds(1f / (200 * 6 / 60));//200 words per min. average reading speed; 6 chars per word
        static readonly TimeSpan DelayPerText = TimeSpan.FromSeconds(0.5f);

        public event Action ClearedTextBuffer;


        public TextInterface(Game game) : base(game) { }

        public override void Draw(GameTime gameTime)
        {
            
            
            base.Draw(gameTime);
        }
    }
}
