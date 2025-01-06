using UnityEngine;
using System.Collections.Generic;

public class NewtonCradleVisualizer : MonoBehaviour
{
    public int numberOfPendulums = 5; // Set this in the Unity Inspector to change the number of pendulums.
    private NewtonCradle cradle;
    private List<CustomSphere> pendulumObjects; // List of CustomSphere instead of GameObject
    private List<GameObject> anchors; // List of GameObjects to act as the anchor points for each pendulum
    private List<LineRenderer> lineRenderers; // List of LineRenderers for each pendulum's line

    void Start()
{
    cradle = new NewtonCradle(numberOfPendulums, 2.0f, 1.0f, 0.2f);
    pendulumObjects = new List<CustomSphere>();
    anchors = new List<GameObject>(); // Initialize the anchors list
    lineRenderers = new List<LineRenderer>();

    // Adjust the spacing to be just the diameter of the ball for touching
    float spacing = cradle.ballRadius * 2.0f;

    for (int i = 0; i < cradle.numberOfBalls; i++)
    {
        // Create a new GameObject and add the CustomSphere component for the ball
        GameObject ballObj = new GameObject("PendulumBall_" + i);
        CustomSphere customSphere = ballObj.AddComponent<CustomSphere>();

        // Set up the custom sphere properties
        customSphere.latitudeSegments = 20;
        customSphere.longitudeSegments = 20;
        customSphere.radius = cradle.ballRadius;

        pendulumObjects.Add(customSphere);

        // Set position of each ball
        float offset = (i - (cradle.numberOfBalls - 1) * 0.5f) * spacing;

        ballObj.transform.position = new Vector3(offset, 0, 0); // Adjust the spacing based on index

        // Create an anchor GameObject for each pendulum and position it above the ball
        GameObject anchorObj = new GameObject("Anchor_" + i);
        anchorObj.transform.position = new Vector3(offset, 3, 0); // Position each anchor above its respective ball
        anchors.Add(anchorObj);

        // Create a LineRenderer for each pendulum ball
        LineRenderer lineRenderer = ballObj.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Start and end positions (anchor and ball)
        lineRenderer.startWidth = 0.1f; // Set the width of the line
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Assign a simple shader material to the line (can be customized)

        // Set the line renderer for this pendulum in the list
        lineRenderers.Add(lineRenderer);
    }
}
void Update()
{
    cradle.SimulateCradle(Time.deltaTime);

    // Update the pendulum positions based on the cradle simulation
    for (int i = 0; i < cradle.numberOfBalls; i++)
    {
        var pos = cradle.positions[i];

        pendulumObjects[i].transform.position = new Vector3(pos.Item1, pos.Item2, 0);
    }

    // Update the LineRenderers to attach each pendulum ball to its respective anchor
    for (int i = 0; i < cradle.numberOfBalls; i++)
    {
        // Set the start position as the anchor position for each pendulum
        var anchorPos = new Vector3(
            (float)cradle.pendulums[i].AnchorPoint.Item1, 
            (float)cradle.pendulums[i].AnchorPoint.Item2, 
            0);
        
        lineRenderers[i].SetPosition(0, anchorPos); // Use anchor position as the start of the line

        // Set the end position as the position of the pendulum ball
        lineRenderers[i].SetPosition(1, pendulumObjects[i].transform.position); // End at the pendulum ball
    }
}

}
