using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class AuxFunc
{
	public static void CopyTo(Stream src, Stream dest)
	{
		byte[] array = new byte[4096];
		int count;
		while ((count = src.Read(array, 0, array.Length)) != 0)
		{
			dest.Write(array, 0, count);
		}
	}

	public static byte[] Zip(string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		using (MemoryStream src = new MemoryStream(bytes))
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream dest = new GZipStream(memoryStream, CompressionMode.Compress))
				{
					CopyTo(src, dest);
				}
				return memoryStream.ToArray();
			}
		}
	}

	public static string Unzip(byte[] bytes)
	{
		using (MemoryStream compressedStream = new MemoryStream(bytes))
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream src = new GZipStream(compressedStream, CompressionMode.Decompress))
				{
					CopyTo(src, memoryStream);
				}
				return Encoding.UTF8.GetString(memoryStream.ToArray());
			}
		}
	}

	public static string GetMD5(byte[] input)
	{
		MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(input);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public static string GetMD5(string input)
	{
		MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public static string CodeRussianName(string name)
	{
		if (name.Length == 0)
		{
			return string.Empty;
		}
		char[] separator = new char[4] { '_', '*', ';', '^' };
		string[] array = name.Split(separator);
		name = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			if (i != 0)
			{
				name += string.Empty;
			}
			name += array[i];
		}
		string text = string.Empty;
		for (int j = 0; j < name.Length; j++)
		{
			text = ((!(name.Substring(j, 1) == "А")) ? ((!(name.Substring(j, 1) == "Б")) ? ((!(name.Substring(j, 1) == "В")) ? ((!(name.Substring(j, 1) == "Г")) ? ((!(name.Substring(j, 1) == "Д")) ? ((!(name.Substring(j, 1) == "Е")) ? ((!(name.Substring(j, 1) == "Ё")) ? ((!(name.Substring(j, 1) == "Ж")) ? ((!(name.Substring(j, 1) == "З")) ? ((!(name.Substring(j, 1) == "И")) ? ((!(name.Substring(j, 1) == "Й")) ? ((!(name.Substring(j, 1) == "К")) ? ((!(name.Substring(j, 1) == "Л")) ? ((!(name.Substring(j, 1) == "М")) ? ((!(name.Substring(j, 1) == "Н")) ? ((!(name.Substring(j, 1) == "О")) ? ((!(name.Substring(j, 1) == "П")) ? ((!(name.Substring(j, 1) == "Р")) ? ((!(name.Substring(j, 1) == "С")) ? ((!(name.Substring(j, 1) == "Т")) ? ((!(name.Substring(j, 1) == "У")) ? ((!(name.Substring(j, 1) == "Ф")) ? ((!(name.Substring(j, 1) == "Х")) ? ((!(name.Substring(j, 1) == "Ц")) ? ((!(name.Substring(j, 1) == "Ч")) ? ((!(name.Substring(j, 1) == "Ш")) ? ((!(name.Substring(j, 1) == "Щ")) ? ((!(name.Substring(j, 1) == "Ъ")) ? ((!(name.Substring(j, 1) == "Ы")) ? ((!(name.Substring(j, 1) == "Ь")) ? ((!(name.Substring(j, 1) == "Э")) ? ((!(name.Substring(j, 1) == "Ю")) ? ((!(name.Substring(j, 1) == "Я")) ? ((!(name.Substring(j, 1) == "а")) ? ((!(name.Substring(j, 1) == "б")) ? ((!(name.Substring(j, 1) == "в")) ? ((!(name.Substring(j, 1) == "г")) ? ((!(name.Substring(j, 1) == "д")) ? ((!(name.Substring(j, 1) == "е")) ? ((!(name.Substring(j, 1) == "ё")) ? ((!(name.Substring(j, 1) == "ж")) ? ((!(name.Substring(j, 1) == "з")) ? ((!(name.Substring(j, 1) == "и")) ? ((!(name.Substring(j, 1) == "й")) ? ((!(name.Substring(j, 1) == "к")) ? ((!(name.Substring(j, 1) == "л")) ? ((!(name.Substring(j, 1) == "м")) ? ((!(name.Substring(j, 1) == "н")) ? ((!(name.Substring(j, 1) == "о")) ? ((!(name.Substring(j, 1) == "п")) ? ((!(name.Substring(j, 1) == "р")) ? ((!(name.Substring(j, 1) == "с")) ? ((!(name.Substring(j, 1) == "т")) ? ((!(name.Substring(j, 1) == "у")) ? ((!(name.Substring(j, 1) == "ф")) ? ((!(name.Substring(j, 1) == "х")) ? ((!(name.Substring(j, 1) == "ц")) ? ((!(name.Substring(j, 1) == "ч")) ? ((!(name.Substring(j, 1) == "ш")) ? ((!(name.Substring(j, 1) == "щ")) ? ((!(name.Substring(j, 1) == "ъ")) ? ((!(name.Substring(j, 1) == "ы")) ? ((!(name.Substring(j, 1) == "ь")) ? ((!(name.Substring(j, 1) == "э")) ? ((!(name.Substring(j, 1) == "ю")) ? ((!(name.Substring(j, 1) == "я")) ? ((!(name.Substring(j, 1) == " ")) ? (text + name[j]) : (text + "_73")) : (text + "_72")) : (text + "_71")) : (text + "_70")) : (text + "_69")) : (text + "_68")) : (text + "_67")) : (text + "_66")) : (text + "_65")) : (text + "_64")) : (text + "_63")) : (text + "_62")) : (text + "_61")) : (text + "_60")) : (text + "_59")) : (text + "_58")) : (text + "_57")) : (text + "_56")) : (text + "_55")) : (text + "_54")) : (text + "_53")) : (text + "_52")) : (text + "_51")) : (text + "_50")) : (text + "_49")) : (text + "_48")) : (text + "_47")) : (text + "_46")) : (text + "_45")) : (text + "_44")) : (text + "_43")) : (text + "_42")) : (text + "_41")) : (text + "_40")) : (text + "_32")) : (text + "_31")) : (text + "_30")) : (text + "_29")) : (text + "_28")) : (text + "_27")) : (text + "_26")) : (text + "_25")) : (text + "_24")) : (text + "_23")) : (text + "_22")) : (text + "_21")) : (text + "_20")) : (text + "_19")) : (text + "_18")) : (text + "_17")) : (text + "_16")) : (text + "_15")) : (text + "_14")) : (text + "_13")) : (text + "_12")) : (text + "_11")) : (text + "_10")) : (text + "_09")) : (text + "_08")) : (text + "_07")) : (text + "_06")) : (text + "_05")) : (text + "_04")) : (text + "_03")) : (text + "_02")) : (text + "_01")) : (text + "_00"));
		}
		return text;
	}

	public static string DecodeRussianName(string name)
	{
		string text = string.Empty;
		if (name == null)
		{
			return string.Empty;
		}
		for (int i = 0; i < name.Length; i++)
		{
			if (name[i] == '_' && name.Length > i + 2)
			{
				if (name[i + 1] == '0' && name[i + 2] == '0')
				{
					text += "А";
				}
				else if (name[i + 1] == '0' && name[i + 2] == '1')
				{
					text += "Б";
				}
				else if (name[i + 1] == '0' && name[i + 2] == '2')
				{
					text += "В";
				}
				else if (name[i + 1] == '0' && name[i + 2] == '3')
				{
					text += "Г";
				}
				else if (name[i + 1] == '0' && name[i + 2] == '4')
				{
					text += "Д";
				}
				else if (name[i + 1] == '0' && name[i + 2] == '5')
				{
					text += "Е";
				}
				else if (name[i + 1] == '0' && name[i + 2] == '6')
				{
					text += "Ё";
				}
				else if (name[i + 1] == '0' && name[i + 2] == '7')
				{
					text += "Ж";
				}
				else if (name[i + 1] == '0' && name[i + 2] == '8')
				{
					text += "З";
				}
				else if (name[i + 1] == '0' && name[i + 2] == '9')
				{
					text += "И";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '0')
				{
					text += "Й";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '1')
				{
					text += "К";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '2')
				{
					text += "Л";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '3')
				{
					text += "М";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '4')
				{
					text += "Н";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '5')
				{
					text += "О";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '6')
				{
					text += "П";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '7')
				{
					text += "Р";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '8')
				{
					text += "С";
				}
				else if (name[i + 1] == '1' && name[i + 2] == '9')
				{
					text += "Т";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '0')
				{
					text += "У";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '1')
				{
					text += "Ф";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '2')
				{
					text += "Х";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '3')
				{
					text += "Ц";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '4')
				{
					text += "Ч";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '5')
				{
					text += "Ш";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '6')
				{
					text += "Щ";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '7')
				{
					text += "Ъ";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '8')
				{
					text += "Ы";
				}
				else if (name[i + 1] == '2' && name[i + 2] == '9')
				{
					text += "Ь";
				}
				else if (name[i + 1] == '3' && name[i + 2] == '0')
				{
					text += "Э";
				}
				else if (name[i + 1] == '3' && name[i + 2] == '1')
				{
					text += "Ю";
				}
				else if (name[i + 1] == '3' && name[i + 2] == '2')
				{
					text += "Я";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '0')
				{
					text += "а";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '1')
				{
					text += "б";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '2')
				{
					text += "в";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '3')
				{
					text += "г";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '4')
				{
					text += "д";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '5')
				{
					text += "е";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '6')
				{
					text += "ё";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '7')
				{
					text += "ж";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '8')
				{
					text += "з";
				}
				else if (name[i + 1] == '4' && name[i + 2] == '9')
				{
					text += "и";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '0')
				{
					text += "й";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '1')
				{
					text += "к";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '2')
				{
					text += "л";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '3')
				{
					text += "м";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '4')
				{
					text += "н";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '5')
				{
					text += "о";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '6')
				{
					text += "п";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '7')
				{
					text += "р";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '8')
				{
					text += "с";
				}
				else if (name[i + 1] == '5' && name[i + 2] == '9')
				{
					text += "т";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '0')
				{
					text += "у";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '1')
				{
					text += "ф";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '2')
				{
					text += "х";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '3')
				{
					text += "ц";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '4')
				{
					text += "ч";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '5')
				{
					text += "ш";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '6')
				{
					text += "щ";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '7')
				{
					text += "ъ";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '8')
				{
					text += "ы";
				}
				else if (name[i + 1] == '6' && name[i + 2] == '9')
				{
					text += "ь";
				}
				else if (name[i + 1] == '7' && name[i + 2] == '0')
				{
					text += "э";
				}
				else if (name[i + 1] == '7' && name[i + 2] == '1')
				{
					text += "ю";
				}
				else if (name[i + 1] == '7' && name[i + 2] == '2')
				{
					text += "я";
				}
				else if (name[i + 1] == '7' && name[i + 2] == '3')
				{
					text += " ";
				}
				i += 2;
			}
			else
			{
				text += name[i];
			}
		}
		return text;
	}

	public static int RandomSelectWithChance(int[] arr)
	{
		int num = 0;
		int[] array = new int[arr.Length];
		int[] array2 = new int[arr.Length];
		for (int i = 0; i < arr.Length; i++)
		{
			num += arr[i];
		}
		for (int j = 0; j < arr.Length; j++)
		{
			if (arr[j] != 0)
			{
				array[j] = num - arr[j];
			}
		}
		num = 0;
		string text = "RamdomTeams = {";
		for (int k = 0; k < arr.Length; k++)
		{
			num = (array2[k] = num + array[k]);
			text = text + " " + array2[k];
		}
		text = text + " } sum=" + num;
		float num2 = Random.Range(0f, num);
		for (int l = 0; l < 4; l++)
		{
			if (arr[l] > 0)
			{
				float num3 = ((l != 0) ? array2[l - 1] : 0);
				float num4 = array2[l];
				if (num2 >= num3 && num2 <= num4)
				{
					return l;
				}
			}
		}
		return 0;
	}
	public static byte[] CreateEncryptLineForBytes(string line)
	{
		byte[] array = new byte[Encoding.UTF8.GetBytes(line).Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)(Encoding.UTF8.GetBytes(line)[i] + 150);
		}
		return array;
	}

	public static string DecodeEncryptLine(byte[] encryptedData)
	{
		byte[] array = new byte[encryptedData.Length];
		for (int i = 0; i < encryptedData.Length; i++)
		{
			array[i] = (byte)(encryptedData[i] - 150);
		}
		return Encoding.UTF8.GetString(array);
	}
}
