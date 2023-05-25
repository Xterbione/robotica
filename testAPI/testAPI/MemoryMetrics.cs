using System.Diagnostics;
using System.Runtime.InteropServices;
using testAPI.Models;

namespace testAPI
{


    public class MemoryMetrics
    {
        private const int DigitsInResult = 2;
        private static long totalMemoryInKb;


        /// <summary>
        /// general function for calling, figures out OS and uses the proper function.
        /// </summary>
        /// <returns>MemoryMetricsModel</returns>
        public MemoryMetricsModel GetMetrics()
        {
            if (SystemInfo.IsUnix())
            {
                return GetUnixMetrics();
            }

            return GetWindowsMetrics();
        }


        /// <summary>
        /// returns the memory metric of the system
        /// </summary>
        /// <returns>MemoryMetricsModel</returns>
        private MemoryMetricsModel GetWindowsMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetricsModel();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }
        /// <summary>
        /// gets the memory metrics for unix
        /// </summary>
        /// <returns>MemoryMetricsModel</returns>
        private MemoryMetricsModel GetUnixMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo("free -m");
            info.FileName = "/bin/bash";
            info.Arguments = "-c \"free -m\"";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
            }

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetricsModel();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);

            return metrics;
        }








        public static long GetTotalDiskSpace()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var path = "C:\\";
                DriveInfo drive = new DriveInfo(Path.GetPathRoot(path));
                return drive.TotalSize;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var path = "/";
                var ps = new System.Diagnostics.Process();
                ps.StartInfo.FileName = "/bin/df";
                ps.StartInfo.Arguments = "-B 1 \"" + Path.GetPathRoot(path) + "\"";
                ps.StartInfo.RedirectStandardOutput = true;
                ps.StartInfo.UseShellExecute = false;
                ps.StartInfo.CreateNoWindow = true;
                ps.Start();
                var output = ps.StandardOutput.ReadToEnd();
                ps.WaitForExit();
                var lines = output.Trim().Split('\n');
                var parts = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return long.Parse(parts[1]);
            }
            else
            {
                throw new NotSupportedException("This method is only supported on Windows and Linux.");
            }
        }

        public static long GetAvailableDiskSpace()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var path = "C:\\";
                DriveInfo drive = new DriveInfo(Path.GetPathRoot(path));
                return drive.AvailableFreeSpace;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var path = "/";
                var ps = new System.Diagnostics.Process();
                ps.StartInfo.FileName = "/bin/df";
                ps.StartInfo.Arguments = "-B 1 \"" + path + "\"";
                ps.StartInfo.RedirectStandardOutput = true;
                ps.StartInfo.UseShellExecute = false;
                ps.StartInfo.CreateNoWindow = true;
                ps.Start();
                var output = ps.StandardOutput.ReadToEnd();
                ps.WaitForExit();
                var lines = output.Trim().Split('\n');
                var parts = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return long.Parse(parts[3]);
            }
            else
            {
                throw new NotSupportedException("This method is only supported on Windows and Linux.");
            }
        }

    }
}
