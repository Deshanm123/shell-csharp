using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_shell.src
{
    public static class Utils
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

        public static string[] GetPathDirectives()
        {
            // Retrieve the PATH environment variable, or use an empty string if null.
            string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
            // Split the PATH string into individual directories based on the platform's PATH separator ignoring any empty entries.
            string[] pathDirs = pathEnv.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
            return pathDirs;
        }
       
    }
}
