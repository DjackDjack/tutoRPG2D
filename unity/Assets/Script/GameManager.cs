using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script qui ne sera jamais détruit
//utilisé dans les différentes scènes permetant de récuperer les différentes informations globales nécessaires


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public string previousZone;


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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
