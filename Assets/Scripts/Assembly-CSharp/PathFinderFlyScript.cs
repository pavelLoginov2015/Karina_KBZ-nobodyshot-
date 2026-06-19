using UnityEngine;
using kube;

public class PathFinderFlyScript : PathFinderMoveScript
{
	public GameObject jetPack;

	public float flySpeed = 8f;

	protected bool flyPath;

	public override bool CanPathTo(Vector3 targetPos)
	{
		if (pathLength > 0)
		{
			return true;
		}
		FindPathToVector3Fly(targetPos);
		if (pathLength > 0)
		{
			return true;
		}
		FindPathToVector3Walking(targetPos);
		if (pathLength == 0)
		{
			return false;
		}
		return true;
	}

	public new void SetPathFinderParams(float _speed, float _jumpSpeed, int _charSizeY)
	{
		base.SetPathFinderParams(_speed, _jumpSpeed, _charSizeY);
	}

	private void FastFly(Vector3 pos)
	{
		int x = Mathf.RoundToInt(lastPathPoint.x);
		int y = Mathf.RoundToInt(lastPathPoint.y);
		int z = Mathf.RoundToInt(lastPathPoint.z);
		int num = Mathf.RoundToInt(pos.x);
		int num2 = Mathf.RoundToInt(pos.y);
		int num3 = Mathf.RoundToInt(pos.z);
		PFAS.ClearArray();
		PFAS.openedArrayNum = 1;
		PFAS.openedArray[0].x = x;
		PFAS.openedArray[0].y = y;
		PFAS.openedArray[0].z = z;
		PFAS.openedArray[0].parent = -1;
		PFAS.openedArray[0].stepNum = 0;
		PFAS.openedArray[0].isClosed = false;
		PFAS.openedArray[0].distFromSource = 0;
		PFAS.openedArray[0].distToTarget = GetDistToTarget(PFAS.openedArray[0].x, PFAS.openedArray[0].y, PFAS.openedArray[0].z, num, num2, num3);
		int num4 = 0;
		int num5 = -1;
		int num6 = 0;
		while (true)
		{
			num6++;
			if (num6 > maxIterations)
			{
				num4 = 1;
				break;
			}
			float num7 = 9999999f;
			num5 = -1;
			for (int i = 0; i < PFAS.openedArrayNum; i++)
			{
				if (!PFAS.openedArray[i].isClosed)
				{
					float num8 = GetElementValue(PFAS.openedArray[i].distFromSource, PFAS.openedArray[i].distToTarget);
					if (num8 < num7)
					{
						num5 = i;
						num7 = num8;
					}
				}
			}
			if (num5 == -1)
			{
				num4 = 1;
				break;
			}
			PFAS.openedArray[num5].cannotStop = false;
			if (PFAS.openedArray[num5].x == num && PFAS.openedArray[num5].y == num2 && PFAS.openedArray[num5].z == num3)
			{
				num4 = 2;
				break;
			}
			PFAS.openedArray[num5].isClosed = true;
			PFAS.closedArray[PFAS.closedArrayNum] = PFAS.openedArray[num5];
			PFAS.closedArrayNum++;
			if (PFAS.openedArray[num5].stepNum > maxPath)
			{
				continue;
			}
			CubePhys cubePhysType = Kube.WHS.GetCubePhysType(PFAS.openedArray[num5].x, PFAS.openedArray[num5].y, PFAS.openedArray[num5].z);
			for (int j = 0; j < PathFinderMoveScript.nX1.Length; j++)
			{
				int num9 = PFAS.openedArray[num5].x + PathFinderMoveScript.nX1[j];
				int num10 = PFAS.openedArray[num5].y + PathFinderMoveScript.nY1[j];
				int num11 = PFAS.openedArray[num5].z + PathFinderMoveScript.nZ1[j];
				if (!Kube.WHS.IsInWorld(num9, num10, num11))
				{
					continue;
				}
				CubePhys cubePhysType2 = Kube.WHS.GetCubePhysType(num9, num10 - 1, num11);
				if (Kube.WHS.isOccupied[num9, num10, num11])
				{
					continue;
				}
				bool flag = false;
				for (int k = 0; k < PFAS.openedArrayNum; k++)
				{
					if (num9 == PFAS.openedArray[k].x && num10 == PFAS.openedArray[k].y && num11 == PFAS.openedArray[k].z)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					continue;
				}
				bool flag2 = false;
				for (int l = 0; l < PFAS.closedArrayNum; l++)
				{
					if (num9 == PFAS.closedArray[l].x && num10 == PFAS.closedArray[l].y && num11 == PFAS.closedArray[l].z)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					continue;
				}
				bool flag3 = true;
				for (int m = 0; m < charSizeY; m++)
				{
					CubePhys cubePhysType3 = Kube.WHS.GetCubePhysType(num9, num10 + m, num11);
					if (cubePhysType3 == CubePhys.solid || cubePhysType3 == CubePhys.ice || Kube.WHS.isOccupied[num9, num10 + m, num11])
					{
						flag3 = false;
						break;
					}
				}
				if (flag3)
				{
					CubePhys cubePhysType4 = Kube.WHS.GetCubePhysType(num9, num10, num11);
					PFAS.openedArray[PFAS.openedArrayNum].distFromSource = PFAS.openedArray[num5].distFromSource + PathFinderMoveScript.wt1[j];
					PFAS.openedArray[PFAS.openedArrayNum].distToTarget = GetDistToTarget(num9, num10, num11, num, num2, num3);
					PFAS.openedArray[PFAS.openedArrayNum].isClosed = false;
					PFAS.openedArray[PFAS.openedArrayNum].parent = num5;
					PFAS.openedArray[PFAS.openedArrayNum].stepNum = PFAS.openedArray[num5].stepNum + 1;
					PFAS.openedArray[PFAS.openedArrayNum].x = num9;
					PFAS.openedArray[PFAS.openedArrayNum].y = num10;
					PFAS.openedArray[PFAS.openedArrayNum].z = num11;
					PFAS.openedArrayNum++;
				}
			}
			for (int n = 0; n < PathFinderMoveScript.nX2.Length; n++)
			{
				int num12 = PFAS.openedArray[num5].x + PathFinderMoveScript.nX2[n];
				int num13 = PFAS.openedArray[num5].y + PathFinderMoveScript.nY2[n];
				int num14 = PFAS.openedArray[num5].z + PathFinderMoveScript.nZ2[n];
				if (!Kube.WHS.IsInWorld(num12, num13, num14))
				{
					continue;
				}
				CubePhys cubePhysType5 = Kube.WHS.GetCubePhysType(num12, num13 - 1, num14);
				if (Kube.WHS.isOccupied[num12, num13, num14])
				{
					continue;
				}
				bool flag4 = false;
				for (int num15 = 0; num15 < PFAS.openedArrayNum; num15++)
				{
					if (num12 == PFAS.openedArray[num15].x && num13 == PFAS.openedArray[num15].y && num14 == PFAS.openedArray[num15].z)
					{
						flag4 = true;
						break;
					}
				}
				if (flag4)
				{
					continue;
				}
				bool flag5 = false;
				for (int num16 = 0; num16 < PFAS.closedArrayNum; num16++)
				{
					if (num12 == PFAS.closedArray[num16].x && num13 == PFAS.closedArray[num16].y && num14 == PFAS.closedArray[num16].z)
					{
						flag5 = true;
						break;
					}
				}
				if (flag5)
				{
					continue;
				}
				bool flag6 = true;
				for (int num17 = 0; num17 < charSizeY; num17++)
				{
					CubePhys cubePhysType6 = Kube.WHS.GetCubePhysType(num12, num13 + num17, num14);
					if (cubePhysType6 == CubePhys.solid || cubePhysType6 == CubePhys.ice || Kube.WHS.isOccupied[num12, num13 + num17, num14])
					{
						flag6 = false;
						break;
					}
				}
				for (int num18 = 0; num18 < charSizeY; num18++)
				{
					CubePhys cubePhysType7 = Kube.WHS.GetCubePhysType(num12 - PathFinderMoveScript.nX2[n], num13 + num18, num14);
					if (cubePhysType7 == CubePhys.solid || cubePhysType7 == CubePhys.ice || Kube.WHS.isOccupied[num12 - PathFinderMoveScript.nX2[n], num13 + num18, num14])
					{
						flag6 = false;
						break;
					}
				}
				for (int num19 = 0; num19 < charSizeY; num19++)
				{
					CubePhys cubePhysType8 = Kube.WHS.GetCubePhysType(num12, num13 + num19, num14 - PathFinderMoveScript.nZ2[n]);
					if (cubePhysType8 == CubePhys.solid || cubePhysType8 == CubePhys.ice || Kube.WHS.isOccupied[num12, num13 + num19, num14 - PathFinderMoveScript.nZ2[n]])
					{
						flag6 = false;
						break;
					}
				}
				if (flag6)
				{
					PFAS.openedArray[PFAS.openedArrayNum].distFromSource = PFAS.openedArray[num5].distFromSource + PathFinderMoveScript.wt2[n];
					PFAS.openedArray[PFAS.openedArrayNum].distToTarget = GetDistToTarget(num12, num13, num14, num, num2, num3);
					PFAS.openedArray[PFAS.openedArrayNum].isClosed = false;
					PFAS.openedArray[PFAS.openedArrayNum].parent = num5;
					PFAS.openedArray[PFAS.openedArrayNum].stepNum = PFAS.openedArray[num5].stepNum + 1;
					PFAS.openedArray[PFAS.openedArrayNum].x = num12;
					PFAS.openedArray[PFAS.openedArrayNum].y = num13;
					PFAS.openedArray[PFAS.openedArrayNum].z = num14;
					PFAS.openedArrayNum++;
				}
			}
			int num20;
			for (num20 = 0; num20 < jumpCubes + charSizeY; num20++)
			{
				CubePhys cubePhysType9 = Kube.WHS.GetCubePhysType(PFAS.openedArray[num5].x, PFAS.openedArray[num5].y + num20, PFAS.openedArray[num5].z);
				if (cubePhysType9 == CubePhys.solid || cubePhysType9 == CubePhys.ice)
				{
					break;
				}
			}
			num20 -= charSizeY;
		}
		if (num4 == 2)
		{
			int num21 = num5;
			pathLength = PFAS.openedArray[num21].stepNum + 1;
			while (num21 != -1)
			{
				if (PFAS.openedArray[num21].parent != -1)
				{
					path[pathLength - PFAS.openedArray[num21].stepNum - 1] = new Vector3(PFAS.openedArray[num21].x, PFAS.openedArray[num21].y, PFAS.openedArray[num21].z);
				}
				num21 = PFAS.openedArray[num21].parent;
			}
			pathLength--;
			return;
		}
		float num22 = 9999999f;
		num5 = -1;
		for (int num23 = 0; num23 < PFAS.openedArrayNum; num23++)
		{
			if (!PFAS.openedArray[num23].cannotStop)
			{
				float num24 = PFAS.openedArray[num23].distToTarget;
				if (num24 < num22)
				{
					num5 = num23;
					num22 = num24;
				}
			}
		}
		int num25 = num5;
		if (num25 < 0)
		{
			return;
		}
		pathLength = PFAS.openedArray[num25].stepNum + 1;
		while (num25 != -1)
		{
			if (PFAS.openedArray[num25].parent != -1)
			{
				path[pathLength - PFAS.openedArray[num25].stepNum - 1] = new Vector3(PFAS.openedArray[num25].x, PFAS.openedArray[num25].y, PFAS.openedArray[num25].z);
			}
			num25 = PFAS.openedArray[num25].parent;
		}
		pathLength--;
	}

