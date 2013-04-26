using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrippingOctoNemesis.Communicator
{
    public struct Speech
    {
        public string Text;
        public Character Speaker;

        public Speech(Character speaker, string text)
        {
            Speaker = speaker;
            Text = text;
        }
    }
}
