using System;

namespace RFRocketLibrary.Plugins.Exceptions
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public sealed class InvalidArgumentException : Exception
    {
        public override string Message { get; }

        public InvalidArgumentException(string? name)
        {
            Message = $"Invalid value for field {name}";
        }

        public InvalidArgumentException(int index)
        {
            Message = $"Invalid value at command position {index + 1}";
        }
    }
}