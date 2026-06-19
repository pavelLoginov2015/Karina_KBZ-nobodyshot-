using System;
using kube;
using UnityEngine;

// Token: 0x020002C0 RID: 704
public class PathFinderMoveScript : PathFinderScript
{
    // Token: 0x060015DE RID: 5598 RVA: 0x0009A288 File Offset: 0x00098488
    // Note: this type is marked as 'beforefieldinit'.
    static PathFinderMoveScript()
    {
        int[] array = new int[6];
        array[0] = 1;
        array[2] = -1;
        PathFinderMoveScript.nX1 = array;
        PathFinderMoveScript.nY1 = new int[]
        {
            0,
            0,
            0,
            0,
            1,
            -1
        };
        int[] array2 = new int[6];
        array2[1] = 1;
        array2[3] = -1;
        PathFinderMoveScript.nZ1 = array2;
        PathFinderMoveScript.wt1 = new int[]
        {
            10,
            10,
            10,
            10,
            10,
            10
        };
        PathFinderMoveScript.nX2 = new int[]
        {
            1,
            -1,
            -1,
            1
        };
        PathFinderMoveScript.nY2 = new int[4];
        PathFinderMoveScript.nZ2 = new int[]
        {
            1,
            1,
            -1,
            -1
        };
        PathFinderMoveScript.wt2 = new int[]
        {
            14,
            14,
            14,
            14
        };
        int[] array3 = new int[4];
        array3[0] = 1;
        array3[2] = -1;
        PathFinderMoveScript.nX3 = array3;
        PathFinderMoveScript.nY3 = new int[4];
        PathFinderMoveScript.nZ3 = new int[]
        {
            0,
            1,
            0,
            -1
        };
        PathFinderMoveScript.wt3 = new int[]
        {
            10,
            10,
            10,
            10
        };
    }

    // Token: 0x060015DF RID: 5599 RVA: 0x0009A378 File Offset: 0x00098578
    public override void SetPathFinderParams(float _speed, float _jumpSpeed, int _charSizeY)
    {
        this.speed = _speed;
        this.jumpSpeed = _jumpSpeed;
        if (this.jumpSpeed < 5f)
        {
            this.jumpCubes = 0;
        }
        else if (this.jumpSpeed < 7f)
        {
            this.jumpCubes = 1;
        }
        else if (this.jumpSpeed < 9f)
        {
            this.jumpCubes = 2;
        }
        else if ((float)this.jumpCubes < 11f)
        {
            this.jumpCubes = 3;
        }
        else
        {
            this.jumpCubes = 4;
        }
        this.charSizeY = _charSizeY;
        this.isMine = true;
    }

    // Token: 0x060015E0 RID: 5600 RVA: 0x00010E55 File Offset: 0x0000F055
    protected int GetDistToTarget(int x1, int y1, int z1, int x2, int y2, int z2)
    {
        return (Mathf.Abs(x2 - x1) + Mathf.Abs(y2 - y1) + Mathf.Abs(z2 - z1)) * 10;
    }

    // Token: 0x060015E1 RID: 5601 RVA: 0x00010E77 File Offset: 0x0000F077
    protected int GetElementValue(int d1, int d2)
    {
        return d1 + d2;
    }

