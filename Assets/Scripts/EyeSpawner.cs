using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvancedEyeSpawner : MonoBehaviour
{
    [Header("Eye Settings")]
    public GameObject eyePrefab;
    public int maxEyesAtOnce = 3;
    [Range(0, 1)]
    public float spawnChance = 0.02f;
    
    [Header("Spawn Area")]
    public float minX = 0f;
    public float maxX = 10f;
    public float minY = -7.5f;
    public float maxY = -4f;
    
    [Header("Timing")]
    public float checkInterval = 2f;
    public float minLifetime = 3f;
    public float maxLifetime = 5f;
    
    [Header("Animation Settings")]
    public string[] animationNames = { "IdleSearch", "Move","IdleEye","TriggerEye","SideMoving" };
    public float fadeInTime = 1f;
    public float fadeOutTime = 3f;

    private Camera mainCamera;
    private int currentEyes = 0;
    private HashSet<string> currentlyPlayingAnimations = new HashSet<string>();

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnCheck());
    }

    IEnumerator SpawnCheck()
    {
        while (true)
        {
            if (currentEyes < maxEyesAtOnce && Random.value < spawnChance)
            {
                // Only spawn if there are available animations
                if (currentlyPlayingAnimations.Count < animationNames.Length)
                {
                    SpawnEye();
                }
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    string GetRandomUniqueAnimation()
    {
        // Create a list of available animations (ones not currently playing)
        List<string> availableAnimations = new List<string>();
        foreach (string anim in animationNames)
        {
            if (!currentlyPlayingAnimations.Contains(anim))
            {
                availableAnimations.Add(anim);
            }
        }

        // If we have available animations, pick one randomly
        if (availableAnimations.Count > 0)
        {
            int randomIndex = Random.Range(0, availableAnimations.Count);
            string selectedAnim = availableAnimations[randomIndex];
            currentlyPlayingAnimations.Add(selectedAnim);
            return selectedAnim;
        }

        return null; // Should never reach here due to our spawn check
    }

    void SpawnEye()
    {
        string selectedAnimation = GetRandomUniqueAnimation();
        if (selectedAnimation == null) return;

        Vector3 camPos = mainCamera.transform.position;
        float spawnX = camPos.x + Random.Range(minX, maxX);
        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        GameObject eye = Instantiate(eyePrefab, spawnPosition, Quaternion.identity);
        currentEyes++;

        // Get components
        Animator animator = eye.GetComponent<Animator>();
        SpriteRenderer spriteRenderer = eye.GetComponent<SpriteRenderer>();

        // Start fully transparent
        if (spriteRenderer != null)
        {
            Color startColor = spriteRenderer.color;
            startColor.a = 0;
            spriteRenderer.color = startColor;
        }

        // Play the selected unique animation
        if (animator != null)
        {
            animator.Play(selectedAnimation);
        }

        // Handle lifetime and fading
        float lifetime = Random.Range(minLifetime, maxLifetime);
        StartCoroutine(HandleEyeLifetime(eye, spriteRenderer, lifetime, selectedAnimation));
    }

    IEnumerator HandleEyeLifetime(GameObject eye, SpriteRenderer spriteRenderer, float lifetime, string animationName)
    {
        // Fade in
        float elapsed = 0;
        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float alpha = elapsed / fadeInTime;
            Color newColor = spriteRenderer.color;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
            yield return null;
        }

        // Wait for main lifetime
        yield return new WaitForSeconds(lifetime - fadeInTime - fadeOutTime);

        // Fade out
        elapsed = 0;
        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float alpha = 1 - (elapsed / fadeOutTime);
            Color newColor = spriteRenderer.color;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
            yield return null;
        }

        // Remove animation from currently playing set
        currentlyPlayingAnimations.Remove(animationName);

        // Destroy
        if (eye != null)
        {
            Destroy(eye);
            currentEyes--;
        }
    }
}