	private void FindPathToVector3Fly(Vector3 targetPos)
	{
		int num = Mathf.RoundToInt(lastPathPoint.x);
		int num2 = Mathf.RoundToInt(lastPathPoint.y);
		int num3 = Mathf.RoundToInt(lastPathPoint.z);
		flyPath = false;
		FastFly(targetPos);
		if (pathLength != 0)
		{
			flyPath = true;
		}
	}

	public override void WalkingFollowTarget(Vector3 targetPos)
	{
		if (Time.time - lastRefindPath > refindPathPeriod)
		{
			int num = Mathf.RoundToInt(lastOccupiedPoint.x);
			int num2 = Mathf.RoundToInt(lastOccupiedPoint.y);
			int num3 = Mathf.RoundToInt(lastOccupiedPoint.z);
			Kube.WHS.isOccupied[num, num2, num3] = false;
			FindPathToVector3Fly(targetPos);
			if (pathLength == 0)
			{
				num = Mathf.RoundToInt(lastOccupiedPoint.x);
				num2 = Mathf.RoundToInt(lastOccupiedPoint.y);
				num3 = Mathf.RoundToInt(lastOccupiedPoint.z);
				Kube.WHS.isOccupied[num, num2, num3] = true;
			}
			flagJumpUp = (flagJumpAcross = (flagFall = false));
			lastRefindPath = Time.time - Random.Range(0f, 0.5f);
		}
		if (flyPath && pathLength > 0)
		{
			int num = Mathf.RoundToInt(lastOccupiedPoint.x);
			int num2 = Mathf.RoundToInt(lastOccupiedPoint.y);
			int num3 = Mathf.RoundToInt(lastOccupiedPoint.z);
			Kube.WHS.isOccupied[num, num2, num3] = false;
			lastOccupiedPoint = path[pathLength - 1];
			num = Mathf.RoundToInt(lastOccupiedPoint.x);
			num2 = Mathf.RoundToInt(lastOccupiedPoint.y);
			num3 = Mathf.RoundToInt(lastOccupiedPoint.z);
			Kube.WHS.isOccupied[num, num2, num3] = true;
			Vector3 vector = path[pathLength - 1] + Vector3.up * deltaHeightTransform;
			base.transform.LookAt(new Vector3(vector.x, base.transform.position.y, vector.z));
			int num4 = Kube.WHS.cubeTypes[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z)];
			base.transform.Translate((vector - base.transform.position).normalized * flySpeed * Time.deltaTime, Space.World);
			if (Vector3.Distance(base.transform.position, vector) < flySpeed * Time.deltaTime * 2f)
			{
				lastPathPoint = path[pathLength - 1];
				pathLength--;
				flagJumpUp = (flagJumpAcross = (flagFall = false));
				if (pathLength > 0 && Kube.WHS.isOccupied[Mathf.RoundToInt(path[pathLength - 1].x), Mathf.RoundToInt(path[pathLength - 1].y), Mathf.RoundToInt(path[pathLength - 1].z)])
				{
					pathLength = 0;
					lastRefindPath = 0f;
				}
			}
		}
		else
		{
			base.WalkingFollowTarget(targetPos);
		}
	}

	private new void Start()
	{
		isFly = true;
		base.Start();
	}

	private new void Update()
	{
		CubePhys cubePhysType = Kube.WHS.GetCubePhysType(base.transform.position - Vector3.up * 0.5f);
		bool flag = cubePhysType == CubePhys.air;
		if ((bool)jetPack)
		{
			jetPack.SendMessage("PlayStop", flag, SendMessageOptions.DontRequireReceiver);
		}
		base.Update();
	}
}
