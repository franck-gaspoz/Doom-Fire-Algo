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

namespace OrbitalShell.Module.DoomFireAlgo
{
    /// <summary>
    /// module Doom Fire Algo Commands
    /// </summary>
    [Commands("a simple module for orbital shell that add a command running the famous doom fire algorithm (C# ANSI version) module commands")]
    [CommandsNamespace(CommandNamespace.games)]
    public class DoomFireAlgoCommands : ICommandsDeclaringType
    {
        public const string DefaultColorPalette =

            "░" +

            "░" +

            "░" +

            "░" +

            "░" +

            "░" +

            "░" +

            "░" +

            "░" +

            "░" +

            "░" +

            "░" +

            "▒" +

            "▒" +

            "▒" +

            "▒" +

            "▒" +

            "▒" +

            "▓" +

            "▓" +

            "▓" +

            "▓" +

            "▓" +

            "▓" +

            "▓" +

            "▓" +

            "█" +

            "█" +

            "█" +

            "█" +

            "█" +

            "█" +

            "█" +

            "█" +

            "█" +

            "█"

            ;

        #region Command

        /// <summary>
        /// enable or disable module
        /// </summary>
        [Command("runs an ASCII Doom Fire Algorithm that output an animation into the console")]
        public CommandVoidResult DoomFireAlgo(
            CommandEvaluationContext context,
            [Option("w", "width", "width in characters", true, true)] int width = 100,
            [Option("h", "height", "height in characters", true, true)] int height = 40,
            [Option("d", "decay-delta", "decay delta", true, true)] int decayDelta = 3,
            [Option(null, "color-palette", "color palette", true, true)] string colorPalette = DefaultColorPalette
        )
        {

            return CommandVoidResult.Instance;
        }

        #endregion
    }
}