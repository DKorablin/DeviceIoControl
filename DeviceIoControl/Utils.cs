using System;
using System.Text;

namespace AlphaOmega.Debug
{
	/// <summary>Utils</summary>
	public struct Utils
	{
		private const Int64 FileSize = 1024;
		private static String[] FileSizeType = new String[] { "bytes", "Kb", "Mb", "Gb", };
		/// <summary>Swap odd/even bytes in array of USHORTS</summary>
		/// <param name="buffer">Buffer</param>
		/// <param name="start">Start position</param>
		/// <param name="count">Count</param>
		/// <returns>String</returns>
		public static String SwapBytes(UInt16[] buffer, UInt64 start, UInt64 count)
		{
			StringBuilder result = new StringBuilder();
			for(UInt64 loop = start;loop < start + count;loop++)
			{
				Byte[] part = BitConverter.GetBytes((buffer[loop] << 8 | buffer[loop] >> 8));
				String zz = Encoding.ASCII.GetString(part, 0, part.Length - 2);
				result.Append(zz);
			}

			return result.ToString();
		}
		/// <summary>Swap chars in IDREGS array</summary>
		/// <param name="buffer">Char buffer</param>
		/// <returns>Swapped string</returns>
		public static String SwapChars(Char[] buffer)
		{
			StringBuilder result = new StringBuilder();
			for(Int32 loop = 0; loop < buffer.Length; loop += 2)
			{
				result.Append(buffer[loop + 1]);
				result.Append(buffer[loop]);
			}
			return result.ToString();
		}
		/// <summary>Find highest non-zero bit position</summary>
		/// <param name="value">Value</param>
		/// <param name="maxPos">highest non-zero bit position</param>
		/// <returns>Bit position</returns>
		public static Int64 HighBitPos(Int64 value, Int32 maxPos)
		{
			Int32 pos;
			for(pos = maxPos;!(pos < 0 || ((value >> pos) & 1) == 1);pos--) ;
			return pos;
		}
		/// <summary>Makes a 32 bit integer from two 16 bit shorts</summary>
		/// <param name="low">The low order value.</param>
		/// <param name="high">The high order value.</param>
		/// <returns></returns>
		public static Int32 MakeDword(UInt16 low, UInt16 high)
		{
			return (low + (high << 16));
		}
		/// <summary>Convert file size in bytes to string with dimention</summary>
		/// <param name="length">size in bytes</param>
		/// <returns>Size with dimention</returns>
		public static String FileSizeToString(UInt64 length)
		{
			UInt64 constSize = 1;
			Int32 sizePosition = 0;
			while(length > constSize * FileSize && sizePosition + 1 < Utils.FileSizeType.Length)
			{
				constSize *= Utils.FileSize;
				sizePosition++;
			}
			return String.Format("{0:n0} {1}", length / constSize, Utils.FileSizeType[sizePosition]);
		}
	}
}