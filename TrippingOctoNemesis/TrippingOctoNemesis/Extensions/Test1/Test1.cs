using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;
using System.Diagnostics;


namespace TrippingOctoNemesis.Extensions.Test1
{
    public class Test1:Extension
    {
        public Test1()
        {
            Name = "Test 1";
            Type = ExtensionTypes.Interface;
            Author = "Elayn";
            Description = "Debug test for the extension manager.";
            Dependencies = new string[0];
            Scripts = new string[] { "DebugInterfaceOverlay.cs" };
        }
    }
}
