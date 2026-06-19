using System;
using System.Collections.Generic;
using System.Reflection;

namespace kube.data
{
	public class DataUtils
	{
		public static KeyValuePair<int, int>[] StringToKwp(string par1)
		{
			string[] array = par1.Split(';');
			int num = array.Length / 2;
			KeyValuePair<int, int>[] array2 = new KeyValuePair<int, int>[num];
			int num2 = 0;
			int num3 = 0;
			while (num2 < array.Length)
			{
				array2[num3] = new KeyValuePair<int, int>(int.Parse(array[num2]), int.Parse(array[num2 + 1]));
				num2 += 2;
				num3++;
			}
			return array2;
		}

		public static object Clone(object obj)
		{
			object obj2 = Activator.CreateInstance(obj.GetType());
			FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				object obj3 = fieldInfo.GetValue(obj);
				if (obj3 is Array)
				{
					obj3 = ((ICloneable)obj3).Clone();
				}
				fieldInfo.SetValue(obj2, obj3);
			}
			return obj2;
		}

		public static int IntParseFast(string value)
		{
			int num = 0;
			foreach (char c in value)
			{
				if (c <= ':' && c >= '0')
				{
					num = 10 * num + (c - 48);
				}
			}
			return num;
		}
	}
}
