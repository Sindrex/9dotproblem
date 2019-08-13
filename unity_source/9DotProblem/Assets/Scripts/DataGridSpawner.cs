using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGridSpawner : MonoBehaviour {

    public GameObject dataGridParent;
    public GameObject rowPrefab;
    public Vector3 start;
    public Vector3 addition;
    public int rowCount;
    public bool debugNames;

	// Use this for initialization
	void Start () {
        Vector3 current = start;

        char rowName = 'A';
        for (int i = 0; i < rowCount; i++)
        {
            GameObject prefab = Instantiate(rowPrefab, dataGridParent.transform);
            prefab.transform.localPosition = current;
            prefab.name = rowName + "";

            for(int j = 0; j < prefab.transform.childCount; j++)
            {
                Transform child = prefab.transform.GetChild(j);
                string name = "" + rowName + (j + 1);
                child.name = name;
                child.GetChild(0).GetComponent<TextMesh>().text = name;
                child.GetChild(0).gameObject.SetActive(debugNames);
            }

            rowName++;
            current += addition;
        }
	}
}
