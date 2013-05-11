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
    public class Test1Desc : ExtensionDescription
    {
        public Test1Desc()
        {
            Name = "Test 1";
            Type = ExtensionTypes.Plugin;
            Author = "Elayn";
            Description = "Debug test for the extension manager.";
            Dependencies = null;
        }
    }
}
