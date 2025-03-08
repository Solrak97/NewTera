namespace NewTera.Rendering;

using Godot;

public partial class SingleCubeRenderer : MeshInstance3D
{
    public override void _Ready()
    {
        // Create a cube mesh procedurally
        ArrayMesh mesh = new ArrayMesh();
        SurfaceTool surfaceTool = new SurfaceTool();
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);

        // Define vertices and faces of a cube
        Vector3[] vertices = {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f)
        };

        int[] indices = {
            0, 1, 2, 2, 3, 0, // Front
            1, 5, 6, 6, 2, 1, // Right
            5, 4, 7, 7, 6, 5, // Back
            4, 0, 3, 3, 7, 4, // Left
            3, 2, 6, 6, 7, 3, // Top
            4, 5, 1, 1, 0, 4  // Bottom
        };

        for (int i = 0; i < indices.Length; i++)
        {
            surfaceTool.AddVertex(vertices[indices[i]]);
        }

        surfaceTool.Index();
        surfaceTool.Commit(mesh);

        // Assign mesh to this MeshInstance3D
        this.Mesh = mesh;
    }
}
