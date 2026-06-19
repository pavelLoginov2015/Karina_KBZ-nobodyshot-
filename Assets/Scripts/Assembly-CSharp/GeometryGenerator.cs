using System;
using UnityEngine;

namespace kube.map
{
	// Token: 0x02000416 RID: 1046
	public class GeometryGenerator
	{
		// Token: 0x06001EA9 RID: 7849 RVA: 0x000E3CFC File Offset: 0x000E1EFC
		public void Init()
		{
			this.WHS = Kube.WHS;
			this.blockTypes = Kube.OH.blockTypes;
			if (this.isInitialized)
			{
				return;
			}
			this.sides3 = new int[6];
			this.sides3[0] = 0;
			this.sides3[1] = 2;
			this.sides3[2] = 1;
			this.sides3[3] = 1;
			this.sides3[4] = 1;
			this.sides3[5] = 1;
			this.normalsSide[0].x = 0f;
			this.normalsSide[0].y = 1f;
			this.normalsSide[0].z = 0f;
			this.normalsSide[1].x = 0f;
			this.normalsSide[1].y = -1f;
			this.normalsSide[1].z = 0f;
			this.normalsSide[2].x = 0f;
			this.normalsSide[2].y = 0f;
			this.normalsSide[2].z = 1f;
			this.normalsSide[3].x = 0f;
			this.normalsSide[3].y = 0f;
			this.normalsSide[3].z = -1f;
			this.normalsSide[4].x = 1f;
			this.normalsSide[4].y = 0f;
			this.normalsSide[4].z = 0f;
			this.normalsSide[5].x = -1f;
			this.normalsSide[5].y = 0f;
			this.normalsSide[5].z = 0f;
			this.vertCoords = new Vector3[6, 4];
			this.vertCoords[0, 0].x = -0.5f;
			this.vertCoords[0, 0].y = 0.5f;
			this.vertCoords[0, 0].z = -0.5f;
			this.vertCoords[0, 1].x = -0.5f;
			this.vertCoords[0, 1].y = 0.5f;
			this.vertCoords[0, 1].z = 0.5f;
			this.vertCoords[0, 2].x = 0.5f;
			this.vertCoords[0, 2].y = 0.5f;
			this.vertCoords[0, 2].z = 0.5f;
			this.vertCoords[0, 3].x = 0.5f;
			this.vertCoords[0, 3].y = 0.5f;
			this.vertCoords[0, 3].z = -0.5f;
			this.vertCoords[1, 0].x = 0.5f;
			this.vertCoords[1, 0].y = -0.5f;
			this.vertCoords[1, 0].z = -0.5f;
			this.vertCoords[1, 1].x = 0.5f;
			this.vertCoords[1, 1].y = -0.5f;
			this.vertCoords[1, 1].z = 0.5f;
			this.vertCoords[1, 2].x = -0.5f;
			this.vertCoords[1, 2].y = -0.5f;
			this.vertCoords[1, 2].z = 0.5f;
			this.vertCoords[1, 3].x = -0.5f;
			this.vertCoords[1, 3].y = -0.5f;
			this.vertCoords[1, 3].z = -0.5f;
			this.vertCoords[2, 0].x = 0.5f;
			this.vertCoords[2, 0].y = -0.5f;
			this.vertCoords[2, 0].z = 0.5f;
			this.vertCoords[2, 1].x = 0.5f;
			this.vertCoords[2, 1].y = 0.5f;
			this.vertCoords[2, 1].z = 0.5f;
			this.vertCoords[2, 2].x = -0.5f;
			this.vertCoords[2, 2].y = 0.5f;
			this.vertCoords[2, 2].z = 0.5f;
			this.vertCoords[2, 3].x = -0.5f;
			this.vertCoords[2, 3].y = -0.5f;
			this.vertCoords[2, 3].z = 0.5f;
			this.vertCoords[3, 0].x = -0.5f;
			this.vertCoords[3, 0].y = -0.5f;
			this.vertCoords[3, 0].z = -0.5f;
			this.vertCoords[3, 1].x = -0.5f;
			this.vertCoords[3, 1].y = 0.5f;
			this.vertCoords[3, 1].z = -0.5f;
			this.vertCoords[3, 2].x = 0.5f;
			this.vertCoords[3, 2].y = 0.5f;
			this.vertCoords[3, 2].z = -0.5f;
			this.vertCoords[3, 3].x = 0.5f;
			this.vertCoords[3, 3].y = -0.5f;
			this.vertCoords[3, 3].z = -0.5f;
			this.vertCoords[4, 0].x = 0.5f;
			this.vertCoords[4, 0].y = -0.5f;
			this.vertCoords[4, 0].z = -0.5f;
			this.vertCoords[4, 1].x = 0.5f;
			this.vertCoords[4, 1].y = 0.5f;
			this.vertCoords[4, 1].z = -0.5f;
			this.vertCoords[4, 2].x = 0.5f;
			this.vertCoords[4, 2].y = 0.5f;
			this.vertCoords[4, 2].z = 0.5f;
			this.vertCoords[4, 3].x = 0.5f;
			this.vertCoords[4, 3].y = -0.5f;
			this.vertCoords[4, 3].z = 0.5f;
			this.vertCoords[5, 0].x = -0.5f;
			this.vertCoords[5, 0].y = -0.5f;
			this.vertCoords[5, 0].z = 0.5f;
			this.vertCoords[5, 1].x = -0.5f;
			this.vertCoords[5, 1].y = 0.5f;
			this.vertCoords[5, 1].z = 0.5f;
			this.vertCoords[5, 2].x = -0.5f;
			this.vertCoords[5, 2].y = 0.5f;
			this.vertCoords[5, 2].z = -0.5f;
			this.vertCoords[5, 3].x = -0.5f;
			this.vertCoords[5, 3].y = -0.5f;
			this.vertCoords[5, 3].z = -0.5f;
			this.vertCoordsWaterAngle = new int[6, 4];
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (this.vertCoords[i, j].y > 0f && this.vertCoords[i, j].x > 0f && this.vertCoords[i, j].z > 0f)
					{
						this.vertCoordsWaterAngle[i, j] = 0;
					}
					else if (this.vertCoords[i, j].y > 0f && this.vertCoords[i, j].x < 0f && this.vertCoords[i, j].z > 0f)
					{
						this.vertCoordsWaterAngle[i, j] = 1;
					}
					else if (this.vertCoords[i, j].y > 0f && this.vertCoords[i, j].x < 0f && this.vertCoords[i, j].z < 0f)
					{
						this.vertCoordsWaterAngle[i, j] = 2;
					}
					else if (this.vertCoords[i, j].y > 0f && this.vertCoords[i, j].x > 0f && this.vertCoords[i, j].z < 0f)
					{
						this.vertCoordsWaterAngle[i, j] = 3;
					}
					else
					{
						this.vertCoordsWaterAngle[i, j] = -1;
					}
				}
			}
			this.isInitialized = true;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x00002FAE File Offset: 0x000011AE
		private void Update()
		{
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000E4738 File Offset: 0x000E2938
		public void SetBlock(Vector3 pos, Vector3 _startCube, Vector3 _endCube)
		{
			this.startCube.x = Mathf.Min(_startCube.x, _endCube.x);
			this.startCube.y = Mathf.Min(_startCube.y, _endCube.y);
			this.startCube.z = Mathf.Min(_startCube.z, _endCube.z);
			this.endCube.x = Mathf.Max(_startCube.x, _endCube.x);
			this.endCube.y = Mathf.Max(_startCube.y, _endCube.y);
			this.endCube.z = Mathf.Max(_startCube.z, _endCube.z);
			this.transformPosition = pos;
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000E47F4 File Offset: 0x000E29F4
		public void collect()
		{
			this.nncollected = 0;
			if (this.collected == null)
			{
				this.collected = new int[Mathf.RoundToInt((this.endCube.x - this.startCube.x) * (this.endCube.y - this.startCube.y) * (this.endCube.z - this.startCube.z))];
			}
			this.numVerts = 0;
			this.numTriangles = 0;
			this.Init();
			for (int i = (int)this.startCube.x; i < (int)this.endCube.x; i++)
			{
				for (int j = (int)this.startCube.y; j < (int)this.endCube.y; j++)
				{
					for (int k = (int)this.startCube.z; k < (int)this.endCube.z; k++)
					{
						int num = this.WHS.cubeTypes[i, j, k];
						int atlas = this.blockTypes[num].atlas;
						int num2 = 0;
						int num3 = (int)this.WHS.cubeData[i, j, k];
						if (num != 0 && num != 128)
						{
							for (int l = 0; l < 6; l++)
							{
								bool flag = true;
								int num4 = i + this.neighborX[l];
								int num5 = j + this.neighborY[l];
								int num6 = k + this.neighborZ[l];
								if (num4 <= this.WHS.sizeX - 1 && num4 >= 0 && num5 <= this.WHS.sizeY - 1 && num5 >= 0 && num6 <= this.WHS.sizeZ - 1 && num6 >= 0)
								{
									int num7 = (int)this.WHS.cubeData[num4, num5, num6];
									int num8 = this.WHS.cubeTypes[num4, num5, num6];
									if (this.WHS.cubesDrawTypes[num8] != 4 && GeometryGenerator.neighbor_has_side[num7][l] && num7 == num3)
									{
										if (this.WHS.cubesDrawTypes[num8] == 0)
										{
											flag = false;
										}
										else if (num8 == num)
										{
											flag = false;
										}
									}
									if (flag)
									{
										num2 |= 1 << l;
									}
								}
							}
							if (num2 != 0)
							{
								this.collected[this.nncollected] = (num2 | i << 24 | j << 16 | k << 8);
								this.nncollected++;
								if (atlas >= 0)
								{
									this.numAtlasBlocks[atlas]++;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x000E4A9C File Offset: 0x000E2C9C
		public void collect(CubeDrawTypes cdttype, int atlasid)
		{
			this.cdttype = cdttype;
			this.numVerts = 0;
			this.numTriangles = 0;
			this.Init();
			for (int i = 0; i < this.nncollected; i++)
			{
				int x = this.collected[i] >> 24 & 255;
				int y = this.collected[i] >> 16 & 255;
				int z = this.collected[i] >> 8 & 255;
				int num = this.collected[i] & 255;
				if (this.numVerts >= 65016)
				{
					this.nncollected = i;
					return;
				}
				int num2 = this.WHS.cubeTypes[x, y, z];
				int atlas = this.blockTypes[num2].atlas;
				if (atlasid == atlas && this.WHS.cubesDrawTypes[num2] == (int)cdttype)
				{
					int num3 = (int)this.WHS.cubeData[x, y, z];
					for (int j = 0; j < 6; j++)
					{
						if ((num >> j & 1) == 1)
						{
							num |= 1 << j;
							if (num3 == 0)
							{
								this.numVerts += 6;
								this.numTriangles += 6;
							}
							else if (num3 != 0)
							{
								this.numVerts += GeometryGenerator.geomNumVerts[num3][j];
								this.numTriangles += GeometryGenerator.geomNumTris[num3][j];
							}
						}
					}
					if (num != 0)
					{
						this.numVerts += GeometryGenerator.geomNumVerts[num3][6];
						this.numTriangles += GeometryGenerator.geomNumTris[num3][6];
					}
				}
			}
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000E4C44 File Offset: 0x000E2E44
		private void AllocBuffers()
		{
			if (this.numVerts <= this._size && this.numTriangles <= this._trisize && this._trisize - this.numTriangles <= 300)
			{
				return;
			}
			int num = this.numTriangles + 300;
			int num2 = this.numVerts + 300;
			this._trisize = num;
			this._size = num2;
			this.MeshVertices = new Vector3[num2];
			this.MeshNormals = new Vector3[num2];
			this.MeshUv = new Vector2[num2];
			this.MeshTriangles = new int[num];
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000E4CDC File Offset: 0x000E2EDC
		private void AddGeometry(Mesh mesh, int x, int y, int z, GeometryGenerator.GeomAddInfo ai)
		{
			Vector3 vector = new Vector3((float)x, (float)y, (float)z) - this.transformPosition;
			Vector3[] meshVertices = this.MeshVertices;
			Vector3[] meshNormals = this.MeshNormals;
			Vector2[] meshUv = this.MeshUv;
			int[] meshTriangles = this.MeshTriangles;
			int num = 0;
			int num2 = (int)this.WHS.cubeData[x, y, z];
			int[][] indexes = GeometryGenerator.g4_indicies_3;
			Vector3[][] points = GeometryGenerator.g4_points_3;
			Vector3[][] array = null;
			Vector2[][] array2 = null;
			if (num2 == 1)
			{
				indexes = GeometryGenerator.g1_indicies;
				points = GeometryGenerator.g1_points;
				array2 = GeometryGenerator.g1_uv;
			}
			else if (num2 == 2)
			{
				indexes = GeometryGenerator.g2_indicies;
				points = GeometryGenerator.g2_points;
				array2 = GeometryGenerator.g2_uv;
			}
			else if (num2 == 3)
			{
				indexes = GeometryGenerator.g3_indicies;
				points = GeometryGenerator.g3_points;
				array = GeometryGenerator.g3_normals;
				array2 = GeometryGenerator.g3_uv;
			}
			else if (num2 >> 2 != 0)
			{
				int num3 = 4 * ((num2 >> 2) - 1) + (num2 & 3);
				indexes = GeometryGenerator.geomInfo[num3].indexes;
				points = GeometryGenerator.geomInfo[num3].points;
				array2 = GeometryGenerator.geomInfo[num3].uv;
			}
			int num4 = this.WHS.cubeTypes[x, y, z];
			int num10;
			int num12;
			for (int i = 0; i < 6; i++)
			{
				bool flag = true;
				int num5 = x + this.neighborX[i];
				int num6 = y + this.neighborY[i];
				int num7 = z + this.neighborZ[i];
				if (num5 <= this.WHS.sizeX - 1 && num5 >= 0 && num6 <= this.WHS.sizeY - 1 && num6 >= 0 && num7 <= this.WHS.sizeZ - 1 && num7 >= 0 && num4 != 0)
				{
					int num8 = (int)this.WHS.cubeData[num5, num6, num7];
					int num9 = this.WHS.cubeTypes[num5, num6, num7];
					if (this.WHS.cubesDrawTypes[num9] != 4 && GeometryGenerator.neighbor_has_side[num8][i] && num8 == num2)
					{
						if (this.WHS.cubesDrawTypes[num9] == 0)
						{
							flag = false;
						}
						else if (num9 == num4)
						{
							flag = false;
						}
					}
					if (flag)
					{
						num |= 1 << i;
						num10 = GeometryGenerator.geomNumVerts[num2][i];
						if (num10 != 0)
						{
							int verts = ai.verts;
							for (int j = 0; j < num10; j++)
							{
								meshVertices[ai.verts] = vector + points[i][j];
								if (array != null && array[i] != null)
								{
									meshNormals[ai.verts] = array[i][j];
								}
								else
								{
									meshNormals[ai.verts] = this.normalsSide[i];
								}
								int num11 = this.blockTypes[num4].itemId;
								if (num11 < 0)
								{
									num11 = this.WHS.cubesSidesTex[-num11, this.sides3[i]];
								}
								if (array2 != null)
								{
									Vector2 vector2 = this.WHS.cubesTexUV[num11, 1];
									meshUv[ai.verts] = vector2 + array2[i][j];
								}
								else
								{
									meshUv[ai.verts] = this.WHS.cubesTexUV[num11, j];
								}
								ai.verts++;
							}
							num12 = indexes[i].Length;
							for (int k = 0; k < num12; k++)
							{
								int[] array3 = meshTriangles;
								int indexes2 = ai.indexes;
								ai.indexes = indexes2 + 1;
								array3[indexes2] = verts + indexes[i][k];
							}
						}
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			int num13 = 6;
			num10 = GeometryGenerator.geomNumVerts[num2][num13];
			if (num10 == 0)
			{
				return;
			}
			int verts2 = ai.verts;
			int num14 = this.blockTypes[num4].itemId;
			if (num14 < 0)
			{
				num14 = this.WHS.cubesSidesTex[-num14, 0];
			}
			for (int l = 0; l < num10; l++)
			{
				meshVertices[ai.verts] = vector + points[num13][l];
				if (array != null && array[num13] != null)
				{
					meshNormals[ai.verts] = array[6][l];
				}
				else
				{
					meshNormals[ai.verts] = this.normalsSide[0];
				}
				if (array2 != null)
				{
					Vector2 vector3 = this.WHS.cubesTexUV[num14, 1];
					meshUv[ai.verts] = vector3 + array2[num13][l];
				}
				else
				{
					meshUv[ai.verts] = this.WHS.cubesTexUV[num14, l % 4];
				}
				ai.verts++;
			}
			num12 = indexes[num13].Length;
			for (int m = 0; m < num12; m++)
			{
				int[] array4 = meshTriangles;
				int indexes2 = ai.indexes;
				ai.indexes = indexes2 + 1;
				array4[indexes2] = verts2 + indexes[num13][m];
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000E5204 File Offset: 0x000E3404
		public void RefreshMesh(Mesh mesh, bool UVhack, int atlas)
		{
			this._mesh = mesh;
			this.AllocBuffers();
			this.numTriangles = 0;
			this.numUVs = 0;
			this.numNormals = 0;
			if (this.numVerts == 0 && this._mesh == null)
			{
				return;
			}
			this.numVerts = 0;
			Vector3[] meshVertices = this.MeshVertices;
			Vector3[] meshNormals = this.MeshNormals;
			Vector2[] meshUv = this.MeshUv;
			int[] meshTriangles = this.MeshTriangles;
			GeometryGenerator.GeomAddInfo geomAddInfo = new GeometryGenerator.GeomAddInfo();
			for (int i = 0; i < this.nncollected; i++)
			{
				int num = this.collected[i] >> 24 & 255;
				int num2 = this.collected[i] >> 16 & 255;
				int num3 = this.collected[i] >> 8 & 255;
				int num4 = this.collected[i] & 255;
				int num5 = this.WHS.cubeTypes[num, num2, num3];
				if (atlas == this.blockTypes[num5].atlas && this.WHS.cubesDrawTypes[num5] == (int)this.cdttype)
				{
					if (this.WHS.cubeData[num, num2, num3] != 0)
					{
						geomAddInfo.indexes = this.numTriangles;
						geomAddInfo.verts = this.numVerts;
						this.AddGeometry(this._mesh, num, num2, num3, geomAddInfo);
						this.numTriangles = geomAddInfo.indexes;
						this.numVerts = geomAddInfo.verts;
						this.numUVs = this.numVerts;
						this.numNormals = this.numVerts;
					}
					else
					{
						Vector3 vector = new Vector3((float)num, (float)num2, (float)num3) - this.transformPosition;
						for (int j = 0; j < 6; j++)
						{
							if ((num4 >> j & 1) == 1)
							{
								Vector3[,] array = this.vertCoords;
								meshVertices[this.numVerts] = vector + array[j, 0];
								meshVertices[this.numVerts + 1] = vector + array[j, 1];
								meshVertices[this.numVerts + 2] = vector + array[j, 2];
								meshVertices[this.numVerts + 3] = vector + array[j, 3];
								int num6 = this.blockTypes[num5].itemId;
								if (num6 < 0)
								{
									num6 = this.WHS.cubesSidesTex[-num6, this.sides3[j]];
								}
								if (UVhack)
								{
									meshUv[this.numUVs] = Vector2.zero;
									meshUv[this.numUVs + 1] = Vector2.up;
									meshUv[this.numUVs + 2] = Vector2.one;
									meshUv[this.numUVs + 3] = Vector2.one;
								}
								else
								{
									meshUv[this.numUVs] = this.WHS.cubesTexUV[num6, 0];
									meshUv[this.numUVs + 1] = this.WHS.cubesTexUV[num6, 1];
									meshUv[this.numUVs + 2] = this.WHS.cubesTexUV[num6, 2];
									meshUv[this.numUVs + 3] = this.WHS.cubesTexUV[num6, 3];
								}
								meshNormals[this.numNormals] = this.normalsSide[j];
								meshNormals[this.numNormals + 1] = this.normalsSide[j];
								meshNormals[this.numNormals + 2] = this.normalsSide[j];
								meshNormals[this.numNormals + 3] = this.normalsSide[j];
								meshTriangles[this.numTriangles] = this.numVerts;
								meshTriangles[this.numTriangles + 1] = this.numVerts + 1;
								meshTriangles[this.numTriangles + 2] = this.numVerts + 2;
								meshTriangles[this.numTriangles + 3] = this.numVerts + 2;
								meshTriangles[this.numTriangles + 4] = this.numVerts + 3;
								meshTriangles[this.numTriangles + 5] = this.numVerts;
								this.numVerts += 4;
								this.numUVs += 4;
								this.numNormals += 4;
								this.numTriangles += 6;
							}
						}
					}
				}
			}
			for (int k = this.numVerts; k < meshVertices.Length; k++)
			{
				meshVertices[k].x = 0f;
				meshVertices[k].y = 0f;
				meshVertices[k].z = 0f;
			}
			for (int l = this.numVerts; l < meshNormals.Length; l++)
			{
				meshNormals[l].x = 0f;
				meshNormals[l].y = 0f;
				meshNormals[l].z = 0f;
			}
			for (int m = this.numVerts; m < meshUv.Length; m++)
			{
				meshUv[m].x = 0f;
				meshUv[m].y = 0f;
			}
			for (int n = this.numTriangles; n < meshTriangles.Length; n++)
			{
				meshTriangles[n] = 0;
			}
			if (atlas >= 0)
			{
				this.numSolidVerts[atlas] = this.numVerts;
			}
			this._mesh.Clear();
			this._mesh.vertices = this.MeshVertices;
			this._mesh.normals = this.MeshNormals;
			this._mesh.uv = this.MeshUv;
			this._mesh.triangles = this.MeshTriangles;
			this._mesh.RecalculateBounds();
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x000E57E0 File Offset: 0x000E39E0
		public GeometryGenerator()
		{
			int[] array = new int[6];
			array[0] = 1;
			array[1] = -1;
			this.neighborY = array;
			int[] array2 = new int[6];
			array2[2] = 1;
			array2[3] = -1;
			this.neighborZ = array2;
			this.normalsSide = new Vector3[6];
		}

		// Token: 0x0400234F RID: 9039
		private Vector3 transformPosition;

		// Token: 0x04002350 RID: 9040
		private WorldHolderScript WHS;

		// Token: 0x04002351 RID: 9041
		private bool isInitialized;

		// Token: 0x04002352 RID: 9042
		private Vector3 startCube;

		// Token: 0x04002353 RID: 9043
		private Vector3 endCube;

		// Token: 0x04002354 RID: 9044
		private ObjectsHolderScript OH;

		// Token: 0x04002355 RID: 9045
		public int[] numAtlasBlocks = new int[8];

		// Token: 0x04002356 RID: 9046
		private int[] numSolidVerts = new int[8];

		// Token: 0x04002357 RID: 9047
		private int numTransVerts;

		// Token: 0x04002358 RID: 9048
		private int numWaterVerts;

		// Token: 0x04002359 RID: 9049
		private int[] neighborX = new int[]
		{
			0,
			0,
			0,
			0,
			1,
			-1
		};

		// Token: 0x0400235A RID: 9050
		private int[] neighborY;

		// Token: 0x0400235B RID: 9051
		private int[] neighborZ;

		// Token: 0x0400235C RID: 9052
		private Vector3[] normalsSide;

		// Token: 0x0400235D RID: 9053
		private int[] sides3;

		// Token: 0x0400235E RID: 9054
		private Vector3[,] vertCoords;

		// Token: 0x0400235F RID: 9055
		private int[,] vertCoordsWaterAngle;

		// Token: 0x04002360 RID: 9056
		protected Vector3[] MeshVertices;

		// Token: 0x04002361 RID: 9057
		protected Vector3[] MeshNormals;

		// Token: 0x04002362 RID: 9058
		protected Color32[] MeshColors32;

		// Token: 0x04002363 RID: 9059
		protected Vector2[] MeshUv;

		// Token: 0x04002364 RID: 9060
		protected int[] MeshTriangles;

		// Token: 0x04002365 RID: 9061
		protected ObjectsHolderScript.BlockType[] blockTypes;

		// Token: 0x04002366 RID: 9062
		private static Vector2[] side_uv = new Vector2[]
		{
			new Vector3(0f, -0.125f),
			new Vector2(0f, 0f),
			new Vector3(0.125f, 0f),
			new Vector3(0.125f, -0.125f)
		};

		// Token: 0x04002367 RID: 9063
		private static Vector2[][] g1_uv = new Vector2[][]
		{
			GeometryGenerator.side_uv,
			new Vector2[0],
			new Vector2[]
			{
				new Vector3(0f, -0.125f),
				new Vector2(0f, -0.093f),
				new Vector3(0.125f, -0.093f),
				new Vector3(0.125f, -0.125f)
			},
			new Vector2[]
			{
				new Vector3(0f, -0.125f),
				new Vector2(0f, -0.093f),
				new Vector3(0.125f, -0.093f),
				new Vector3(0.125f, -0.125f)
			},
			new Vector2[]
			{
				new Vector3(0f, -0.125f),
				new Vector2(0f, -0.093f),
				new Vector3(0.125f, -0.093f),
				new Vector3(0.125f, -0.125f)
			},
			new Vector2[]
			{
				new Vector3(0f, -0.125f),
				new Vector2(0f, -0.093f),
				new Vector3(0.125f, -0.093f),
				new Vector3(0.125f, -0.125f)
			},
			GeometryGenerator.side_uv
		};

		// Token: 0x04002368 RID: 9064
		private static Vector3[][] g1_points = new Vector3[][]
		{
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, 0f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0f, -0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, 0f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, 0f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, 0f, -0.5f),
				new Vector3(0.5f, 0f, 0.5f),
				new Vector3(-0.5f, 0f, 0.5f),
				new Vector3(-0.5f, 0f, -0.5f)
			}
		};

		// Token: 0x04002369 RID: 9065
		private static int[][] g1_indicies = new int[][]
		{
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x0400236A RID: 9066
		private static Vector2[][] g2_uv = new Vector2[][]
		{
			new Vector2[0],
			GeometryGenerator.side_uv,
			new Vector2[]
			{
				new Vector3(0f, -0.125f),
				new Vector2(0f, -0.093f),
				new Vector3(0.125f, -0.093f),
				new Vector3(0.125f, -0.125f)
			},
			new Vector2[]
			{
				new Vector3(0f, -0.125f),
				new Vector2(0f, -0.093f),
				new Vector3(0.125f, -0.093f),
				new Vector3(0.125f, -0.125f)
			},
			new Vector2[]
			{
				new Vector3(0f, -0.125f),
				new Vector2(0f, -0.093f),
				new Vector3(0.125f, -0.093f),
				new Vector3(0.125f, -0.125f)
			},
			new Vector2[]
			{
				new Vector3(0f, -0.125f),
				new Vector2(0f, -0.093f),
				new Vector3(0.125f, -0.093f),
				new Vector3(0.125f, -0.125f)
			},
			GeometryGenerator.side_uv
		};

		// Token: 0x0400236B RID: 9067
		private static Vector3[][] g2_points = new Vector3[][]
		{
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, 0f, 0.5f),
				new Vector3(-0.5f, 0f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, 0f, -0.5f),
				new Vector3(0.5f, 0f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, 0f, -0.5f),
				new Vector3(0.5f, 0f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, 0f, 0.5f),
				new Vector3(-0.5f, 0f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0f, -0.5f),
				new Vector3(-0.5f, 0f, 0.5f),
				new Vector3(0.5f, 0f, 0.5f),
				new Vector3(0.5f, 0f, -0.5f)
			}
		};

		// Token: 0x0400236C RID: 9068
		private static int[][] g2_indicies = new int[][]
		{
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x0400236D RID: 9069
		private static Vector2[][] g3_uv = new Vector2[][]
		{
			new Vector2[]
			{
				new Vector3(0.032f, -0.093f),
				new Vector2(0.032f, -0.032f),
				new Vector3(0.093f, -0.032f),
				new Vector3(0.093f, -0.093f)
			},
			new Vector2[]
			{
				new Vector3(0.032f, -0.093f),
				new Vector2(0.032f, -0.032f),
				new Vector3(0.093f, -0.032f),
				new Vector3(0.093f, -0.093f)
			},
			new Vector2[0],
			new Vector2[0],
			new Vector2[0],
			new Vector2[0],
			new Vector2[]
			{
				new Vector3(0.032f, -0.125f),
				new Vector2(0.032f, 0f),
				new Vector3(0.093f, 0f),
				new Vector3(0.093f, -0.125f),
				new Vector3(0.032f, -0.125f),
				new Vector2(0.032f, 0f),
				new Vector3(0.093f, 0f),
				new Vector3(0.093f, -0.125f),
				new Vector3(0.032f, -0.125f),
				new Vector2(0.032f, 0f),
				new Vector3(0.093f, 0f),
				new Vector3(0.093f, -0.125f),
				new Vector3(0.032f, -0.125f),
				new Vector2(0.032f, 0f),
				new Vector3(0.093f, 0f),
				new Vector3(0.093f, -0.125f)
			}
		};

		// Token: 0x0400236E RID: 9070
		private static Vector3[][] g3_points = new Vector3[][]
		{
			new Vector3[]
			{
				new Vector3(-0.25f, 0.5f, -0.25f),
				new Vector3(-0.25f, 0.5f, 0.25f),
				new Vector3(0.25f, 0.5f, 0.25f),
				new Vector3(0.25f, 0.5f, -0.25f)
			},
			new Vector3[]
			{
				new Vector3(0.25f, -0.5f, -0.25f),
				new Vector3(0.25f, -0.5f, 0.25f),
				new Vector3(-0.25f, -0.5f, 0.25f),
				new Vector3(-0.25f, -0.5f, -0.25f)
			},
			new Vector3[0],
			new Vector3[0],
			new Vector3[0],
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.25f, -0.5f, 0.25f),
				new Vector3(0.25f, 0.5f, 0.25f),
				new Vector3(-0.25f, 0.5f, 0.25f),
				new Vector3(-0.25f, -0.5f, 0.25f),
				new Vector3(-0.25f, -0.5f, -0.25f),
				new Vector3(-0.25f, 0.5f, -0.25f),
				new Vector3(0.25f, 0.5f, -0.25f),
				new Vector3(0.25f, -0.5f, -0.25f),
				new Vector3(0.25f, -0.5f, -0.25f),
				new Vector3(0.25f, 0.5f, -0.25f),
				new Vector3(0.25f, 0.5f, 0.25f),
				new Vector3(0.25f, -0.5f, 0.25f),
				new Vector3(-0.25f, -0.5f, 0.25f),
				new Vector3(-0.25f, 0.5f, 0.25f),
				new Vector3(-0.25f, 0.5f, -0.25f),
				new Vector3(-0.25f, -0.5f, -0.25f)
			}
		};

		// Token: 0x0400236F RID: 9071
		private static Vector3[][] g3_normals = new Vector3[][]
		{
			default(Vector3[]),
			default(Vector3[]),
			default(Vector3[]),
			default(Vector3[]),
			default(Vector3[]),
			default(Vector3[]),
			new Vector3[]
			{
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, -1f),
				new Vector3(0f, 0f, -1f),
				new Vector3(0f, 0f, -1f),
				new Vector3(0f, 0f, -1f),
				new Vector3(1f, 0f, 0f),
				new Vector3(1f, 0f, 0f),
				new Vector3(1f, 0f, 0f),
				new Vector3(1f, 0f, 0f),
				new Vector3(-1f, 0f, 0f),
				new Vector3(-1f, 0f, 0f),
				new Vector3(-1f, 0f, 0f),
				new Vector3(-1f, 0f, 0f)
			}
		};

		// Token: 0x04002370 RID: 9072
		private static int[][] g3_indicies = new int[][]
		{
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[0],
			new int[0],
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0,
				4,
				5,
				6,
				6,
				7,
				4,
				8,
				9,
				10,
				10,
				11,
				8,
				12,
				13,
				14,
				14,
				15,
				12
			}
		};

		// Token: 0x04002371 RID: 9073
		public static int[][] g4_indicies_3 = new int[][]
		{
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2
			},
			new int[]
			{
				0,
				2,
				1
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x04002372 RID: 9074
		public static Vector3[][] g4_points_3 = new Vector3[][]
		{
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, -0.5f)
			}
		};

		// Token: 0x04002373 RID: 9075
		private static Vector2[] g4_side_uv = new Vector2[]
		{
			new Vector3(0f, 0f),
			new Vector2(0f, -0.125f),
			new Vector3(0.125f, -0.125f)
		};

		// Token: 0x04002374 RID: 9076
		private static Vector2[][] g4_uv_3 = new Vector2[][]
		{
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.g4_side_uv,
			GeometryGenerator.g4_side_uv,
			GeometryGenerator.side_uv
		};

		// Token: 0x04002375 RID: 9077
		public static int[][] g4_indicies_1 = new int[][]
		{
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2
			},
			new int[]
			{
				0,
				2,
				1
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x04002376 RID: 9078
		public static Vector3[][] g4_points_1 = new Vector3[][]
		{
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f)
			}
		};

		// Token: 0x04002377 RID: 9079
		private static Vector2[][] g4_uv_1 = new Vector2[][]
		{
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.g4_side_uv,
			GeometryGenerator.g4_side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv
		};

		// Token: 0x04002378 RID: 9080
		public static int[][] g4_indicies_0 = new int[][]
		{
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				2,
				1
			},
			new int[]
			{
				0,
				1,
				2
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x04002379 RID: 9081
		public static Vector3[][] g4_points_0 = new Vector3[][]
		{
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f)
			}
		};

		// Token: 0x0400237A RID: 9082
		private static Vector2[][] g4_uv_0 = new Vector2[][]
		{
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.g4_side_uv,
			GeometryGenerator.g4_side_uv,
			GeometryGenerator.side_uv
		};

		// Token: 0x0400237B RID: 9083
		public static int[][] g4_indicies_2 = new int[][]
		{
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				2,
				1
			},
			new int[]
			{
				0,
				1,
				2
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x0400237C RID: 9084
		public static Vector3[][] g4_points_2 = new Vector3[][]
		{
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			}
		};

		// Token: 0x0400237D RID: 9085
		private static Vector2[][] g4_uv_2 = new Vector2[][]
		{
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.g4_side_uv,
			GeometryGenerator.g4_side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv
		};

		// Token: 0x0400237E RID: 9086
		public static int[][] g5_indicies_3 = new int[][]
		{
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2
			},
			new int[]
			{
				0,
				2,
				1
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x0400237F RID: 9087
		public static Vector3[][] g5_points_3 = new Vector3[][]
		{
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f)
			}
		};

		// Token: 0x04002380 RID: 9088
		private static Vector2[] g5_side_uv = new Vector2[]
		{
			new Vector3(0f, 0f),
			new Vector2(0f, -0.125f),
			new Vector3(0.125f, 0f)
		};

		// Token: 0x04002381 RID: 9089
		private static Vector2[][] g5_uv_3 = new Vector2[][]
		{
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.g5_side_uv,
			GeometryGenerator.g5_side_uv,
			GeometryGenerator.side_uv
		};

		// Token: 0x04002382 RID: 9090
		public static int[][] g5_indicies_1 = new int[][]
		{
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2
			},
			new int[]
			{
				0,
				2,
				1
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x04002383 RID: 9091
		public static Vector3[][] g5_points_1 = new Vector3[][]
		{
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, 0.5f)
			}
		};

		// Token: 0x04002384 RID: 9092
		private static Vector2[][] g5_uv_1 = new Vector2[][]
		{
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.g5_side_uv,
			GeometryGenerator.g5_side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv
		};

		// Token: 0x04002385 RID: 9093
		public static int[][] g5_indicies_0 = new int[][]
		{
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[]
			{
				0,
				2,
				1
			},
			new int[]
			{
				0,
				1,
				2
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x04002386 RID: 9094
		public static Vector3[][] g5_points_0 = new Vector3[][]
		{
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f)
			},
			new Vector3[0],
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f)
			}
		};

		// Token: 0x04002387 RID: 9095
		private static Vector2[][] g5_uv_0 = new Vector2[][]
		{
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.g5_side_uv,
			GeometryGenerator.g5_side_uv,
			GeometryGenerator.side_uv
		};

		// Token: 0x04002388 RID: 9096
		public static int[][] g5_indicies_2 = new int[][]
		{
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[]
			{
				0,
				2,
				1
			},
			new int[]
			{
				0,
				1,
				2
			},
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			},
			new int[0],
			new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			}
		};

		// Token: 0x04002389 RID: 9097
		public static Vector3[][] g5_points_2 = new Vector3[][]
		{
			new Vector3[]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f)
			},
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f)
			},
			new Vector3[0],
			new Vector3[]
			{
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f)
			}
		};

		// Token: 0x0400238A RID: 9098
		private static Vector2[][] g5_uv_2 = new Vector2[][]
		{
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.g5_side_uv,
			GeometryGenerator.g5_side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv,
			GeometryGenerator.side_uv
		};

		// Token: 0x0400238B RID: 9099
		public static int[][] geomNumVerts = new int[][]
		{
			new int[]
			{
				4,
				4,
				4,
				4,
				4,
				4,
				0
			},
			new int[]
			{
				4,
				0,
				4,
				4,
				4,
				4,
				4
			},
			new int[]
			{
				0,
				4,
				4,
				4,
				4,
				4,
				4
			},
			new int[]
			{
				4,
				4,
				0,
				0,
				0,
				0,
				16
			},
			new int[]
			{
				0,
				4,
				0,
				4,
				3,
				3,
				4
			},
			new int[]
			{
				0,
				4,
				3,
				3,
				0,
				4,
				4
			},
			new int[]
			{
				0,
				4,
				3,
				3,
				4,
				0,
				4
			},
			new int[]
			{
				0,
				4,
				4,
				0,
				3,
				3,
				4
			},
			new int[]
			{
				4,
				0,
				0,
				4,
				3,
				3,
				4
			},
			new int[]
			{
				4,
				0,
				3,
				3,
				0,
				4,
				4
			},
			new int[]
			{
				4,
				0,
				3,
				3,
				4,
				0,
				4
			},
			new int[]
			{
				4,
				0,
				4,
				0,
				3,
				3,
				4
			},
			g6_wall.geomNumVerts_0,
			g6_wall.geomNumVerts_90,
			g6_wall.geomNumVerts_270,
			g6_wall.geomNumVerts_180,
			g7_c.geomNumVerts_0,
			g7_c.geomNumVerts_90,
			g7_c.geomNumVerts_270,
			g7_c.geomNumVerts_180,
			g8_c.geomNumVerts_0,
			g8_c.geomNumVerts_90,
			g8_c.geomNumVerts_270,
			g8_c.geomNumVerts_180,
			g9.geomNumVerts_0,
			g9.geomNumVerts_90,
			g9.geomNumVerts_270,
			g9.geomNumVerts_180,
			gA.geomNumVerts_0,
			gA.geomNumVerts_90,
			gA.geomNumVerts_270,
			gA.geomNumVerts_180,
			gB.geomNumVerts_0,
			gB.geomNumVerts_90,
			gB.geomNumVerts_270,
			gB.geomNumVerts_180,
			gC.geomNumVerts_0,
			gC.geomNumVerts_90,
			gC.geomNumVerts_270,
			gC.geomNumVerts_180,
			gD.geomNumVerts_0,
			gD.geomNumVerts_90,
			gD.geomNumVerts_270,
			gD.geomNumVerts_180,
			gE.geomNumVerts_0,
			gE.geomNumVerts_90,
			gE.geomNumVerts_270,
			gE.geomNumVerts_180
		};

		// Token: 0x0400238C RID: 9100
		private static int[][] geomNumTris = new int[][]
		{
			new int[]
			{
				6,
				6,
				6,
				6,
				6,
				6,
				0
			},
			new int[]
			{
				6,
				0,
				6,
				6,
				6,
				6,
				6
			},
			new int[]
			{
				0,
				6,
				6,
				6,
				6,
				6,
				6
			},
			new int[]
			{
				6,
				6,
				0,
				0,
				0,
				0,
				24
			},
			new int[]
			{
				0,
				6,
				0,
				6,
				3,
				3,
				6
			},
			new int[]
			{
				0,
				6,
				3,
				3,
				0,
				6,
				6
			},
			new int[]
			{
				0,
				6,
				3,
				3,
				6,
				0,
				6
			},
			new int[]
			{
				0,
				6,
				6,
				0,
				6,
				6,
				6
			},
			new int[]
			{
				6,
				0,
				0,
				6,
				3,
				3,
				6
			},
			new int[]
			{
				6,
				0,
				3,
				3,
				0,
				6,
				6
			},
			new int[]
			{
				6,
				0,
				3,
				3,
				6,
				0,
				6
			},
			new int[]
			{
				6,
				0,
				6,
				0,
				6,
				6,
				6
			},
			g6_wall.geomNumTris_0,
			g6_wall.geomNumTris_90,
			g6_wall.geomNumTris_270,
			g6_wall.geomNumTris_180,
			g7_c.geomNumTris_0,
			g7_c.geomNumTris_90,
			g7_c.geomNumTris_270,
			g7_c.geomNumTris_180,
			g8_c.geomNumTris_0,
			g8_c.geomNumTris_90,
			g8_c.geomNumTris_270,
			g8_c.geomNumTris_180,
			g9.geomNumTris_0,
			g9.geomNumTris_90,
			g9.geomNumTris_270,
			g9.geomNumTris_180,
			gA.geomNumTris_0,
			gA.geomNumTris_90,
			gA.geomNumTris_270,
			gA.geomNumTris_180,
			gB.geomNumTris_0,
			gB.geomNumTris_90,
			gB.geomNumTris_270,
			gB.geomNumTris_180,
			gC.geomNumTris_0,
			gC.geomNumTris_90,
			gC.geomNumTris_270,
			gC.geomNumTris_180,
			gD.geomNumTris_0,
			gD.geomNumTris_90,
			gD.geomNumTris_270,
			gD.geomNumTris_180,
			gE.geomNumTris_0,
			gE.geomNumTris_90,
			gE.geomNumTris_270,
			gE.geomNumTris_180
		};

		// Token: 0x0400238D RID: 9101
		private static bool[][] neighbor_has_side = new bool[][]
		{
			new bool[]
			{
				true,
				true,
				true,
				true,
				true,
				true,
				true
			},
			new bool[]
			{
				false,
				true,
				true,
				true,
				true,
				true,
				true
			},
			new bool[]
			{
				true,
				false,
				true,
				true,
				true,
				true,
				true
			},
			new bool[7],
			new bool[]
			{
				true,
				false,
				true,
				false,
				true,
				true,
				false
			},
			new bool[]
			{
				true,
				false,
				true,
				true,
				true,
				false,
				false
			},
			new bool[]
			{
				true,
				false,
				true,
				true,
				false,
				true,
				false
			},
			new bool[]
			{
				true,
				false,
				false,
				true,
				true,
				true,
				false
			},
			new bool[]
			{
				false,
				true,
				true,
				false,
				true,
				true,
				false
			},
			new bool[]
			{
				false,
				true,
				true,
				true,
				true,
				false,
				false
			},
			new bool[]
			{
				false,
				true,
				true,
				true,
				false,
				true,
				false
			},
			new bool[]
			{
				false,
				true,
				false,
				true,
				true,
				true,
				false
			},
			g6_wall.neighbor_has_side_0,
			g6_wall.neighbor_has_side_90,
			g6_wall.neighbor_has_side_270,
			g6_wall.neighbor_has_side_180,
			g7_c.neighbor_has_side_0,
			g7_c.neighbor_has_side_90,
			g7_c.neighbor_has_side_270,
			g7_c.neighbor_has_side_180,
			g8_c.neighbor_has_side_0,
			g8_c.neighbor_has_side_90,
			g8_c.neighbor_has_side_270,
			g8_c.neighbor_has_side_180,
			g9.neighbor_has_side_0,
			g9.neighbor_has_side_90,
			g9.neighbor_has_side_270,
			g9.neighbor_has_side_180,
			gA.neighbor_has_side_0,
			gA.neighbor_has_side_90,
			gA.neighbor_has_side_270,
			gA.neighbor_has_side_180,
			gB.neighbor_has_side_0,
			gB.neighbor_has_side_90,
			gB.neighbor_has_side_270,
			gB.neighbor_has_side_180,
			gC.neighbor_has_side_0,
			gC.neighbor_has_side_90,
			gC.neighbor_has_side_270,
			gC.neighbor_has_side_180,
			gD.neighbor_has_side_0,
			gD.neighbor_has_side_90,
			gD.neighbor_has_side_270,
			gD.neighbor_has_side_180,
			gE.neighbor_has_side_0,
			gE.neighbor_has_side_90,
			gE.neighbor_has_side_270,
			gE.neighbor_has_side_180
		};

		// Token: 0x0400238E RID: 9102
		private static GeometryGenerator.GeomInfo[] geomInfo = new GeometryGenerator.GeomInfo[]
		{
			new GeometryGenerator.GeomInfo(GeometryGenerator.g4_indicies_0, GeometryGenerator.g4_points_0, GeometryGenerator.g4_uv_0, null),
			new GeometryGenerator.GeomInfo(GeometryGenerator.g4_indicies_1, GeometryGenerator.g4_points_1, GeometryGenerator.g4_uv_1, null),
			new GeometryGenerator.GeomInfo(GeometryGenerator.g4_indicies_2, GeometryGenerator.g4_points_2, GeometryGenerator.g4_uv_2, null),
			new GeometryGenerator.GeomInfo(GeometryGenerator.g4_indicies_3, GeometryGenerator.g4_points_3, GeometryGenerator.g4_uv_3, null),
			new GeometryGenerator.GeomInfo(GeometryGenerator.g5_indicies_0, GeometryGenerator.g5_points_0, GeometryGenerator.g5_uv_0, null),
			new GeometryGenerator.GeomInfo(GeometryGenerator.g5_indicies_1, GeometryGenerator.g5_points_1, GeometryGenerator.g5_uv_1, null),
			new GeometryGenerator.GeomInfo(GeometryGenerator.g5_indicies_2, GeometryGenerator.g5_points_2, GeometryGenerator.g5_uv_2, null),
			new GeometryGenerator.GeomInfo(GeometryGenerator.g5_indicies_3, GeometryGenerator.g5_points_3, GeometryGenerator.g5_uv_3, null),
			new GeometryGenerator.GeomInfo(g6_wall.indexes_0, g6_wall.points_0, g6_wall.uv_0, null),
			new GeometryGenerator.GeomInfo(g6_wall.indexes_90, g6_wall.points_90, g6_wall.uv_90, null),
			new GeometryGenerator.GeomInfo(g6_wall.indexes_270, g6_wall.points_270, g6_wall.uv_270, null),
			new GeometryGenerator.GeomInfo(g6_wall.indexes_180, g6_wall.points_180, g6_wall.uv_180, null),
			new GeometryGenerator.GeomInfo(g7_c.indexes_0, g7_c.points_0, g7_c.uv_0, null),
			new GeometryGenerator.GeomInfo(g7_c.indexes_90, g7_c.points_90, g7_c.uv_90, null),
			new GeometryGenerator.GeomInfo(g7_c.indexes_270, g7_c.points_270, g7_c.uv_270, null),
			new GeometryGenerator.GeomInfo(g7_c.indexes_180, g7_c.points_180, g7_c.uv_180, null),
			new GeometryGenerator.GeomInfo(g8_c.indexes_0, g8_c.points_0, g8_c.uv_0, null),
			new GeometryGenerator.GeomInfo(g8_c.indexes_90, g8_c.points_90, g8_c.uv_90, null),
			new GeometryGenerator.GeomInfo(g8_c.indexes_270, g8_c.points_270, g8_c.uv_270, null),
			new GeometryGenerator.GeomInfo(g8_c.indexes_180, g8_c.points_180, g8_c.uv_180, null),
			new GeometryGenerator.GeomInfo(g9.indexes_0, g9.points_0, g9.uv_0, null),
			new GeometryGenerator.GeomInfo(g9.indexes_90, g9.points_90, g9.uv_90, null),
			new GeometryGenerator.GeomInfo(g9.indexes_270, g9.points_270, g9.uv_270, null),
			new GeometryGenerator.GeomInfo(g9.indexes_180, g9.points_180, g9.uv_180, null),
			new GeometryGenerator.GeomInfo(gA.indexes_0, gA.points_0, gA.uv_0, null),
			new GeometryGenerator.GeomInfo(gA.indexes_90, gA.points_90, gA.uv_90, null),
			new GeometryGenerator.GeomInfo(gA.indexes_270, gA.points_270, gA.uv_270, null),
			new GeometryGenerator.GeomInfo(gA.indexes_180, gA.points_180, gA.uv_180, null),
			new GeometryGenerator.GeomInfo(gB.indexes_0, gB.points_0, gB.uv_0, null),
			new GeometryGenerator.GeomInfo(gB.indexes_90, gB.points_90, gB.uv_90, null),
			new GeometryGenerator.GeomInfo(gB.indexes_270, gB.points_270, gB.uv_270, null),
			new GeometryGenerator.GeomInfo(gB.indexes_180, gB.points_180, gB.uv_180, null),
			new GeometryGenerator.GeomInfo(gC.indexes_0, gC.points_0, gC.uv_0, null),
			new GeometryGenerator.GeomInfo(gC.indexes_90, gC.points_90, gC.uv_90, null),
			new GeometryGenerator.GeomInfo(gC.indexes_270, gC.points_270, gC.uv_270, null),
			new GeometryGenerator.GeomInfo(gC.indexes_180, gC.points_180, gC.uv_180, null),
			new GeometryGenerator.GeomInfo(gD.indexes_0, gD.points_0, gD.uv_0, null),
			new GeometryGenerator.GeomInfo(gD.indexes_90, gD.points_90, gD.uv_90, null),
			new GeometryGenerator.GeomInfo(gD.indexes_270, gD.points_270, gD.uv_270, null),
			new GeometryGenerator.GeomInfo(gD.indexes_180, gD.points_180, gD.uv_180, null),
			new GeometryGenerator.GeomInfo(gE.indexes_0, gE.points_0, gE.uv_0, null),
			new GeometryGenerator.GeomInfo(gE.indexes_90, gE.points_90, gE.uv_90, null),
			new GeometryGenerator.GeomInfo(gE.indexes_270, gE.points_270, gE.uv_270, null),
			new GeometryGenerator.GeomInfo(gE.indexes_180, gE.points_180, gE.uv_180, null)
		};

		// Token: 0x0400238F RID: 9103
		public int numVerts;

		// Token: 0x04002390 RID: 9104
		private int numUVs;

		// Token: 0x04002391 RID: 9105
		private int numNormals;

		// Token: 0x04002392 RID: 9106
		public int numTriangles;

		// Token: 0x04002393 RID: 9107
		private Mesh _mesh;

		// Token: 0x04002394 RID: 9108
		private CubeDrawTypes cdttype;

		// Token: 0x04002395 RID: 9109
		protected int[] collected;

		// Token: 0x04002396 RID: 9110
		protected int nncollected;

		// Token: 0x04002397 RID: 9111
		protected int _trisize;

		// Token: 0x04002398 RID: 9112
		protected int _size;

		// Token: 0x02000417 RID: 1047
		private class GeomInfo
		{
			// Token: 0x06001EB3 RID: 7859 RVA: 0x0001665B File Offset: 0x0001485B
			public GeomInfo(int[][] i, Vector3[][] p, Vector2[][] u = null, Vector3[][] n = null)
			{
				this.indexes = i;
				this.points = p;
				this.uv = u;
				this.normals = n;
			}

			// Token: 0x04002399 RID: 9113
			public int[][] indexes;

			// Token: 0x0400239A RID: 9114
			public Vector3[][] points;

			// Token: 0x0400239B RID: 9115
			public Vector3[][] normals;

			// Token: 0x0400239C RID: 9116
			public Vector2[][] uv;
		}

		// Token: 0x02000418 RID: 1048
		private class GeomAddInfo
		{
			// Token: 0x0400239D RID: 9117
			public int verts;

			// Token: 0x0400239E RID: 9118
			public int indexes;
		}
	}
}
