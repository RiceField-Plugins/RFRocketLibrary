using System;
using System.Collections.Generic;
using System.Linq;

namespace RFRocketLibrary.Plugins
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public sealed class Permissions : Attribute
    {
        public List<string> PermissionValues { get; set; }

        public Permissions(params string[] perms)
        {
            PermissionValues = perms.ToList();
        }
    }
}