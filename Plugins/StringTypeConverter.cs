using System.Linq;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace RFRocketLibrary.Plugins
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet

    /// <summary>
    /// Used to parse strings
    /// </summary>
    public static class StringTypeConverter
    {
        private static readonly string[] ValueTrue = {"true", "enabled", "enable", "on", "active", "t", "1", "yes"};
        private static readonly string[] ValueFalse = {"false", "disabled", "disable", "off", "inactive", "f", "0", "no"};

        public static EParseResult Parse<T>(string input, out T? result)
        {
            result = default;
            var t = typeof(T);
            if (t == typeof(string))
            {
                result = (T) (object) input;
                return EParseResult.Parsed;
            }

            if (t == typeof(sbyte))
            {
                if (!sbyte.TryParse(input, out var nResult))
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;
            }

            if (t == typeof(byte))
            {
                if (!byte.TryParse(input, out var nResult))
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;
            }

            if (t == typeof(short))
            {
                if (!short.TryParse(input, out var nResult))
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;
            }

            if (t == typeof(ushort))
            {
                if (!ushort.TryParse(input, out var nResult))
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;
            }

            if (t == typeof(int))
            {
                if (!int.TryParse(input, out var nResult))
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;
            }

            if (t == typeof(uint))
            {
                if (!uint.TryParse(input, out var nResult))
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;
            }

            if (t == typeof(float))
            {
                if (!float.TryParse(input, out var nResult)) 
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;
            }

            if (t == typeof(double))
            {
                if (!double.TryParse(input, out var nResult)) 
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;
            }

            if (t == typeof(long))
            {
                if (!long.TryParse(input, out var nResult)) 
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;

            }

            if (t == typeof(ulong))
            {
                if (!ulong.TryParse(input, out var nResult)) 
                    return EParseResult.ParseFailed;
                result = (T) (object) nResult;
                return EParseResult.Parsed;

            }

            if (t == typeof(Player))
            {
                var pl = ParsePlayer(input);
                if (pl == null) 
                    return EParseResult.PlayerNotFound;
                result = (T) (object) pl;
                return EParseResult.Parsed;

            }

            if (t == typeof(UnturnedPlayer))
            {
                var pl = ParsePlayer(input);
                if (pl == null) 
                    return EParseResult.PlayerNotFound;
                result = (T) (object) UnturnedPlayer.FromPlayer(pl);
                return EParseResult.Parsed;

            }

            if (t == typeof(SteamPlayer))
            {
                var pl = ParsePlayer(input);
                if (pl == null) 
                    return EParseResult.PlayerNotFound;
                result = (T) (object) pl.channel.owner;
                return EParseResult.Parsed;

            }

            if (t == typeof(bool))
            {
                var lower = input.ToLowerInvariant();

                if (ValueTrue.Contains(lower))
                {
                    result = (T) (object) true;
                    return EParseResult.Parsed;
                }

                if (ValueFalse.Contains(lower))
                {
                    result = (T) (object) false;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            return EParseResult.InvalidType;
        }

        public static Player ParsePlayer(string handle)
        {
            if (!ulong.TryParse(handle, out var uid)) 
                return PlayerTool.getPlayer(handle);
            var pl = PlayerTool.getPlayer(new CSteamID(uid));
            return pl;

        }
    }
}