using UnityEngine;
using System.Collections;
using KBEngine;
public class PlantUnit : MonoBehaviour {
    public IsoObject Iso;
    public Plant Plant;
	// Use this for initialization
	void Start () {
        Iso.Position = Plant.position;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
