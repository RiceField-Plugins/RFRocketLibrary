using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RFRocketLibrary.Plugins.Exceptions;

namespace RFRocketLibrary.Plugins
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public class ArgumentList : IEnumerable<string>
    {
        private readonly List<string> m_Items;

        public ArgumentList(IEnumerable<string> arguments)
        {
            m_Items = arguments.ToList();
        }

        // Backward Compatibility

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, and player not found.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <param name="defaultValue">Default value to be returned instead of the argument was not supplied.</param>
        /// <returns>Parsed Value</returns>
        public T? Get<T>(int index, T? defaultValue) => Get<T>(index, defaultValue, null);

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, player not found, and argument missing.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <returns>Parsed Value</returns>
        public T? Get<T>(int index) => Get<T>(index, null);

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, and player not found.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <param name="defaultValue">Default value to be returned instead of the argument was not supplied.</param>
        /// <param name="paramName">The parameter name to be used in User Friendly error messages</param>
        /// <returns>Parsed Value</returns>
        public T? Get<T>(int index, T? defaultValue, string? paramName)
        {
            if (index >= m_Items.Count)
            {
                return defaultValue;
            }

            var value = m_Items[index];

            var result = StringTypeConverter.Parse<T>(value, out var res);

            return result switch
            {
                EParseResult.InvalidType => throw new InvalidCastException(
                    $"Type {typeof(T).Name} is not valid for automatic string parsing"),
                EParseResult.PlayerNotFound => defaultValue,
                EParseResult.ParseFailed when !string.IsNullOrEmpty(paramName) => throw new InvalidArgumentException(
                    paramName),
                EParseResult.ParseFailed => throw new InvalidArgumentException(index),
                _ => res
            };
        }

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, player not found, and argument missing.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <param name="paramName">The parameter name to be used in User Friendly error messages</param>
        /// <returns>Parsed Value</returns>
        public T? Get<T>(int index, string? paramName)
        {
            if (index >= m_Items.Count)
            {
                if (!string.IsNullOrEmpty(paramName))
                {
                    throw new ArgumentMissingException(paramName);
                }

                throw new ArgumentMissingException(index);
            }

            var value = m_Items[index];

            var result = StringTypeConverter.Parse<T>(value, out var res);

            return result switch
            {
                EParseResult.InvalidType => throw new InvalidCastException(
                    $"Type {typeof(T).Name} is not valid for automatic string parsing"),
                EParseResult.PlayerNotFound => throw new PlayerNotFoundException(value),
                EParseResult.ParseFailed when !string.IsNullOrEmpty(paramName) => throw new InvalidArgumentException(
                    paramName),
                EParseResult.ParseFailed => throw new InvalidArgumentException(index),
                _ => res
            };
        }

        public IEnumerator<string> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }
    }
}