using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DataCollector : MonoBehaviour {

    public string playerID = "temp";
    public bool urlOK = false;

    [SerializeField]
    public List<ProblemTry> tries;

    [DllImport("__Internal")]
    private static extern string GetURL();

    public bool trySent = false;

    private void Start()
    {
        //Dontdestroyonload
        GameObject clone = GameObject.Find(gameObject.name);
        if(clone != null && clone != this.gameObject)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        tries = new List<ProblemTry>();

        string url = GetURL();
        Debug.Log("URL gotten: " + url);
        if (url.Contains("id="))
        {
            urlOK = true;
            playerID = url.Split('=')[1];
            Debug.Log("ID sat to: " + playerID);
        }
        else
        {
            urlOK = false;
            Debug.Log("URL not accepted");
        }
    }

    public void add(List<GameObject> lines, HTTPController http)
    {
        if (trySent)
        {
            Debug.Log("Try already sent! Not sending again");
            return;
        }

        foreach (GameObject go in lines)
        {
            LineRenderer lr = go.GetComponent<LineRenderer>();
            print("Collecting: " + lr.GetPosition(0) + "/" + lr.GetPosition(1));
        }

        List<Vector2> points = new List<Vector2>();
        foreach(GameObject go in lines)
        {
            LineRenderer lr = go.GetComponent<LineRenderer>();
            points.Add(lr.GetPosition(0));
            points.Add(lr.GetPosition(1));
        }
        ProblemTry newTry = new ProblemTry(points);
        tries.Add(newTry);

        //send right away
        http.sendOne(playerID, (tries.IndexOf(newTry) + 1), newTry);
    }
}
