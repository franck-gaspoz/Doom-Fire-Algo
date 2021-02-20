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
using System.Threading;
using System.Diagnostics;
using System.Text;

namespace OrbitalShell.Module.DoomFireAlgo
{
    /// <summary>
    /// module Doom Fire Algo Commands
    /// </summary>
    [Commands("a simple module for orbital shell that add a command running the famous doom fire algorithm (C# ANSI version) module commands")]
    [CommandsNamespace(CommandNamespace.games)]
    public class DoomFireAlgoCommands : ICommandsDeclaringType
    {
        public const string PalettePatternSeparator = "_";
        public const string DefaultFirePattern = " _ _ _ _ _ _ _ _ _ _░_░_▒_▒_▒_▒_▒_▒_▓_▓_▓_▓_▓_▓_▓_▓_█_█_█_█_█_█_█_█_█_" ;

        #region Command

        [Command("runs an ASCII Doom Fire Algorithm that output an animation into the console")]
        public CommandVoidResult DoomFireAlgo(
            CommandEvaluationContext context,
            [Option("w", "width", "width in characters", true, true)] int width = 100,
            [Option("h", "height", "height in characters", true, true)] int height = 40,
            [Option("d", "decay-delta", "decay delta", true, true)] int decayDelta = 3,
            [Option("g", "gray","gray mode - no colors")] bool gray = false,
            [Option("s", "slow","0 max speed - pause time between images in ms",true,true)] int slow = 0,
            [Option("t", "no-text","do not add text info above anim")] bool noText = false,
            [Option(null, "color-palette", "color palette. 36 symbols separated by " + PalettePatternSeparator, true, true)] string firePattern = DefaultFirePattern
        )
        {
            // remove directives to speed up (TODO: the directives lead to stack overflow in this case)
            firePattern = firePattern.ToLower();

            var firePallete = firePattern.Split(PalettePatternSeparator);

            if (!gray)
            {
                // add colors
                var n = firePallete.Length;
                var stp = 256 / n;
                int r = 0;
                int g = 0;
                int b = 0;
                for (int i = 0; i < firePallete.Length; i++)
                {
                    firePallete[i] = ANSI.SGR_SetForegroundColor24bits(r, g, b) + firePallete[i];
                    r += stp;
                    if (r > 100) g += stp;
                }
            }

            var o = context.Out;
            o.Echo("pixelPalete.Length=" + firePallete.Length);
            var pixels = width * height;
            var pixelsArray = new int[pixels];
            var random = new Random();

            void createPixelsStructure() {                
                for (var i = 0; i < pixels; i++)
                    pixelsArray[i] = 0;
            };

            void calculatePropagation() {
                for (var column = 0; column < width; column++)
                {
                    for (var row = 0; row < height; row++)
                    {
                        var index = column + (width * row);
                        updatePixelIntensity(index);
                    }
                }
            };

            void updatePixelIntensity(int index) {
                var belowIndex = index + width;
                if (belowIndex < width * height)
                {
                    var decay = (int)Math.Floor(random.NextDouble() * decayDelta);
                    var belowIntensity = pixelsArray[belowIndex];
                    var newIntensity = belowIntensity - decay;

                    if (index-decay>0)
                        pixelsArray[index - decay] = newIntensity > 0 ? newIntensity : 0;
                }
            };

            void createSource() {
                for (var column = 0; column < width; column++)
                {
                    var overflowPixel = width * height;
                    var index = (overflowPixel - width) + column;

                    pixelsArray[index] = firePallete.Length - 1;
                }
            };

            void render() {
                var sb = new StringBuilder();
                sb.Append(ANSI.CUP());

                for (var row = 0; row < height; row++)
                {
                    for (var column = 0; column < width; column++)
                    {
                        var index = column + (width * row);
                        var intensity = pixelsArray[index];
                        sb.Append(firePallete[intensity]);
                    }
                    sb.AppendLine();
                }
                o.ConsolePrint(sb.ToString());     // fast print
            };

            void start()
            {
                o.ClearScreen();
                o.HideCur();
                createPixelsStructure();
                createSource();
            }

            start();

            var end = false;
            while (!end)
            {
                var sw0 = Stopwatch.StartNew();
                calculatePropagation();
                sw0.Stop();
                var sw1 = Stopwatch.StartNew();
                render();
                sw1.Stop();
                if (!noText) o.Echoln($"(rdc)Doom fire algo - {Math.Round(1d / (sw0.ElapsedMilliseconds+sw1.ElapsedMilliseconds) * 1000,2)} fps");
                end = context.CommandLineProcessor.CancellationTokenSource.IsCancellationRequested;
                if (slow>0) Thread.Sleep(slow);
            }

            o.ShowCur();

            return CommandVoidResult.Instance;
        }

        #endregion

        #region private impl.



        #endregion
    }
}