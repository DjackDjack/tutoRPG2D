using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( fileName = "Quest", menuName = "ScriptableObject/Quest", order = 0)]

public class QuestSO : ScriptableObject
{
    public bool isEnabled, hasSeries, thisPnj;
    public int id, seriesID;
    public string title;
    public string description;
    public string[] sentences, InProgressSentence, completeSentence, afterQuestSentence;
    public string objectToFind;
    public int actualAmount, amountToFind;
    public int goldToGive;

    [System.Serializable]
    //enum = petit menu déroulant
    public enum Statut
    {
        none,
        accepter,
        complete
    }

    public Statut statut;



}