    // Token: 0x060015E2 RID: 5602 RVA: 0x0009A41C File Offset: 0x0009861C
    protected void FindPathToVector3Walking(Vector3 pos)
    {
        int x = Mathf.RoundToInt(this.lastPathPoint.x);
        int y = Mathf.RoundToInt(this.lastPathPoint.y);
        int z = Mathf.RoundToInt(this.lastPathPoint.z);
        int num = Mathf.RoundToInt(pos.x);
        int num2 = Mathf.RoundToInt(pos.y);
        int num3 = Mathf.RoundToInt(pos.z);
        this.PFAS.ClearArray();
        this.PFAS.openedArrayNum = 1;
        this.PFAS.openedArray[0].x = x;
        this.PFAS.openedArray[0].y = y;
        this.PFAS.openedArray[0].z = z;
        this.PFAS.openedArray[0].parent = -1;
        this.PFAS.openedArray[0].stepNum = 0;
        this.PFAS.openedArray[0].isClosed = false;
        this.PFAS.openedArray[0].distFromSource = 0;
        this.PFAS.openedArray[0].distToTarget = this.GetDistToTarget(this.PFAS.openedArray[0].x, this.PFAS.openedArray[0].y, this.PFAS.openedArray[0].z, num, num2, num3);
        int num4 = -1;
        int num5 = 0;
        for (; ; )
        {
            num5++;
            if (num5 > this.maxIterations)
            {
                break;
            }
            float num6 = 9999999f;
            num4 = -1;
            for (int i = 0; i < this.PFAS.openedArrayNum; i++)
            {
                if (!this.PFAS.openedArray[i].isClosed)
                {
                    float num7 = (float)this.GetElementValue(this.PFAS.openedArray[i].distFromSource, this.PFAS.openedArray[i].distToTarget);
                    if (num7 < num6)
                    {
                        num4 = i;
                        num6 = num7;
                    }
                }
            }
            if (num4 == -1)
            {
                goto Block_5;
            }
            this.PFAS.openedArray[num4].cannotStop = false;
            if (this.PFAS.openedArray[num4].x == num && this.PFAS.openedArray[num4].y == num2 && this.PFAS.openedArray[num4].z == num3)
            {
                goto Block_8;
            }
            this.PFAS.openedArray[num4].isClosed = true;
            this.PFAS.closedArray[this.PFAS.closedArrayNum] = this.PFAS.openedArray[num4];
            this.PFAS.closedArrayNum++;
            if (this.PFAS.openedArray[num4].stepNum <= this.maxPath)
            {
                CubePhys cubePhysType = Kube.WHS.GetCubePhysType(this.PFAS.openedArray[num4].x, this.PFAS.openedArray[num4].y, this.PFAS.openedArray[num4].z);
                CubePhys cubePhysType2 = Kube.WHS.GetCubePhysType(this.PFAS.openedArray[num4].x, this.PFAS.openedArray[num4].y - 1, this.PFAS.openedArray[num4].z);
                int xo = Mathf.RoundToInt((float)PFAS.openedArray[num4].x);
                int yo = Mathf.RoundToInt((float)(PFAS.openedArray[num4].y - 1));
                int zo = Mathf.RoundToInt((float)PFAS.openedArray[num4].z);
                if ( num4 < PFAS.openedArray.Length && cubePhysType == CubePhys.air && cubePhysType2 == CubePhys.air )
                {
                    int j;
                    for (j = this.PFAS.openedArray[num4].y - 1; j >= 0; j--)
                    {
                        CubePhys cubePhysType3 = Kube.WHS.GetCubePhysType(this.PFAS.openedArray[num4].x, j, this.PFAS.openedArray[num4].z);
                        if ((cubePhysType3 != CubePhys.air && cubePhysType3 != CubePhys.water) || Kube.WHS.isOccupied[this.PFAS.openedArray[num4].x, j, this.PFAS.openedArray[num4].z])
                        {
                            break;
                        }
                    }
                    int x2 = this.PFAS.openedArray[num4].x;
                    int num8 = j + 1;
                    int z2 = this.PFAS.openedArray[num4].z;
                    this.PFAS.openedArray[num4].cannotStop = true;
                    this.PFAS.openedArray[this.PFAS.openedArrayNum].distFromSource = this.PFAS.openedArray[num4].distFromSource + PathFinderMoveScript.wt1[0];
                    this.PFAS.openedArray[this.PFAS.openedArrayNum].distToTarget = this.GetDistToTarget(x2, num8, z2, num, num2, num3);
                    this.PFAS.openedArray[this.PFAS.openedArrayNum].isClosed = false;
                    this.PFAS.openedArray[this.PFAS.openedArrayNum].parent = num4;
                    this.PFAS.openedArray[this.PFAS.openedArrayNum].stepNum = this.PFAS.openedArray[num4].stepNum + 1;
                    this.PFAS.openedArray[this.PFAS.openedArrayNum].x = x2;
                    this.PFAS.openedArray[this.PFAS.openedArrayNum].y = num8;
                    this.PFAS.openedArray[this.PFAS.openedArrayNum].z = z2;
                    this.PFAS.openedArrayNum++;
                }
                else
                {
                    for (int k = 0; k < PathFinderMoveScript.nX1.Length; k++)
                    {
                        int num9 = this.PFAS.openedArray[num4].x + PathFinderMoveScript.nX1[k];
                        int num10 = this.PFAS.openedArray[num4].y + PathFinderMoveScript.nY1[k];
                        int num11 = this.PFAS.openedArray[num4].z + PathFinderMoveScript.nZ1[k];
                        if (Kube.WHS.IsInWorld(num9, num10, num11))
                        {
                            CubePhys cubePhysType4 = Kube.WHS.GetCubePhysType(num9, num10 - 1, num11);
                            if (PathFinderMoveScript.nY1[k] == 0 && cubePhysType4 == CubePhys.air && !Kube.WHS.isOccupied[num9, num10 - 1, num11])
                            {
                                cubePhysType4 = Kube.WHS.GetCubePhysType(this.PFAS.openedArray[num4].x, this.PFAS.openedArray[num4].y - 1, this.PFAS.openedArray[num4].z);
                                if (cubePhysType4 == CubePhys.air)
                                {
                                    goto IL_B61;
                                }
                                num10--;
                            }
                            if (!Kube.WHS.isOccupied[num9, num10, num11])
                            {
                                bool flag = false;
                                for (int l = 0; l < this.PFAS.openedArrayNum; l++)
                                {
                                    if (num9 == this.PFAS.openedArray[l].x && num10 == this.PFAS.openedArray[l].y && num11 == this.PFAS.openedArray[l].z)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (!flag)
                                {
                                    bool flag2 = false;
                                    for (int m = 0; m < this.PFAS.closedArrayNum; m++)
                                    {
                                        if (num9 == this.PFAS.closedArray[m].x && num10 == this.PFAS.closedArray[m].y && num11 == this.PFAS.closedArray[m].z)
                                        {
                                            flag2 = true;
                                            break;
                                        }
                                    }
                                    if (!flag2)
                                    {
                                        bool flag3 = true;
                                        for (int n = 0; n < this.charSizeY; n++)
                                        {
                                            CubePhys cubePhysType5 = Kube.WHS.GetCubePhysType(num9, num10 + n, num11);
                                            if (cubePhysType5 == CubePhys.solid || cubePhysType5 == CubePhys.ice || Kube.WHS.isOccupied[num9, num10 + n, num11])
                                            {
                                                flag3 = false;
                                                break;
                                            }
                                        }
                                        if (flag3)
                                        {
                                            CubePhys cubePhysType6 = Kube.WHS.GetCubePhysType(num9, num10, num11);
                                            if (PathFinderMoveScript.nY1[k] <= 0 || cubePhysType6 == CubePhys.water || cubePhysType6 == CubePhys.ledder)
                                            {
                                                this.PFAS.openedArray[this.PFAS.openedArrayNum].distFromSource = this.PFAS.openedArray[num4].distFromSource + PathFinderMoveScript.wt1[k];
                                                this.PFAS.openedArray[this.PFAS.openedArrayNum].distToTarget = this.GetDistToTarget(num9, num10, num11, num, num2, num3);
                                                this.PFAS.openedArray[this.PFAS.openedArrayNum].isClosed = false;
                                                this.PFAS.openedArray[this.PFAS.openedArrayNum].parent = num4;
                                                this.PFAS.openedArray[this.PFAS.openedArrayNum].stepNum = this.PFAS.openedArray[num4].stepNum + 1;
                                                this.PFAS.openedArray[this.PFAS.openedArrayNum].x = num9;
                                                this.PFAS.openedArray[this.PFAS.openedArrayNum].y = num10;
                                                this.PFAS.openedArray[this.PFAS.openedArrayNum].z = num11;
                                                this.PFAS.openedArrayNum++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    IL_B61:;
                    }
                    for (int num12 = 0; num12 < PathFinderMoveScript.nX2.Length; num12++)
                    {
                        int num13 = this.PFAS.openedArray[num4].x + PathFinderMoveScript.nX2[num12];
                        int num14 = this.PFAS.openedArray[num4].y + PathFinderMoveScript.nY2[num12];
                        int num15 = this.PFAS.openedArray[num4].z + PathFinderMoveScript.nZ2[num12];
                        if (Kube.WHS.IsInWorld(num13, num14, num15))
                        {
                            CubePhys cubePhysType7 = Kube.WHS.GetCubePhysType(num13, num14 - 1, num15);
                            if (PathFinderMoveScript.nY2[num12] == 0 && cubePhysType7 == CubePhys.air && !Kube.WHS.isOccupied[num13, num14 - 1, num15])
                            {
                                cubePhysType7 = Kube.WHS.GetCubePhysType(this.PFAS.openedArray[num4].x, this.PFAS.openedArray[num4].y - 1, this.PFAS.openedArray[num4].z);
                                if (cubePhysType7 == CubePhys.air)
                                {
                                    goto IL_10B4;
                                }
                                num14--;
                            }
                            if (!Kube.WHS.isOccupied[num13, num14, num15])
                            {
                                bool flag4 = false;
                                for (int num16 = 0; num16 < this.PFAS.openedArrayNum; num16++)
                                {
                                    if (num13 == this.PFAS.openedArray[num16].x && num14 == this.PFAS.openedArray[num16].y && num15 == this.PFAS.openedArray[num16].z)
                                    {
                                        flag4 = true;
                                        break;
                                    }
                                }
                                if (!flag4)
                                {
                                    bool flag5 = false;
                                    for (int num17 = 0; num17 < this.PFAS.closedArrayNum; num17++)
                                    {
                                        if (num13 == this.PFAS.closedArray[num17].x && num14 == this.PFAS.closedArray[num17].y && num15 == this.PFAS.closedArray[num17].z)
                                        {
                                            flag5 = true;
                                            break;
                                        }
                                    }
                                    if (!flag5)
                                    {
                                        bool flag6 = true;
                                        for (int num18 = 0; num18 < this.charSizeY; num18++)
                                        {
                                            CubePhys cubePhysType8 = Kube.WHS.GetCubePhysType(num13, num14 + num18, num15);
                                            if (cubePhysType8 == CubePhys.solid || cubePhysType8 == CubePhys.ice || Kube.WHS.isOccupied[num13, num14 + num18, num15])
                                            {
                                                flag6 = false;
                                                break;
                                            }
                                        }
                                        for (int num19 = 0; num19 < this.charSizeY; num19++)
                                        {
                                            CubePhys cubePhysType9 = Kube.WHS.GetCubePhysType(num13 - PathFinderMoveScript.nX2[num12], num14 + num19, num15);
                                            if (cubePhysType9 == CubePhys.solid || cubePhysType9 == CubePhys.ice || Kube.WHS.isOccupied[num13 - PathFinderMoveScript.nX2[num12], num14 + num19, num15])
                                            {
                                                flag6 = false;
                                                break;
                                            }
                                        }
                                        for (int num20 = 0; num20 < this.charSizeY; num20++)
                                        {
                                            CubePhys cubePhysType10 = Kube.WHS.GetCubePhysType(num13, num14 + num20, num15 - PathFinderMoveScript.nZ2[num12]);
                                            if (cubePhysType10 == CubePhys.solid || cubePhysType10 == CubePhys.ice || Kube.WHS.isOccupied[num13, num14 + num20, num15 - PathFinderMoveScript.nZ2[num12]])
                                            {
                                                flag6 = false;
                                                break;
                                            }
                                        }
                                        if (flag6)
                                        {
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].distFromSource = this.PFAS.openedArray[num4].distFromSource + PathFinderMoveScript.wt2[num12];
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].distToTarget = this.GetDistToTarget(num13, num14, num15, num, num2, num3);
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].isClosed = false;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].parent = num4;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].stepNum = this.PFAS.openedArray[num4].stepNum + 1;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].x = num13;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].y = num14;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].z = num15;
                                            this.PFAS.openedArrayNum++;
                                        }
                                    }
                                }
                            }
                        }
                    IL_10B4:;
                    }
                    int num21;
                    for (num21 = 0; num21 < this.jumpCubes + this.charSizeY; num21++)
                    {
                        CubePhys cubePhysType11 = Kube.WHS.GetCubePhysType(this.PFAS.openedArray[num4].x, this.PFAS.openedArray[num4].y + num21, this.PFAS.openedArray[num4].z);
                        if (cubePhysType11 == CubePhys.solid || cubePhysType11 == CubePhys.ice)
                        {
                            break;
                        }
                    }
                    num21 -= this.charSizeY;
                    if (num21 >= 1)
                    {
                        for (int num22 = 0; num22 < PathFinderMoveScript.nX3.Length; num22++)
                        {
                            int num23 = this.PFAS.openedArray[num4].x + PathFinderMoveScript.nX3[num22];
                            int num24 = this.PFAS.openedArray[num4].y + PathFinderMoveScript.nY3[num22];
                            int num25 = this.PFAS.openedArray[num4].z + PathFinderMoveScript.nZ3[num22];
                            if (Kube.WHS.IsInWorld(num23, num24, num25))
                            {
                                bool flag7 = false;
                                for (int num26 = 0; num26 < this.PFAS.openedArrayNum; num26++)
                                {
                                    if (num23 == this.PFAS.openedArray[num26].x && num24 == this.PFAS.openedArray[num26].y && num25 == this.PFAS.openedArray[num26].z)
                                    {
                                        flag7 = true;
                                        break;
                                    }
                                }
                                if (!flag7)
                                {
                                    bool flag8 = false;
                                    for (int num27 = 0; num27 < this.PFAS.closedArrayNum; num27++)
                                    {
                                        if (num23 == this.PFAS.closedArray[num27].x && num24 == this.PFAS.closedArray[num27].y && num25 == this.PFAS.closedArray[num27].z)
                                        {
                                            flag8 = true;
                                            break;
                                        }
                                    }
                                    if (!flag8)
                                    {
                                        bool flag9 = true;
                                        int num28;
                                        for (num28 = 1; num28 <= num21; num28++)
                                        {
                                            flag9 = true;
                                            for (int num29 = -1; num29 < this.charSizeY; num29++)
                                            {
                                                CubePhys cubePhysType12 = Kube.WHS.GetCubePhysType(num23, num24 + num29 + num28, num25);
                                                if (num29 == -1 && cubePhysType12 != CubePhys.solid && cubePhysType12 != CubePhys.ice && cubePhysType12 != CubePhys.ledder && cubePhysType12 != CubePhys.water && !Kube.WHS.isOccupied[num23, num24 + num29 + num28, num25])
                                                {
                                                    flag9 = false;
                                                    break;
                                                }
                                                if (num29 >= 0 && (cubePhysType12 == CubePhys.solid || cubePhysType12 == CubePhys.ice || Kube.WHS.isOccupied[num23, num24 + num29 + num28, num25]))
                                                {
                                                    flag9 = false;
                                                    break;
                                                }
                                            }
                                            if (flag9)
                                            {
                                                break;
                                            }
                                        }
                                        if (flag9)
                                        {
                                            for (int num30 = 1; num30 < num28 + this.charSizeY; num30++)
                                            {
                                                CubePhys cubePhysType13 = Kube.WHS.GetCubePhysType(this.PFAS.openedArray[num4].x, this.PFAS.openedArray[num4].y + num30, this.PFAS.openedArray[num4].z);
                                                if (cubePhysType13 == CubePhys.solid || cubePhysType13 == CubePhys.ice || Kube.WHS.isOccupied[this.PFAS.openedArray[num4].x, this.PFAS.openedArray[num4].y + num30, this.PFAS.openedArray[num4].z])
                                                {
                                                    flag9 = false;
                                                    break;
                                                }
                                            }
                                        }
                                        if (flag9)
                                        {
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].distFromSource = this.PFAS.openedArray[num4].distFromSource + PathFinderMoveScript.wt3[num22];
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].distToTarget = this.GetDistToTarget(num23, num24 + num28, num25, num, num2, num3);
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].isClosed = false;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].parent = num4;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].stepNum = this.PFAS.openedArray[num4].stepNum + 1;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].x = num23;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].y = num24 + num28;
                                            this.PFAS.openedArray[this.PFAS.openedArrayNum].z = num25;
                                            this.PFAS.openedArrayNum++;
                                        }
                                        for (int num31 = 2; num31 <= num21 + 1; num31++)
                                        {
                                            num23 = this.PFAS.openedArray[num4].x + PathFinderMoveScript.nX3[num22] * num31;
                                            num24 = this.PFAS.openedArray[num4].y + PathFinderMoveScript.nY3[num22] * num31;
                                            num25 = this.PFAS.openedArray[num4].z + PathFinderMoveScript.nZ3[num22] * num31;
                                            if (Kube.WHS.IsInWorld(num23, num24, num25))
                                            {
                                                bool flag10 = true;
                                                for (int num32 = 0; num32 <= num31; num32++)
                                                {
                                                    for (int num33 = 0; num33 < this.charSizeY + num21; num33++)
                                                    {
                                                        int num34 = this.PFAS.openedArray[num4].x + PathFinderMoveScript.nX3[num22] * num32;
                                                        int num35 = this.PFAS.openedArray[num4].y + PathFinderMoveScript.nY3[num22] * num32 + num33;
                                                        int num36 = this.PFAS.openedArray[num4].z + PathFinderMoveScript.nZ3[num22] * num32;
                                                        CubePhys cubePhysType14 = Kube.WHS.GetCubePhysType(num34, num35, num36);
                                                        if (cubePhysType14 == CubePhys.solid || Kube.WHS.isOccupied[num34, num35, num36])
                                                        {
                                                            flag10 = false;
                                                            break;
                                                        }
                                                    }
                                                    if (!flag10)
                                                    {
                                                        break;
                                                    }
                                                }
                                                if (flag10)
                                                {
                                                    this.PFAS.openedArray[this.PFAS.openedArrayNum].distFromSource = this.PFAS.openedArray[num4].distFromSource + PathFinderMoveScript.wt3[num22];
                                                    this.PFAS.openedArray[this.PFAS.openedArrayNum].distToTarget = this.GetDistToTarget(num23, num24, num25, num, num2, num3);
                                                    this.PFAS.openedArray[this.PFAS.openedArrayNum].isClosed = false;
                                                    this.PFAS.openedArray[this.PFAS.openedArrayNum].parent = num4;
                                                    this.PFAS.openedArray[this.PFAS.openedArrayNum].stepNum = this.PFAS.openedArray[num4].stepNum + 1;
                                                    this.PFAS.openedArray[this.PFAS.openedArrayNum].x = num23;
                                                    this.PFAS.openedArray[this.PFAS.openedArrayNum].y = num24;
                                                    this.PFAS.openedArray[this.PFAS.openedArrayNum].z = num25;
                                                    this.PFAS.openedArrayNum++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        int num37 = 1;
        goto IL_1976;
    Block_5:
        num37 = 1;
        goto IL_1976;
    Block_8:
        num37 = 2;
    IL_1976:
        if (num37 == 2)
        {
            int num38 = num4;
            this.pathLength = this.PFAS.openedArray[num38].stepNum + 1;
            while (num38 != -1)
            {
                if (this.PFAS.openedArray[num38].parent != -1)
                {
                    this.path[this.pathLength - this.PFAS.openedArray[num38].stepNum - 1] = new Vector3((float)this.PFAS.openedArray[num38].x, (float)this.PFAS.openedArray[num38].y, (float)this.PFAS.openedArray[num38].z);
                }
                num38 = this.PFAS.openedArray[num38].parent;
            }
            this.pathLength--;
        }
        else
        {
            float num39 = 9999999f;
            num4 = -1;
            for (int num40 = 0; num40 < this.PFAS.openedArrayNum; num40++)
            {
                if (!this.PFAS.openedArray[num40].cannotStop)
                {
                    float num41 = (float)this.PFAS.openedArray[num40].distToTarget;
                    if (num41 < num39)
                    {
                        num4 = num40;
                        num39 = num41;
                    }
                }
            }
            int num42 = num4;
            if (num42 >= 0)
            {
                this.pathLength = this.PFAS.openedArray[num42].stepNum + 1;
                while (num42 != -1)
                {
                    if (this.PFAS.openedArray[num42].parent != -1)
                    {
                        this.path[this.pathLength - this.PFAS.openedArray[num42].stepNum - 1] = new Vector3((float)this.PFAS.openedArray[num42].x, (float)this.PFAS.openedArray[num42].y, (float)this.PFAS.openedArray[num42].z);
                    }
                    num42 = this.PFAS.openedArray[num42].parent;
                }
                this.pathLength--;
            }
        }
    }

    // Token: 0x060015E3 RID: 5603 RVA: 0x0009C00C File Offset: 0x0009A20C
    public override bool CanPathTo(Vector3 targetPos)
    {
        if (this.pathLength > 0)
        {
            return true;
        }
        this.FindPathToVector3Walking(targetPos);
        return this.pathLength != 0 && (targetPos - this.path[0]).magnitude <= 1f;
    }

    // Token: 0x060015E4 RID: 5604 RVA: 0x0009C068 File Offset: 0x0009A268
    public override void WalkingFollowTarget(Vector3 targetPos)
    {
        if (Time.time - this.lastRefindPath > this.refindPathPeriod && !this.flagJumpUp && !this.flagJumpAcross && !this.flagFall)
        {
            this.tx = Mathf.RoundToInt(this.lastOccupiedPoint.x);
            this.ty = Mathf.RoundToInt(this.lastOccupiedPoint.y);
            this.tz = Mathf.RoundToInt(this.lastOccupiedPoint.z);
            Kube.WHS.isOccupied[this.tx, this.ty, this.tz] = false;
            this.FindPathToVector3Walking(targetPos);
            if (this.pathLength == 0)
            {
                this.tx = Mathf.RoundToInt(this.lastOccupiedPoint.x);
                this.ty = Mathf.RoundToInt(this.lastOccupiedPoint.y);
                this.tz = Mathf.RoundToInt(this.lastOccupiedPoint.z);
                Kube.WHS.isOccupied[this.tx, this.ty, this.tz] = true;
            }
            this.flagJumpUp = (this.flagJumpAcross = (this.flagFall = false));
            this.lastRefindPath = Time.time - UnityEngine.Random.Range(0f, 0.5f);
        }
        if (this.pathLength > 0)
        {
            this.tx = Mathf.RoundToInt(this.lastOccupiedPoint.x);
            this.ty = Mathf.RoundToInt(this.lastOccupiedPoint.y);
            this.tz = Mathf.RoundToInt(this.lastOccupiedPoint.z);
            Kube.WHS.isOccupied[this.tx, this.ty, this.tz] = false;
            this.lastOccupiedPoint = this.path[this.pathLength - 1];
            this.tx = Mathf.RoundToInt(this.lastOccupiedPoint.x);
            this.ty = Mathf.RoundToInt(this.lastOccupiedPoint.y);
            this.tz = Mathf.RoundToInt(this.lastOccupiedPoint.z);
            Kube.WHS.isOccupied[this.tx, this.ty, this.tz] = true;
            Vector3 vector = this.path[this.pathLength - 1] + Vector3.up * this.deltaHeightTransform;
            base.transform.LookAt(new Vector3(vector.x, base.transform.position.y, vector.z));
            int num = Kube.WHS.cubeTypes[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z)];
            if (num == 156 || num == 157)
            {
                vector += Vector3.up * 0.5f;
            }
            bool flag = false;
            for (int i = 0; i < this.charSizeY; i++)
            {
                if (Kube.WHS.cubes[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y) + i, Mathf.RoundToInt(vector.z)].prop == CubeProps.closedDoor)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                if (Time.time - this.lastDoorOpenTime > this.doorOpenDeltaTime)
                {
                    Collider[] array = Physics.OverlapSphere(base.transform.position, 2f);
                    for (int j = 0; j < array.Length; j++)
                    {
                        array[j].gameObject.transform.root.gameObject.SendMessage("MonsterHit", base.transform.position, SendMessageOptions.DontRequireReceiver);
                    }
                    this.lastDoorOpenTime = Time.time;
                }
            }
            else if (this.flagJumpAcross)
            {
                Vector3 a = new Vector3(this.flagTo.x - this.flagFrom.x, 0f, this.flagTo.z - this.flagFrom.z);
                Vector3 axis = new Vector3(a.z, 0f, -a.x);
                float num2 = Mathf.Min((Time.time - this.flagTimeFrom) / (this.flagTimeTo - this.flagTimeFrom), 1f);
                Vector3 position = (this.flagTo + this.flagFrom) * 0.5f + Quaternion.AngleAxis(180f * num2, axis) * (-a * 0.5f) + Vector3.up * this.deltaHeightTransform;
                base.transform.position = position;
            }
            else if (this.flagFall)
            {
                float num3 = Mathf.Min(Time.time - this.flagTimeFrom, this.flagTimeTo - this.flagTimeFrom);
                float d = -this.speed * num3 + Physics.gravity.y * num3 * num3 / 2f;
                base.transform.position = this.flagFrom + Vector3.up * d + Vector3.up * this.deltaHeightTransform;
            }
            else
            {
                base.transform.Translate((vector - base.transform.position).normalized * this.speed * Time.deltaTime, Space.World);
            }
            if (Vector3.Distance(base.transform.position, vector) < this.speed * Time.deltaTime * 2f)
            {
                this.lastPathPoint = this.path[this.pathLength - 1];
                this.pathLength--;
                this.flagJumpUp = (this.flagJumpAcross = (this.flagFall = false));
                if (this.pathLength > 0 && Kube.WHS.isOccupied[Mathf.RoundToInt(this.path[this.pathLength - 1].x), Mathf.RoundToInt(this.path[this.pathLength - 1].y), Mathf.RoundToInt(this.path[this.pathLength - 1].z)])
                {
                    this.pathLength = 0;
                    return;
                }
                if (this.pathLength == 0 || Time.time - this.lastRefindPath > this.refindPathPeriod)
                {
                    this.FindPathToVector3Walking(targetPos);
                    this.flagJumpUp = (this.flagJumpAcross = (this.flagFall = false));
                    this.lastRefindPath = Time.time - UnityEngine.Random.Range(0f, 0.5f);
                }
                if (this.pathLength > 0)
                {
                    float num4 = Mathf.Sqrt((this.path[this.pathLength - 1].x - this.lastPathPoint.x) * (this.path[this.pathLength - 1].x - this.lastPathPoint.x) + (this.path[this.pathLength - 1].z - this.lastPathPoint.z) * (this.path[this.pathLength - 1].z - this.lastPathPoint.z));
                    if (this.path[this.pathLength - 1].y == this.lastPathPoint.y && num4 >= 2f && num4 <= (float)(this.jumpCubes + 1))
                    {
                        this.flagJumpAcross = true;
                        this.flagFrom = this.lastPathPoint;
                        this.flagTo = this.path[this.pathLength - 1];
                        this.flagTimeFrom = Time.time;
                        this.flagTimeTo = Time.time + num4 / (2f * this.speed);
                    }
                    if (num4 == 0f && this.path[this.pathLength - 1].y + 1f < this.lastPathPoint.y)
                    {
                        this.flagFall = true;
                        this.flagFrom = this.lastPathPoint;
                        this.flagTo = this.path[this.pathLength - 1];
                        this.flagTimeFrom = Time.time;
                        float num5 = this.lastPathPoint.y - this.path[this.pathLength - 1].y;
                        this.flagTimeTo = Time.time + (-this.speed + Mathf.Sqrt(this.speed * this.speed - 2f * Physics.gravity.y * num5)) / -Physics.gravity.y;
                    }
                }
            }
        }
    }

    // Token: 0x060015E5 RID: 5605 RVA: 0x0009C9A0 File Offset: 0x0009ABA0
    protected void Start()
    {
        this.PFAS = GameObject.FindGameObjectWithTag("PathFinder").GetComponent<PathFinderArrayScript>();
        this.lastOccupiedPoint = (this.lastPathPoint = base.transform.position);
    }

    // Token: 0x060015E6 RID: 5606 RVA: 0x0009C9DC File Offset: 0x0009ABDC
    protected void Update()
    {
        Vector3 vector = base.transform.position - Vector3.up * this.deltaHeightTransform;
        if (!this.isMine && Kube.WHS.IsInWorld(Mathf.RoundToInt(this.lastOccupiedPoint.x), Mathf.RoundToInt(this.lastOccupiedPoint.y), Mathf.RoundToInt(this.lastOccupiedPoint.z)) && Kube.WHS.IsInWorld(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z)) && Kube.WHS.isOccupied != null)
        {
            Kube.WHS.isOccupied[Mathf.RoundToInt(this.lastOccupiedPoint.x), Mathf.RoundToInt(this.lastOccupiedPoint.y), Mathf.RoundToInt(this.lastOccupiedPoint.z)] = false;
            this.lastOccupiedPoint = vector;
            Kube.WHS.isOccupied[Mathf.RoundToInt(this.lastOccupiedPoint.x), Mathf.RoundToInt(this.lastOccupiedPoint.y), Mathf.RoundToInt(this.lastOccupiedPoint.z)] = true;
        }
    }

