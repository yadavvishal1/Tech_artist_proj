using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SpeedLinesController : MonoBehaviour
{
    [Header("Speed Line Settings")]
    [Tooltip("How fast the lines fly past the screen")]
    public float lineSpeed = 60f;
    [Tooltip("Number of lines emitted per second")]
    public int density = 150;
    [Tooltip("How stretched the lines appear")]
    public float stretchLength = 0.05f;
    [Tooltip("Color of the speed lines")]
    public Color lineColor = new Color(1f, 1f, 1f, 0.4f);

    private ParticleSystem speedLinesPS;
    private ParticleSystemRenderer psRenderer;

    private void Start()
    {
        CreateProceduralSpeedLines();
    }

    private void CreateProceduralSpeedLines()
    {
        // Create the VFX Object
        GameObject psObject = new GameObject("VFX_SpeedLines");
        psObject.transform.SetParent(transform); // Attach to this Camera
        
        // Position it 25 units in front of the camera, rotated 180 to shoot strictly backwards
        psObject.transform.localPosition = new Vector3(0, 0, 25f);
        psObject.transform.localRotation = Quaternion.Euler(0, 180f, 0);

        // Add Particle System
        speedLinesPS = psObject.AddComponent<ParticleSystem>();
        
        // --- Main Module ---
        var main = speedLinesPS.main;
        main.duration = 1f;
        main.loop = true;
        main.startLifetime = 1.5f;
        main.startSpeed = lineSpeed;
        main.startSize = 0.15f; // Thin width
        main.startColor = lineColor;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.playOnAwake = true;

        // --- Emission Module ---
        var emission = speedLinesPS.emission;
        emission.rateOverTime = density;

        // --- Shape Module ---
        var shape = speedLinesPS.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 18f; // Wide circle
        shape.radiusThickness = 0.2f; // Only emit from the outer rim of the circle, avoiding the direct center screen
        
        // --- Renderer Module ---
        psRenderer = psObject.GetComponent<ParticleSystemRenderer>();
        psRenderer.renderMode = ParticleSystemRenderMode.Stretch;
        psRenderer.cameraVelocityScale = 0f;
        psRenderer.velocityScale = stretchLength; // Stretches based on high speed
        psRenderer.lengthScale = 1f;
        
        // Assign default Universal Render Pipeline Unlit Particle material to avoid pink squares
        Material defaultMaterial = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
        psRenderer.material = defaultMaterial;
    }
}
