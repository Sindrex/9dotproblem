using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DataCollector : MonoBehaviour {

    public static readonly int MAX_POINTS = 5;

    public string playerID = "temp";
    public bool urlOK = false;
    public bool doRedirect;
    public bool showTimer;

    [SerializeField]
    public List<ProblemTry> tries;

    [DllImport("__Internal")]
    private static extern string GetURL();

    public bool trySent = false;

    //Lerping
    public bool trainingLerpDone;
    public bool mainLerpDone;

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

        //URL stuff
        string url = GetURL();
        Debug.Log("URL gotten: " + url);
        string[] parameters = url.Split('?')[1].Split('&');
        /*
        Debug.Log("Params:");
        foreach(string s in parameters)
        {
            Debug.Log(s);
        }*/

        Dictionary<string, string> ParamDict = new Dictionary<string, string>();
        foreach(string s in parameters)
        {
            string[] keyvalue = s.Split('=');
            if(keyvalue.Length != 2)
            {
                continue;
            }
            ParamDict.Add(keyvalue[0], keyvalue[1]);
        }

        string id, redirect, showt;
        if(ParamDict.TryGetValue("id", out id))
        {
            urlOK = true;
            playerID = id;
            Debug.Log("URL OK. ID sat to: " + playerID);
        }
        else
        {
            urlOK = false;
            Debug.Log("URL not accepted");
        }
        if(ParamDict.TryGetValue("redirect", out redirect))
        {
            if (redirect.Equals("true"))
            {
                doRedirect = true;
            }
            else
            {
                doRedirect = false;
            }
            Debug.Log("Redirect: " + redirect);
        }
        if(ParamDict.TryGetValue("showt", out showt))
        {
            if (showt.Equals("true"))
            {
                showTimer = true;
            }
            else
            {
                showTimer = false;
            }
            Debug.Log("Show Timer: " + showTimer);
        }
    }

    public void add(List<LineDataPointController> points, bool accepted, HTTPController http, double tabbedOutTime, int tabbedOutAmount)
    {
        if (trySent)
        {
            Debug.Log("Try already sent! Not sending again");
            return;
        }
        else if (points.Count > MAX_POINTS)
        {
            throw new System.Exception("Too many points! Should be max " + MAX_POINTS);
        }

        trySent = true;

        List<Vector2> positions = new List<Vector2>();
        List<string> nodes = new List<string>();
        List<float> timers = new List<float>();
        var index = 0;
        foreach (LineDataPointController point in points)
        {
            //print("Collecting: " + point.transform.position);
            positions.Add(point.transform.position);
            nodes.Add(point.nodeName);
            timers.Add(point.timerAtCreation);
            if(index > 0)
            {
                var previous = timers[index - 1];
                timers.Add(point.timerAtNextDraw - previous);
            }
            index++;
        }

        print("timers length: " + timers.Count);

        ProblemTry newTry = new ProblemTry();
        tries.Add(newTry);
        newTry.positions = positions;
        newTry.nodes = nodes;
        newTry.timers = timers;
        newTry.accepted = accepted;
        newTry.tabbedOutTime = tabbedOutTime;
        newTry.tabbedOutAmount = tabbedOutAmount;

        //send right away
        int tryNr = (tries.IndexOf(newTry) + 1);
        http.sendOne(playerID, tryNr, newTry);
    }
}
