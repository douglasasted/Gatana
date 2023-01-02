using UnityEngine;
using TMPro;


public class MenuController : MonoBehaviour
{
    [SerializeField] TMP_Text bestRunTimeText;
    [SerializeField] TMP_Text leastDeathsText;
    [SerializeField] TMP_Text tokensText;

    // Start is called before the first frame update
    void Start()
    {
        // Replayability feature

        // Temporary Variables
        int _leastDeaths = StatsManager.Instance.leastDeaths;
        float _bestRunTime = StatsManager.Instance.bestRunTime;
        float _collectedTokens = StatsManager.Instance.bestTokens;
        float _maxTokens = StatsManager.Instance.maxTokens;


        if (_leastDeaths == -1)
            leastDeathsText.text = "";
        else
            leastDeathsText.text = "Least Deaths | " + _leastDeaths.ToString();

        if (_bestRunTime == -1)
            bestRunTimeText.text =  "";
        else
            bestRunTimeText.text = "Best Time | " + StatsManager.Instance.ConvertToMinutesSeconds(_bestRunTime);

        if (_collectedTokens == -1)
            tokensText.text =  "";
        else
            tokensText.text = "Soul Tokens | " + _collectedTokens + "/" + _maxTokens;
    }

    public void NewGame()
    {
        CameraTransition.Instance.TransitionScene("Chapter1_1");
    }
    
    public void Exit()
    {
        CameraTransition.Instance.TransitionScene("Exit");
    }
}
