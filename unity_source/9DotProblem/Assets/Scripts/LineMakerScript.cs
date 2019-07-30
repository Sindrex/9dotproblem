﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMakerScript : MonoBehaviour {

    public GameController GC;

    private LineRenderer lr; //current line
    public List<GameObject> myLines;

    public int maxLines = 4;
    public float lineWidth = 0.1f;

    public bool done = false;

    public string buttonTag;

    // Use this for initialization
    void Start () {
        myLines = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!done)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, new Vector3(0, 0, -1), 100F);
            foreach(RaycastHit2D hit in hits)
            {
                //print(hit.transform.tag + "/" + hit.transform.name);
                if (hit.transform.CompareTag(buttonTag))
                {
                    //print("hit button");
                    return;
                }
            }

            if (Input.GetMouseButtonDown(0)) //make line!
            {
                Debug.Log("Making line!");
                GameObject myLine = new GameObject("Line" + myLines.Count);
                myLine.AddComponent<LineRenderer>();
                lr = myLine.GetComponent<LineRenderer>();

                //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

                lr.startWidth = lineWidth;
                lr.endWidth = lineWidth;

                lr.startColor = Color.black;
                lr.endColor = Color.black;
                lr.material.color = Color.black;

                if (myLines.Count <= 0) //first line
                {
                    Vector3 startPos = new Vector3(mousePos.x, mousePos.y, 0);

                    myLine.transform.position = startPos;

                    lr.SetPosition(0, startPos);
                    lr.SetPosition(1, startPos);
                }
                else if(myLines.Count < maxLines) //not first line
                {
                    Vector3 prevEndPos = myLines[myLines.Count - 1].GetComponent<LineRenderer>().GetPosition(1);

                    myLine.transform.position = prevEndPos;

                    lr.SetPosition(0, prevEndPos);
                    lr.SetPosition(1, prevEndPos);
                }
                else //too many lines
                {
                    print("Enough lines!");
                }

                myLines.Add(myLine);
                addColl(lr);
            }
            else if (Input.GetMouseButton(0)) //drag
            {
                Vector3 end = new Vector3(mousePos.x, mousePos.y, 0);

                lr.SetPosition(1, end);
                transformCollider(lr);
            }
            else if (Input.GetMouseButtonUp(0)) //up
            {
                transformCollider(lr);

                if(myLines.Count == maxLines)
                {
                    done = true;
                    GC.checkDone();
                }
            }
        }
	}

    private void addColl(LineRenderer lr)
    {
        BoxCollider2D col = new GameObject("Collider").AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.transform.parent = lr.transform; // Collider is added as child object of line
        col.transform.position = new Vector3(0, 0, 0);
    }

    private void transformCollider(LineRenderer lr)
    {
        Vector2 startPos = lr.GetPosition(0);
        Vector2 endPos = lr.GetPosition(1);

        if (startPos == endPos)
        {
            //print("Same spot still");
            return; //same spot
        }

        BoxCollider2D col = lr.transform.GetChild(0).GetComponent<BoxCollider2D>();

        float lineLength = Vector3.Distance(startPos, endPos); // length of line
        col.size = new Vector3(lineLength, lineWidth, 1f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint; // setting position of collider object

        // Following lines calculate the angle between startPos and endPos
        col.transform.rotation = new Quaternion(0, 0, 0, 0);
        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));
        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);
    }
}
