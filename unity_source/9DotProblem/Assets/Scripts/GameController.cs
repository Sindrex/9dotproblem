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
    public InputField copyText;
    public bool accepted;

    public HTTPController http;
    public DataCollector data;
    public TimerScript timer;

    [DllImport("__Internal")]
    private static extern void OpenURL(string url);

    private void Awake()
    {
        data = GameObject.Find("DataCollector").GetComponent<DataCollector>();
        http = GameObject.Find("Http").GetComponent<HTTPController>();
        timer = GameObject.Find("Timer").GetComponent<TimerScript>();
    }

    private void Start()
    {
        winText.SetActive(false);
        tryAgainText.SetActive(false);
        data.trySent = false;
    }

    public void checkDone()
    {
        StartCoroutine(checkDoneIEnum());
    }

    private IEnumerator checkDoneIEnum()
    {
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
        if (data.doRedirect)
        {
            if (!http.config.RedirectUrl.Trim().Equals(""))
            {
                StartCoroutine(redirectWait(http.config.RedirectUrl, http.config.RedirectTime));
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
        data.add(lineMaker.points, accepted, http);
    }

    IEnumerator redirectWait(string url, int waitTime)
    {
        string fullURL = url + "/?id=" + data.playerID;
        copyText.text = fullURL;
        for (int i = 1; i <= waitTime; i++)
        {
            redirectText.text = "You will be redirected in " + (waitTime - i) + "...";
            yield return new WaitForSeconds(1);
        }

        //Application.OpenURL(url);
        OpenURL(fullURL);

        redirectText.text = "If you are not redirected, please copy:";
    }
}
