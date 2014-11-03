using System;
using CommandLine;

namespace PdIncidents
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                var pagerdutyHelper = new PagerDutyHelper(PagerDutyHelper.GetPagerDutyConfig(options.Config));
                
                switch (options.Action)
                {
                    case Action.Raise:
                        pagerdutyHelper.RaiseNew(options.Subject, options.Message);
                        break;
                    case Action.Acknowledge:
                        pagerdutyHelper.Acknowledge(options.Subject, options.Message);
                        break;
                    case Action.Resolve:
                        pagerdutyHelper.Resolve(options.Subject);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }          
        }

        internal class Options
        {
            [Option('c', "config", DefaultValue = "PagerDuty.config")]
            public string Config { get; set; }

            [Option('s', "subject", DefaultValue = "New Incident")]
            public string Subject { get; set; }

            [Option('m', "message", DefaultValue = "")]
            public string Message { get; set; }

            [Option('a', "action", DefaultValue = Action.Raise)]
            public Action Action { get; set; }
        }

        internal enum Action
        {
            Raise,
            Acknowledge,
            Resolve
        }


    }
}