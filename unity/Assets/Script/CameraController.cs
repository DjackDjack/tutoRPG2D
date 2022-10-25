using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Lib lib;

    public Transform target;

    public float ymin, ymax, xmin, xmax;

    public static CameraController instance;

    private void Start()
    {
        lib = GameObject.FindGameObjectWithTag("Lib").GetComponent<Lib>();
    }

    private void Awake() 
    {

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

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.transform.position;
        targetPos.z = -10;
        transform.position = new Vector3(Mathf.Clamp(targetPos.x, xmin, xmax), Mathf.Clamp(targetPos.y, ymin, ymax), targetPos.z);
    }
}
