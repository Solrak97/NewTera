using Godot;
using System;
using System.Collections.Generic;
using NewTera.Core.Systems.Terrain;
using NewTera.Core.Systems.Voxels;

namespace NewTera.Rendering;

public partial class ChunkRenderer : MeshInstance3D
{
	private VoxelType[,,] _chunkData;
	private Vector3 _position;

	private int _chunkWidth;
	private int _chunkHeight;
	
	private List<Vector3> _vertices;
	private List<Vector2> _uvs;
	private List<Vector3> _normals;
	private List<int> _indices;

	private Material _material;
	private ArrayMesh _mesh;
	private SurfaceTool _surfaceTool;

	//Collider


	#region VoxelConstantsTable

	static readonly Vector3[] voxelFaceChecks = new Vector3[6]
	{
		new Vector3(0, 0, -1),
		new Vector3(0, 0, 1),
		new Vector3(-1, 0, 0),
		new Vector3(1, 0, 0),
		new Vector3(0, -1, 0),
		new Vector3(0, 1, 0)
	};

	static readonly Vector3[] voxelVertices = new Vector3[8]
	{
		new Vector3(0, 0, 0),
		new Vector3(1, 0, 0),
		new Vector3(0, 1, 0),
		new Vector3(1, 1, 0),
		new Vector3(0, 0, 1),
		new Vector3(1, 0, 1),
		new Vector3(0, 1, 1),
		new Vector3(1, 1, 1),
	};

	static readonly int[,] voxelVertexIndex = new int[6, 4]
	{
		{ 0, 1, 2, 3 },
		{ 4, 5, 6, 7 },
		{ 4, 0, 6, 2 },
		{ 5, 1, 7, 3 },
		{ 0, 1, 4, 5 },
		{ 2, 3, 6, 7 },
	};

	static readonly Vector2[] voxelUVs = new Vector2[4]
	{
		new Vector2(0, 0),
		new Vector2(0, 1),
		new Vector2(1, 0),
		new Vector2(1, 1)
	};

	static readonly int[,] voxelTris = new int[6, 6]
	{
		{ 0, 2, 3, 0, 3, 1 },
		{ 0, 1, 2, 1, 3, 2 },
		{ 0, 2, 3, 0, 3, 1 },
		{ 0, 1, 2, 1, 3, 2 },
		{ 0, 1, 2, 1, 3, 2 },
		{ 0, 2, 3, 0, 3, 1 },
	};

	#endregion

	public ChunkRenderer()
	{
		_mesh = new ArrayMesh();
		_surfaceTool = new SurfaceTool();
		_surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
	}
	
	public void Initialize(Material material, Vector2 position, VoxelType[,,] chunkData, string name="ChunkRenderer")
	{
		this.Name = name;
		this._material = material;
		this._chunkData = chunkData;
		
		_chunkWidth = chunkData.GetLength(0);
		_chunkHeight = chunkData.GetLength(1);
		
		this._position = new Vector3(position.X, 0, position.Y);
		
		_vertices = [];
		_uvs = [];
		_normals = [];
		_indices = [];
	}

	public void LoadChunkData(VoxelType[,,] chunkData)
	{
		_chunkData = chunkData;
	}


	public void UploadMesh()
	{
		foreach (var t in _indices)
		{
			_surfaceTool.AddVertex(_vertices[t]);
		}
		
		_surfaceTool.Index();
		_surfaceTool.Commit(_mesh);
		
		this.Mesh = _mesh;
	}

	private bool IsValidPosition(Vector3 position)
	{
		return (0 >= position.X && position.X > _chunkWidth && 
				0 >= position.Y && position.Y >= _chunkHeight && 
				0 >= position.Z && position.Z > _chunkWidth);
	}

	public void GenerateMesh()
	{
		for (var x = 0; x < _chunkWidth; x++)
		{
			for (var y = 0; y < _chunkHeight; y++)
			{
				for (var z = 0; z < _chunkWidth; z++)
				{
					var voxel = _chunkData[x, y, z];

					if (!voxel.IsSolid) continue;
					
					var blockPos = new Vector3(
						x,
						y,
						z
						);
					

					for (var i = 0; i < 6; i++)
					{
						
						var neighborPos = blockPos + voxelFaceChecks[i];

						if (IsValidPosition(neighborPos))
						{
							var neighborVoxel = _chunkData[
								(int)neighborPos.X, 
								(int)neighborPos.Y,
								(int)neighborPos.Z
							];

							if (neighborVoxel.IsSolid) continue;
						}
						
						var faceVertices = new Vector3[4];
						var faceUVs = new Vector2[4];
						var faceColors = new Color[4];

						for (var j = 0; j < 4; j++)
						{
							faceVertices[j] = voxelVertices[voxelVertexIndex[i, j]] + blockPos;
							//faceUVs[j] = voxel.GetUVs(j);
							faceColors[j] = voxel.Color;
						}

						for (int j = 0; j < 6; j++)
						{
							_vertices.Add(faceVertices[voxelTris[i, j]]);
							_uvs.Add(faceUVs[voxelTris[i, j]]);
							//colors.Add(faceColors[voxelTris[i, j]]);
							_indices.Add(_vertices.Count - 1);
						}
					}
				}
			}
		}
	}
}
