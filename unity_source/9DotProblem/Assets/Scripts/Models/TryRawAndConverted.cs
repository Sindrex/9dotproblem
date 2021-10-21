using System;
using UnityEngine;

[Serializable]
public class TryRawAndConverted
{
    public DateTime created_at { get; set; }
    public string player_id { get; set; }
    public int try_nr { get; set; }
    public string point1 { get; set; }
    public string point2 { get; set; }
    public string point3 { get; set; }
    public string point4 { get; set; }
    public string point5 { get; set; }

    //timers
    public float timer1 { get; set; }
    public float timer2 { get; set; }
    public float timer3 { get; set; }
    public float timer4 { get; set; }
    public float timer5 { get; set; }
    public float timer6 { get; set; }
    public float timer7 { get; set; }
    public float timer8 { get; set; }

    //tabbed out
    public bool hasTabbedOut { get; set; }
    public float totalTabbedOutTime { get; set; }

    //converted
    public string node1 { get; set; }
    public string node2 { get; set; }
    public string node3 { get; set; }
    public string node4 { get; set; }
    public string node5 { get; set; }
    public bool accepted { get; set; }
}