    // Token: 0x060015E7 RID: 5607 RVA: 0x0009CB20 File Offset: 0x0009AD20
    private void OnDestroy()
    {
        if (Kube.WHS != null && Kube.WHS.IsInWorld(Mathf.RoundToInt(this.lastOccupiedPoint.x), Mathf.RoundToInt(this.lastOccupiedPoint.y), Mathf.RoundToInt(this.lastOccupiedPoint.z)) && Kube.WHS.isOccupied != null)
        {
            Kube.WHS.isOccupied[Mathf.RoundToInt(this.lastOccupiedPoint.x), Mathf.RoundToInt(this.lastOccupiedPoint.y), Mathf.RoundToInt(this.lastOccupiedPoint.z)] = false;
        }
    }

    // Token: 0x04001756 RID: 5974
    public float deltaHeightTransform = -0.45f;

    // Token: 0x04001757 RID: 5975
    private float jumpSpeed = 5f;

    // Token: 0x04001758 RID: 5976
    public int jumpCubes = 1;

    // Token: 0x04001759 RID: 5977
    private float speed = 4f;

    // Token: 0x0400175A RID: 5978
    protected int charSizeY = 2;

    // Token: 0x0400175B RID: 5979
    private bool isMine;

    // Token: 0x0400175C RID: 5980
    protected PathFinderArrayScript PFAS;

