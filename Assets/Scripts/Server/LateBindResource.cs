using UnityEngine;

public class LateBindResource : ScriptableObject
{
	public enum ResourceType
	{
		Item = 0,
		SpecialItem = 1,
		Weapon = 2,
		Clothes = 3,
		Skin = 4,
		Bullet = 5
	}

	public int id;

	public ResourceType t;

	public Texture icon;

	public GameObject go;

	public new string name;

	public string desc;
}
