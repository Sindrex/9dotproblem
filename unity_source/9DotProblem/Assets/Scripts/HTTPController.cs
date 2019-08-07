using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HTTPController : MonoBehaviour {

    public string url = "temp";
    public int maxsec = 0;

    //current format: id (varchar), try_nr (int), point1, point2, ... point8 (varchar)

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

        //get URL once
        StartCoroutine(startUp());
    }

    IEnumerator startUp()
    {
        yield return getURL();
        Debug.Log("Fetched URL");
        yield return getMaxSec();
        Debug.Log("Fetched MaxSec");
        //yield return getShowTimer();
        Debug.Log("Fetched MaxSec");
        //yield return getHelpText();
    }

    IEnumerator getURL() //string id, List<ProblemTry> tries
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
            Debug.Log("Received: " + uwr.downloadHandler.text);

            //string response = System.Text.Encoding.UTF8.GetString(uwr.downloadHandler.data);
            //URLWrapper newURL = JsonUtility.FromJson<URLWrapper>(response);
            //Debug.Log("Got URL response: " + response);
            url = uwr.downloadHandler.text;
            url = url.Replace("\"", "");
            Debug.Log("URL set to: " + url);
        }
    }

    IEnumerator getMaxSec()
    {
        string myURL = url.Substring(0, url.Length - 11) + "maxsec";

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
            Debug.Log("Received: " + uwr.downloadHandler.text);

            //string response = System.Text.Encoding.UTF8.GetString(uwr.downloadHandler.data);
            //URLWrapper newURL = JsonUtility.FromJson<URLWrapper>(response);
            //Debug.Log("Got URL response: " + response);

            maxsec = int.Parse(uwr.downloadHandler.text);
        }
    }

    public void send(string id, List<ProblemTry> tries)
    {
        beforeSend(id, tries);
    }

    private void beforeSend(string id, List<ProblemTry> tries)
    {
        int trynr = 1;
        foreach (ProblemTry pt in tries)
        {
            string json = "{\"player_id\": \"" + id + "\"" + ", " + "\"try_nr\": \"" + trynr + "\"";
            int index = 1;
            foreach (Vector2 coord in pt.points)
            {
                json += ", \"point" + index + "\": \"" + coord + "\"";
                index++;
            }
            json += "}";
            print("json: " + json);
            trynr++;

            StartCoroutine(sendPOST(json, url));
        }
    }

    public void sendOne(string id, int trynr, ProblemTry pt)
    {
        string json = "{\"player_id\": \"" + id + "\"" + ", " + "\"try_nr\": \"" + trynr + "\"";
        int index = 1;
        foreach (Vector2 coord in pt.points)
        {
            json += ", \"point" + index + "\": \"" + coord + "\"";
            index++;
        }
        json += "}";
        print("json: " + json);

        StartCoroutine(sendPOST(json, url));
    }

    IEnumerator sendPOST(string json, string url)
    {
        //string url = ResourceLoader.loadJson<URLWrapper>("/config.json").url; //streamingassets, did not work with webGL
        
        print("Sending post req to: " + url + "\nSending: " + json);
        var uwr = new UnityWebRequest(url, "POST");
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
