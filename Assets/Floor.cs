using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {
    public GameObject g;
	// Use this for initialization
	void Start () {
        for (int x = 0; x < 200; x++)
            for (int y = 0; y < 200; y++)
            {
                var a = Instantiate(g);
                a.transform.GetComponent<IsoObject>().Position = new Vector3(x, y, 0);
                a.transform.SetParent(transform);
            }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
