using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour {

    public bool takeTime = true;

    public float fullTimer;
    public float curTimer;

    // Use this for initialization
    void Start () {
        //Dontdestroyonload
        GameObject clone = GameObject.Find(gameObject.name);
        if (clone != null && clone != this.gameObject)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        takeTime = true;
        fullTimer = 0;
        curTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (takeTime)
        {
            fullTimer += Time.deltaTime;
            curTimer += Time.deltaTime;
        }
    }

    public void Lap(){
        curTimer = 0;
    }
}
