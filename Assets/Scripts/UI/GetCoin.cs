using UnityEngine;
using System.Collections;
using KBEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GetCoin : MonoBehaviour {

    public Text BCoinText;
    public Text SCoinText;
    // Use this for initialization
    void Start()
    {
        set_Coin((KBEngine.Avatar)KBEngineApp.app.player());
        KBEngine.Event.registerOut("set_Coin", this, "set_Coin");
    }
    public void set_Coin(KBEngine.Avatar avatar)
    {
        BCoinText.text = avatar.Coin.ToString();
        SCoinText.text = avatar.Coin.ToString();

    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
