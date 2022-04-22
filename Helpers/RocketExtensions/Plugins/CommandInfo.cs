using System;

namespace RocketExtensions.Plugins
{
    public sealed class CommandInfo : Attribute
    {
        public string? Syntax { get; }
        public string? Help { get; }

        public CommandInfo(string? help = "", string? syntax = "")
        {
            Syntax = syntax;
            Help = help;
        }
    }
}
