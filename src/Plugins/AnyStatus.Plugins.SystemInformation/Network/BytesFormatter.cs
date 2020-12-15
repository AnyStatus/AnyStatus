using System.Runtime.InteropServices;
using System.Text;

namespace AnyStatus.Plugins.SystemInformation.Network
{
    public static class BytesFormatter
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern long StrFormatByteSizeW(long qdw, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszBuf, int cchBuf);

        public static string Format(long bytes)
        {
            var sb = new StringBuilder(32);

            StrFormatByteSizeW(bytes, sb, sb.Capacity);

            return sb.ToString();
        }
    }
}
