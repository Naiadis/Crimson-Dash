using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour 
{
    public Slider slider;
    public Image mainFill;    
    public Image trailFill;   
    public float trailSpeed = 0.5f;
    public Gradient trailGradient; 

    private float targetFillAmount;
    private float currentFillAmount;

    void Start()
    {
        currentFillAmount = slider.value / slider.maxValue;
        targetFillAmount = currentFillAmount;
        
        mainFill.fillAmount = currentFillAmount;
        trailFill.fillAmount = currentFillAmount;

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
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        targetFillAmount = (float)health / slider.maxValue;
        mainFill.fillAmount = targetFillAmount;
    }

    void Update()
    {
        if (trailFill.fillAmount > mainFill.fillAmount)
        {
            trailFill.fillAmount = Mathf.Lerp(trailFill.fillAmount, mainFill.fillAmount, Time.deltaTime * trailSpeed);
            
            // Calculate gradient progress based on how far the trail is from the health bar
            float gradientProgress = (trailFill.fillAmount - mainFill.fillAmount) / (1 - mainFill.fillAmount);
            gradientProgress = Mathf.Clamp01(gradientProgress);
            
            // Update trail color based on gradient
            trailFill.color = trailGradient.Evaluate(gradientProgress);
        }
    }
}