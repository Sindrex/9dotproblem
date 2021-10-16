using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {

    public Button playButton;
    public InputField IDInput;

    public DataCollector data;
    public CamLerper lerper;

    public Text titleText;
    public HTTPController http;

    private void Start()
    {
        playButton.interactable = false;
        data = GameObject.Find("DataCollector").GetComponent<DataCollector>();
        titleText.text = "";
        http.onConfigDownloaded.Add(() => titleText.text = http.config.Title);
    }

    //For testing
    public void onIDInput()
    {
        if(IDInput.text.Length > 0)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    private void Update()
    {
        playButton.interactable = data.urlOK;

        //testing
        //if (Input.GetKeyDown(KeyCode.E)) play();
    }

    public void play()
    {
        Debug.Log("Starting main, lerping first");
        //data.playerID = IDInput.text; //Testing
        lerper.lerpOutro(loadMain);
    }

    public void loadMain()
    {
        SceneManager.LoadScene("Main");
    }
}
