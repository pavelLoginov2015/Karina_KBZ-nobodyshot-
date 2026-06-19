using System;
using CodeStage.AntiCheat.Detectors;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredUInt : IEquatable<ObscuredUInt>, IFormattable
	{
		private static uint cryptoKey = 240513u;

		private uint currentCryptoKey;

		private uint hiddenValue;

		private uint fakeValue;

		private bool inited;

		private ObscuredUInt(uint value)
		{
			currentCryptoKey = cryptoKey;
			hiddenValue = value;
			fakeValue = 0u;
			inited = true;
		}

		public static void SetNewCryptoKey(uint newKey)
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

		public static uint Encrypt(uint value)
		{
			return Encrypt(value, 0u);
		}

		public static uint Decrypt(uint value)
		{
			return Decrypt(value, 0u);
		}

		public static uint Encrypt(uint value, uint key)
		{
			if (key == 0)
			{
				return value ^ cryptoKey;
			}
			return value ^ key;
		}

		public static uint Decrypt(uint value, uint key)
		{
			if (key == 0)
			{
				return value ^ cryptoKey;
			}
			return value ^ key;
		}

		public uint GetEncrypted()
		{
			ApplyNewCryptoKey();
			return hiddenValue;
		}

		public void SetEncrypted(uint encrypted)
		{
			inited = true;
			hiddenValue = encrypted;
			if (ObscuredCheatingDetector.isRunning)
			{
				fakeValue = InternalDecrypt();
			}
		}

		private uint InternalDecrypt()
		{
			if (!inited)
			{
				currentCryptoKey = cryptoKey;
				hiddenValue = Encrypt(0u);
				fakeValue = 0u;
				inited = true;
			}
			uint key = cryptoKey;
			if (currentCryptoKey != cryptoKey)
			{
				key = currentCryptoKey;
			}
			uint num = Decrypt(hiddenValue, key);
			if (ObscuredCheatingDetector.isRunning && fakeValue != 0 && num != fakeValue)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ObscuredUInt))
			{
				return false;
			}
			ObscuredUInt obscuredUInt = (ObscuredUInt)obj;
			return hiddenValue == obscuredUInt.hiddenValue;
		}

		public bool Equals(ObscuredUInt obj)
		{
			return hiddenValue == obj.hiddenValue;
		}

		public override string ToString()
		{
			return InternalDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InternalDecrypt().ToString(format);
		}

		public override int GetHashCode()
		{
			return InternalDecrypt().GetHashCode();
		}

		public string ToString(IFormatProvider provider)
		{
			return InternalDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return InternalDecrypt().ToString(format, provider);
		}

		public static implicit operator ObscuredUInt(uint value)
		{
			ObscuredUInt result = new ObscuredUInt(Encrypt(value));
			if (ObscuredCheatingDetector.isRunning)
			{
				result.fakeValue = value;
			}
			return result;
		}

		public static implicit operator uint(ObscuredUInt value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredUInt operator ++(ObscuredUInt input)
		{
			uint value = input.InternalDecrypt() + 1;
			input.hiddenValue = Encrypt(value, input.currentCryptoKey);
			if (ObscuredCheatingDetector.isRunning)
			{
				input.fakeValue = value;
			}
			return input;
		}

		public static ObscuredUInt operator --(ObscuredUInt input)
		{
			uint value = input.InternalDecrypt() - 1;
			input.hiddenValue = Encrypt(value, input.currentCryptoKey);
			if (ObscuredCheatingDetector.isRunning)
			{
				input.fakeValue = value;
			}
			return input;
		}
	}
}
