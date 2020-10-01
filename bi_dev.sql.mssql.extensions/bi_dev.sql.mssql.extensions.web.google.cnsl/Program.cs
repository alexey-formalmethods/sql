using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bi_dev.sql.mssql.extensions.web.google;
using CommandLine;
using CommandLine.Text;

namespace bi_dev.sql.mssql.extensions.web.google.cnsl
{
    // Define a class to receive parsed values
    class Options
    {
        [Option('c', "credentialFilePath", Required = true,
          HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option('s', "scope", Required = true,
          HelpText = "scopes")]
        public IEnumerable<string> Scope { get; set; }

        
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            string argsString = Console.ReadLine();
            
            CommandLine.Parser.Default.ParseArguments<Options>(argsString.Split(' '))
                    .WithParsed(RunOptions)
                    .WithNotParsed(HandleParseError);
            
        }
        static void RunOptions(Options opts)
        {
            Auth.Do(opts.InputFile, opts.Scope.ToArray());
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Invalid args");
        }
    }
}
