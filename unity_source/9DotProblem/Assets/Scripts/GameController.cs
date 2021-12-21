using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public UIController ui;
    public LineMakerScript lineMaker;
    public DotController[] dots;

    public float waitWhenCheckDone = 0.5f;
    public GameObject winText;
    public GameObject tryAgainText;
    public Text redirectText;
    public Text redirectTextTimeUp;
    public InputField copyText;
    public InputField copyTextTimeUp;
    private string fullRedirectUrl;
    public bool accepted;

    public HTTPController http;
    public DataCollector data;
    public TimerScript timer;

    [DllImport("__Internal")]
    private static extern void OpenURL(string url);

    [DllImport("__Internal")]
    private static extern bool CheckVisible();
    private int tabbedOutAmount;
    private bool hasTabbedOutLock = false;
    private DateTime tabbedOutDateTime;
    private double tabbedOutSeconds;

    //training
    public bool isTraining = false;

    private void Awake()
    {
        data = GameObject.Find("DataCollector").GetComponent<DataCollector>();
        http = GameObject.Find("Http").GetComponent<HTTPController>();
        if(!isTraining)
        {
            timer = GameObject.Find("Timer").GetComponent<TimerScript>();
        }
    }

    private void Start()
    {
        if(!isTraining)
        {
            winText.SetActive(false);
            tryAgainText.SetActive(false);
            data.trySent = false;
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log("hasTabbedOutLock=" + hasTabbedOutLock);
        if(!isTraining && !CheckVisible() && !hasTabbedOutLock)
        {
            hasTabbedOutLock = true;
            tabbedOutAmount += 1;
            tabbedOutDateTime = DateTime.Now;
            Debug.Log("Tabbed out! tabbedOutAmount=" + tabbedOutAmount);
        }
        else if(!isTraining && CheckVisible() && hasTabbedOutLock)
        {
            var endTabbedOutDateTime = DateTime.Now;
            var delta = endTabbedOutDateTime - tabbedOutDateTime;
            tabbedOutSeconds += delta.TotalSeconds;

            hasTabbedOutLock = false;
            Debug.Log("Tabbed in! Releasing lock. tabbedOutSeconds=" + tabbedOutSeconds);
        }
    }

    public void checkDone()
    {
        StartCoroutine(checkDoneIEnum());
    }

    private IEnumerator checkDoneIEnum()
    {
        if(isTraining)
        {
            yield return new WaitForSeconds(0);
            yield break;
        }
        yield return new WaitForSeconds(waitWhenCheckDone);

        int accepted = 0;
        foreach (DotController dot in dots)
        {
            if (dot.accepted)
            {
                accepted++;
            }
        }
        if (accepted == dots.Length)
        {
            //u win bro
            Debug.Log("u win!");
            winText.SetActive(true);
            ui.setNonInteractableButtons();
            timer.takeTime = false;
            this.accepted = true;
            redirect();
        }
        else
        {
            //4 lines but no win
            Debug.Log("no win :c");
            tryAgainText.SetActive(true);
        }
        addPoints();
    }

    public void redirect()
    {
        print("redirecting: " + data.doRedirect);
        if (data.doRedirect)
        {
            if (!http.config.RedirectUrl.Trim().Equals(""))
            {
                redirectWait(http.config.RedirectUrl, http.config.RedirectTime);
            }
            else
            {
                redirectText.gameObject.SetActive(false);
                Debug.Log("Error: Redirect URL is null");
            }
        }
        else
        {
            redirectText.gameObject.SetActive(false);
        }
    }

    public void addPoints()
    {
        data.add(lineMaker.points, accepted, http, tabbedOutSeconds, tabbedOutAmount);
    }

    public void redirectWait(string url, int waitTime)
    {
        string fullURL = url + "id=" + data.playerID;
        print("Redirect URL: " + fullURL);
        OpenURL(fullURL);
    }

    public void CopyRedirectUrlToClipboard()
    {
        GUIUtility.systemCopyBuffer = fullRedirectUrl;
    }
}
