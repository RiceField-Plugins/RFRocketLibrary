using System;

namespace RFRocketLibrary.Plugins
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public sealed class AllowedCaller : Attribute
    {
        public Rocket.API.AllowedCaller Caller { get; }

        public AllowedCaller(Rocket.API.AllowedCaller allowedCaller)
        {
            Caller = allowedCaller;
        }
    }
}