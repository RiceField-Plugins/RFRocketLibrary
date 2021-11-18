using System;
using System.Collections.Generic;
using System.Reflection;
using RFRocketLibrary.Plugins.Exceptions;
using RFRocketLibrary.Utils;
using Rocket.API;
using Rocket.Core.Logging;
using RocketCaller = Rocket.API.AllowedCaller;

namespace RFRocketLibrary.Plugins
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public abstract class RocketCommand : IRocketCommand
    {
        private AllowedCaller? m_AllowedCaller;

        public RocketCaller AllowedCaller
        {
            get
            {
                var typ = GetType();
                m_AllowedCaller = typ.GetCustomAttribute<AllowedCaller>() ?? new AllowedCaller(RocketCaller.Both);
                return m_AllowedCaller.Caller;
            }
        }

        private string? m_Name;

        public string? Name
        {
            get
            {
                var typ = GetType();

                var nm = typ.GetCustomAttribute<CommandName>();
                if (nm != null)
                {
                    m_Name = nm.Name;
                }
                else
                {
                    var className = typ.Name;

                    var commandStrIndex =
                        className.IndexOf("command", 0, StringComparison.InvariantCultureIgnoreCase);

                    if (commandStrIndex != -1)
                    {
                        className = className.Remove(commandStrIndex, 7);
                    }

                    m_Name = className;
                }

                return m_Name;
            }
        }

        private string? m_Help;
        private string? m_Syntax;

        private void m_InitInfo()
        {
            var typ = GetType();

            var info = typ.GetCustomAttribute<CommandInfo>();

            if (info != null)
            {
                m_Help = info.Help;

                m_Syntax = !string.IsNullOrEmpty(info.Syntax) ? info.Syntax : Name;
            }
            else
            {
                m_Help = "";
                m_Syntax = Name;
            }
        }

        public string? Help
        {
            get
            {
                m_InitInfo();
                return m_Help;
            }
        }

        public string? Syntax
        {
            get
            {
                m_InitInfo();
                return m_Syntax;
            }
        }

        private List<string> m_Aliases = new();

        public List<string> Aliases
        {
            get
            {
                var info = GetType().GetCustomAttribute<Aliases>();
                m_Aliases = info != null ? info.AliasList : new List<string>();
                return m_Aliases;
            }
        }

        private List<string> m_Permissions = new();

        public List<string> Permissions
        {
            get
            {
                var typ = GetType();
                var inst = typ.GetCustomAttribute<Permissions>();
                if (inst != null)
                {
                    m_Permissions = inst.PermissionValues;
                }
                else
                {
                    var asmName = typ.Assembly.GetName().Name;

                    m_Permissions = new List<string> {$"{asmName}.{Name}"};
                }

                return m_Permissions;
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var context = new CommandContext(caller, command);
            System.Threading.Tasks.Task.Run(async () => await RunAsync(context));
        }

        private async System.Threading.Tasks.Task RunAsync(CommandContext context)
        {
            try
            {
                await ExecuteAsync(context);
            }
            catch (InvalidArgumentException invalid)
            {
                await context.ReplyAsync(invalid.Message, UnityEngine.Color.red);
                await context.ReplyAsync($"Command Usage: /{Name} {Syntax}", UnityEngine.Color.cyan);
            }
            catch (PlayerNotFoundException player)
            {
                await context.ReplyAsync(player.Message, UnityEngine.Color.red);
            }
            catch (ArgumentMissingException missing)
            {
                await context.ReplyAsync(missing.Message, UnityEngine.Color.red);
                await context.ReplyAsync($"Command Usage: /{Name} {Syntax}", UnityEngine.Color.cyan);
            }
            catch (WrongUsageOfCommandException usage)
            {
                await ThreadUtil.RunOnGameThreadAsync(() => throw usage);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while executing /{Name}");
                Logger.LogError($"[{ex.GetType().FullName}] {ex.Message}");
                Logger.LogError(ex.StackTrace);
                await context.ReplyAsync("<color=red>An error occurred during the execution of this command</color>");
            }
        }

        public abstract System.Threading.Tasks.Task ExecuteAsync(CommandContext context);
    }
}