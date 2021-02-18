using OrbitalShell.Component.CommandLine.CommandModel;
using OrbitalShell.Component.Shell.Module;
using OrbitalShell.Component.Shell.Hook;
using OrbitalShell.Component.Shell;
using OrbitalShell.Component.Shell.Variable;
using OrbitalShell.Component.CommandLine.Processor;
using System;
using System.IO;
using System.Linq;
using OrbitalShell.Component.Console;
using System.Collections.Generic;

namespace OrbitalShell.Module.{ModuleID}
{
    /// <summary>
    /// module commands : prompt git infos
    /// </summary>
    [Commands("{ModuleDescription} module commands")]
    [CommandsNamespace(CommandNamespace.tools, ToolNamespace)]
    [Hooks]
    public class {ModuleID} : ICommandsDeclaringType
    {
        #region attributes 

        public const string ToolNamespace = "{ModuleToolNamespace}";
        public const string ToolVarSettingsName = "{ModuleToolVarSettingsName}";
        public const string VarIsEnabled = "isEnabled";
        
        string _namespace => Variables.Nsp(ShellEnvironmentNamespace.com + "", ToolNamespace, ToolVarSettingsName);

        #endregion

        #region init

        /// <summary>
        /// init module hook
        /// </summary>
        [Hook(Hooks.ModuleInit)]
        public void Init(CommandEvaluationContext context)
        {
            // init settings
            context.ShellEnv.AddNew(_namespace, VarIsEnabled, true, false);

            // ... add your own inits here ...            
        }

        #endregion

        #region Command

        /// <summary>
        /// enable or disable module
        /// </summary>
        [Command("enable/disable module {ModuleTitle}")]
        public CommandVoidResult {ModuleID}Com(
            CommandEvaluationContext context,
            [Option("e", "enable", "if true enable the module, otherwise disable it", true, true)] bool isEnabled = true
        )
        {
            context.Variables.SetValue(Variables.Nsp(VariableNamespace.env + "", _namespace), VarIsEnabled, isEnabled);
            return CommandVoidResult.Instance;
        }

        // add your own hooks ...

        #endregion                
    }
}