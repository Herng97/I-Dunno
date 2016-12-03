using UnityEngine;
using System.Collections;
using KBEngine;
public class List_Item_Coin : MonoBehaviour {

	// Use this for initialization
	void Start () {
        set_Coin((KBEngine.Avatar)KBEngineApp.app.player());
        KBEngine.Event.registerOut("set_Coin", this, "set_Coin");
	}
    public void set_Coin(KBEngine.Avatar avatar)
    {
        print(avatar.Coin);

        //Resources.Load("bagIcon/"+id,typeof(Sprite)) 拉(Assest/REsource/BagIcon)地下的icon
    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
