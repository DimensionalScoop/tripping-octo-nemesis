using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;

using X45Game.Input;
using X45Game.Extensions;

namespace TrippingOctoNemesis.Communicator
{
    public class TextInterface : DrawableGameComponent
    {
        List<Speech> Text = new List<Speech>();

        TimeSpan LastTextRefresh;
        Font font = new Font("font");
        SpriteBatch spriteBatch;
        Vector2 screenSize;
        Vector2 textPosition
        {
            get
            {
                if (Text.Count == 0) return Vector2.Zero;
                else 
                    return new Vector2(Text[0].Speaker.Sprite.Texture.Width + 50, screenSize.Y - Text[0].Speaker.Sprite.Texture.Height);
            }
        }

        static readonly TimeSpan CharacterSlideSpeed = TimeSpan.FromSeconds(0.25f);
        static readonly TimeSpan DelayPerChar = TimeSpan.FromSeconds(1f / (200 * 6 / 60));//200 words per min. average reading speed; 6 chars per word
        static readonly TimeSpan DelayPerText = TimeSpan.FromSeconds(1f);

        public event Action ClearedTextBuffer;


        public TextInterface(Game game,Vector2 screenSize) : base(game) 
        {
            Game.Components.Add(this);
            this.screenSize = screenSize;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            base.LoadContent();
        }

        public void AddSpeech(Speech speech)
        {
            string newText = speech.Speaker.Name+"\n";
            string currentLine="";
            foreach (string word in speech.Text.Split(' '))
            {
                if (font.SpriteFont.MeasureString(currentLine + word + " ").X > screenSize.X - textPosition.X)
                {
                    newText += currentLine + "\n";
                    currentLine = word+" ";
                }
                else
                {
                    currentLine += word + " ";
                }
            }
            newText += currentLine;

            speech.Text = newText;
            Text.Add(speech);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Text.Count == 0) return;
            if (LastTextRefresh == TimeSpan.Zero) LastTextRefresh = gameTime.TotalGameTime;

            var speaker=Text[0].Speaker;
            var text=Text[0].Text;

            spriteBatch.Begin();

            var characterPos=new Vector2(
                MathHelper.Lerp(-speaker.Sprite.Texture.Width,0,MathHelper.Clamp((float)((gameTime.TotalGameTime-LastTextRefresh).TotalSeconds/CharacterSlideSpeed.TotalSeconds),0,1)),
                screenSize.Y-speaker.Sprite.Texture.Height
                );
            int textLength = (int)((gameTime.TotalGameTime - LastTextRefresh - CharacterSlideSpeed).TotalSeconds / DelayPerChar.TotalSeconds);

            spriteBatch.Draw(speaker.Sprite, characterPos, Color.White);
            if (gameTime.TotalGameTime - LastTextRefresh > CharacterSlideSpeed)
                spriteBatch.DrawText(text.Substring(0,Math.Min(textLength,text.Length)), textPosition, false, font, Color.WhiteSmoke);

            spriteBatch.End();

            if (gameTime.TotalGameTime - LastTextRefresh - CharacterSlideSpeed - TimeSpan.FromSeconds(DelayPerChar.TotalSeconds * Text[0].Text.Length) - DelayPerText > TimeSpan.Zero)
            {
                Text.RemoveAt(0);
                LastTextRefresh = gameTime.TotalGameTime;
            }
            
            base.Draw(gameTime);
        }
    }
}
