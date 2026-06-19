using System;
using UnityEngine;

namespace CodeStage.AntiCheat.Detectors
{
	[DisallowMultipleComponent]
	public class SpeedHackDetector : ActDetectorBase
	{
		private const string COMPONENT_NAME = "Speed Hack Detector";

		private const long TICKS_PER_SECOND = 10000000L;

		private const int THRESHOLD = 5000000;

		internal static bool isRunning;

		[Tooltip("Time (in seconds) between detector checks.")]
		public float interval = 1f;

		[Tooltip("Maximum false positives count allowed before registering speed hack.")]
		public byte maxFalsePositives = 3;

		[Tooltip("Amount of sequential successful checks before clearing internal false positives counter.\nSet 0 to disable Cool Down feature.")]
		public int coolDown = 30;

		private byte currentFalsePositives;

		private int currentCooldownShots;

		private long ticksOnStart;

		private long vulnerableTicksOnStart;

		private long prevTicks;

		private long prevIntervalTicks;

		public static SpeedHackDetector Instance { get; private set; }

		private static SpeedHackDetector GetOrCreateInstance
		{
			get
			{
				if (Instance == null)
				{
					SpeedHackDetector speedHackDetector = UnityEngine.Object.FindObjectOfType<SpeedHackDetector>();
					if (speedHackDetector != null)
					{
						Instance = speedHackDetector;
					}
					else
					{
						if (ActDetectorBase.detectorsContainer == null)
						{
							ActDetectorBase.detectorsContainer = new GameObject("Anti-Cheat Toolkit Detectors");
						}
						ActDetectorBase.detectorsContainer.AddComponent<SpeedHackDetector>();
					}
				}
				return Instance;
			}
		}

		private SpeedHackDetector()
		{
		}

		public static void StartDetection(Action callback)
		{
			StartDetection(callback, GetOrCreateInstance.interval);
		}

		public static void StartDetection(Action callback, float checkInterval)
		{
			StartDetection(callback, checkInterval, GetOrCreateInstance.maxFalsePositives);
		}

		public static void StartDetection(Action callback, float checkInterval, byte falsePositives)
		{
			StartDetection(callback, checkInterval, falsePositives, GetOrCreateInstance.coolDown);
		}

		public static void StartDetection(Action callback, float checkInterval, byte falsePositives, int shotsTillCooldown)
		{
			GetOrCreateInstance.StartDetectionInternal(callback, checkInterval, falsePositives, shotsTillCooldown);
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
			if (Init(Instance, "Speed Hack Detector"))
			{
				Instance = this;
			}
		}

		private void StartDetectionInternal(Action callback, float checkInterval, byte falsePositives, int shotsTillCooldown)
		{
			if (isRunning)
			{
				Debug.LogWarning("[ACTk] Speed Hack Detector already running!");
				return;
			}
			if (!base.enabled)
			{
				Debug.LogWarning("[ACTk] Speed Hack Detector disabled but StartDetection still called from somewhere!");
				return;
			}
			onDetection = callback;
			interval = checkInterval;
			maxFalsePositives = falsePositives;
			coolDown = shotsTillCooldown;
			ResetStartTicks();
			currentFalsePositives = 0;
			currentCooldownShots = 0;
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

		private void ResetStartTicks()
		{
			ticksOnStart = DateTime.UtcNow.Ticks;
			vulnerableTicksOnStart = (long)Environment.TickCount * 10000L;
			prevTicks = ticksOnStart;
			prevIntervalTicks = ticksOnStart;
		}

		private void OnApplicationPause(bool pause)
		{
			if (!pause)
			{
				ResetStartTicks();
			}
		}

		private void Update()
		{
			if (!isRunning)
			{
				return;
			}
			long num = 0L;
			num = DateTime.UtcNow.Ticks;
			long num2 = num - prevTicks;
			if (num2 < 0 || num2 > 10000000)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("[ACTk] SpeedHackDetector: System DateTime change or > 1 second game freeze detected!");
				}
				ResetStartTicks();
				return;
			}
			prevTicks = num;
			long num3 = (long)(interval * 10000000f);
			if (num - prevIntervalTicks < num3)
			{
				return;
			}
			long num4 = 0L;
			num4 = (long)Environment.TickCount * 10000L;
			if (Mathf.Abs(num4 - vulnerableTicksOnStart - (num - ticksOnStart)) > 5000000f)
			{
				currentFalsePositives++;
				if (currentFalsePositives > maxFalsePositives)
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("[ACTk] SpeedHackDetector: final detection!");
					}
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
						StopDetection();
					}
				}
				else
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("[ACTk] SpeedHackDetector: detection! Allowed false positives left: " + (maxFalsePositives - currentFalsePositives));
					}
					currentCooldownShots = 0;
					ResetStartTicks();
				}
			}
			else if (currentFalsePositives > 0 && coolDown > 0)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("[ACTk] SpeedHackDetector: success shot! Shots till Cooldown: " + (coolDown - currentCooldownShots));
				}
				currentCooldownShots++;
				if (currentCooldownShots >= coolDown)
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("[ACTk] SpeedHackDetector: Cooldown!");
					}
					currentFalsePositives = 0;
				}
			}
			prevIntervalTicks = num;
		}
	}
}
