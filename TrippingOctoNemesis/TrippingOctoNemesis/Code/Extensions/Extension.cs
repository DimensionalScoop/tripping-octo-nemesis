using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TrippingOctoNemesis.Extensions
{
    [Flags]
    public enum ExtensionTypes
    {
        None=0,
        Map=1<<0,
        Enemy=1<<1,
        Weapon=1<<2,
        Group=1<<3,
        Script=1<<4,
        Plugin=1<<5
    }

    public class ExtensionDescription
    {
        public string Name;
        public string Description;
        public ExtensionTypes Type;
        public string Author;
        public string[] Dependencies;
    }

    public class Extension
    {
        public ExtensionDescription Info;

        public DirectoryInfo Directory;
        public FileInfo DescriptionFile;

        public List<Type> Maps;
        public List<Type> Encounters;
        public List<Type> Groups;
        public List<Type> Enemies;

        public List<Type> Plugins;
        public List<Type> Scripts;


        public Extension(DirectoryInfo dir)
        {
            Directory = dir;
        }
    }
}
