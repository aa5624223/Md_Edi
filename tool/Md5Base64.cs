using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Md_Edi.tool
{
	public class Md5Base64
	{
		public static string encode(String str)
		{
			Base64Encoder base64 = new Base64Encoder();
			var ss = (byte[])MD5.md5(str, true, Encoding.UTF8);
			return base64.GetEncoded((byte[])MD5.md5(str, true, Encoding.Unicode));

		}

	}
	public class MD5
	{
		// 格式化md5 hash 字节数组所用的格式（两位小写16进制数字） 
		private static readonly string m_strHexFormat = "x2";
		private MD5() { }
		/// <summary> 
		/// 使用当前缺省的字符编码对字符串进行加密 
		/// </summary> 
		/// <param name="str">需要进行md5演算的字符串</param> 
		/// <returns>用小写字母表示的32位16进制数字字符串</returns> 
		public static string md5(string str)
		{
			return (string)md5(str, false, Encoding.Default);
		}
		/// <summary> 
		/// 对字符串进行md5 hash计算 
		/// </summary> 
		/// <param name="str">需要进行md5演算的字符串</param> 
		/// <param name="raw_output"> 
		/// false则返回经过格式化的加密字符串(等同于 string md5(string) ) 
		/// true则返回原始的md5 hash 长度16 的 byte[] 数组 
		/// </param> 
		/// <returns> 
		/// byte[] 数组或者经过格式化的 string 字符串 
		/// </returns> 
		public static object md5(string str, bool raw_output)
		{
			return md5(str, raw_output, Encoding.Default);
		}
		/// <summary> 
		/// 对字符串进行md5 hash计算 
		/// </summary> 
		/// <param name="str">需要进行md5演算的字符串</param> 
		/// <param name="raw_output"> 
		/// false则返回经过格式化的加密字符串(等同于 string md5(string) ) 
		/// true则返回原始的md5 hash 长度16 的 byte[] 数组 
		/// </param> 
		/// <param name="charEncoder"> 
		/// 用来指定对输入字符串进行编解码的 Encoding 类型， 
		/// 当输入字符串中包含多字节文字（比如中文）的时候 
		/// 必须保证进行匹配的 md5 hash 所使用的字符编码相同， 
		/// 否则计算出来的 md5 将不匹配。 
		/// </param> 
		/// <returns> 
		/// byte[] 数组或者经过格式化的 string 字符串 
		/// </returns> 
		public static object md5(string str, bool raw_output,
													Encoding charEncoder)
		{
			if (!raw_output)
				return md5str(str, charEncoder);
			else
				return md5raw(str, charEncoder);
		}

		/// <summary> 
		/// 使用当前缺省的字符编码对字符串进行加密 
		/// </summary> 
		/// <param name="str">需要进行md5演算的字符串</param> 
		/// <returns>用小写字母表示的32位16进制数字字符串</returns> 
		protected static string md5str(string str)
		{
			return md5str(str, Encoding.Default);
		}
		/// <summary> 
		/// 对字符串进行md5加密 
		/// </summary> 
		/// <param name="str">需要进行md5演算的字符串</param> 
		/// <param name="charEncoder"> 
		/// 指定对输入字符串进行编解码的 Encoding 类型 
		/// </param> 
		/// <returns>用小写字母表示的32位16进制数字字符串</returns> 
		protected static string md5str(string str, Encoding charEncoder)
		{
			byte[] bytesOfStr = md5raw(str, charEncoder);
			int bLen = bytesOfStr.Length;
			StringBuilder pwdBuilder = new StringBuilder(32);
			for (int i = 0; i < bLen; i++)
			{
				pwdBuilder.Append(bytesOfStr[i].ToString(m_strHexFormat));
			}
			return pwdBuilder.ToString();
		}
		/// <summary> 
		/// 使用当前缺省的字符编码对字符串进行加密 
		/// </summary> 
		/// <param name="str">需要进行md5演算的字符串</param> 
		/// <returns>长度16 的 byte[] 数组</returns> 
		protected static byte[] md5raw(string str)
		{
			return md5raw(str, Encoding.Default);
		}
		/// <summary> 
		/// 对字符串进行md5加密 
		/// </summary> 
		/// <param name="str">需要进行md5演算的字符串</param> 
		/// <param name="charEncoder"> 
		/// 指定对输入字符串进行编解码的 Encoding 类型 
		/// </param> 
		/// <returns>长度16 的 byte[] 数组</returns> 
		protected static byte[] md5raw(string str, Encoding charEncoder)
		{
			System.Security.Cryptography.MD5 md5 =
				System.Security.Cryptography.MD5.Create();
			return md5.ComputeHash(charEncoder.GetBytes(str));
		}
	}
	/// <summary>
	/// Base64编码类。
	/// 将byte[]类型转换成Base64编码的string类型。
	/// </summary>
	public class Base64Encoder
	{
		byte[] source;
		int length, length2;
		int blockCount;
		int paddingCount;
		public static Base64Encoder Encoder = new Base64Encoder();

		public Base64Encoder()
		{
		}

		private void init(byte[] input)
		{
			source = input;
			length = input.Length;
			if ((length % 3) == 0)
			{
				paddingCount = 0;
				blockCount = length / 3;
			}
			else
			{
				paddingCount = 3 - (length % 3);
				blockCount = (length + paddingCount) / 3;
			}
			length2 = length + paddingCount;
		}

		public string GetEncoded(byte[] input)
		{
			//初始化
			init(input);

			byte[] source2;
			source2 = new byte[length2];

			for (int x = 0; x < length2; x++)
			{
				if (x < length)
				{
					source2[x] = source[x];
				}
				else
				{
					source2[x] = 0;
				}
			}

			byte b1, b2, b3;
			byte temp, temp1, temp2, temp3, temp4;
			byte[] buffer = new byte[blockCount * 4];
			char[] result = new char[blockCount * 4];
			for (int x = 0; x < blockCount; x++)
			{
				b1 = source2[x * 3];
				b2 = source2[x * 3 + 1];
				b3 = source2[x * 3 + 2];

				temp1 = (byte)((b1 & 252) >> 2);

				temp = (byte)((b1 & 3) << 4);
				temp2 = (byte)((b2 & 240) >> 4);
				temp2 += temp;

				temp = (byte)((b2 & 15) << 2);
				temp3 = (byte)((b3 & 192) >> 6);
				temp3 += temp;

				temp4 = (byte)(b3 & 63);

				buffer[x * 4] = temp1;
				buffer[x * 4 + 1] = temp2;
				buffer[x * 4 + 2] = temp3;
				buffer[x * 4 + 3] = temp4;

			}

			for (int x = 0; x < blockCount * 4; x++)
			{
				result[x] = sixbit2char(buffer[x]);
			}


			switch (paddingCount)
			{
				case 0: break;
				case 1: result[blockCount * 4 - 1] = '='; break;
				case 2:
					result[blockCount * 4 - 1] = '=';
					result[blockCount * 4 - 2] = '=';
					break;
				default: break;
			}
			return new string(result);
		}
		private char sixbit2char(byte b)
		{
			char[] lookupTable = new char[64]{
				  'A','B','C','D','E','F','G','H','I','J','K','L','M',
				 'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
				 'a','b','c','d','e','f','g','h','i','j','k','l','m',
				 'n','o','p','q','r','s','t','u','v','w','x','y','z',
				 '0','1','2','3','4','5','6','7','8','9','+','/'};

			if ((b >= 0) && (b <= 63))
			{
				return lookupTable[(int)b];
			}
			else
			{

				return ' ';
			}
		}
	}
}