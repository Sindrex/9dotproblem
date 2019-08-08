using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProblemTry {

    [SerializeField]
    public List<Vector2> positions; // 0-5 points in space

    [SerializeField]
    public List<string> nodes; // 0-5 nodes in space, e.g. "A1"

    [SerializeField]
    public bool accepted;
}
