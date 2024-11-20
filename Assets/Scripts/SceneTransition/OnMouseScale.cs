using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    [SerializeField] private float scaleAmount = 1.2f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    // when mouse enters
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleAmount;
    }

    // when mouse exits
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}