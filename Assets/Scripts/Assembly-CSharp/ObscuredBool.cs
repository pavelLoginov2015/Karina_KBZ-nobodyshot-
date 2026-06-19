using System;
using CodeStage.AntiCheat.Detectors;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredBool : IEquatable<ObscuredBool>
	{
		private static byte cryptoKey = 215;

		[SerializeField]
		private byte currentCryptoKey;

		[SerializeField]
		private int hiddenValue;

		[SerializeField]
		private bool fakeValue;

		[SerializeField]
		private bool fakeValueChanged;

		[SerializeField]
		private bool inited;

		private ObscuredBool(int value)
		{
			currentCryptoKey = cryptoKey;
			hiddenValue = value;
			fakeValue = false;
			fakeValueChanged = false;
			inited = true;
		}

		public static void SetNewCryptoKey(byte newKey)
		{
			cryptoKey = newKey;
		}

		public void ApplyNewCryptoKey()
		{
			if (currentCryptoKey != cryptoKey)
			{
				hiddenValue = Encrypt(InternalDecrypt(), cryptoKey);
				currentCryptoKey = cryptoKey;
			}
		}

		public static int Encrypt(bool value)
		{
			return Encrypt(value, 0);
		}

		public static int Encrypt(bool value, byte key)
		{
			if (key == 0)
			{
				key = cryptoKey;
			}
			int num = ((!value) ? 181 : 213);
			return num ^ key;
		}

		public static bool Decrypt(int value)
		{
			return Decrypt(value, 0);
		}

		public static bool Decrypt(int value, byte key)
		{
			if (key == 0)
			{
				key = cryptoKey;
			}
			value ^= key;
			return value != 181;
		}

		public int GetEncrypted()
		{
			ApplyNewCryptoKey();
			return hiddenValue;
		}

		public void SetEncrypted(int encrypted)
		{
			inited = true;
			hiddenValue = encrypted;
			if (ObscuredCheatingDetector.isRunning)
			{
				fakeValue = InternalDecrypt();
				fakeValueChanged = true;
			}
		}

		private bool InternalDecrypt()
		{
			if (!inited)
			{
				currentCryptoKey = cryptoKey;
				hiddenValue = Encrypt(false);
				fakeValue = false;
				fakeValueChanged = true;
				inited = true;
			}
			byte b = cryptoKey;
			if (currentCryptoKey != cryptoKey)
			{
				b = currentCryptoKey;
			}
			int num = hiddenValue;
			num ^= b;
			bool flag = num != 181;
			if (ObscuredCheatingDetector.isRunning && fakeValueChanged && flag != fakeValue)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return flag;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ObscuredBool))
			{
				return false;
			}
			ObscuredBool obscuredBool = (ObscuredBool)obj;
			return hiddenValue == obscuredBool.hiddenValue;
		}

		public bool Equals(ObscuredBool obj)
		{
			return hiddenValue == obj.hiddenValue;
		}

		public override int GetHashCode()
		{
			return InternalDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InternalDecrypt().ToString();
		}

		public static implicit operator ObscuredBool(bool value)
		{
			ObscuredBool result = new ObscuredBool(Encrypt(value));
			if (ObscuredCheatingDetector.isRunning)
			{
				result.fakeValue = value;
				result.fakeValueChanged = true;
			}
			return result;
		}

		public static implicit operator bool(ObscuredBool value)
		{
			return value.InternalDecrypt();
		}
	}
}
