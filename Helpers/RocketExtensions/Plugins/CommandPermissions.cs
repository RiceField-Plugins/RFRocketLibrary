using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketExtensions.Plugins
{
    public sealed class CommandPermissions : Attribute
    {
        public List<string> Permissions { get; private set; }

        public CommandPermissions(params string[] perms)
        {
            Permissions = perms.ToList();
        }
    }
}