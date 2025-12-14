using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Dialog : MonoBehaviour
{
    [Header("Question Dialog")]
    [SerializeField]
    private TMP_Text notiText;
    public TMP_Text NotiText { get { return notiText; } set { notiText = value; } }

    [SerializeField]
    private Button btnYes;

    [SerializeField]
    private TMP_Text yesText;

    [SerializeField]
    private Button btnNo;

    [SerializeField]
    private TMP_Text noText;

    [SerializeField]
    private Button btnOkay;

    [SerializeField]
    private TMP_Text okayText;

    [SerializeField]
    private Image charImage;

    [Header("Combat Dialog")]

    //Attacker
    [SerializeField]
    private Image attackerFactionImage;

    [SerializeField]
    private Image[] attackerImage;

    [SerializeField]
    private TMP_Text[] attackerNameText;

    [SerializeField]
    private TMP_Text[] attackerStrText;

    //Defender
    [SerializeField]
    private Image defenderFactionImage;

    [SerializeField]
    private Image[] defenderImage;

    [SerializeField]
    private TMP_Text[] defenderNameText;

    [SerializeField]
    private TMP_Text[] defenderStrText;


    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup()
    {
        float duration = 0.4f;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            
            // Ease Out Back implementation for a "pop" effect
            // c1 represents the overshoot amount
            float c1 = 1.70158f;
            float c3 = c1 + 1;
            
            float scale = 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
            
            transform.localScale = Vector3.one * scale;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    public void DialogGoToEuropeInit(DialogData data)
    {
        if (notiText != null)
            notiText.text = data.question;

        if (yesText != null)
            yesText.text = data.answers[0];

        if (noText != null)
            noText.text = data.answers[1];

        if (btnYes != null)
            btnYes.gameObject.SetActive(true);

        if (btnNo != null)
            btnNo.gameObject.SetActive(true);

        if (btnOkay != null)
            btnOkay.gameObject.SetActive(false);
    }

    public void DialogCombatAnalysisInit(Unit attacker, Unit defender)
    {
        Debug.Log(attackerFactionImage);
        attackerFactionImage.sprite = attacker.Faction.ShieldIcon;
        attackerImage[0].sprite = attacker.UnitSprite.sprite;
        attackerNameText[0].text = attacker.UnitName;
        attackerStrText[0].text = attacker.Strength.ToString();

        defenderFactionImage.sprite = defender.Faction.ShieldIcon;
        defenderImage[0].sprite = defender.UnitSprite.sprite;
        defenderNameText[0].text = defender.UnitName;
        defenderStrText[0].text = defender.Strength.ToString();
    }

    public void DialogCombatResultInit(Unit winner, Unit loser)
    {
        if (notiText != null)
            notiText.text = 
                ($"<color=yellow>{winner.Faction.Nation} {winner.UnitName}</color> defeats " +
                $"<color=yellow>{loser.Faction.Nation} {loser.UnitName}</color> ");

        if (btnYes != null)
            btnYes.gameObject.SetActive(false);

        if (btnNo != null)
            btnNo.gameObject.SetActive(false);

        if (btnOkay != null)
            btnOkay.gameObject.SetActive(true);
    }

    public void DialogNativeIntroInit(Faction faction)
    {
        if (charImage != null && faction.LeaderSprite != null)
            charImage.sprite = faction.LeaderSprite;

        if (notiText != null)
        {
            if (faction.IsEuropean)
                notiText.text =
                    ($"Greetings and welcome to <color=yellow>New {faction.Nation}</color>. " +
                    $"We have justly claimed all of this land in the name of King of " +
                    $"<color=yellow>{faction.Nation}</color>. Please do not interfere with us. " +
                    $"We propose a <color=yellow>peace treaty</color>. Do you agree?");
            else
                notiText.text =
                    ($"The <color=yellow>{faction.Nation}</color> tribe welcomes you." +
                    $"To celebrate our friendship, we generously offer you " +
                    $"the land you now occupy as a gift. Will you accept our treaty " +
                    $"and live with us in peace as brothers?");
        }

        if (btnYes != null)
            btnYes.gameObject.SetActive(true);

        if (btnNo != null)
            btnNo.gameObject.SetActive(true);

        if (btnOkay != null)
            btnOkay.gameObject.SetActive(false);
    }

    public void ShowNotiText(string s)
    {
        notiText.text = s;

        if (btnYes != null)
            btnYes.gameObject.SetActive(false);

        if (btnNo != null)
            btnNo.gameObject.SetActive(false);

        if (btnOkay != null)
            btnOkay.gameObject.SetActive(true);
    }

    public void ThisMeansWar(Faction faction)
    {
        if (notiText != null)
        {
            if (faction.IsEuropean)
                notiText.text =
                    ($"This means you are at war with <color=yellow>{faction.Nation}</color> already. " +
                    $"See you on the battlefield!");
            else
                notiText.text =
                    ($"My tribe, <color=yellow>{faction.Nation}</color> tribe will kill all of you. " +
                    $"Prepare to Die!!!");
        }

        if (btnYes != null)
            btnYes.gameObject.SetActive(false);

        if (btnNo != null)
            btnNo.gameObject.SetActive(false);

        if (btnOkay != null)
            btnOkay.gameObject.SetActive(true);
    }

    public void Close()
    {
        StartCoroutine(AnimateClose());
    }

    private IEnumerator AnimateClose()
    {
        float duration = 0.3f;
        float time = 0;
        Vector3 initialScale = transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            
            // Ease In Back logic: start at 1, overshoot slightly (scale up), then shrink to 0.
            float c1 = 1.70158f;
            float c3 = c1 + 1;
            float scale = 1 - (c3 * t * t * t - c1 * t * t);
            
            transform.localScale = initialScale * scale;
            yield return null;
        }
        
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}