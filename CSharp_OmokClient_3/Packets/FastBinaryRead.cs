using System;
using System.Text;

namespace OmokClient.Packets
{
    public class FastBinaryRead
    {
        public static unsafe ushort UInt16(byte[] bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(ushort*)(ptr + offset);
            }
        }
        public static unsafe ushort UInt16(ReadOnlySpan<byte> bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(ushort*)(ptr + offset);
            }
        }

    //     #region Timestamp/Duration      
    //     public static unsafe TimeSpan TimeSpan(ref byte[] bytes, int offset)
    //     {
    //         checked
    //         {
    //             fixed (byte* ptr = bytes)
    //             {
    //                 var seconds = *(long*)(ptr + offset);
    //                 var nanos = *(int*)(ptr + offset + 8);
    //
    //                 if (!Duration.IsNormalized(seconds, nanos))
    //                 {
    //                     throw new InvalidOperationException("Duration was not a valid normalized duration");
    //                 }
    //                 long ticks = seconds * System.TimeSpan.TicksPerSecond + nanos / Duration.NanosecondsPerTick;
    //                 return System.TimeSpan.FromTicks(ticks);
    //             }
    //         }
    //     }
    //
    //     public static unsafe DateTime DateTime(ref byte[] bytes, int offset)
    //     {
    //         fixed (byte* ptr = bytes)
    //         {
    //             var seconds = *(long*)(ptr + offset);
    //             var nanos = *(int*)(ptr + offset + 8);
    //
    //             if (!Timestamp.IsNormalized(seconds, nanos))
    //             {
    //                 throw new InvalidOperationException(string.Format(@"Timestamp contains invalid values: Seconds={0}; Nanos={1}", seconds, nanos));
    //             }
    //             return Timestamp.UnixEpoch.AddSeconds(seconds).AddTicks(nanos / Duration.NanosecondsPerTick);
    //         }
    //     }
    //
    //     internal static class Timestamp
    //     {
    //         internal static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    //         internal const long BclSecondsAtUnixEpoch = 62135596800;
    //         internal const long UnixSecondsAtBclMaxValue = 253402300799;
    //         internal const long UnixSecondsAtBclMinValue = -BclSecondsAtUnixEpoch;
    //         internal const int MaxNanos = Duration.NanosecondsPerSecond - 1;
    //
    //         internal static bool IsNormalized(long seconds, int nanoseconds)
    //         {
    //             return nanoseconds >= 0 &&
    //                 nanoseconds <= MaxNanos &&
    //                 seconds >= UnixSecondsAtBclMinValue &&
    //                 seconds <= UnixSecondsAtBclMaxValue;
    //         }
    //     }
    //
    //     internal static class Duration
    //     {
    //         public const int NanosecondsPerSecond = 1000000000;
    //         public const int NanosecondsPerTick = 100;
    //         public const long MaxSeconds = 315576000000L;
    //         public const long MinSeconds = -315576000000L;
    //         internal const int MaxNanoseconds = NanosecondsPerSecond - 1;
    //         internal const int MinNanoseconds = -NanosecondsPerSecond + 1;
    //
    //         internal static bool IsNormalized(long seconds, int nanoseconds)
    //         {
    //             // Simple boundaries
    //             if (seconds < MinSeconds || seconds > MaxSeconds ||
    //                 nanoseconds < MinNanoseconds || nanoseconds > MaxNanoseconds)
    //             {
    //                 return false;
    //             }
    //             // We only have a problem is one is strictly negative and the other is
    //             // strictly positive.
    //             return Math.Sign(seconds) * Math.Sign(nanoseconds) != -1;
    //         }
    //     }
    //     #endregion
    }
    
    // internal static class StringEncoding
    // {
    //     public static Encoding UTF8 = new UTF8Encoding(false);
    // }
}
