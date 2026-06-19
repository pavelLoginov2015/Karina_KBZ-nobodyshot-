using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class PingCloudRegions : MonoBehaviour
{
	private const string playerPrefsKey = "PUNCloudBestRegion";

	public static CloudServerRegion closestRegion = CloudServerRegion.US;

	public static PingCloudRegions SP;

	private bool isPinging;

	private int lowestRegionAverage = -1;

	private void Awake()
	{
		SP = this;
		if (PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty) != string.Empty)
		{
			string @string = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
			closestRegion = (CloudServerRegion)(int)Enum.Parse(typeof(CloudServerRegion), @string, true);
		}
		else
		{
			StartCoroutine(PingAllRegions());
		}
	}

	public static void OverrideRegion(CloudServerRegion region)
	{
		SetRegion(region);
	}

	public static void RefreshCloudServerRating()
	{
		if (SP != null)
		{
			SP.StartCoroutine(SP.PingAllRegions());
		}
	}

	public static void ConnectToBestRegion(string gameVersion)
	{
		SP.StartCoroutine(SP.ConnectToBestRegionInternal(gameVersion));
	}

	public IEnumerator PingAllRegions()
	{
	
		isPinging = true;
		foreach (int region in Enum.GetValues(typeof(CloudServerRegion)))
		{
			yield return StartCoroutine(PingRegion((CloudServerRegion)region));
		}
		isPinging = false;
	}

	private IEnumerator PingRegion(CloudServerRegion region)
	{
		yield break;
	}

	private static void SetRegion(CloudServerRegion region)
	{
		closestRegion = region;
		PlayerPrefs.SetString("PUNCloudBestRegion", region.ToString());
	}

	private IEnumerator ConnectToBestRegionInternal(string gameVersion)
	{
		while (isPinging)
		{
			yield return 0;
		}
		
		
	}

	public static string ResolveHost(string hostString)
	{
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostString);
			foreach (IPAddress iPAddress in hostAddresses)
			{
				if (iPAddress != null && iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					return iPAddress.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log("Exception caught! " + ex.Source + " Message: " + ex.Message);
		}
		return string.Empty;
	}
}
