using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrippingOctoNemesis.Extensions
{
    [Flags]
    public enum ExtensionTypes
    {
        None=0,
        Map=1<<0,
        SpaceShips=1<<1,
        Weapon=1<<2,
        Modification=1<<3,
        Other=1<<4,
        Interface=1<<5,
    }

    public class Extension
    {
        public string Name;
        public ExtensionTypes Type;
        public string Description;
        public string Author;
        public string[] Dependencies;
        public string[] Scripts;

        public List<Type> ImportedTypes = new List<Type>();
    }
}
