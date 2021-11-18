using System;

namespace RFRocketLibrary.Plugins
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public sealed class CommandName : Attribute
    {
        public string? Name { get; set; }

        public CommandName(string? name)
        {
            Name = name;
        }

    }
}
