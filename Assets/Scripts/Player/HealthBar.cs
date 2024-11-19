using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour 
{
    public Slider slider;
    public Image mainFill;    
    public Image trailFill;   
    public float trailSpeed = 0.5f;
    public Gradient trailGradient;
    
    [Header("Border Animation")]
    public Animator borderAnimator;  // Reference to the border's Animator component
    
    private float targetFillAmount;
    private float currentFillAmount;
    private float previousHealth;
    private static readonly int DrainTrigger = Animator.StringToHash("Drain");

    void Start()
    {
        currentFillAmount = slider.value / slider.maxValue;
        targetFillAmount = currentFillAmount;
        
        mainFill.fillAmount = currentFillAmount;
        trailFill.fillAmount = currentFillAmount;
        
        // Get the animator component from the Border object
        if (borderAnimator == null)
        {
            // Try to find the Animator on the Border child object
            Transform borderTransform = transform.Find("Border");
            if (borderTransform != null)
            {
                borderAnimator = borderTransform.GetComponent<Animator>();
            }
            
            if (borderAnimator == null)
            {
                Debug.LogWarning("Border Animator not found! Please assign it in the inspector.");
            }
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        
        currentFillAmount = 1f;
        targetFillAmount = 1f;
        mainFill.fillAmount = 1f;
        trailFill.fillAmount = 1f;
        trailFill.color = trailGradient.Evaluate(1f);
        
        previousHealth = health;
    }

    public void SetHealth(int health)
    {
        if (health < previousHealth && borderAnimator != null)
        {
            // Trigger the drain animation
            borderAnimator.SetTrigger(DrainTrigger);
        }
        
        slider.value = health;
        targetFillAmount = (float)health / slider.maxValue;
        mainFill.fillAmount = targetFillAmount;
        
        previousHealth = health;
    }

    void Update()
    {
        if (trailFill.fillAmount > mainFill.fillAmount)
        {
            trailFill.fillAmount = Mathf.Lerp(trailFill.fillAmount, mainFill.fillAmount, Time.deltaTime * trailSpeed);
            
            float gradientProgress = (trailFill.fillAmount - mainFill.fillAmount) / (1 - mainFill.fillAmount);
            gradientProgress = Mathf.Clamp01(gradientProgress);
            
            trailFill.color = trailGradient.Evaluate(gradientProgress);
        }
    }
}