using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class NewtonCradle
{
    public int numberOfBalls = 5;          // Number of pendulum balls
    public float length = 2.0f;            // Length of each string
    public float mass = 1.0f;              // Mass of each ball
    public float ballRadius = 0.1f;        // Radius of the ball
    public float angleSpawn = 0f;        // Initial angle for the first ball
    public float initialAngle = 0f;      // Initial angle for the first ball

    public List<Pendulum> pendulums;      // List to store pendulums
    public List<(float, float)> positions; // Store the positions of the pendulums for visualization

    public NewtonCradle(int numberOfBalls, float length, float mass, float initialAngle)
    {
        this.numberOfBalls = numberOfBalls;
        this.length = length;
        this.mass = mass;
        this.initialAngle = initialAngle;

        pendulums = new List<Pendulum>();
        positions = new List<(float, float)>();

        InitializePendulums();
    }public void InitializePendulums()
{
    // Starting point to space out pendulums horizontally
    float anchorOffset = 0.0f; 

    // Set the position for each pendulum
    for (int i = 0; i < numberOfBalls; i++)
    {
        // Set each pendulum's initial angle
        // The first pendulum gets an initial angle of 1f, others get 0f
        float angle = (i == 0) ? -1f : 0f;  // First pendulum has 1f angle, others have 0f

        // Create the pendulum with its length, mass, and initial angle
        var pendulum = new Pendulum(length, mass, angle, (anchorOffset, 0)); // Provide anchor point here

        // Add this pendulum to the list
        pendulums.Add(pendulum);

        // Calculate the next offset for the next pendulum
        anchorOffset += ballRadius * 2; // Adjust spacing between pendulums

        // Track the position for visualization or updates
        positions.Add(pendulum.Position);
    }
}





    public void SimulateCradle(float deltaTime)
    {
        // Update physics for each pendulum
        for (int i = 0; i < pendulums.Count; i++)
        {
            pendulums[i].ApplyPhysics(deltaTime);
            positions[i] = pendulums[i].Position;  // Update positions
        }

        // Handle collisions between pendulums
        for (int i = 0; i < pendulums.Count - 1; i++)
        {
            if (Distance(positions[i], positions[i + 1]) <= 2 * ballRadius)
            {
                HandleCollision(pendulums[i], pendulums[i + 1]);
            }
        }
    }


private void HandleCollision(Pendulum ball1, Pendulum ball2)
{
    // Calculate the distance between two pendulums (balls)
    float distance = Distance(ball1.Position, ball2.Position);

    // Check if the distance is less than the sum of the two radii
    if (distance <= 2 * ballRadius)
    {
        // Calculate the overlap (how much they are intersecting)
        float overlap = 2 * ballRadius - distance;

        // Push the balls apart along the line connecting their centers
        Vector2 direction = new Vector2(ball2.Position.Item1 - ball1.Position.Item1, ball2.Position.Item2 - ball1.Position.Item2).normalized;

        // Adjust the positions to resolve the overlap
        ball1.Position = (ball1.Position.Item1 - direction.x * overlap / 2, ball1.Position.Item2 - direction.y * overlap / 2);
        ball2.Position = (ball2.Position.Item1 + direction.x * overlap / 2, ball2.Position.Item2 + direction.y * overlap / 2);

        // Swap the velocities (for elastic collision)
        float tempVelocity = ball1.AngularVelocity;
        ball1.AngularVelocity = ball2.AngularVelocity;
        ball2.AngularVelocity = tempVelocity;
    }
}
    private float Distance((float, float) pos1, (float, float) pos2)
    {
        // Calculate Euclidean distance between two points (x1, y1) and (x2, y2)
        return (float)Math.Sqrt(Math.Pow(pos2.Item1 - pos1.Item1, 2) + Math.Pow(pos2.Item2 - pos1.Item2, 2));
    }
}
