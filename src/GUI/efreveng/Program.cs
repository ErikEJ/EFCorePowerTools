﻿using ReverseEngineer20;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.IO;

namespace efreveng
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    if (args[0].EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase))
                    { 
                        return BuildDacpacList(args[0]);
                    }

                    if (!File.Exists(args[0]))
                    {
                        Console.Out.WriteLine("Error:");
                        Console.Out.WriteLine($"Could not open options file: {args[0]}");
                        return 1;
                    }

                    var options = ReverseEngineerOptionsExtensions.TryDeserialize(File.ReadAllText(args[0], System.Text.Encoding.UTF8));

                    if (options == null)
                    {
                        Console.Out.WriteLine("Error:");
                        Console.Out.WriteLine("Could not read options");
                        return 1;
                    }

                    var runner = new ReverseEngineerRunner();

                    var result = runner.GenerateFiles(options);

                    Console.Out.WriteLine("Result:");
                    Console.Out.WriteLine(result.Write());
                }
                else
                {
                    Console.Out.WriteLine("Error:");
                    Console.Out.WriteLine("Invalid command line");
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error:");
                Console.Out.WriteLine(ex);
                return 1;
            }
        }

        private static int BuildDacpacList(string dacpacPath)
        {
            if (!File.Exists(dacpacPath))
            {
                Console.Out.WriteLine("Error:");
                Console.Out.WriteLine($"Could not open .dacpac file: {dacpacPath}");
                return 1;
            }

            var builder = new DacpacTableListBuilder(dacpacPath);

            var result = builder.GetTableDefinitions();

            Console.Out.WriteLine("Result:");
            Console.Out.WriteLine(result.Write());

            return 0;
        }
    }
}
