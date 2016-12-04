using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KBEngine;
public class BagItem : MonoBehaviour {
    public byte Index;
    public byte Id;
    // Use this for initialization
    void Start () {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("itemIcon\\" + Id);
        transform.localScale = new Vector3(1,1,1);
	}
    public void Use()
    {
        ((KBEngine.Avatar)KBEngineApp.app.player()).ReqUse(Index);

    }

}
