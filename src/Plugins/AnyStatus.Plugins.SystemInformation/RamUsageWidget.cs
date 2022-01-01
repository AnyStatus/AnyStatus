using AnyStatus.API.Widgets;
using MediatR;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace AnyStatus.Plugins.SystemInformation.OperatingSystem
{
    [Category("System Information")]
    [DisplayName("RAM Usage")]
    [Description("The total RAM usage (percentage)")]
    public class RamUsageWidget : MetricWidget, IPollable, IStandardWidget
    {
        public RamUsageWidget()
        {
            MinValue = 0;
            MaxValue = 100;
            Name = "RAM Usage";
        }

        public override string ToString() => Value.ToString("0\\%");
    }

    public class RamUsageQuery : RequestHandler<MetricRequest<RamUsageWidget>>
    {
        protected override void Handle(MetricRequest<RamUsageWidget> request)
        {
            request.Context.Value = (int)RamInformation.GetPercentageOfMemoryInUseMiB();
            request.Context.Status = Status.OK;
        }
    }

    public static class RamInformation
    {
        private const string PsapiDLL = "psapi.dll";
        private const int MegabyteFactor = 1024 * 1024;
        private static readonly int piSize = Marshal.SizeOf(new PerformanceInformation());

        [DllImport(PsapiDLL, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPerformanceInfo([Out] out PerformanceInformation performanceInformation, [In] int size);

        public static decimal GetPercentageOfMemoryInUseMiB()
        {
            if (!GetPerformanceInfo(out PerformanceInformation pi, piSize))
            {
                throw new Exception("An error occurred while getting performance information.");
            }

            var pageSize = pi.PageSize.ToInt64();
            var availableMemory = pi.PhysicalAvailable.ToInt64() * pageSize / MegabyteFactor;
            var totalMemory = pi.PhysicalTotal.ToInt64() * pageSize / MegabyteFactor;
            var usedMemory = totalMemory - availableMemory;

            return (decimal)usedMemory / totalMemory * 100;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }
    }
}
