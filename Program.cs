using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace WinProcessMonitor
{
    // More readable and easy to use command line parameters.
    // If it is neccessary, it is possible to rework it using 'Value' attribute, so executing this app will be like "WinProcessMonitor.exe notepad 1 2".
    public class CommandLineOptions
    {
        [Option(shortName: 'n', longName: "name", Required = true, HelpText = "Windows process' name (Case insensitive. Without .exe)")]
        public string ProcessName { get; set; }
        [Option(shortName: 'l', longName: "lifetime", Required = true, HelpText = "Maximum possible lifetime of a process in minutes. Be careful setting this parameter to 0")]
        public uint LifeTime { get; set; }
        [Option(shortName: 'f', longName: "frequency", Required = true, HelpText = "Update frequency in minutes. Be careful setting this parameter to 0")]
        public uint UpdateFrequency { get; set; } // uint to prevent negative values
    }
    class Program
    {
        private static void Main(string[] args)
        {
            CommandLineOptions options = new();
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(parsed => options = parsed)
                .WithNotParsed(errors => { Environment.Exit(1); });
            
            var defaultConsoleBackgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Args: --name={options.ProcessName} --lifetime={options.LifeTime} --frequency={options.UpdateFrequency}");
            Console.WriteLine($"Monitoring process: {options.ProcessName.ToLower()}.exe");
            Console.WriteLine("Press ENTER to close the app.");

            Console.BackgroundColor = ConsoleColor.DarkGreen;

            // This implementation assumes that the process will be monitored, even if it doesn't exist when the app starts.
            // So there are no any validations for the process' name correctness.

            Task.Run(() =>
            {
                // Using an infinite loop is obviously a bad practice but I guess that it is acceptable in this test task.
                // If it were a real app, it would be better to implement it as a windows service.
                while (true)
                {
                    KillProcessByLifetime(options.ProcessName, (int)options.LifeTime);
                    Thread.Sleep((int)options.UpdateFrequency * 60000);
                }
            });
            
            Console.ReadLine();
            Console.BackgroundColor = defaultConsoleBackgroundColor;
        }

        private static void KillProcessByLifetime(string name, int maxLifetime)
        {
            Process[] processes = Process.GetProcessesByName(name);
            if(processes.Length == 0) return;

            foreach (var process in processes)
            {
                if (process.StartTime.AddMinutes(maxLifetime) < DateTime.Now)
                {
                    process.Kill();
                    Console.WriteLine($"Process {name}.exe with PID: {process.Id} has been terminated. Lifetime: {DateTime.Now - process.StartTime}");
                }    
            }
        }
    } 
}
