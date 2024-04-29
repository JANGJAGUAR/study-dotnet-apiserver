namespace OmokClient.Packets
{
    public class FastBinaryWrite
    {
        public static unsafe int UInt16(byte[] bytes, int offset, ushort value)
        {
            fixed (byte* ptr = bytes)
            {
                *(ushort*)(ptr + offset) = value;
            }

            return 2;
        }
        
        // #region Timestamp/Duration
        // public static unsafe int TimeSpan(ref byte[] bytes, int offset, TimeSpan timeSpan)
        // {
        //     checked
        //     {
        //         long ticks = timeSpan.Ticks;
        //         long seconds = ticks / System.TimeSpan.TicksPerSecond;
        //         int nanos = (int)(ticks % System.TimeSpan.TicksPerSecond) * Duration.NanosecondsPerTick;
        //
        //         fixed (byte* ptr = bytes)
        //         {
        //             *(long*)(ptr + offset) = seconds;
        //             *(int*)(ptr + offset + 8) = nanos;
        //         }
        //
        //         return 12;
        //     }
        // }
        //
        // public static unsafe int DateTime(ref byte[] bytes, int offset, DateTime dateTime)
        // {
        //     dateTime = dateTime.ToUniversalTime();
        //
        //     // Do the arithmetic using DateTime.Ticks, which is always non-negative, making things simpler.
        //     long secondsSinceBclEpoch = dateTime.Ticks / System.TimeSpan.TicksPerSecond;
        //     int nanoseconds = (int)(dateTime.Ticks % System.TimeSpan.TicksPerSecond) * Duration.NanosecondsPerTick;
        //
        //     fixed (byte* ptr = bytes)
        //     {
        //         *(long*)(ptr + offset) = (secondsSinceBclEpoch - Timestamp.BclSecondsAtUnixEpoch);
        //         *(int*)(ptr + offset + 8) = nanoseconds;
        //     }
        //
        //     return 12;
        // }
        //
        // internal static class Timestamp
        // {
        //     internal static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //     internal const long BclSecondsAtUnixEpoch = 62135596800;
        //     internal const long UnixSecondsAtBclMaxValue = 253402300799;
        //     internal const long UnixSecondsAtBclMinValue = -BclSecondsAtUnixEpoch;
        //     internal const int MaxNanos = Duration.NanosecondsPerSecond - 1;
        //
        //     internal static bool IsNormalized(long seconds, int nanoseconds)
        //     {
        //         return nanoseconds >= 0 &&
        //             nanoseconds <= MaxNanos &&
        //             seconds >= UnixSecondsAtBclMinValue &&
        //             seconds <= UnixSecondsAtBclMaxValue;
        //     }
        // }
        //
        // internal static class Duration
        // {
        //     public const int NanosecondsPerSecond = 1000000000;
        //     public const int NanosecondsPerTick = 100;
        //     public const long MaxSeconds = 315576000000L;
        //     public const long MinSeconds = -315576000000L;
        //     internal const int MaxNanoseconds = NanosecondsPerSecond - 1;
        //     internal const int MinNanoseconds = -NanosecondsPerSecond + 1;
        //
        //     internal static bool IsNormalized(long seconds, int nanoseconds)
        //     {
        //         // Simple boundaries
        //         if (seconds < MinSeconds || seconds > MaxSeconds ||
        //             nanoseconds < MinNanoseconds || nanoseconds > MaxNanoseconds)
        //         {
        //             return false;
        //         }
        //         // We only have a problem is one is strictly negative and the other is
        //         // strictly positive.
        //         return Math.Sign(seconds) * Math.Sign(nanoseconds) != -1;
        //     }
        // }
        // #endregion
    }
}
