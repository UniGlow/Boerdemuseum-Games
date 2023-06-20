using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class Healthbar : MonoBehaviour
{

    #region Variable Declarations
    [Space]
    [SerializeField] Gradient colorGradient = null;

    [Header("Settings")]
    [SerializeField] bool useFillAmount;

    [Header("Tweening")]
    [SerializeField] bool punchOnUpdate = true;
    [ConditionalHide("punchOnUpdate", true, false)]
    [SerializeField] float punchAmount = 0.1f;
    [ConditionalHide("punchOnUpdate", true, false)]
    [SerializeField] float punchDuration = 0.3f;

    [Header("References")]
    [SerializeField] Transform healthbarForeground = null;

    Image foregroundImage;
    #endregion



    #region Unity Event Functions
    private void Awake()
    {
        foregroundImage = healthbarForeground.GetRequiredComponent<Image>();
    }
    #endregion



    #region Public Functions
    public void UpdateHealthbar(float percentage)
    {
        // Error handling
        if (percentage > 1f || percentage < -0.01f)
        {
            //Debug.LogError("Tried to update healthbar with invalid percentage value: " + percentage);
            return;
        }

        // Set new color of the healthbar
        healthbarForeground.GetComponent<Image>().color = colorGradient.Evaluate(percentage);

        if (useFillAmount)
        {
            foregroundImage.fillAmount = percentage;
        }
        else
        {
            // Rescale the green part of the healthbar
            Vector3 newScale = new Vector3(percentage, healthbarForeground.localScale.y, healthbarForeground.localScale.z);
            healthbarForeground.transform.localScale = newScale;

            // Add Juiciness
            if (punchOnUpdate && punchAmount > 0f && punchDuration > 0f) healthbarForeground.DOPunchScale(newScale + Vector3.one * punchAmount, punchDuration);
        }
    }


    public void UpdateHealthBarSpecial(float value, float min, float max)
    {
        float normal = Mathf.InverseLerp(min, max, value);
        float bValue = Mathf.Lerp(0, 1, normal);
        UpdateHealthbar(bValue);
    }
    #endregion
}
