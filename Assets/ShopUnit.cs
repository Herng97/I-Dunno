using UnityEngine;
using System.Collections;
using KBEngine;
public class ShopUnit : MonoBehaviour {
    public IsoObject Iso;
    public Shop Shop;
	// Use this for initialization
	void Start () {
        Iso.Position = Shop.position;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTap(TapGesture gesture)
    {
        KBEngine.Event.fireOut("OpenShop",Shop);
    }
}
