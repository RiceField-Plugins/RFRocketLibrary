// using Cysharp.Threading.Tasks;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Plugins;
using RocketExtensions.Core;
using RocketExtensions.Models;
using RocketExtensions.Models.Exceptions;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using RFRocketLibrary.Helpers;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using RocketCaller = Rocket.API.AllowedCaller;

namespace RocketExtensions.Plugins
{
    public abstract class RocketCommand : IRocketCommand
    {
        private CommandActor? _actor;
        private IRocketPlugin? _plugin;
        private bool _pluginInit;

        public IRocketPlugin? Plugin
        {
            get
            {
                if (!_pluginInit)
                {
                    _plugin = R.Plugins.GetPlugin(GetType().Assembly);
                    _pluginInit = true;
                }
                
                return _plugin;
            }
        }

        public RocketCaller AllowedCaller
        {
            get
            {
                var typ = GetType();
                _actor ??= typ.GetCustomAttribute<CommandActor>() ?? new CommandActor(RocketCaller.Both);
                return _actor.Actor;
            }
        }

        private string? _name;

        public string? Name
        {
            get
            {
                if (_name == null)
                {
                    var typ = GetType();

                    var nm = typ.GetCustomAttribute<CommandName>();
                    if (nm != null)
                        _name = nm.Name;
                    else
                    {
                        var className = typ.Name;
                        var cmdIndex = className.IndexOf("command", 0, StringComparison.InvariantCultureIgnoreCase);
                        if (cmdIndex != -1)
                            className = className.Remove(cmdIndex, 7);

                        _name = className;
                    }
                }

                return _name;
            }
        }

        private string? _help;
        private string? _syntax;

        private void m_InitInfo()
        {
            var typ = GetType();
            var info = typ.GetCustomAttribute<CommandInfo>();
            if (info != null)
            {
                _help = info.Help;
                _syntax = !string.IsNullOrEmpty(info.Syntax) ? info.Syntax : Name;
            }
            else
            {
                _help = "";
                _syntax = Name;
            }
        }

        public string? Help
        {
            get
            {
                if (_help == null)
                    m_InitInfo();
                
                return _help;
            }
        }

        public string? Syntax
        {
            get
            {
                if (_syntax == null)
                    m_InitInfo();

                return _syntax;
            }
        }

        private List<string>? _aliases;

        public List<string>? Aliases
        {
            get
            {
                if (_aliases == null)
                {
                    var info = GetType().GetCustomAttribute<CommandAliases>();
                    _aliases = info != null ? info.Aliases : new List<string>();
                }
                
                return _aliases;
            }
        }

        private List<string> _permissions = new();

        public List<string> Permissions
        {
            get
            {
                var typ = GetType();
                var inst = typ.GetCustomAttribute<CommandPermissions>();
                if (inst != null)
                    _permissions = inst.Permissions;
                else
                {
                    var asmName = typ.Assembly.GetName().Name;
                    _permissions = new List<string> { $"{asmName}.{Name}" };
                }

                return _permissions;
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            CoreSetup.CheckInit();
            var context = new CommandContext(caller, this, command);
            Task.Run(async () => await Run(context));
        }

        private async Task Run(CommandContext context)
        {
            try
            {
                await Execute(context);
            }
            catch (InvalidArgumentException invalid)
            {
                await context.ReplyAsync(invalid.Message, Color.red);
                await context.ReplyAsync($"Command Usage: /{Name} {Syntax}", Color.cyan);
                await context.CancelCooldownAsync();
            }
            catch (PlayerNotFoundException player)
            {
                await context.ReplyAsync(player.Message, Color.red);
                await context.CancelCooldownAsync();
            }
            catch (ArgumentMissingException missing)
            {
                await context.ReplyAsync(missing.Message, Color.red);
                await context.ReplyAsync($"Command Usage: /{Name} {Syntax}", Color.cyan);
                await context.CancelCooldownAsync();
            }
            catch (WrongUsageOfCommandException usage)
            {
                await context.CancelCooldownAsync();
                throw usage;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while executing /{Name}");
                Logger.LogError($"[{ex.GetType().FullName}] {ex.Message}");
                Logger.LogError(ex.StackTrace);
                await context.ReplyAsync("An error occurred during the execution of this command", Color.red);
            }
        }

        public T? GetPluginInstance<T>() where T : IRocketPlugin
        {
            return typeof(T).Assembly.TryGetPlugin<T>();
        }

        public T? GetPluginConfig<T>() where T : IRocketPluginConfiguration
        {
            var cType = typeof(RocketPlugin<>).MakeGenericType(typeof(T));
            dynamic? plugin = typeof(T).Assembly.TryGetPlugin(cType);
            if (plugin == null)
                return default;
            
            var configInstance = plugin.Configuration.Instance;

            if (configInstance == null)
                return default;

            if (configInstance is T t)
                return t;

            if (configInstance is T instance)
                return instance;
            
            return default;
        }

        public abstract Task Execute(CommandContext context);

        /// <summary>
        /// Sends a message to the specified player
        /// </summary>
        public async Task SayAsync(IRocketPlayer player, string message, Color? messageColor = null, string? iconUrl = null)
        {
            messageColor ??= Color.green;
            await ThreadTool.RunOnGameThreadAsync(ChatHelper.Say, player, message, messageColor, iconUrl);
        }

        /// <summary>
        /// Sends a message to all online players.
        /// </summary>
        public async Task AnnounceAsync(string message, Color? messageColor = null, string? iconUrl = null)
        {
            messageColor ??= Color.green;
            await ThreadTool.RunOnGameThreadAsync(ChatHelper.Broadcast, message, messageColor, iconUrl);
        }
    }
}