    // Token: 0x0400175D RID: 5981
    protected Vector3[] path = new Vector3[100];

    // Token: 0x0400175E RID: 5982
    protected int pathLength;

    // Token: 0x0400175F RID: 5983
    public static int[] nX1;

    // Token: 0x04001760 RID: 5984
    public static int[] nY1;

    // Token: 0x04001761 RID: 5985
    public static int[] nZ1;

    // Token: 0x04001762 RID: 5986
    public static int[] wt1;

    // Token: 0x04001763 RID: 5987
    public static int[] nX2;

    // Token: 0x04001764 RID: 5988
    public static int[] nY2;

    // Token: 0x04001765 RID: 5989
    public static int[] nZ2;

    // Token: 0x04001766 RID: 5990
    public static int[] wt2;

    // Token: 0x04001767 RID: 5991
    public static int[] nX3;

    // Token: 0x04001768 RID: 5992
    public static int[] nY3;

    // Token: 0x04001769 RID: 5993
    public static int[] nZ3;

    // Token: 0x0400176A RID: 5994
    public static int[] wt3;

    // Token: 0x0400176B RID: 5995
    public int maxPath = 10;

    // Token: 0x0400176C RID: 5996
    public int maxIterations = 1000;

    // Token: 0x0400176D RID: 5997
    private bool needJump;

    // Token: 0x0400176E RID: 5998
    private Vector3 moveDirection;

    // Token: 0x0400176F RID: 5999
    protected bool flagJumpAcross;

    // Token: 0x04001770 RID: 6000
    protected bool flagFall;

    // Token: 0x04001771 RID: 6001
    protected bool flagJumpUp;

    // Token: 0x04001772 RID: 6002
    private Vector3 flagFrom;

    // Token: 0x04001773 RID: 6003
    private Vector3 flagTo;

    // Token: 0x04001774 RID: 6004
    private float flagTimeFrom;

    // Token: 0x04001775 RID: 6005
    private float flagTimeTo;

    // Token: 0x04001776 RID: 6006
    private int tx;

    // Token: 0x04001777 RID: 6007
    private int ty;

    // Token: 0x04001778 RID: 6008
    private int tz;

    // Token: 0x04001779 RID: 6009
    protected Vector3 lastPathPoint;

    // Token: 0x0400177A RID: 6010
    protected Vector3 lastOccupiedPoint;

    // Token: 0x0400177B RID: 6011
    private float lastDoorOpenTime;

    // Token: 0x0400177C RID: 6012
    private float doorOpenDeltaTime = 0.5f;
}
