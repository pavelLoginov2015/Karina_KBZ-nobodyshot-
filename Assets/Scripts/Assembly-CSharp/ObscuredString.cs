using System;
using CodeStage.AntiCheat.Detectors;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public sealed class ObscuredString
	{
		private static string cryptoKey = "4441";

		[SerializeField]
		private string currentCryptoKey;

		[SerializeField]
		private byte[] hiddenValue;

		[SerializeField]
		private string fakeValue;

		[SerializeField]
		private bool inited;

		private ObscuredString()
		{
		}

		private ObscuredString(byte[] value)
		{
			currentCryptoKey = cryptoKey;
			hiddenValue = value;
			fakeValue = null;
			inited = true;
		}

		public static void SetNewCryptoKey(string newKey)
		{
			cryptoKey = newKey;
		}

		public void ApplyNewCryptoKey()
		{
			if (currentCryptoKey != cryptoKey)
			{
				hiddenValue = InternalEncrypt(InternalDecrypt());
				currentCryptoKey = cryptoKey;
			}
		}

		public static string EncryptDecrypt(string value)
		{
			return EncryptDecrypt(value, string.Empty);
		}

		public static string EncryptDecrypt(string value, string key)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(key))
			{
				key = cryptoKey;
			}
			int length = key.Length;
			int length2 = value.Length;
			char[] array = new char[length2];
			for (int i = 0; i < length2; i++)
			{
				array[i] = (char)(value[i] ^ key[i % length]);
			}
			return new string(array);
		}

		public string GetEncrypted()
		{
			ApplyNewCryptoKey();
			return GetString(hiddenValue);
		}

		public void SetEncrypted(string encrypted)
		{
			inited = true;
			hiddenValue = GetBytes(encrypted);
			if (ObscuredCheatingDetector.isRunning)
			{
				fakeValue = InternalDecrypt();
			}
		}

		private static byte[] InternalEncrypt(string value)
		{
			return GetBytes(EncryptDecrypt(value, cryptoKey));
		}

		private string InternalDecrypt()
		{
			if (!inited)
			{
				currentCryptoKey = cryptoKey;
				hiddenValue = InternalEncrypt(string.Empty);
				fakeValue = string.Empty;
				inited = true;
			}
			string text = cryptoKey;
			if (currentCryptoKey != cryptoKey)
			{
				text = currentCryptoKey;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = cryptoKey;
			}
			string text2 = EncryptDecrypt(GetString(hiddenValue), text);
			if (ObscuredCheatingDetector.isRunning && !string.IsNullOrEmpty(fakeValue) && text2 != fakeValue)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return text2;
		}

		public override string ToString()
		{
			return InternalDecrypt();
		}

		public override bool Equals(object obj)
		{
			ObscuredString obscuredString = obj as ObscuredString;
			string objB = null;
			if (obscuredString != null)
			{
				objB = GetString(obscuredString.hiddenValue);
			}
			return object.Equals(hiddenValue, objB);
		}

		public bool Equals(ObscuredString value)
		{
			byte[] a = null;
			if (value != null)
			{
				a = value.hiddenValue;
			}
			return ArraysEquals(hiddenValue, a);
		}

		public bool Equals(ObscuredString value, StringComparison comparisonType)
		{
			string b = null;
			if (value != null)
			{
				b = value.InternalDecrypt();
			}
			return string.Equals(InternalDecrypt(), b, comparisonType);
		}

		public override int GetHashCode()
		{
			return InternalDecrypt().GetHashCode();
		}

		private static byte[] GetBytes(string str)
		{
			byte[] array = new byte[str.Length * 2];
			Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
			return array;
		}

		private static string GetString(byte[] bytes)
		{
			char[] array = new char[bytes.Length / 2];
			Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
			return new string(array);
		}

		private static bool ArraysEquals(byte[] a1, byte[] a2)
		{
			if (a1 == a2)
			{
				return true;
			}
			if (a1 != null && a2 != null)
			{
				if (a1.Length != a2.Length)
				{
					return false;
				}
				for (int i = 0; i < a1.Length; i++)
				{
					if (a1[i] != a2[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static implicit operator ObscuredString(string value)
		{
			if (value == null)
			{
				return null;
			}
			ObscuredString obscuredString = new ObscuredString(InternalEncrypt(value));
			if (ObscuredCheatingDetector.isRunning)
			{
				obscuredString.fakeValue = value;
			}
			return obscuredString;
		}

		public static implicit operator string(ObscuredString value)
		{
			if (value == null)
			{
				return null;
			}
			return value.InternalDecrypt();
		}

		public static bool operator ==(ObscuredString a, ObscuredString b)
		{
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}
			if ((object)a == null || (object)b == null)
			{
				return false;
			}
			return ArraysEquals(a.hiddenValue, b.hiddenValue);
		}

		public static bool operator !=(ObscuredString a, ObscuredString b)
		{
			return !(a == b);
		}
	}
}
