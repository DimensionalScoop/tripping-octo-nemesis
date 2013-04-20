﻿using System;
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

namespace TrippingOctoNemesis
{
    class ControlKeySettings
    {
        public Keys Up;
        public Keys Down;
        public Keys Left;
        public Keys Right;
        public Keys Fire;
        public Keys ActionOk;


        public static ControlKeySettings DefaultPlayerOne()
        {
            return new ControlKeySettings()
            {
                Up = Keys.W,
                Down = Keys.S,
                Left = Keys.A,
                Right = Keys.D,
                Fire = Keys.Space,
                ActionOk = Keys.LeftShift
            };
        }

        public static ControlKeySettings DefaultPlayerTwo()
        {
            return new ControlKeySettings()
            {
                Up = Keys.Up,
                Down = Keys.Down,
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.NumPad0,
                ActionOk = Keys.LeftControl
            };
        }
    }
}