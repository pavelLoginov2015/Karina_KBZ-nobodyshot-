using UnityEngine;

public class LightningScript : MonoBehaviour
{
	private enum PositionType
	{
		TargetVector = 0,
		TargetTransform = 1
	}

	public Texture[] lightningTex;

	public float deltaTime = 0.1f;

	private float lastChange;

	private PositionType startPosType;

	private PositionType endPosType;

	private Transform startPosTransform;

	private Transform endPosTransform;

	private Vector3 startPosVector;

	private Vector3 endPosVector;

	private LineRenderer line;

	private bool initialized;

	private void Init()
	{
		if (!initialized)
		{
			line = GetComponent<LineRenderer>();
			initialized = true;
		}
	}

	private void SetMaterialTile()
	{
		Vector3 position = startPosVector;
		if (startPosType == PositionType.TargetTransform)
		{
			position = startPosTransform.position;
		}
		Vector3 position2 = endPosVector;
		if (endPosType == PositionType.TargetTransform)
		{
			position2 = endPosTransform.position;
		}
		float x = Vector3.Distance(position, position2);
		GetComponent<Renderer>().material.mainTextureScale = new Vector2(x, 1f);
	}

	private void SetSource(Transform _source)
	{
		startPosType = PositionType.TargetTransform;
		startPosTransform = _source;
		startPosVector = _source.position;
		SetMaterialTile();
	}

	private void SetDestination(Transform _destination)
	{
		endPosType = PositionType.TargetTransform;
		endPosTransform = _destination;
		endPosVector = _destination.position;
		SetMaterialTile();
	}

	private void SetSource(Vector3 _source)
	{
		startPosType = PositionType.TargetVector;
		startPosVector = _source;
		Init();
		line.SetPosition(0, startPosVector);
		SetMaterialTile();
	}

	private void SetDestination(Vector3 _destination)
	{
		endPosType = PositionType.TargetVector;
		endPosVector = _destination;
		Init();
		line.SetPosition(1, endPosVector);
		SetMaterialTile();
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		if (Time.time - lastChange > deltaTime)
		{
			GetComponent<Renderer>().material.mainTexture = lightningTex[Random.Range(0, lightningTex.Length)];
			lastChange = Time.time;
		}
		if (startPosType == PositionType.TargetTransform)
		{
			if (startPosTransform == null)
			{
				startPosType = PositionType.TargetVector;
				line.SetPosition(0, startPosVector);
				SetMaterialTile();
			}
			else
			{
				line.SetPosition(0, startPosTransform.position);
				startPosVector = startPosTransform.position;
			}
		}
		if (endPosType == PositionType.TargetTransform)
		{
			if (endPosTransform == null)
			{
				endPosType = PositionType.TargetVector;
				line.SetPosition(1, endPosVector);
				SetMaterialTile();
			}
			else
			{
				line.SetPosition(1, endPosTransform.position);
				endPosVector = endPosTransform.position;
			}
		}
		if (startPosType == PositionType.TargetTransform || endPosType == PositionType.TargetTransform)
		{
			SetMaterialTile();
		}
	}
}
