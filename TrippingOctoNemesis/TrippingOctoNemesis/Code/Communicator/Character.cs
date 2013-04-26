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
    public class Character
    {
        public static List<Character> Characters = new List<Character>();

        public Sprite Sprite;
        public string Name;
        public string ShortDescription;
        public string ExtendedDescription;
        public string Rank;

        #region Important characters

        public static readonly Character 
            PlayerOne,
            PlayerTwo,
            Mechanic,
            CiC,
            Merchant
            ;

        #endregion

        public Character() { }


        static Character()
        {
            PlayerOne=new Character()
            {
                Sprite=new Sprite("c\\player-1"),
                Name="Player One",
                Rank="Captain",
                ShortDescription="Age 23, likes the sun when it's night"//SCOTT
            };
            Characters.Add(PlayerOne);

            PlayerTwo=new Character()
            {
                Sprite=new Sprite("c\\player-2"),
                Name="Elayn",
                Rank="Skipper",
                ShortDescription="Age 21, sometimes pretends to be the necromancer queen of the poorly lit lands"//LADY ESMERELDA THE NECROMANCER QUEEN OF THE POORLY LIT LANDS
            };
            Characters.Add(PlayerTwo);

            Mechanic=new Character()
            {
                Sprite=new Sprite("c\\mechanic"),
                Name="Machinist",
                Rank="Head Engineer",
                ShortDescription="Age 30, \"I liked steam before it became popular\"",
            };
            Characters.Add(Mechanic);

            CiC = new Character()
            {
                Sprite = new Sprite("c\\cic"),
                Name = "Commander in Chief",
                Rank = "CiC",
                ShortDescription = "Age 48, was born ready"
            };
            Characters.Add(CiC);

            Merchant = new Character()
            {
                Sprite = new Sprite("c\\merchant"),
                Name = "Arcane Merchant",
                Rank = "Supreme Head of the Deep Space Guild",
                ShortDescription = "Age Unknown, \"No, I'm can't use magic\""
            };
            Characters.Add(Merchant);
        }
    }
}
