using System;

namespace Acidmanic.Utilities.Filtering.Utilities
{
    public static class NumericalCompare
    {
        public static int Compare(int x, int y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(long x, long y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(byte x, byte y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(sbyte x, sbyte y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(short x, short y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(decimal x, decimal y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(float x, float y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(ulong x, ulong y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(uint x, uint y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(char x, char y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static int Compare(Type type, object x, object y)
        {
            if (type == typeof(int))
            {
                return Compare((int)x, (int)y);
            }

            if (type == typeof(long))
            {
                return Compare((long)x, (long)y);
            }

            if (type == typeof(short))
            {
                return Compare((short)x, (short)y);
            }

            if (type == typeof(byte))
            {
                return Compare((byte)x, (byte)y);
            }

            if (type == typeof(sbyte))
            {
                return Compare((sbyte)x, (sbyte)y);
            }

            if (type == typeof(decimal))
            {
                return Compare((decimal)x, (decimal)y);
            }

            if (type == typeof(float))
            {
                return Compare((float)x, (float)y);
            }

            if (type == typeof(ulong))
            {
                return Compare((ulong)x, (ulong)y);
            }

            if (type == typeof(uint))
            {
                return Compare((uint)x, (uint)y);
            }

            if (type == typeof(char))
            {
                return Compare((char)x, (char)y);
            }

            if (type == typeof(Single))
            {
                return Compare((float)x, (float)y);
            }

            return 0;
        }
    }
}