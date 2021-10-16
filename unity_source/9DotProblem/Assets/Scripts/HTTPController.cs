using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HTTPController : MonoBehaviour {

    public ConfigWrapper config;
    public List<Action> onConfigDownloaded;

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
            Debug.Log("URL: " + tempconfig.Url);
            Debug.Log("MAX_SEC: " + tempconfig.TimeLimitSeconds);
            Debug.Log("SHOW_TIMER: " + tempconfig.ShowTimer);
            Debug.Log("HELP_TEXT: " + tempconfig.HelpText);
            Debug.Log("REDIRECT_URL: " + tempconfig.RedirectUrl);

            config = tempconfig;

            foreach(Action a in onConfigDownloaded)
            {
                a();
            }
        }
    }

    public void sendOne(string id, int trynr, ProblemTry pt)
    {
        //All in one json package
        string json = "{" + "\"created_at\": \"" + System.DateTime.Now + "\", " + "\"player_id\": \"" + id + "\", " + "\"try_nr\": \"" + trynr + "\"";
        int index = 1;
        foreach (Vector2 coord in pt.positions)
        {
            json += ", \"point" + index + "\": \"" + coord + "\"";
            index++;
        }
        index = 1;
        foreach (string nodeName in pt.nodes)
        {
            json += ", \"node" + index + "\": \"" + nodeName + "\"";
            index++;
        }
        json += ", \"accepted\": \"" + pt.accepted + "\"";
        json += "}";
        print("json: " + json);

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