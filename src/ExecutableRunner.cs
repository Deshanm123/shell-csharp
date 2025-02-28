using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_shell.src
{
    public static class ExecutableRunner
    {
        public static async Task<bool> Run(string progName, string progArgs)
        {
            //Executing the executable
            try
            {
                using var process = new Process();
                process.StartInfo.FileName = progName;
                process.StartInfo.Arguments = progArgs;
                process.Start();
                await process.WaitForExitAsync();
                return process.ExitCode == 0; 
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}
