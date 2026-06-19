using UnityEngine;
using kube;

public class AsyncObjectScript : MonoBehaviour
{
	public string path;

	private void Start()
	{
		GameObject original = (GameObject)Resources.Load(path);
		original.transform.parent = base.transform;
		original.transform.localPosition = Vector3.zero;
	}

}
