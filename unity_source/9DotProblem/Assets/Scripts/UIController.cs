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
    public GameObject helpObject;
    public Text helpText;
    public int maxsec;

    public TimerScript timer;
    public GameObject timerWrapper;
    public Text timerFull;
    public GameObject timeUpText;

    public CamLerper lerper;
    public CamLerper lerperTraining;
    public Vector3 endTarget;

    public GameObject lineAmountText;
    public GameObject triesAmountText;
    public Text TrainingScreenText;

    private void Start()
    {
        if(!GC.isTraining)
        {
            quitText.SetActive(false);
            tryCount.text = "" + GC.data.tries.Count;
            timer = GC.timer;
            //config
            helpText.text = GC.http.config.HelpText;
            maxsec = GC.http.config.TimeLimitSeconds;
            timerWrapper.SetActive(GC.http.config.ShowTimer || GC.data.showTimer);
            helpObject.SetActive(GC.http.config.ShowHelpText);

            lineAmountText.SetActive(GC.http.config.ShowLineAmount);
            triesAmountText.SetActive(GC.http.config.ShowTriesAmount);
        }
        if ((!GC.isTraining && !GC.data.mainLerpDone) || (GC.isTraining && !GC.data.trainingLerpDone))
        {
            lerper.lerpIntro(finishedLerp);
        }
        if(GC.isTraining)
        {
            TrainingScreenText.text = GC.http.config.TrainingScreenText;
            Debug.Log("Setting GC.http.config.TrainingScreenText: \n" + GC.http.config.TrainingScreenText);
            Debug.Log("Setting TrainingScreenText.text: \n" + TrainingScreenText.text);
        }
    }

    public void finishedLerp()
    {
        print("Lerping done!");
        if(!GC.isTraining) GC.data.mainLerpDone = true;
        if(GC.isTraining) GC.data.trainingLerpDone = true;
    }

    // Update is called once per frame
    private void Update () {
        lineCount.text = "" + (lineMaker.maxLines - lineMaker.myLines.Count);

        if(!GC.isTraining)
        {
            timerFull.text = "" + FormatTime(maxsec - timer.fullTimer);
            if(timer.fullTimer >= maxsec && maxsec > 0 && timer.takeTime) //check if maxtime is surpassed and maxtime is gotten (not 0).
            {
                Debug.Log("Time's up Sunny!");
                setNonInteractableButtons();
                lineMaker.done = true;
                timer.takeTime = false;
                timeUpText.SetActive(true);
                GC.addPoints();
                GC.redirect();
            }
        }
	}

    public void tryAgain()
    {
        if(!GC.isTraining)
        {
            GC.addPoints(); //add data and send it!
            timer.curTimer = 0;
        }
        reloadScene();
    }

    //training
    public void continueToMain()
    {
        lerperTraining.lerpOutro(loadNextMain);
    }

    private void loadNextMain()
    {
        SceneManager.LoadScene("Main");
    }

    private void reloadScene()
    {
        if(!GC.isTraining)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            SceneManager.LoadScene("Training");
        }
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
