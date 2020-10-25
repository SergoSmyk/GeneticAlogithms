using System;
using System.Text;


namespace Labs.api
{
    class Tools
    {
        public delegate String Converter<T>(T item);

        public static Converter<byte> ByteConverter = new Converter<byte>(byteConverter);

        private static String byteConverter(byte item)
        {
            return Convert.ToString(item, 2).PadLeft(8, '0');
        }

        public static String arrToString<T>(T[] arr, Converter<T> itemConverter = null)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('[');
            foreach (T item in arr)
            {
                String sItem = itemConverter?.Invoke(item) ?? item.ToString();

                stringBuilder.Append(sItem);
                stringBuilder.Append(',');
            }

            var currLength = stringBuilder.Length;
            stringBuilder.Remove(currLength - 1, 1);
            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        public static long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            return (Math.Abs(longRand % (max - min + 1)) + min);
        }

        public static double DoubleRandom(double min, double max, Random rand)
        {
            return rand.NextDouble() * (max - min) + min;
        }

        public static long mergeTwoValues(long a, long b, Random rand, long min, long max)
        {
            byte bitCount = 8 * 8;
            byte[] firstParentBytes = BitConverter.GetBytes(Math.Abs(a));
            byte[] secondParentBytes = BitConverter.GetBytes(Math.Abs(b));

            byte[] childBytes = new byte[8];
            long childValue;

            int separatorPosition = rand.Next(0, bitCount);
            int countOfFirstParentBytes = separatorPosition / 8;
            int currChildByteIndex = 0;
            copyBytesFromParent(firstParentBytes, childBytes, ref currChildByteIndex, countOfFirstParentBytes);

            childBytes[currChildByteIndex] = Tools.byteCombiner(
                first: firstParentBytes[currChildByteIndex],
                second: secondParentBytes[currChildByteIndex],
                separatorPosition: (byte)(separatorPosition % 8));

            currChildByteIndex++;
            copyBytesFromParent(secondParentBytes, childBytes, ref currChildByteIndex);
            int signMultipler = Math.Sign(a);
            if (rand.NextDouble() > 0.5)
            {
                signMultipler = Math.Sign(b);
            }
            childValue = signMultipler * BitConverter.ToInt64(childBytes, 0);

            if (childValue > max)
            {
                childValue = max;
            }
            else if (childValue < min)
            {
                childValue = min;
            }

            return childValue;
        }

        public static long mutateValue(long value, Random rand, long min, long max)
        {
            long mutatedValue;

            byte[] childBytes = BitConverter.GetBytes(Math.Abs(value));

            int bitIndex = rand.Next(0, 64);
            int childByteIndex = bitIndex / 8;
            Tools.singleBitInverter(
                item: ref childBytes[childByteIndex],
                bitPosition: bitIndex % 8);

            int signMultipler = 1;
            if (rand.NextDouble() < 0.5)
            {
                signMultipler = -1;
            }
            mutatedValue = signMultipler * BitConverter.ToInt64(childBytes, 0);

            if (mutatedValue > max)
            {
                mutatedValue = max;
            }
            else if (mutatedValue < min)
            {
                mutatedValue = min;
            }

            return mutatedValue;
        }

        private static void copyBytesFromParent(byte[] parent, byte[] child, ref int index, int count = int.MaxValue)
        {
            for (int i = index; i < Math.Min(count, parent.Length); i++)
            {
                child[index] = parent[index];
                index++;
            }
        }

        public static byte byteCombiner(byte first, byte second, int separatorPosition)
        {
            if (0 > separatorPosition || 8 <= separatorPosition)
            {
                throw new Exception("Wrong separator index");
            }
            int inverSepPos = 8 - separatorPosition;
            byte separatorByte = (byte)(255 >> inverSepPos << inverSepPos);
            byte invertedSeparatorByte = (byte)~separatorByte;

            byte answer = (byte)((first & separatorByte) | (second & invertedSeparatorByte));
            //Log.console("First:             {0} = {1}", Convert.ToString(first, 2).PadLeft(8, '0'), first);
            //Log.console("Second:            {0} = {1}", Convert.ToString(second, 2).PadLeft(8, '0'), second);
            //Log.console("SeparatorPosition: {0}", separatorPosition);
            //Log.console("Separator:         {0}", Convert.ToString(separatorByte, 2).PadLeft(8, '0'));
            //Log.console("InvertedSeparator: {0}", Convert.ToString(invertedSeparatorByte, 2).PadLeft(8, '0'));
            //Log.console("Part1:             {0}", Convert.ToString((first & separatorByte), 2).PadLeft(8, '0'));
            //Log.console("Part2:             {0}", Convert.ToString((second & invertedSeparatorByte), 2).PadLeft(8, '0'));
            //Log.console("Answer:            {0} = {1}", Convert.ToString(answer, 2), answer);
            return answer;
        }

        public static void singleBitInverter(ref byte item, int bitPosition)
        {
            byte invertor = (byte)(1 << (7 - bitPosition));
            item ^= invertor;
        }
    }
}
