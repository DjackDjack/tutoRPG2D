using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public GameObject dialogueHolder, continueButton, choice1, choice2;
    public  TextMeshProUGUI nameDisplay, textDisplay, moneyText;

    
    private void Awake() 
    {
        //Singleton = référence pour avoir accès à toutes les varibales et toutes les fonctions propre à un script
        //il ne peut avoir qu'un seule instance de ce script
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        moneyText.text = PlayerController.instance.money.ToString();
    }

}
