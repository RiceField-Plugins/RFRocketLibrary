using System;
using System.Globalization;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util
{
    /// <summary>
    ///     Utilities for working with color (relevant for embed).
    /// </summary>
    public static class ColorUtil
    {
        /// <summary>
        ///     Converts HEX to Decimal, for use in embed.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     <paramref name="hex"/> is empty or null,
        ///     or isn't hex.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Invalid hex format.
        /// </exception>
        public static int FromHex(string hex)
        {
            Checks.CheckForArgument(string.IsNullOrEmpty(hex), nameof(hex), "Value cannot be null or empty");

            // ReSharper disable once PossibleNullReferenceException
            if (!hex.StartsWith("#", StringComparison.InvariantCulture))
                throw new ArgumentException("The value must start with a '#'", nameof(hex));
            hex = hex.Substring(1);
            return int.Parse(hex, NumberStyles.HexNumber);
        }
    }
}
