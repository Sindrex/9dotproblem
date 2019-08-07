using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public GameController GC;

    public LineMakerScript lineMaker;
    public Text lineCount;

    public Text tryCount;

    public GameObject quitText;
    public Button quitButton;
    public Button tryAgainButton;

    //set by config
    public Text helpText;
    public int maxsec;

    public TimerScript timer;
    public GameObject timerWrapper;
    public Text timerFull;
    public Text timerCurrent;
    public GameObject timeUpText;

    public CamLerper lerper;
    public Vector3 endTarget;

    private void Start()
    {
        quitText.SetActive(false);
        tryCount.text = "" + GC.data.tries.Count;
        timer = GC.timer;

        //config
        helpText.text = GC.http.config.HELP_TEXT;
        maxsec = GC.http.config.MAX_SEC;
        if (!GC.http.config.SHOW_TIMER)
        {
            timerWrapper.SetActive(false);
        }

        if (GC.data.tries.Count <= 0)
        {
            lerper.lerpIntro(finishedLerp);
            //lineMaker.done = true;
        }
    }

    public void finishedLerp()
    {
        print("Lerping done!");
        //lineMaker.done = false;
    }

    // Update is called once per frame
    private void Update () {
        lineCount.text = "" + (lineMaker.maxLines - lineMaker.myLines.Count);

        timerFull.text = "" + FormatTime(timer.fullTimer);
        timerCurrent.text = "" + FormatTime(timer.curTimer);
        if(timer.fullTimer >= maxsec && maxsec > 0 && timer.takeTime) //check if maxtime is surpassed and maxtime is gotten (not 0).
        {
            Debug.Log("Time's up Sunny!");
            setNonInteractableButtons();
            lineMaker.done = true;
            timer.takeTime = false;
            timeUpText.SetActive(true);
            GC.data.add(lineMaker.myLines, GC.http); //send the remaining
        }
	}

    public void tryAgain()
    {
        GC.data.add(lineMaker.myLines, GC.http);
        timer.curTimer = 0;
        GC.data.trySent = true; //kinda redundant as we're reloading scene
        reloadScene();
    }

    private void reloadScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void setNonInteractableButtons()
    {
        quitButton.interactable = false;
        tryAgainButton.interactable = false;
    }

    //Courtesy of https://answers.unity.com/questions/1476208/string-format-to-show-float-as-time.html
    public string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        //int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
