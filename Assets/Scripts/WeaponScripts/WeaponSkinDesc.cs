using System;
using UnityEngine;

public enum rare
{
	def,
	epic,
	legendary,
	secret,
}

[Serializable]
public class WeaponSkinDesc
{
	public string name;

	public int weaponId;

	public int price;

	[NonSerialized]
	public int id;

	public bool hidden;

	public rare Rare;
}
