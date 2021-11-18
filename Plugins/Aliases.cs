using System;
using System.Collections.Generic;
using System.Linq;

namespace RFRocketLibrary.Plugins
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public sealed class Aliases : Attribute
    {
        public List<string> AliasList { get; set; }

        public Aliases(params string[] aliases)
        {
            AliasList = aliases.ToList();
        }
    }
}