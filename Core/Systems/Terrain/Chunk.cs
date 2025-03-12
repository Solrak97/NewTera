using Godot;
using NewTera.Core.Systems.Voxels;
using NewTera.Rendering;

namespace NewTera.Core.Systems.Terrain;

public partial class Chunk : Node3D
{
    private const int ChunkWidth = 16;
    private const int ChunkHeight = 128;
    private VoxelType[,,] _voxels;
    private ChunkRenderer _chunkRenderer;
    private bool _modified = true;

    private Vector2I ChunkPosition { get; set; }


    public Chunk()
    {
        _chunkRenderer = new ChunkRenderer();
        _voxels = new VoxelType[ChunkWidth, ChunkHeight, ChunkWidth];
    }

    public void Initialize(Vector2I chunkPos, string name = "Chunk")
    {
        Name = name;
        AddChild(_chunkRenderer);

        ChunkPosition = chunkPos;
        Position = new Vector3I(chunkPos.X * ChunkWidth, 0, chunkPos.Y * ChunkWidth);
        
        var voxMaterial = new ShaderMaterial();
        voxMaterial.Shader = (Shader)ResourceLoader.Load("res://Core/Data/BlockShader.gdshader");

        _chunkRenderer.Initialize(material: voxMaterial, ChunkPosition, _voxels,
            name: $"chunk_renderer: {ChunkPosition}");

        Generate();
    }


    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!_modified) return;
        _chunkRenderer.LoadChunkData(_voxels);
        _chunkRenderer.GenerateMesh();
        _chunkRenderer.UploadMesh();

        _modified = false;
    }


    public void Generate()
    {
        for (var x = 0; x < ChunkWidth; x++)
        {
            for (var y = 0; y < ChunkHeight; y++)
            {
                for (var z = 0; z < ChunkWidth; z++)
                {
                    _voxels[x, y, z] = (y <= 16) ? VoxelRegistry.GetRandomVoxel() : VoxelRegistry.GetVoxel("Air");
                }
            }
        }
    }
}