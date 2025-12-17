using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Image unitIconImage;
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private TMP_Text movePointText;
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private TMP_Text visualRangeText;
    
    // Optional additional status
    [SerializeField] private GameObject armedStatusObj;
    [SerializeField] private TMP_Text armedText;

    public void UpdateInfo(Unit unit)
    {
        GameObject targetObj = contentParent != null ? contentParent : gameObject;

        if (unit == null)
        {
            targetObj.SetActive(false);
            return;
        }

        targetObj.SetActive(true);

        if (unitIconImage != null && unit.UnitSprite != null)
            unitIconImage.sprite = unit.UnitSprite.sprite;

        if (unitNameText != null)
            unitNameText.text = unit.UnitName;

        if (movePointText != null)
            movePointText.text = $"MP: {unit.MovePoint}";

        if (strengthText != null)
            strengthText.text = $"STR: {unit.Strength}";

        if (visualRangeText != null)
            visualRangeText.text = $"View: {unit.VisualRange}";

        // Handle Unit Type specific data if needed
        if (unit.UnitType == UnitType.Land)
        {
            LandUnit landUnit = (LandUnit)unit;
            // Example: Show armed status
            if (armedStatusObj != null)
            {
                armedStatusObj.SetActive(false); 
            }
        }
        else if (unit.UnitType == UnitType.Naval)
        {
             // Naval specific
             if (armedStatusObj != null)
                armedStatusObj.SetActive(false);
        }
    }
}
