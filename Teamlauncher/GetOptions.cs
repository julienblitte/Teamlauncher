using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Teamlauncher
{
    public enum OptionPrefix { shortPrefix, longPrefix, winPrefix, noPrefix, anyPrefix };

    class GetOptions
    {
        string[] options;
        OptionPrefix[] prefixes;
        int len;

        public int Length { get { return len; } }

        public GetOptions(string[] args)
        {
            int i;
            Regex shortReg, longReg, winReg;

            len = args.Length;
            options = new string[len];
            prefixes = new OptionPrefix[len];

            shortReg = new Regex("^-[^-].*$");
            longReg = new Regex("^--[^-].*");
            winReg = new Regex("^/[^/].*");

            for (i = 0; i < len; i++)
            {
                string arg;
                arg = args[i];

                if (shortReg.IsMatch(arg))
                {
                    options[i] = arg.Substring(1);
                    prefixes[i] = OptionPrefix.longPrefix;
                }
                else if (longReg.IsMatch(arg))
                {
                    options[i] = arg.Substring(2);
                    prefixes[i] = OptionPrefix.longPrefix;
                }
                else if (winReg.IsMatch(arg))
                {
                    options[i] = arg.Substring(1);
                    prefixes[i] = OptionPrefix.winPrefix;
                }
                else
                {
                    options[i] = arg;
                    prefixes[i] = OptionPrefix.noPrefix;
                }
            }
        }

        public int isOption(string option, OptionPrefix prefix)
        {
            int i;

            for(i = 0; i < len; i++)
            {
                if ((prefix == OptionPrefix.anyPrefix) || (prefix == prefixes[i]))
                {
                    if (options[i] == option)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
