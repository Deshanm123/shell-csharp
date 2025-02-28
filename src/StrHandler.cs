using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace codecrafters_shell.src
{
    public static class StrHandler
    {
       public enum ReserveWord
        {
            echo,
            type,
            exit,
            pwd,
            cd
        };

        public static string extractInput(string command, string sliceWord)
        {
            return command.Substring(sliceWord.Length).Trim();
        }

        public static char[] RemoveCharFromString(string keyword, char character)
        {
            return keyword.ToCharArray()
                           .Where(chr => chr != character)
                           .ToArray();
        }

        public static Match[] GetPatternMatchesByRegex(string strPhrase, string regPattern)
        {
            return Regex.Matches(strPhrase, regPattern).ToArray();
        }

    }
}
