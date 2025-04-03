using UnityEngine;
using UnityEngine.UI;

public class DimensionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image energyBarFill;
    [SerializeField] private Image energyBarBackground;
    [SerializeField] private Color energyBarColor = new Color(0.5f, 0.2f, 0.8f, 1f);
    [SerializeField] private Color energyBarBackgroundColor = new Color(0.2f, 0.1f, 0.3f, 0.5f);

    private void Start()
    {
        if (energyBarFill != null)
        {
            energyBarFill.color = energyBarColor;
        }
        if (energyBarBackground != null)
        {
            energyBarBackground.color = energyBarBackgroundColor;
        }
    }
} 