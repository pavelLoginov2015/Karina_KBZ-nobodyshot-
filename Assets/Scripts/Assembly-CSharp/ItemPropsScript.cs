using UnityEngine;
using kube;

public class ItemPropsScript : MonoBehaviour
{
	public bool magic;

	public bool buildMagic;

	public bool canTake;

	public bool canMove;

	public bool canRotate;

	public bool canActivate;

	public bool canSetup;

	public bool isTrigger;

	public bool automaticTakeIfNear;

	public ItemPlaceType placeType;

	public CubePhys physType;

	public SoundMaterialType soundMaterialType;

	public byte health;

	public Color32 lightColor;

	public int id;

	public int state;

	public int type;

	public Vector2 doorSize;

	public CubeProps mapProps;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void MoveItem(Vector3 newPos)
	{
		ActionAreaScript component = GetComponent<ActionAreaScript>();
		TriggerScript component2 = GetComponent<TriggerScript>();
		WireScript component3 = GetComponent<WireScript>();
		NetworkObjectScript nO = Kube.BCS.NO;
		if (!(component != null))
		{
			if (component2 != null)
			{
				nO.MoveItem(id, newPos);
			}
			else if (!(component3 != null))
			{
				nO.MoveItem(id, newPos);
			}
		}
	}
}
