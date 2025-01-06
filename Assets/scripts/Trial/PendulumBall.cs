using System;
public class Pendulum
{
    public float Length { get; private set; }  // Length of the string
    public float Mass { get; private set; }    // Mass of the ball
    public float Angle { get; set; }           // Angle from the vertical
    public float AngularVelocity { get; set; }
    public float AngularAcceleration { get; private set; }
    public (float, float) Position { get; set; } // Position as 2D coordinates (x, y)

    private const float Gravity = 9.81f;
    public (float, float) AnchorPoint { get; set; } // Add anchor point to store the starting point of the pendulum

    // Modify the constructor to accept anchorPoint
    public Pendulum(float length, float mass, float initialAngle, (float, float) anchorPoint)
    {
        Length = length;
        Mass = mass;
        Angle = initialAngle;
        AngularVelocity = 0;
        AngularAcceleration = 0;
        AnchorPoint = anchorPoint; // Store the anchor point
        UpdatePosition();
    }

    public void ApplyPhysics(float deltaTime)
    {
        // Calculate angular acceleration based on the angle
        AngularAcceleration = -(Gravity / Length) * (float)Math.Sin(Angle);

        // Update angular velocity and angle using Euler's method
        AngularVelocity += AngularAcceleration * deltaTime;
        Angle += AngularVelocity * deltaTime;

        // Optional damping (reduces angular velocity over time)
        AngularVelocity *= 0.99f;

        // Update position in Cartesian coordinates (x, y)
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // Convert polar (r, θ) to Cartesian (x, y)
        Position = (
            AnchorPoint.Item1 + Length * (float)Math.Sin(Angle),    // x = AnchorPoint.x + Length * sin(θ)
            AnchorPoint.Item2 - Length * (float)Math.Cos(Angle)     // y = AnchorPoint.y - Length * cos(θ)
        );
    }
}
