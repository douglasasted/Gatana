using UnityEngine;
using TMPro;


public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text bestRunTimeText;
    [SerializeField] TMP_Text leastDeathsText;
    [SerializeField] TMP_Text tokensText;

    [Space]

    [SerializeField] GameObject baseMenu;
    [SerializeField] GameObject settingsMenu;

    [Space]

    [SerializeField] AudioSource pressSound;


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
        // Sound
        pressSound.Play();


        // Utility
        CameraTransition.Instance.TransitionScene("Chapter1_1");
    }
    
    public void Settings()
    {
        // Sound
        pressSound.Play();


        // Utility
        baseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    
    public void Back()
    {
        // Sound
        pressSound.Play();


        // Utility
        baseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void Exit()
    {
        // Sound
        pressSound.Play();


        // Utility
        CameraTransition.Instance.TransitionScene("Exit");
    }
}
