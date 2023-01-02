using UnityEngine;
using TMPro;


public class CreditsController : MonoBehaviour
{
    [SerializeField] TMP_Text deathsText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text tokensText;

    // Start is called before the first frame update
    void Start()
    {
        // Temporary variables
        StatsManager statsManager = StatsManager.Instance;


        deathsText.text = statsManager.deaths.ToString();

        tokensText.text = statsManager.collectedTokens.ToString() + "/" + statsManager.maxTokens;

        timeText.text = statsManager.ConvertToMinutesSeconds(statsManager.runTime);

        statsManager.SaveRun();
    }
}
