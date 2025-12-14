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
        if (unit == null)
        {
            if (contentParent != null)
                contentParent.SetActive(false);
            return;
        }

        if (contentParent != null)
            contentParent.SetActive(true);

        if (unitIconImage != null && unit.UnitSprite != null)
            unitIconImage.sprite = unit.UnitSprite.sprite;

        if (unitNameText != null)
            unitNameText.text = unit.UnitName;

        if (movePointText != null)
            movePointText.text = $"MP: {unit.MovePoint}/{unit.MovePointMax}";

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
                // Assuming we might have an IsArmed property or similar in the future, 
                // for now we can rely on data or just hide it.
                // Based on LandUnitData, there is 'armed' bool.
                // But LandUnit itself might not expose it directly easily without checking Data.
                // Let's check if we can access Data.
                // unit doesn't strictly hold reference to LandUnitData publically in base Unit?
                // Let's check Unit.cs later. For now, basic info is good.
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
