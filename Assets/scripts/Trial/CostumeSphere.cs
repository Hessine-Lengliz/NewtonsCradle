using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CustomSphere : MonoBehaviour
{
    public Mesh mesh;

    [Header("Sphere Properties")]
    public int latitudeSegments = 20;  // Number of latitude segments
    public int longitudeSegments = 20; // Number of longitude segments
    public float radius = 1f;          // Radius of the sphere

    [Header("Material Settings")]
    public Material customMaterial;  // Custom material to apply

    void Start()
    {
        // Check if the MeshFilter component is attached; if not, add one
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        // Check if the MeshRenderer component is attached; if not, add one
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        // Generate the sphere mesh
        GenerateSphereMesh();

        // Apply the generated mesh to the MeshFilter component
        meshFilter.mesh = mesh;

        // Apply the custom material to the MeshRenderer
        if (customMaterial != null)
        {
            meshRenderer.material = customMaterial;  // Set the custom material
        }
        else
        {
            // Optionally, use a default material if no custom material is assigned
            meshRenderer.material = new Material(Shader.Find("Standard"));
        }
    }

    void GenerateSphereMesh()
    {
        // Generate vertices
        Vector3[] vertices = new Vector3[(latitudeSegments + 1) * (longitudeSegments + 1)];
        int[] triangles = new int[longitudeSegments * latitudeSegments * 6];

        int vertIndex = 0;
        for (int lat = 0; lat <= latitudeSegments; lat++)
        {
            float theta = lat * Mathf.PI / latitudeSegments; // Latitude angle
            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float phi = lon * 2 * Mathf.PI / longitudeSegments; // Longitude angle
                float x = radius * Mathf.Sin(theta) * Mathf.Cos(phi);
                float y = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta) * Mathf.Sin(phi);
                vertices[vertIndex++] = new Vector3(x, y, z);
            }
        }

        // Generate triangles
        int triIndex = 0;
        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                int current = lat * (longitudeSegments + 1) + lon;
                int next = current + longitudeSegments + 1;

                // First triangle
                triangles[triIndex++] = current;
                triangles[triIndex++] = next;
                triangles[triIndex++] = current + 1;

                // Second triangle
                triangles[triIndex++] = next;
                triangles[triIndex++] = next + 1;
                triangles[triIndex++] = current + 1;
            }
        }

        // Create the mesh
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate normals for lighting and mesh bounds
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // Apply a transformation matrix to the sphere's vertices
    public void ApplyTransformation(Matrix4x4 matrix)
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            // Transform each vertex using the matrix
            vertices[i] = matrix.MultiplyPoint3x4(vertices[i]);
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();  // Recalculate mesh bounds
    }
}
