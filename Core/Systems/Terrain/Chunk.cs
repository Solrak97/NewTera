using Godot;
using NewTera.Core.Systems.Voxels;
using NewTera.Rendering;

namespace NewTera.Core.Systems.Terrain;

public partial class Chunk : Node3D
{
    private const int ChunkSize = 16;
    private const int ChunkHeight = 128;
    private VoxelType[,,] _voxels = new VoxelType[ChunkSize, ChunkHeight, ChunkSize];
    private ChunkRenderer _chunkRenderer;

    private Vector2I ChunkPosition { get; set; }

    
    public Chunk()
    {
        GD.Print("Chunk position: ");
    }
    
    
    public Chunk(Vector2I chunkPos)
    {
        GD.Print("Chunk position: " + chunkPos);
        ChunkPosition = chunkPos;
    }



    public void Generate()
    {
        for (int x = 0; x < ChunkSize; x++)
        {
            for (int y = 0; y < ChunkHeight; y++)
            {
                for (int z = 0; z < ChunkSize; z++)
                {
                    _voxels[x, y, z] = (y == 0) ? VoxelRegistry.GetVoxel("Stone") : VoxelRegistry.GetVoxel("Air");
                }
            }
        }
        BuildMesh();
    }

    private void BuildMesh()
    {
    }
}
