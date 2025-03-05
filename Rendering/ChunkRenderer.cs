using Godot;
using System;
using System.Collections.Generic;
using NewTera.Core.Systems.Terrain;

namespace NewTera.Rendering;

public partial class ChunkRenderer : Node3D
{
//     private Vector3 _chunkPosition;
//     private MeshInstance3D _meshInstance;
//     private MeshCollider _meshCollider;
//     private Chunk _chunkData;
//     private MeshData _meshData = new MeshData();
//     
//     public ChunkRenderer()
//     {
//     }
// 
//     public ChunkRenderer(Material mat, Vector3 position, Chunk chunk)
//     {
//         ConfigureComponents();
//         _meshInstance.MaterialOverride = mat;
//         _chunkPosition = position;
//         _chunkData = chunk;
//     }
//     
// 
//     public override void _Process(float delta)
//     {
//         if (_chunkData.modified)
//         {
//             GenerateMesh();
//             UploadMesh();
//             _chunkData.modified = false;
//         }
//     }
// 
//     private void ConfigureComponents()
//     {
//         _meshInstance = GetNode<MeshInstance3D>("MeshInstance3D");
//         _meshCollider = GetNode<MeshCollider>("MeshCollider");
//     }
// 
//     public void GenerateMesh()
//     {
//         _meshData.ClearData();
// 
//         for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
//         {
//             for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
//             {
//                 for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
//                 {
//                     // Get the VoxelType for this position
//                     VoxelType voxel = _chunkData.GetVoxelAt(x, y, z);
//                     
//                     if (voxel.IsSolid()) // Only add solids to the mesh
//                     {
//                         Vector3 blockPos = new Vector3(x, y, z);
// 
//                         // Check for each face of the voxel (6 faces)
//                         for (int i = 0; i < 6; i++)
//                         {
//                             Vector3 neighborPos = blockPos + voxelFaceChecks[i];
//                             VoxelType neighborVoxel = _chunkData.GetVoxelAt((int)neighborPos.x, (int)neighborPos.y, (int)neighborPos.z);
// 
//                             if (neighborVoxel.IsSolid()) continue;
// 
//                             Vector3[] faceVertices = new Vector3[4];
//                             Vector2[] faceUVs = new Vector2[4];
//                             Color[] faceColors = new Color[4];
// 
//                             for (int j = 0; j < 4; j++)
//                             {
//                                 faceVertices[j] = voxelVertices[voxelVertexIndex[i, j]] + blockPos + _chunkPosition;
//                                 faceUVs[j] = voxel.GetUVs(j); // Assuming VoxelType has a GetUVs method
//                                 faceColors[j] = voxel.GetColor(); // Assuming VoxelType has a GetColor method
//                             }
// 
//                             for (int j = 0; j < 6; j++)
//                             {
//                                 _meshData.vertices.Add(faceVertices[voxelTris[i, j]]);
//                                 _meshData.UVs.Add(faceUVs[voxelTris[i, j]]);
//                                 _meshData.colors.Add(faceColors[voxelTris[i, j]]);
//                                 _meshData.triangles.Add(_meshData.vertices.Count - 1);
//                             }
//                         }
//                     }
//                 }
//             }
//         }
//     }
// 
//     public void UploadMesh()
//     {
//         _meshData.UploadMesh();
// 
//         if (_meshInstance == null)
//             ConfigureComponents();
// 
//         var newMesh = _meshData.mesh;
//         _meshInstance.Mesh = newMesh;
// 
//         if (_meshData.vertices.Count > 3)
//             _meshCollider.SharedMesh = newMesh;
//     }
// 
//     public struct MeshData
//     {
//         public Mesh mesh;
//         public List<Vector3> vertices;
//         public List<int> triangles;
//         public List<Vector2> UVs;
//         public List<Color> colors;
// 
//         public bool initialized;
// 
//         public void ClearData()
//         {
//             if (!initialized)
//             {
//                 mesh = new Mesh();
//                 vertices = new List<Vector3>();
//                 triangles = new List<int>();
//                 UVs = new List<Vector2>();
//                 colors = new List<Color>();
//             }
//             else
//             {
//                 mesh.Clear();
//                 vertices.Clear();
//                 triangles.Clear();
//                 UVs.Clear();
//                 colors.Clear();
//             }
//         }
// 
//         public void UploadMesh(bool sharedVertices = false)
//         {
//             mesh.SetVertices(vertices);
//             mesh.SetTriangles(triangles, 0, false);
//             mesh.SetUVs(0, UVs);
//             mesh.Colors = colors.ToArray();
// 
//             mesh.Optimize();
//             mesh.RecalculateNormals();
//             mesh.RecalculateBounds();
//             mesh.UploadMeshData(false);
//         }
//     }
// 
//     static readonly Vector3[] voxelFaceChecks = new Vector3[6]
//     {
//         new Vector3(0, 0, -1), 
//         new Vector3(0, 0, 1),  
//         new Vector3(-1, 0, 0), 
//         new Vector3(1, 0, 0),  
//         new Vector3(0, -1, 0), 
//         new Vector3(0, 1, 0)   
//     };
// 
//     static readonly Vector3[] voxelVertices = new Vector3[8]
//     {
//         new Vector3(0, 0, 0),
//         new Vector3(1, 0, 0),
//         new Vector3(0, 1, 0),
//         new Vector3(1, 1, 0),
//         new Vector3(0, 0, 1),
//         new Vector3(1, 0, 1),
//         new Vector3(0, 1, 1),
//         new Vector3(1, 1, 1),
//     };
// 
//     static readonly int[,] voxelVertexIndex = new int[6, 4]
//     {
//         { 0, 1, 2, 3 }, 
//         { 4, 5, 6, 7 }, 
//         { 4, 0, 6, 2 }, 
//         { 5, 1, 7, 3 }, 
//         { 0, 1, 4, 5 }, 
//         { 2, 3, 6, 7 }, 
//     };
// 
//     static readonly Vector2[] voxelUVs = new Vector2[4]
//     {
//         new Vector2(0, 0),
//         new Vector2(0, 1),
//         new Vector2(1, 0),
//         new Vector2(1, 1)
//     };
// 
//     static readonly int[,] voxelTris = new int[6, 6]
//     {
//         { 0, 2, 3, 0, 3, 1 }, 
//         { 0, 1, 2, 1, 3, 2 }, 
//         { 0, 2, 3, 0, 3, 1 }, 
//         { 0, 1, 2, 1, 3, 2 }, 
//         { 0, 1, 2, 1, 3, 2 }, 
//         { 0, 2, 3, 0, 3, 1 }, 
//     };
}
