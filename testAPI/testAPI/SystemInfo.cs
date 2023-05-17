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

    public class SystemInfo
    {


        //dit is een testbranch

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern UInt64 GetTickCount64();
        public static int GetCpuCores()
        {
            return Environment.ProcessorCount;
        }

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
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"top -bn1 | grep 'Cpu(s)' | sed 's/.*, *\\([0-9.]*\\)%* id.*/\\1/'\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var process = Process.Start(psi);
                process.WaitForExit();
                var output = process.StandardOutput.ReadToEnd();
                var cpuUsage = double.Parse(output.Trim());
                return Math.Round(100 - cpuUsage, 2);
            }
            else
            {
                throw new PlatformNotSupportedException($"The current platform '{RuntimeInformation.OSDescription}' is not supported.");
            }
        }

        public static SystemInfoDataModel GetSystemInfo()
        {
            var m = new MemoryMetricsClient();
            SystemInfoDataModel data = new SystemInfoDataModel();
            data.TotalMemory = m.GetMetrics().Total;
            data.UsedMemory = m.GetMetrics().Used;
            data.CpuCores = GetCpuCores();
            data.CpuUsage = GetCpuUsage();
            data.TotalDisk = (MemoryMetricsClient.GetTotalDiskSpace() / 1024 / 1024);
            data.DiskFree = MemoryMetricsClient.GetAvailableDiskSpace() / 1024 / 1024;
            data.ObjectDetection = false;
            data.TextToSpeech = false;
            data.RuntimeMinutes = SystemInfo.GetRuntimeMinutes();
            data.ServiceStatus = "";
            return data;
        }
        public static string GetServiceStatus()
        {

            var data = GetServices();
            return data;
        }

        public static string GetServices()
        {

            if (MemoryMetricsClient.IsUnix())
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

        public static string GetRuntimeMinutes()
        {
            string runtimeMinutes = "0";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows
                var uptime = GetTickCount64();
                runtimeMinutes += TimeSpan.FromMilliseconds(uptime).TotalMinutes;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
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
    }
}