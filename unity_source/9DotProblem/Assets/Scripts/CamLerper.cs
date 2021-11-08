using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamLerper : MonoBehaviour {

    public Canvas myCanvas;
    public Camera cam;
    private bool doLerp = false;
    private float lerpFactor = 0;
    public float timingFactor = 0.5f;
    public Vector3 startPos;
    public Vector3 target;
    public bool setRenderModeAtTarget = false;
    public UnityAction methodAtFinish;
	
	// Update is called once per frame
	void Update () {
        if (doLerp)
        {
            //print("yo2");
            float x = cam.transform.position.x;
            float y = cam.transform.position.y;
            float z = cam.transform.position.z;
            Vector3 lerpPos = new Vector3(Mathf.Lerp(x, target.x, lerpFactor), Mathf.Lerp(y, target.y, lerpFactor), z);
            cam.transform.position = lerpPos;

            lerpFactor += timingFactor * Time.deltaTime;

            if (cam.transform.position == target)
            {
                doLerp = false;
                lerpFactor = 0;
                methodAtFinish();
            }
        }
    }

    public void lerpOutro(UnityAction finish)
    {
        myCanvas.renderMode = RenderMode.WorldSpace;
        doLerp = true;
        methodAtFinish = finish;
    }
    public void lerpIntro(UnityAction finish)
    {
        myCanvas.renderMode = RenderMode.WorldSpace;
        cam.transform.position = startPos;
        doLerp = true;
        methodAtFinish = finish;
    }
}
