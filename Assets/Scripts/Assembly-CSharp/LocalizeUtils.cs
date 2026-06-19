using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace kube.data
{
	public class LocalizeUtils
	{
		public static int IntParseFast(string value)
		{
			int num = 0;
			foreach (char c in value)
			{
				if (c == ']')
				{
					break;
				}
				num = 10 * num + (c - 48);
			}
			return num;
		}

		private static string extractString(string str)
		{
			int length = str.Length;
			if (str == string.Empty)
			{
				return str;
			}
			if (str[0] == '"')
			{
				str = str.Substring(1, length - 2);
				str = str.Replace("\"\"", "\"");
			}
			return str;
		}

		public static void load(byte[] csv)
		{
			MemoryStream stream = new MemoryStream(csv);
			StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
			Localize.hash.Clear();
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			Type typeFromHandle = typeof(Localize);
			streamReader.ReadLine();
			while (!streamReader.EndOfStream)
			{
				string text = streamReader.ReadLine();
				int num = text.IndexOf(";");
				if (num == -1)
				{
					continue;
				}
				string[] array = new string[2]
				{
					string.Empty,
					string.Empty
				};
				array[0] = extractString(text.Substring(0, num));
				array[1] = extractString(text.Substring(num + 1));
				int num2 = array[0].IndexOf('[');
				int num3 = 0;
				array[1] = array[1].Replace("\\n", "\n");
				if (num2 != -1)
				{
					num3 = IntParseFast(array[0].Substring(num2 + 1));
					array[0] = array[0].Substring(0, num2);
				}
				FieldInfo field = typeFromHandle.GetField(array[0]);
				if (field == null)
				{
					if (Localize.hash.ContainsKey(array[0]))
					{
						Debug.LogError("Duplicate locale " + array[0]);
					}
					if (num2 == -1)
					{
						Localize.hash[array[0]] = array[1];
						continue;
					}
					Localize.hash[array[0] + "[" + num3 + "]"] = array[1];
				}
				else if (field.FieldType == typeof(string[]))
				{
					List<string> list;
					if (dictionary.ContainsKey(field.Name))
					{
						list = dictionary[field.Name];
					}
					else
					{
						list = new List<string>();
						dictionary[field.Name] = list;
					}
					while (list.Count <= num3)
					{
						list.Add(string.Empty);
					}
					if (array[1] != string.Empty)
					{
						list[num3] = array[1];
					}
				}
				else
				{
					field.SetValue(null, array[1]);
				}
			}
			foreach (string key in dictionary.Keys)
			{
				FieldInfo field2 = typeFromHandle.GetField(key);
				if (field2 == null)
				{
					continue;
				}
				string[] array2 = (string[])field2.GetValue(null);
				List<string> list2 = dictionary[key];
				if (array2 != null)
				{
					int num4 = Math.Min(array2.Length, list2.Count);
					for (int i = 0; i < num4; i++)
					{
						array2[i] = list2[i];
					}
					field2.SetValue(null, dictionary[key].ToArray());
				}
				else
				{
					field2.SetValue(null, dictionary[key].ToArray());
				}
			}
		}
	}
}
