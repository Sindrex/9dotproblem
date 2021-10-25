using System;
using UnityEngine;

[Serializable]
public class TryRawAndConverted
{
    public string created_at;
    public string player_id;
    public int try_nr;
    public string point1;
    public string point2;
    public string point3;
    public string point4;
    public string point5;

    //timers
    public float timer1;
    public float timer2;
    public float timer3;
    public float timer4;
    public float timer5;
    public float timer6;
    public float timer7;

    //tabbed out
    public bool hasTabbedOut;
    public float totalTabbedOutTime;

    //converted
    public string node1;
    public string node2;
    public string node3;
    public string node4;
    public string node5;
    public bool accepted;
}