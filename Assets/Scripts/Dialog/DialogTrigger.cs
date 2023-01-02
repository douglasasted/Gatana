using UnityEngine;


public class DialogTrigger : MonoBehaviour
{
    [SerializeField] DialogManager dialogManager;


    public void StartDialog()
    {
        dialogManager.StartDialog();
    }
}
