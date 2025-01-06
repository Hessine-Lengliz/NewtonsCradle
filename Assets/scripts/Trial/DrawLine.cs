using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject object1; // First GameObject
    public GameObject object2; // Second GameObject

    private LineRenderer lineRenderer;

    void Start()
    {
        // Add a LineRenderer component to the GameObject this script is attached to
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the width of the line
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Set the material and color of the line
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        // Set the number of positions in the LineRenderer
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Update the positions of the LineRenderer to match the positions of the two GameObjects
        lineRenderer.SetPosition(0, object1.transform.position);
        lineRenderer.SetPosition(1, object2.transform.position);
    }
}