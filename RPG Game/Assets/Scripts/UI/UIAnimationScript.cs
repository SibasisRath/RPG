using System.Collections;
using UnityEngine;

namespace RPG.UI
{
    public class UIAnimationScript : MonoBehaviour
    {
        private const float overShootAmountConstant = 0.8f;
        private const float SettelDownAmountConstant = 0.2f;
        [SerializeField] private RectTransform uiElement;
        [SerializeField] private Vector2 startPoint;
        [SerializeField] private Vector2 endPoint;
        [SerializeField] private float duration = 1f;
        [SerializeField] private float overshootAmount = 20f; // How much it overshoots before settling
        private float elapsedTime;
        private void Start()
        {
            StartCoroutine(AnimateUIElement());
        }

        private IEnumerator AnimateUIElement()
        {
            elapsedTime = 0f;
            Vector3 overshootPoint = endPoint + (endPoint - startPoint).normalized * overshootAmount;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = EaseInOutCubic(t);

                if (t < overShootAmountConstant) // Move towards overshoot point
                {
                    uiElement.anchoredPosition = Vector3.Lerp(startPoint, overshootPoint, t / overShootAmountConstant);
                }
                else // Settle back to the final position
                {
                    float settleT = (t - overShootAmountConstant) / SettelDownAmountConstant;
                    uiElement.anchoredPosition = Vector3.Lerp(overshootPoint, endPoint, settleT);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            uiElement.anchoredPosition = endPoint;
        }

        private float EaseInOutCubic(float t)
        {
            return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
        }
    }
}