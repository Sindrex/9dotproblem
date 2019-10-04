using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DataCollector : MonoBehaviour {

    public static readonly int MAX_POINTS = 5;

    public string playerID = "temp";
    public bool urlOK = false;
    public bool redirect;

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

        if (parameters[0].Contains("id="))
        {
            urlOK = true;
            playerID = parameters[0].Split('=')[1];
            Debug.Log("ID sat to: " + playerID);

            if (parameters.Length > 1 && parameters[1].Contains("redirect="))
            {
                string redirectString = parameters[1].Split('=')[1];
                if (redirectString.Equals("true"))
                {
                    redirect = true;
                }
                else
                {
                    redirect = false;
                }
            }
            Debug.Log("Redirect: " + redirect);
        }
        else
        {
            urlOK = false;
            Debug.Log("URL not accepted");
        }
    }

    public void add(List<LineDataPointController> points, bool accepted, HTTPController http)
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
        foreach (LineDataPointController point in points)
        {
            //print("Collecting: " + point.transform.position);
            positions.Add(point.transform.position);
            nodes.Add(point.nodeName);
        }

        ProblemTry newTry = new ProblemTry();
        tries.Add(newTry);
        newTry.positions = positions;
        newTry.nodes = nodes;
        newTry.accepted = accepted;

        //send right away
        int tryNr = (tries.IndexOf(newTry) + 1);
        http.sendOne(playerID, tryNr, newTry);
    }
}
