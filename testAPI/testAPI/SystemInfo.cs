namespace testAPI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Management;

    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using global::testAPI.Models;
    using System.Globalization;

    public class SystemInfo
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern UInt64 GetTickCount64();

        /// <summary>
        /// gets the cpu cores of the pc and returns the number as an int
        /// </summary>
        /// <returns>CPU Core Count Int</returns>
        public static int GetCpuCores()
        {
            return Environment.ProcessorCount;
        }


        /// <summary>
        /// returns the cpu usage as a double
        /// </summary>
        /// <returns>cpu usage as Double</returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static double GetCpuUsage()
        {
            double systemUsage = 0;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cpuCounter = Process.GetCurrentProcess().TotalProcessorTime;
                var systemCounter = new ManagementObjectSearcher("SELECT PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'").Get().GetEnumerator();
                while (systemCounter.MoveNext())
                {
                    systemUsage = Convert.ToDouble((UInt64)(systemCounter.Current["PercentProcessorTime"]));
                    break; // get the first result
                }
                return Math.Round(systemUsage / Environment.ProcessorCount / cpuCounter.TotalSeconds * 100, 2);
            }
            else if (SystemInfo.IsUnix())
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"top -bn1 | awk '/^%Cpu/{print $2}'\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var process = Process.Start(psi);
                process.WaitForExit();
                var output = process.StandardOutput.ReadToEnd();
                var cpuUsage = double.Parse(output.Trim());
                return Math.Round(cpuUsage/10);
            }
            else
            {
                throw new PlatformNotSupportedException($"The current platform '{RuntimeInformation.OSDescription}' is not supported.");
            }
        }

        /// <summary>
        /// returns system info and combines several functions of several classes to return the full data object
        /// </summary>
        /// <returns>sysinfo data object SystemInfoDataModel</returns>
        public static SystemInfoDataModel GetSystemInfo()
        {
            var m = new MemoryMetrics();
            SystemInfoDataModel data = new SystemInfoDataModel();
            data.TotalMemory = m.GetMetrics().Total;
            data.UsedMemory = m.GetMetrics().Used;
            data.CpuCores = GetCpuCores();
            data.CpuUsage = GetCpuUsage();
            data.TotalDisk = (MemoryMetrics.GetTotalDiskSpace() / 1024 / 1024);
            data.DiskFree = (MemoryMetrics.GetAvailableDiskSpace() / 1024 / 1024);
            data.ObjectDetection = false;
            data.TextToSpeech = false;
            data.RuntimeMinutes = SystemInfo.GetRuntimeMinutes();
            data.ServiceStatus = "";
            data.DiskInput = 0;
            data.DiskOutput = 0;
            return data;
        }


        /// <summary>
        /// gets the services installed on the linux system and shows wether they are enabled or not
        /// </summary>
        /// <returns></returns>

        public static string GetServiceStatus()
        {

            if (SystemInfo.IsUnix())
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = "service  --status-all", RedirectStandardOutput = true };
                    Process proc = new Process() { StartInfo = startInfo };
                    proc.Start();
                    string output = proc.StandardOutput.ReadToEnd();
                    return output;
                }
                catch (Exception e)
                {
                    return "feature is linux only; :" + e.ToString();
                }
            }
            else
            {
                return "feature is linux only";
            }
        }


        public static string GetNodeList()
        {
            if (SystemInfo.IsUnix())
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo()
                    {
                        FileName = "ros2",
                        Arguments = "node list",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    Process proc = new Process() { StartInfo = startInfo };
                    proc.Start();
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();

                    return output;
                }
                catch (Exception e)
                {
                    return "Failed to execute command: " + e.ToString();
                }
            }
            else
            {
                return "This feature is only available on Unix-like systems.";
            }
        }



        /// <summary>
        /// gets the runtime of the OS
        /// </summary>
        /// <returns>runtime of OS in string</returns>
        /// <exception cref="Exception"></exception>
        public static string GetRuntimeMinutes()
        {
            string runtimeMinutes = "0";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows
                var uptime = GetTickCount64();
                runtimeMinutes += TimeSpan.FromMilliseconds(uptime).TotalMinutes;
            }
            else if (SystemInfo.IsUnix())
            {
                // Linux or macOS
                var uptimeProcess = new Process();
                uptimeProcess.StartInfo.FileName = "/usr/bin/uptime";
                uptimeProcess.StartInfo.Arguments = "-p";
                uptimeProcess.StartInfo.UseShellExecute = false;
                uptimeProcess.StartInfo.RedirectStandardOutput = true;
                uptimeProcess.Start();
                var output = uptimeProcess.StandardOutput.ReadToEnd();
                uptimeProcess.WaitForExit();
                if (uptimeProcess.ExitCode != 0)
                {
                    throw new Exception("Command exited with non-zero status code: " + uptimeProcess.ExitCode);
                }
                return output;
            }
            else
            {
                // Unsupported platform
                throw new Exception("Unsupported platform: " + RuntimeInformation.OSDescription);
            }
            return runtimeMinutes;
        }

        /// <summary>
        /// gets the current disk Usage of the pc
        /// </summary>
        /// <returns>Disk IO</returns>
        public static double[] GetDiskIO()
        {
            double[] diskIO = new double[2]; 

            if (SystemInfo.IsUnix())
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "sudo",
                    Arguments = "/usr/sbin/iotop -b -n1 -qqq",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = Process.Start(psi);
                process.WaitForExit();
                var output = process.StandardOutput.ReadToEnd();

                var lines = output.Split("\n");

                foreach (var line in lines)
                {
                    if (line.StartsWith("Total DISK READ:"))
                    {
                        var parts = line.Split(new[] { "B/s" }, StringSplitOptions.RemoveEmptyEntries);
                        var diskReadValue = parts[0].Split(':')[1].Trim();
                        diskIO[0] = double.Parse(diskReadValue);
                    }
                    else if (line.StartsWith("Total DISK WRITE:"))
                    {
                        var parts = line.Split(new[] { "B/s" }, StringSplitOptions.RemoveEmptyEntries);
                        var diskWriteValue = parts[0].Split(':')[1].Trim();
                        diskIO[1] = double.Parse(diskWriteValue);
                    }
                }
            }

            return diskIO;
        }
        /// <summary>
        /// finds out wether the platform is unix based
        /// </summary>
        /// <returns>Bool</returns>

        public static bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                         RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            return isUnix;
        }
    }
}