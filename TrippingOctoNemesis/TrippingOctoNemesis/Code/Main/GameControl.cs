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
using TrippingOctoNemesis.Extensions;
using TrippingOctoNemesis.Communicator;
using System.IO;

namespace TrippingOctoNemesis
{
    public class GameControl:DrawableGameComponent
    {
        #region Fields
        Map CurrentMap;
        public static List<SpaceShip> Ships = new List<SpaceShip>();
        public static List<Fraction> Fractions = new List<Fraction>();
        public static Player[] Player = new Player[2];
        public static Fraction[] Enemys = new Fraction[5];

        SpriteBatch spriteBatch;
        Random random = new Random();
        public static GameTime LastUpdate = new GameTime();

        public static Hud Hud;
        TextInterface text;
        ExtensionsManager extensions;
        InputProvider input;
        ParticleProvider particles;
        public static StarField Stars;

        DirectoryInfo path = new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

        

        static readonly float minCollisionRadius = (float)Math.Pow(10, 2);
        #endregion


        public GameControl(Game game)
            : base(game)
        {
            input = new InputProvider(game);
            particles = new ParticleProvider();
            Stars = new StarField(game, 2, 1, 0.5f);

            Hud = new Hud(game);
            text = new TextInterface(game, new Vector2(1360, 730));

            extensions = new ExtensionsManager();
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            InitExtensions(Game);
            InitFractions();
            InitDebug();

            base.LoadContent();
        }

        private void InitDebug()
        {
            #if DEBUG
            
            #endif

            extensions.Maps.Add(new TestMap());
            SelectMap(extensions.Maps.First().Id);
        }
        
        private void InitExtensions(Game game)
        {
            foreach (var dir in new DirectoryInfo(path.FullName + "\\Extensions").GetDirectories())
                extensions.AddDirectory(dir.FullName);
            extensions.Load(game);
        }

        private void InitFractions()
        {
            GameTime gameTime = new GameTime();

            CreatePlayer(0, ControlKeySettings.DefaultPlayerOne(), gameTime);
            CreatePlayer(1, ControlKeySettings.DefaultPlayerTwo(), gameTime);
            Player[0].Allys |= Player[1].Id;
            Player[1].Allys |= Player[0].Id;
            
            for(int i=0;i<=4;i++)
            CreateEnemy(i);

            Fractions.AddRange(Player);
            Fractions.AddRange(Enemys);
        }
        private void CreateEnemy(int p)
        {
            Enemys[p] = new Fraction();
        }
        private void CreatePlayer(int p, ControlKeySettings keys, GameTime gameTime)
        {
            Player[p] = new Player() { Keys = keys };

            var motherShip = new Carrier(Hud, Player[p]) { Position = new Vector2(300 + 300 * p, 500), Ki = new SpaceShip.NoScreenMovement() };
            Player[p].AssignCarrier(motherShip);
            Ships.Add(motherShip);

            for (int i = 0; i < 4; i++)
            {
                var ship = new SpaceShip(Player[p]) { Carrier = motherShip };
                Player[p].AddShipToCarrier(ship);
                Ships.Add(ship);
                motherShip.Slots[i] = new DeploySlots(ship);
            }
        }


        public override void Update(GameTime gameTime)
        {
            LastUpdate = gameTime;

            FrameUpdate(gameTime);
            LongUpdate(gameTime);

            base.Update(gameTime);
        }

        private void FrameUpdate(GameTime gameTime)
        {
            UpdateMap(gameTime);
            UpdateShips(gameTime);
            Fractions.ForEach(p => p.Update(gameTime, Hud));

            Stars.MoveCamera(new Vector2(0, 1));
            particles.Update(gameTime);
            HandleInput(gameTime);

            LongUpdate(gameTime);
        }

        private void UpdateMap(GameTime gameTime)
        {
            if (CurrentMap.DeleteFlag)
            {
                if (CurrentMap.NextMap != Guid.Empty)
                    SelectMap(CurrentMap.NextMap);
                else
                    Finish();
            }

            CurrentMap.Update(gameTime);
        }

        private void SelectMap(Guid id)
        {
            CurrentMap = extensions.Maps.Find(p => p.Id == id);
            CurrentMap.Reset();
            CurrentMap.Start(LastUpdate);
        }

        private void Finish()
        {
            //TODO: score screen after Finish()
            SelectMap(CurrentMap.Id);
        }

        #region MethodFields
        const int worldUpdatesPerSecond = 10;
        TimeSpan lastLongUpdateDuration;
        TimeSpan lastFirstLongUpdate;
        int updaterPosition = 0;
        #endregion
        private void LongUpdate(GameTime gameTime)
        {
            int possibleUpdates = (int)(gameTime.ElapsedGameTime.TotalSeconds * worldUpdatesPerSecond * Ships.Count);

            while (possibleUpdates > 0)
            {
                possibleUpdates--;
                updaterPosition++;
                if (updaterPosition >= Ships.Count)
                {
                    updaterPosition = 0;
                    lastLongUpdateDuration = gameTime.TotalGameTime - lastFirstLongUpdate;
                    lastFirstLongUpdate = gameTime.TotalGameTime;
                }

                Ships[updaterPosition].LongUpdate(lastLongUpdateDuration);
            }
        }

        private void UpdateShips(GameTime gameTime)
        {
            Ships.ForEach(p => p.Update(gameTime));
            if (SpaceShip.DeleteableShips) Ships.RemoveAll(p => p.DeleteFlag);
            UpdateShipEvasion();
        }

        private void UpdateShipEvasion()
        {
            //XXX: Possible performance issue
            List<SpaceShip> exclude = new List<SpaceShip>();
            for (int i = 0; i < Ships.Count; i++)
            {
                if (exclude.Contains(Ships[i]) || Ships[i] as Carrier != null) continue;
                for (int j = i + 1; j < Ships.Count; j++)
                    if (Vector2.DistanceSquared(Ships[i].Position, Ships[j].Position) < minCollisionRadius)
                    {
                        Ships[i].ActivateEvasion = true;
                        exclude.Add(Ships[j]);
                    }
            }
        }

        private void HandleInput(GameTime gameTime)
        {
            if (input.Key.KeysStroked.Contains(Keys.Escape))
                Game.Exit();

            #if DEBUG
            if (input.Mouse.Clicks.Contains(MouseButtons.Right))
                Ships.FindAll(p => (p.Position + Hud.Camera - input.Mouse.Position).Length() < 30).ForEach(p => p.Delete(SpaceShip.DeleteReasons.Destroyed));
            #endif
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);


            Fractions.ForEach(p => p.Draw(spriteBatch, Hud, gameTime));
            Ships.ForEach(p => p.Draw(spriteBatch, Hud, gameTime));
            
            particles.Draw(spriteBatch, Hud.Camera, gameTime);
            
            CurrentMap.DrawMapUI(spriteBatch, new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), true,gameTime);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
