using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HTTPController : MonoBehaviour 
{
    private bool configDownloaded;
    public ConfigWrapper config;
    private List<Action> onConfigDownloaded = new List<Action>();

    private void Awake()
    {
        //Dontdestroyonload
        GameObject clone = GameObject.Find(gameObject.name);
        if (clone != null && clone != this.gameObject)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        //get config once
        StartCoroutine(getConfig());
    }

    IEnumerator getConfig() //string id, List<ProblemTry> tries
    {
        string myURL = Application.streamingAssetsPath;

        Debug.Log("Sending get req to: " + myURL);
        var uwr = new UnityWebRequest(myURL, "GET");
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: \n" + uwr.downloadHandler.text);

            var tempconfig = ConfigWrapper.CreateFromJSON(uwr.downloadHandler.text);

            config = tempconfig;
            configDownloaded = true;

            foreach(Action a in onConfigDownloaded)
            {
                a();
            }
        }
    }

    public void addOnConfigDownloaded(Action a)
    {
        if(!configDownloaded)
        {
            onConfigDownloaded.Add(a);
        }
        else
        {
            a();
        }

    }

    public void sendOne(string id, int trynr, ProblemTry pt)
    {
        TryRawAndConverted fullTry = new TryRawAndConverted
        {
            created_at = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            player_id = id,
            try_nr = trynr,
            point1 = pt.positions.Count > 0 ? pt.positions[0].ToString() : null,
            point2 = pt.positions.Count > 1 ? pt.positions[1].ToString() : null,
            point3 = pt.positions.Count > 2 ? pt.positions[2].ToString() : null,
            point4 = pt.positions.Count > 3 ? pt.positions[3].ToString() : null,
            point5 = pt.positions.Count > 4 ? pt.positions[4].ToString() : null,
            node1 = pt.nodes.Count > 0 ? pt.nodes[0] : null,
            node2 = pt.nodes.Count > 1 ? pt.nodes[1] : null,
            node3 = pt.nodes.Count > 2 ? pt.nodes[2] : null,
            node4 = pt.nodes.Count > 3 ? pt.nodes[3] : null,
            node5 = pt.nodes.Count > 4 ? pt.nodes[4] : null,
            accepted = pt.accepted,

            timer1 = pt.timers.Count > 0 ? pt.timers[0] : -1,
            timer2 = pt.timers.Count > 1 ? pt.timers[1] : -1,
            timer3 = pt.timers.Count > 2 ? pt.timers[2] : -1,
            timer4 = pt.timers.Count > 3 ? pt.timers[3] : -1,
            timer5 = pt.timers.Count > 4 ? pt.timers[4] : -1,
            timer6 = pt.timers.Count > 5 ? pt.timers[5] : -1,
            timer7 = pt.timers.Count > 6 ? pt.timers[6] : -1,
            timer8 = pt.timers.Count > 7 ? pt.timers[7] : -1,

            hasTabbedOut = pt.totalTabbedOutTimer > 0,
            totalTabbedOutTime = pt.totalTabbedOutTimer
        };

        var json = JsonUtility.ToJson(fullTry);
        print(json);
        StartCoroutine(sendPOST(json));
    }

    IEnumerator sendPOST(string json)
    {
        print("Sending post req to: " + config.Url + "\nSending: " + json);
        //yield return new WaitForSeconds(1);
        var uwr = new UnityWebRequest(config.Url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}