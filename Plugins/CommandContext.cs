using RFRocketLibrary.Helpers;
using RFRocketLibrary.Utils;
using Rocket.API;
using UnityEngine;

namespace RFRocketLibrary.Plugins
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    
    /// <summary>
    /// Contains the context for command execution
    /// </summary>
    public struct CommandContext
    {
        public IRocketPlayer Player { get; private set; }
        public string[] CommandRawArguments { get; private set; }
        public ArgumentList Arguments { get; private set; }

        public CommandContext(IRocketPlayer player, string[] args)
        {
            Player = player;
            CommandRawArguments = args;
            Arguments = new ArgumentList(args);
        }

        public async System.Threading.Tasks.Task ReplyAsync(string message, Color? messageColor = null, string? iconURL = null)
        {
            messageColor ??= Color.green;
            await ThreadUtil.RunOnGameThreadAsync((player, msg) =>
            {
                ChatHelper.Say(player, msg, messageColor, iconURL);
            }, Player, message);
        }

        public async Cysharp.Threading.Tasks.UniTask ReplyUniTaskAsync(string message, Color? messageColor = null, string? iconURL = null)
        {
            messageColor ??= Color.green;
            await ThreadUtil.RunOnGameThreadAsync((player, msg) =>
            {
                ChatHelper.Say(player, msg, messageColor, iconURL);
            }, Player, message);
        }
    }
}