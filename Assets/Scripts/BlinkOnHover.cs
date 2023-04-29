using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BlinkOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textMeshPro;
    private Color origColour;
    private bool isBlinking = false;

    private void Start() {
        origColour = textMeshPro.color;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isBlinking) {
            StartCoroutine(BlinkText());
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        isBlinking = false;
        textMeshPro.color = origColour;
    }

    private IEnumerator BlinkText() {
        isBlinking = true;

        while (isBlinking) {
            Color textColor = textMeshPro.color;
            textColor.a = textColor.a == origColour.a ? 1f : origColour.a;
            textMeshPro.color = textColor;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
