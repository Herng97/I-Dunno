using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BagItem : MonoBehaviour {
    public byte Index;
    public byte Id;
    // Use this for initialization
    void Start () {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("bagIcon\\" + Id);
	}
	

}
