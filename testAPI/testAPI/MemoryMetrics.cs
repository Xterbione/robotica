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
                return GetUnixMetrics();
            

            return GetWindowsMetrics();
        }


        /// <summary>
        /// returns the memory metric of the system
        /// </summary>
        /// <returns>MemoryMetricsModel</returns>
        private MemoryMetricsModel GetWindowsMetrics()
        {
            var output = "";
            // Use WMI (Windows Management Instrumentation) to get memory information
            var info = new ProcessStartInfo();
            //This specifies the executable or document file to start when the process is launched. 
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;

            //the using statment ensures that the process object will be properly disposed of when it goes out of scope.
            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }
            //This line trims any leading or trailing whitespace from the output string and then splits it into an array of strings using the newline character ("\n") as the separator
            var lines = output.Trim().Split("\n");
            /* 
            these lines further process the lines of output. They split each line into an array of substrings using the "=" character as the separator
            the StringSplitOptions
            removeEmptyEntries option is used to remove any empty entries from the resulting array
            this is done because the output is expected to have a key-value format, and by splitting on "=", you can extract the values
             */
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);
            //doing some transformations to get the data we want
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
            // This specifies the executable or document file to start when the process is launched.
            info.FileName = "/bin/bash";
            info.Arguments = "-c \"free -m\"";
            info.RedirectStandardOutput = true;

            // The using statement ensures that the process object will be properly disposed of when it goes out of scope.
            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
                // Output the captured output for debugging purposes.
                Console.WriteLine(output);
            }

            var lines = output.Split("\n");
            // Splitting the second line of the output into an array of substrings using the space character (" ") as the separator.
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetricsModel();
            // Parsing and assigning the total memory value from the memory array.
            metrics.Total = double.Parse(memory[1]);
            // Parsing and assigning the used memory value from the memory array.
            metrics.Used = double.Parse(memory[2]);
            // Parsing and assigning the free memory value from the memory array.
            metrics.Free = double.Parse(memory[3]);

            return metrics;
        }









        public static long GetTotalDiskSpace()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var path = "C:\\";
                // Creating a DriveInfo object to get information about the drive where the specified path is located.
                DriveInfo drive = new DriveInfo(Path.GetPathRoot(path));
                // Returning the total size of the drive in bytes.
                return drive.TotalSize;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var path = "/";
                var ps = new System.Diagnostics.Process();
                // Setting the filename or executable to be run by the process.
                ps.StartInfo.FileName = "/bin/df";
                // Setting the command-line arguments to be passed to the process.
                ps.StartInfo.Arguments = "-B 1 \"" + Path.GetPathRoot(path) + "\"";
                // Redirecting the standard output of the process so that it can be read programmatically.
                ps.StartInfo.RedirectStandardOutput = true;
                // Setting UseShellExecute to false to prevent the process from being started in a new window.
                ps.StartInfo.UseShellExecute = false;
                // Setting CreateNoWindow to true to prevent the creation of a window for the process.
                ps.StartInfo.CreateNoWindow = true;
                ps.Start();
                // Reading the output of the process until the end is reached.
                var output = ps.StandardOutput.ReadToEnd();
                ps.WaitForExit();
                // Trimming the output and splitting it into an array of lines.
                var lines = output.Trim().Split('\n');
                // Splitting the second line of the output into an array of substrings using space characters as the separator.
                var parts = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                // Parsing and returning the total disk space value from the parts array.
                return long.Parse(parts[1]);
            }
            else
            {
                // Throwing a NotSupportedException if the method is not supported on the current operating system.
                throw new NotSupportedException("This method is only supported on Windows and Linux.");
            }
        }

        /// <summary>
        /// gets available disk space
        /// </summary>
        /// <returns>long diskspace</returns>
        /// <exception cref="NotSupportedException"></exception>
        public static long GetAvailableDiskSpace()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var path = "C:\\";
                // creating a DriveInfo object to get information about the drive where the specified path is located.
                DriveInfo drive = new DriveInfo(Path.GetPathRoot(path));
                // Returning the available free space on the drive in bytes.
                return drive.AvailableFreeSpace;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var path = "/";
                var ps = new System.Diagnostics.Process();
                // Setting the filename or executable to be run by the process.
                ps.StartInfo.FileName = "/bin/df";
                // Setting the command-line arguments to be passed to the process.
                ps.StartInfo.Arguments = "-B 1 \"" + path + "\"";
                // Redirecting the standard output of the process so that it can be read programmatically.
                ps.StartInfo.RedirectStandardOutput = true;
                // Setting UseShellExecute to false to prevent the process from being started in a new window.
                ps.StartInfo.UseShellExecute = false;
                // Setting CreateNoWindow to true to prevent the creation of a window for the process.
                ps.StartInfo.CreateNoWindow = true;
                ps.Start();
                // Reading the output of the process until the end is reached.
                var output = ps.StandardOutput.ReadToEnd();
                ps.WaitForExit();
                // Trimming the output and splitting it into an array of lines.
                var lines = output.Trim().Split('\n');
                // Splitting the second line of the output into an array of substrings using space characters as the separator.
                var parts = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                // Parsing and returning the available disk space value from the parts array.
                return long.Parse(parts[3]);
            }
            else
            {
                // Throwing a NotSupportedException if the method is not supported on the current operating system.
                throw new NotSupportedException("This method is only supported on Windows and Linux.");
            }
        }


    }
}
