using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketExtensions.Plugins
{
    public sealed class CommandAliases : Attribute
    {
        public List<string>? Aliases { get; }

        public CommandAliases(params string[] aliases)
        {
            Aliases = aliases.ToList();
        }
    }
}