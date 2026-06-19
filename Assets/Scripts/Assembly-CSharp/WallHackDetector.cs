using System;
using UnityEngine;

namespace CodeStage.AntiCheat.Detectors
{
	[DisallowMultipleComponent]
	public class WallHackDetector : ActDetectorBase
	{
		private const string COMPONENT_NAME = "WallHack Detector";

		private const string SERVICE_CONTAINER_NAME = "[WH Detector Service]";

		private readonly Vector3 rigidPlayerVelocity = new Vector3(0f, 0f, 1f);

		internal static bool isRunning;

		[Tooltip("World position of the container for service objects within 3x3x3 cube (drawn as red wireframe cube in scene).")]
		public Vector3 spawnPosition;

		private int whLayer = -1;

		private GameObject serviceContainer;

		private Rigidbody rigidPlayer;

		private CharacterController charControllerPlayer;

		private float charControllerVelocity;

		public static WallHackDetector Instance { get; private set; }

		private static WallHackDetector GetOrCreateInstance
		{
			get
			{
				if (Instance == null)
				{
					WallHackDetector wallHackDetector = UnityEngine.Object.FindObjectOfType<WallHackDetector>();
					if (wallHackDetector != null)
					{
						Instance = wallHackDetector;
					}
					else
					{
						if (ActDetectorBase.detectorsContainer == null)
						{
							ActDetectorBase.detectorsContainer = new GameObject("Anti-Cheat Toolkit Detectors");
						}
						ActDetectorBase.detectorsContainer.AddComponent<WallHackDetector>();
					}
				}
				return Instance;
			}
		}

		private WallHackDetector()
		{
		}

		public static void StartDetection(Action callback)
		{
			StartDetection(callback, GetOrCreateInstance.spawnPosition);
		}

		public static void StartDetection(Action callback, Vector3 servicePosition)
		{
			GetOrCreateInstance.StartDetectionInternal(callback, servicePosition);
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
			if (Init(Instance, "WallHack Detector"))
			{
				Instance = this;
			}
		}

		private void StartDetectionInternal(Action callback, Vector3 servicePosition)
		{
			if (isRunning)
			{
				Debug.LogWarning("[ACTk] WallHack Detector already running!");
				return;
			}
			if (!base.enabled)
			{
				Debug.LogWarning("[ACTk] WallHack Detector disabled but StartDetection still called from somewhere!");
				return;
			}
			onDetection = callback;
			spawnPosition = servicePosition;
			InitDetector();
			isRunning = true;
		}

		protected override void StopDetectionInternal()
		{
			if (isRunning)
			{
				UninitDetector();
				onDetection = null;
				isRunning = false;
			}
		}

		protected override void PauseDetector()
		{
			if (isRunning)
			{
				isRunning = false;
				StopRigidModule();
				StopControllerModule();
			}
		}

		protected override void ResumeDetector()
		{
			isRunning = true;
			StartRigidModule();
			StartControllerModule();
		}

		protected override void DisposeInternal()
		{
			base.DisposeInternal();
			if (Instance == this)
			{
				Instance = null;
			}
		}

		private void InitDetector()
		{
			InitCommon();
			InitRigidModule();
			InitControllerModule();
			StartRigidModule();
			StartControllerModule();
		}

		private void UninitDetector()
		{
			isRunning = false;
			StopRigidModule();
			StopControllerModule();
			UnityEngine.Object.Destroy(serviceContainer);
		}

		private void InitCommon()
		{
			if (whLayer == -1)
			{
				whLayer = LayerMask.NameToLayer("Ignore Raycast");
			}
			serviceContainer = new GameObject("[WH Detector Service]");
			serviceContainer.layer = whLayer;
			serviceContainer.transform.position = spawnPosition;
			UnityEngine.Object.DontDestroyOnLoad(serviceContainer);
			GameObject gameObject = new GameObject("Wall");
			gameObject.AddComponent<BoxCollider>();
			gameObject.layer = whLayer;
			gameObject.transform.parent = serviceContainer.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = new Vector3(3f, 3f, 0.5f);
		}

		private void InitRigidModule()
		{
			GameObject gameObject = new GameObject("RigidPlayer");
			gameObject.AddComponent<CapsuleCollider>().height = 2f;
			gameObject.layer = whLayer;
			gameObject.transform.parent = serviceContainer.transform;
			gameObject.transform.localPosition = new Vector3(0.75f, 0f, -1f);
			rigidPlayer = gameObject.AddComponent<Rigidbody>();
			rigidPlayer.useGravity = false;
		}

		private void InitControllerModule()
		{
			GameObject gameObject = new GameObject("ControlledPlayer");
			gameObject.AddComponent<CapsuleCollider>().height = 2f;
			gameObject.layer = whLayer;
			gameObject.transform.parent = serviceContainer.transform;
			gameObject.transform.localPosition = new Vector3(-0.75f, 0f, -1f);
			charControllerPlayer = gameObject.AddComponent<CharacterController>();
		}

		private void StartRigidModule()
		{
			rigidPlayer.rotation = Quaternion.identity;
			rigidPlayer.angularVelocity = Vector3.zero;
			rigidPlayer.transform.localPosition = new Vector3(0.75f, 0f, -1f);
			rigidPlayer.velocity = rigidPlayerVelocity;
			Invoke("StartRigidModule", 4f);
		}

		private void StopRigidModule()
		{
			rigidPlayer.velocity = Vector3.zero;
			CancelInvoke("StartRigidModule");
		}

		private void StartControllerModule()
		{
			charControllerPlayer.transform.localPosition = new Vector3(-0.75f, 0f, -1f);
			charControllerVelocity = 0.01f;
			Invoke("StartControllerModule", 4f);
		}

		private void StopControllerModule()
		{
			charControllerVelocity = 0f;
			CancelInvoke("StartControllerModule");
		}

		private void FixedUpdate()
		{
			if (isRunning && rigidPlayer.transform.localPosition.z > 1f)
			{
				StopRigidModule();
				Detect();
			}
		}

		private void Update()
		{
			if (isRunning && charControllerVelocity > 0f)
			{
				charControllerPlayer.Move(new Vector3(UnityEngine.Random.Range(-0.002f, 0.002f), 0f, charControllerVelocity));
				if (charControllerPlayer.transform.localPosition.z > 1f)
				{
					StopControllerModule();
					Detect();
				}
			}
		}

		private void Detect()
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
				StopDetection();
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(spawnPosition, new Vector3(3f, 3f, 3f));
		}
	}
}
