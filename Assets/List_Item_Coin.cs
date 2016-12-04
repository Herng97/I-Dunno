using UnityEngine;
using System.Collections;
using KBEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class List_Item_Coin : MonoBehaviour {
    public GameObject ItemPrefab;
    public Transform Items;
    public Text CoinText;
    // Use this for initialization
    void Start () {
        set_Coin((KBEngine.Avatar)KBEngineApp.app.player());
        set_Bag((KBEngine.Avatar)KBEngineApp.app.player());
        KBEngine.Event.registerOut("set_Coin", this, "set_Coin");
        KBEngine.Event.registerOut("set_Bag", this, "set_Bag");
    }
    public void set_Coin(KBEngine.Avatar avatar)
    {
        CoinText.text = avatar.Coin.ToString();

    }
    public void set_Bag(KBEngine.Avatar avatar)
    {
        for(int i=0; i< Items.childCount ;i++)
        {
            Destroy(Items.GetChild(i).gameObject);
        }


        List<object> bag = avatar.Bag;
        for(byte i=0;i<bag.Count; i++)
        {
            var obj = Instantiate(ItemPrefab, Items) as GameObject;
            BagItem bitems =  obj.GetComponent<BagItem>();
            bitems.Index =  i;
            bitems.Id = (byte)bag[i];
        }


    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
