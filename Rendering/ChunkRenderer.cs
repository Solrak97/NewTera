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
	
	private List<Vector2> _uvs;
	private List<Vector3> _normals;
	
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

		if (material != null)
		{
			MaterialOverride = material;
		}
		
		this._chunkData = chunkData;
		
		_chunkWidth = chunkData.GetLength(0);
		_chunkHeight = chunkData.GetLength(1);
		
		this._position = new Vector3(position.X, 0, position.Y);
		
		_uvs = [];
		_normals = [];
	}

	public void LoadChunkData(VoxelType[,,] chunkData)
	{
		_chunkData = chunkData;
	}


	public void UploadMesh()
	{
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
		for (var i = 0; i < _chunkWidth * _chunkHeight * _chunkWidth; i++)
		{
			var x = i % _chunkWidth;
			var y = (i / _chunkWidth) % _chunkHeight;
			var z = i / (_chunkWidth * _chunkHeight);
			
			var voxel = _chunkData[x, y, z];
			
			AddVoxel(voxel, new Vector3I(x, y, z));
		}
	}

	private void AddVoxel(VoxelType voxel, Vector3 position)
	{
		if (!voxel.IsSolid) return;
		
		for (var i = 0; i < 6; i++)
		{
			var neighborPos = position + voxelFaceChecks[i];

			if (IsValidPosition(neighborPos))
			{
				var neighborVoxel = _chunkData[
					(int)neighborPos.X, 
					(int)neighborPos.Y,
					(int)neighborPos.Z
				];

				if (neighborVoxel.IsSolid) continue;
			}
						
			AddFace(voxel, i, neighborPos);
			
		}
		
	}

	private void AddFace(VoxelType voxel, int index, Vector3 position)
	{
		var faceVertices = new Vector3[4];
		//var faceUVs = new Vector2[4];

		for (var i = 0; i < 4; i++)
		{
			faceVertices[i] = voxelVertices[voxelVertexIndex[index, i]] + position;
			//faceUVs[j] = voxel.GetUVs(j);
		}

		_surfaceTool.SetColor(new Color(0.55f, 0.27f, 0.07f));
		
		for (var i = 0; i < 6; i++)
		{
			_surfaceTool.AddVertex(faceVertices[voxelTris[index, i]]);
			//(faceUVs[voxelTris[index, i]]);
			//_colors.Add(faceColors[voxelTris[index, i]]);
		}
	}
}
