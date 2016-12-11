using UnityEngine;
using System.Collections;
using KBEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GetCoin : MonoBehaviour {

    public Text BagCoinText;
    public Text ShopCoinText;
    public Text InterfaceCoinText;
    // Use this for initialization
    void Start()
    {
        set_Coin((KBEngine.Avatar)KBEngineApp.app.player());
        KBEngine.Event.registerOut("set_Coin", this, "set_Coin");
    }
    public void set_Coin(KBEngine.Avatar avatar)
    {
        BagCoinText.text = avatar.Coin.ToString();
        ShopCoinText.text = avatar.Coin.ToString();
        InterfaceCoinText.text = avatar.Coin.ToString();

    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
