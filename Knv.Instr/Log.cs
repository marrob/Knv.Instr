namespace Knv.Instr
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Log
    {
        readonly List<string> LogLines = new List<string>();

        public void LogWriteLine(string line)
        {
            line = line.Trim(new char[] {' ', '\r', '\n' });
            var dt = DateTime.Now;
            LogLines.Add($"{dt:yyyy}.{dt:MM}.{dt:dd} {dt:HH}:{dt:mm}:{dt:ss}:{dt:fff} {line}");
        }

        public void LogSave(string directory)
        {
            LogSave(directory, "");
        }

        public void LogSave(string directory, string prefix)
        {
            if (!System.IO.File.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            LogWriteLine("Log Saved.");
            var dt = DateTime.Now;
            var fileName = $"{prefix}_{dt:yyyy}{dt:MM}{dt:dd}_{dt:HH}{dt:mm}{dt:ss}.log";
            using (var file = new System.IO.StreamWriter($"{directory}\\{fileName}", true, Encoding.ASCII))
                LogLines.ForEach(file.WriteLine);
        }
    }
}
