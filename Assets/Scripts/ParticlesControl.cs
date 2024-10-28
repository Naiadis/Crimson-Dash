using UnityEngine;

public class ParticleParallax : MonoBehaviour
{
    public GameObject cam;
    public float parallaxEffect = 0.5f;
    private float startPosition;
    private float length;
    private new ParticleSystem particleSystem;
    
    void Start()
    {
        startPosition = transform.position.x;
        particleSystem = GetComponent<ParticleSystem>();
        
        // Get the width of your particle system's shape
        var shape = particleSystem.shape;
        length = shape.scale.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);
        
        // Update position
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        // Loop the position
        if (movement > startPosition + length)
        {
            startPosition += length;
        }
        else if (movement < startPosition - length)
        {
            startPosition -= length;
        }

        // Update particle system's simulation space
        var mainModule = particleSystem.main;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
    }
}