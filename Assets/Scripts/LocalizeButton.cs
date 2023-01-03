using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;


public class LocalizeButton : MonoBehaviour
{
    [SerializeField] LocalizedSprite defaultButtonSprite;
    [SerializeField] LocalizedSprite selectedButtonSprite;


    // Start is called before the first frame update
    void Start()
    {
        SpriteState buttonSS = new SpriteState();

        buttonSS.highlightedSprite = selectedButtonSprite.LoadAsset();
        
        GetComponent<Button>().spriteState = buttonSS;
        GetComponent<Image>().sprite = defaultButtonSprite.LoadAsset();
    }
}
