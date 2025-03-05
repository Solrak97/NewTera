using System.Collections.Generic;
using Godot;


namespace NewTera.Core.Systems.Terrain;

public partial class Terrain : Node
{
	[Export] public int WorldSize;
	private readonly Dictionary<Vector2I, Chunk> _chunks = new Dictionary<Vector2I, Chunk>();
	private const int ChunkSize = 16;

	
	public Terrain()
	{
	}

	public override void _Ready()
	{
		base._Ready();
		
		GD.Print("Creating terrain");
		GenerateWorld();
		
	}


	public void GenerateWorld()
	{
		GD.Print("Generating world: ", WorldSize);
		for (var x = 0; x < WorldSize / ChunkSize; x++)
		{
			for (var z = 0; z < WorldSize / ChunkSize; z++)
			{
				var chunkPos = new Vector2I(x, z);
				var chunk = new Chunk(chunkPos);
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
