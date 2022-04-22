using System;

namespace RocketExtensions.Plugins
{
    public sealed class CommandName : Attribute
    {
        public string? Name { get; }

        public CommandName(string? name)
        {
            Name = name;
        }

    }
}
