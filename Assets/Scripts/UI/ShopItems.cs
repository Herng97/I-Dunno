using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KBEngine;

public class ShopItems : MonoBehaviour {
    public object id,price;
	// Use this for initialization
	void Start ()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("itemIcon\\" + id);
        transform.localScale = new Vector3(1, 1, 1);
        GetComponentInChildren<Text>().text = price.ToString();

    }
    public void Buy()
    {
        ((KBEngine.Avatar)KBEngineApp.app.player()).ReqBuy((byte)id,(byte)price);
    }
	
}
