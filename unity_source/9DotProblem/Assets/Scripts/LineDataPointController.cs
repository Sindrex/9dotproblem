using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDataPointController : MonoBehaviour {

    public string nodeName;
    public bool collided = false;
    public bool prioSwapped = false;
    public string dataGridTag;
    public string dotTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collided)
        {
            //print("Collided with: " + collision.name);
            if (collision.CompareTag(dataGridTag))
            {
                //print("Setting node: " + collision.name);
                nodeName = collision.name;
                collided = true;
            }
            else if (collision.CompareTag(dotTag))
            {
                //print("Setting node: " + collision.name);
                nodeName = collision.name;
                collided = true;
                prioSwapped = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!prioSwapped && collision.CompareTag(dotTag))
        {
            print("PrioSwapped to: " + collision.name);
            nodeName = collision.name;
            prioSwapped = true;
        }
    }
}
