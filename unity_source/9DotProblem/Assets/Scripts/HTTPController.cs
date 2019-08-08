using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HTTPController : MonoBehaviour {

    public ConfigWrapper config;

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

            config = ConfigWrapper.CreateFromJSON(uwr.downloadHandler.text);
            Debug.Log("URL: " + config.URL);
            Debug.Log("MAX_SEC: " + config.MAX_SEC);
            Debug.Log("SHOW_TIMER: " + config.SHOW_TIMER);
            Debug.Log("HELP_TEXT: " + config.HELP_TEXT);
        }
    }

    public void sendOne(string id, int trynr, ProblemTry pt)
    {
        //All in one json package
        string json = "{\"player_id\": \"" + id + "\"" + ", " + "\"try_nr\": \"" + trynr + "\"";
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
        print("Sending post req to: " + config.URL + "\nSending: " + json);

        var uwr = new UnityWebRequest(config.URL, "POST");
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

[System.Serializable]
public class ConfigWrapper
{
    public string URL;
    public int MAX_SEC;
    public bool SHOW_TIMER;
    public string HELP_TEXT;

    public static ConfigWrapper CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ConfigWrapper>(jsonString);
    }
}
