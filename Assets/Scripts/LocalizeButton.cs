using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


// Script for updating button when language changes
public class LocalizeButton : MonoBehaviour
{
    [SerializeField] LocalizedSprite localizedDefaultButtonSprite;
    [SerializeField] LocalizedSprite localizedSelectedButtonSprite;

    // Local variables
    Sprite defaultButtonSprite;
    Sprite selectedButtonSprite;


    void OnEnable() 
    {
        // Adding "SelectedLocaleChanged" to be called when the language changes
        LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
        // Loading the asset for the first time
        StartCoroutine(LoadAssetCourotine());
    }

    void OnDisable() 
    {
        // If the object is disabled there's no need to update the button
        // when language changes
        LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
    }


    void SelectedLocaleChanged(Locale obj) 
    {
        StartCoroutine(LoadAssetCourotine());
    }

    IEnumerator LoadAssetCourotine() 
    {
        var operation = localizedDefaultButtonSprite.LoadAssetAsync();
        
        yield return operation;
        
        defaultButtonSprite = operation.Result;

        operation = localizedSelectedButtonSprite.LoadAssetAsync();
        
        yield return operation;

        selectedButtonSprite = operation.Result;

        UpdateButton();
    }

    public void UpdateButton() 
    {
        SpriteState buttonSS = new SpriteState();

        buttonSS.highlightedSprite = selectedButtonSprite;
        buttonSS.pressedSprite = selectedButtonSprite;
        buttonSS.selectedSprite = selectedButtonSprite;
        
        GetComponent<Button>().spriteState = buttonSS;
        GetComponent<Image>().sprite = defaultButtonSprite;
    }
}
