using System;
using System.Runtime.InteropServices;

namespace AnyStatus.Plugins.SystemInformation.OperatingSystem
{
    internal class PowerStatus
    {
        private SYSTEM_POWER_STATUS _systemPowerStatus;

        public PowerLineStatus PowerLineStatus
        {
            get
            {
                UpdateSystemPowerStatus();
                return (PowerLineStatus)_systemPowerStatus.ACLineStatus;
            }
        }

        public BatteryChargeStatus BatteryChargeStatus
        {
            get
            {
                UpdateSystemPowerStatus();
                return (BatteryChargeStatus)_systemPowerStatus.BatteryFlag;
            }
        }

        public int BatteryFullLifetime
        {
            get
            {
                UpdateSystemPowerStatus();
                return _systemPowerStatus.BatteryFullLifeTime;
            }
        }

        public float BatteryLifePercent
        {
            get
            {
                UpdateSystemPowerStatus();
                float lifePercent = _systemPowerStatus.BatteryLifePercent / 100f;
                return lifePercent > 1f ? 1f : lifePercent;
            }
        }

        public int BatteryLifeRemaining
        {
            get
            {
                UpdateSystemPowerStatus();
                return _systemPowerStatus.BatteryLifeTime;
            }
        }

        private void UpdateSystemPowerStatus() => GetSystemPowerStatus(ref _systemPowerStatus);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern BOOL GetSystemPowerStatus(ref SYSTEM_POWER_STATUS lpSystemPowerStatus);
    }
    
    internal enum BOOL : int
    {
        FALSE = 0,
        TRUE = 1,
    }

    internal struct SYSTEM_POWER_STATUS
    {
        public byte ACLineStatus;
        public byte BatteryFlag;
        public byte BatteryLifePercent;
        public byte Reserved1;
        public int BatteryLifeTime;
        public int BatteryFullLifeTime;
    }

    internal enum PowerLineStatus
    {
        Offline = 0,
        Online = 1,
        Unknown = 255
    }

    [Flags]
    internal enum BatteryChargeStatus
    {
        High = 1,
        Low = 2,
        Critical = 4,
        Charging = 8,
        NoSystemBattery = 128,
        Unknown = 255
    }
}
