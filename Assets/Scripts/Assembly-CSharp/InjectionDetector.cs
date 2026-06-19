using System;
using System.IO;
using System.Reflection;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace CodeStage.AntiCheat.Detectors
{
	[DisallowMultipleComponent]
	public class InjectionDetector : ActDetectorBase
	{
		private class AllowedAssembly
		{
			public readonly string name;

			public readonly int[] hashes;

			public AllowedAssembly(string name, int[] hashes)
			{
				this.name = name;
				this.hashes = hashes;
			}
		}

		private const string COMPONENT_NAME = "Injection Detector";

		internal static bool isRunning;

		private bool signaturesAreNotGenuine;

		private AllowedAssembly[] allowedAssemblies;

		private string[] hexTable;

		public static InjectionDetector Instance { get; private set; }

		private static InjectionDetector GetOrCreateInstance
		{
			get
			{
				if (Instance == null)
				{
					InjectionDetector injectionDetector = UnityEngine.Object.FindObjectOfType<InjectionDetector>();
					if (injectionDetector != null)
					{
						Instance = injectionDetector;
					}
					else
					{
						if (ActDetectorBase.detectorsContainer == null)
						{
							ActDetectorBase.detectorsContainer = new GameObject("Anti-Cheat Toolkit Detectors");
						}
						ActDetectorBase.detectorsContainer.AddComponent<InjectionDetector>();
					}
				}
				return Instance;
			}
		}

		private InjectionDetector()
		{
		}

		public static void StartDetection(Action callback)
		{
			GetOrCreateInstance.StartDetectionInternal(callback);
		}

		public static void StopDetection()
		{
			if (Instance != null)
			{
				Instance.StopDetectionInternal();
			}
		}

		public static void Dispose()
		{
			if (Instance != null)
			{
				Instance.DisposeInternal();
			}
		}

		private void Awake()
		{
			if (Init(Instance, "Injection Detector"))
			{
				Instance = this;
			}
		}

		private void StartDetectionInternal(Action callback)
		{
			if (isRunning)
			{
				Debug.LogWarning("[ACTk] Injection Detector already running!");
				return;
			}
			if (!base.enabled)
			{
				Debug.LogWarning("[ACTk] Injection Detector disabled but StartDetection still called from somewhere!");
				return;
			}
			onDetection = callback;
			if (allowedAssemblies == null)
			{
				LoadAndParseAllowedAssemblies();
			}
			if (signaturesAreNotGenuine)
			{
				OnInjectionDetected();
			}
			else if (!FindInjectionInCurrentAssemblies())
			{
				AppDomain.CurrentDomain.AssemblyLoad += OnNewAssemblyLoaded;
				isRunning = true;
			}
			else
			{
				OnInjectionDetected();
			}
		}

		protected override void StopDetectionInternal()
		{
			if (isRunning)
			{
				AppDomain.CurrentDomain.AssemblyLoad -= OnNewAssemblyLoaded;
				onDetection = null;
				isRunning = false;
			}
		}

		protected override void PauseDetector()
		{
			isRunning = false;
			AppDomain.CurrentDomain.AssemblyLoad -= OnNewAssemblyLoaded;
		}

		protected override void ResumeDetector()
		{
			isRunning = true;
			AppDomain.CurrentDomain.AssemblyLoad += OnNewAssemblyLoaded;
		}

		protected override void DisposeInternal()
		{
			base.DisposeInternal();
			if (Instance == this)
			{
				Instance = null;
			}
		}

		private void OnInjectionDetected()
		{
			if (onDetection != null)
			{
				onDetection();
			}
			if (autoDispose)
			{
				Dispose();
			}
			else
			{
				StopDetectionInternal();
			}
		}

		private void OnNewAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
		{
			if (!AssemblyAllowed(args.LoadedAssembly))
			{
				OnInjectionDetected();
			}
		}

		private bool FindInjectionInCurrentAssemblies()
		{
			bool result = false;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			if (assemblies.Length == 0)
			{
				result = true;
			}
			else
			{
				Assembly[] array = assemblies;
				foreach (Assembly ass in array)
				{
					if (!AssemblyAllowed(ass))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private bool AssemblyAllowed(Assembly ass)
		{
			string text = ass.GetName().Name;
			int assemblyHash = GetAssemblyHash(ass);
			bool result = false;
			for (int i = 0; i < allowedAssemblies.Length; i++)
			{
				AllowedAssembly allowedAssembly = allowedAssemblies[i];
				if (allowedAssembly.name == text && Array.IndexOf(allowedAssembly.hashes, assemblyHash) != -1)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		private void LoadAndParseAllowedAssemblies()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("fndid", typeof(TextAsset));
			if (textAsset == null)
			{
				signaturesAreNotGenuine = true;
				return;
			}
			string[] separator = new string[1] { ":" };
			MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			int num = binaryReader.ReadInt32();
			allowedAssemblies = new AllowedAssembly[num];
			for (int i = 0; i < num; i++)
			{
				string value = binaryReader.ReadString();
				value = ObscuredString.EncryptDecrypt(value, "Elina");
				string[] array = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				int num2 = array.Length;
				if (num2 > 1)
				{
					string text = array[0];
					int[] array2 = new int[num2 - 1];
					for (int j = 1; j < num2; j++)
					{
						array2[j - 1] = int.Parse(array[j]);
					}
					allowedAssemblies[i] = new AllowedAssembly(text, array2);
					continue;
				}
				signaturesAreNotGenuine = true;
				binaryReader.Close();
				memoryStream.Close();
				return;
			}
			binaryReader.Close();
			memoryStream.Close();
			Resources.UnloadAsset(textAsset);
			hexTable = new string[256];
			for (int k = 0; k < 256; k++)
			{
				hexTable[k] = k.ToString("x2");
			}
		}

		private int GetAssemblyHash(Assembly ass)
		{
			AssemblyName assemblyName = ass.GetName();
			byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
			string text = ((publicKeyToken.Length != 8) ? assemblyName.Name : (assemblyName.Name + PublicKeyTokenToString(publicKeyToken)));
			int num = 0;
			int length = text.Length;
			for (int i = 0; i < length; i++)
			{
				num += text[i];
				num += num << 10;
				num ^= num >> 6;
			}
			num += num << 3;
			num ^= num >> 11;
			return num + (num << 15);
		}

		private string PublicKeyTokenToString(byte[] bytes)
		{
			string text = string.Empty;
			for (int i = 0; i < 8; i++)
			{
				text += hexTable[bytes[i]];
			}
			return text;
		}
	}
}
