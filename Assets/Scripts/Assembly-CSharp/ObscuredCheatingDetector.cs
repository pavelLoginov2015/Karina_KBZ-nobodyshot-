using System;
using UnityEngine;

namespace CodeStage.AntiCheat.Detectors
{
	[DisallowMultipleComponent]
	public class ObscuredCheatingDetector : ActDetectorBase
	{
		private const string COMPONENT_NAME = "Obscured Cheating Detector";

		internal static bool isRunning;

		[HideInInspector]
		public float floatEpsilon = 0.0001f;

		[HideInInspector]
		public float vector2Epsilon = 0.1f;

		[HideInInspector]
		public float vector3Epsilon = 0.1f;

		[HideInInspector]
		public float quaternionEpsilon = 0.1f;

		public static ObscuredCheatingDetector Instance { get; private set; }

		private static ObscuredCheatingDetector GetOrCreateInstance
		{
			get
			{
				if (Instance == null)
				{
					ObscuredCheatingDetector obscuredCheatingDetector = UnityEngine.Object.FindObjectOfType<ObscuredCheatingDetector>();
					if (obscuredCheatingDetector != null)
					{
						Instance = obscuredCheatingDetector;
					}
					else
					{
						if (ActDetectorBase.detectorsContainer == null)
						{
							ActDetectorBase.detectorsContainer = new GameObject("Anti-Cheat Toolkit Detectors");
						}
						ActDetectorBase.detectorsContainer.AddComponent<ObscuredCheatingDetector>();
					}
				}
				return Instance;
			}
		}

		private ObscuredCheatingDetector()
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
			if (Init(Instance, "Obscured Cheating Detector"))
			{
				Instance = this;
			}
		}

		private void StartDetectionInternal(Action callback)
		{
			if (isRunning)
			{
				Debug.LogWarning("[ACTk] Obscured Cheating Detector already running!");
				return;
			}
			if (!base.enabled)
			{
				Debug.LogWarning("[ACTk] Obscured Cheating Detector disabled but StartDetection still called from somewhere!");
				return;
			}
			onDetection = callback;
			isRunning = true;
		}

		protected override void StopDetectionInternal()
		{
			if (isRunning)
			{
				onDetection = null;
				isRunning = false;
			}
		}

		protected override void PauseDetector()
		{
			isRunning = false;
		}

		protected override void ResumeDetector()
		{
			isRunning = true;
		}

		protected override void DisposeInternal()
		{
			base.DisposeInternal();
			if (Instance == this)
			{
				Instance = null;
			}
		}

		internal void OnCheatingDetected()
		{
			if (onDetection != null)
			{
				onDetection();
				if (autoDispose)
				{
					Dispose();
				}
				else
				{
					StopDetection();
				}
			}
		}
	}
}
