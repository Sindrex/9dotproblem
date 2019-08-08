using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public UIController ui;
    public LineMakerScript lineMaker;
    public DotController[] dots;

    public float waitWhenCheckDone = 0.5f;
    public GameObject winText;
    public bool accepted;

    public HTTPController http;
    public DataCollector data;
    public TimerScript timer;

    private void Awake()
    {
        data = GameObject.Find("DataCollector").GetComponent<DataCollector>();
        http = GameObject.Find("Http").GetComponent<HTTPController>();
        timer = GameObject.Find("Timer").GetComponent<TimerScript>();
    }

    private void Start()
    {
        winText.SetActive(false);
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
        }
        else
        {
            //4 lines but no win
            Debug.Log("no win :c");
        }
        addPoints();
    }

    public void addPoints()
    {
        data.add(lineMaker.points, accepted, http);
    }
}
