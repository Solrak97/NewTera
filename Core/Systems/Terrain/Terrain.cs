using System.Collections.Generic;
using Godot;
using NewTera.Core.Systems.Voxels;
using NewTera.Rendering;


namespace NewTera.Core.Systems.Terrain;

public partial class Terrain : Node
{
	[Export] public int WorldSize;
	private readonly Dictionary<Vector2I, Chunk> _chunks = new Dictionary<Vector2I, Chunk>();
	private const int ChunkSize = 16;
	private SingleCubeRenderer _cubeRenderer;

	
	public Terrain()
	{
	}

	public override void _Ready()
	{
		base._Ready();
		
		GD.Print("Creating terrain");
		GenerateWorld();

		_cubeRenderer = new SingleCubeRenderer();
		_cubeRenderer.Name = "Cube Renderer";
		
		AddChild(_cubeRenderer);

	}


	public void GenerateWorld()
	{
		
		VoxelRegistry.LoadVoxels("C:/Users/luisc/Gamedev/NewTera/Core/Data/BlockTypes.json");
		
		GD.Print("Generating world: ", WorldSize);
		for (var x = 0; x < WorldSize / ChunkSize; x++)
		{
			for (var z = 0; z < WorldSize / ChunkSize; z++)
			{
				var chunkPos = new Vector2I(x, z);
				var chunk = new Chunk();
				chunk.Initialize(chunkPos, $"chunk: {chunkPos}");
				chunk.Generate();
				_chunks[chunkPos] = chunk;
				AddChild(chunk);
			}
		}
		
		
	}
	

	public Chunk GetChunk(Vector3 worldPos)
	{
		var chunkCoord = new Vector2I(
			Mathf.FloorToInt(worldPos.X / ChunkSize), 
			Mathf.FloorToInt(worldPos.Z / ChunkSize)
		);
		return _chunks.GetValueOrDefault(chunkCoord, null);
	}
}
