using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData instance;

    private void Awake() {
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

    private void Start()
    {
        LoadData();
    }

    private void OnApplicationQuit() 
    {
        Save();
    }

    public void Save()
    {
        //Player Pref = sauvegarde locale sur la machine enregistrant les préférences du joueur (type de base)

        PlayerPrefs.SetFloat("positionX", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("positionY", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("positionZ", PlayerController.instance.transform.position.z);

        for (int i = 0; i < QuestManager.instance.allQuest.Count; i++)
        {
            PlayerPrefs.SetString(QuestManager.instance.allQuest[i].title, QuestManager.instance.allQuest[i].title);
            // 0 = true et 1 = false
            PlayerPrefs.SetInt(QuestManager.instance.allQuest[i].title + "/Enabled", QuestManager.instance.allQuest[i].isEnabled ? 0:1);
            PlayerPrefs.SetString(QuestManager.instance.allQuest[i].title + "/Statut", QuestManager.instance.allQuest[i].statut.ToString());
            PlayerPrefs.SetInt(QuestManager.instance.allQuest[i].title + "/amount", QuestManager.instance.allQuest[i].actualAmount);
            

        }

        PlayerPrefs.Save();
    }

    public bool intToBool(int val)
    {
        if(val == 0)
            return true;
        else
            return false;
    }

    //edit clear AllPlayerPref pour les tests

    public void LoadData()
    {
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("positionX", 0), PlayerPrefs.GetFloat("positionY", 0), PlayerPrefs.GetFloat("positionZ", 0));

        for (var i = 0; i < QuestManager.instance.allQuest.Count; i++)
        {
           if(PlayerPrefs.HasKey(QuestManager.instance.allQuest[i].title))
           {
                QuestManager.instance.allQuest[i].isEnabled = intToBool(PlayerPrefs.GetInt(QuestManager.instance.allQuest[i].title + "/Enabled", 0));
                QuestManager.instance.allQuest[i].statut = (QuestSO.Statut)System.Enum.Parse(typeof(QuestSO.Statut), 
                                                            PlayerPrefs.GetString(QuestManager.instance.allQuest[i].title + "/Statut", 
                                                            QuestSO.Statut.none.ToString()));
                QuestManager.instance.allQuest[i].actualAmount = PlayerPrefs.GetInt(QuestManager.instance.allQuest[i].title + "/amount", 0);

           }
        }

        
    }


}
