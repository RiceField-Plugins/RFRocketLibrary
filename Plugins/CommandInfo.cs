using System;

namespace RFRocketLibrary.Plugins
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public sealed class CommandInfo : Attribute
    {
        public string? Syntax { get; private set; }
        public string? Help { get; private set; }

        public CommandInfo(string? Help = "", string? Syntax = "")
        {
            this.Syntax = Syntax;
            this.Help = Help;
        }
    }
}
