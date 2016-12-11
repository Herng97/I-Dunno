using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using KBEngine;
using System.Collections.Generic;

public class BagUI : MonoBehaviour
{
    public GameObject ItemPrefab;
    public Transform Items;


    // Use this for initialization
    void Start()
    {
        set_Bag((KBEngine.Avatar)KBEngineApp.app.player());
        KBEngine.Event.registerOut("set_Bag", this, "set_Bag");
        KBEngine.Event.registerOut("CloseBag", this, "CloseBag");
    }
    public void RunBag()
    {
        transform.GetChild(3).DOLocalMoveY(0, 0.5f);
        StopAllCoroutines();
        StartCoroutine(DeFade());
        
    }

    public void CloseBag()
    {
        transform.GetChild(3).DOLocalMoveY(-600, 0.5f);
        StopAllCoroutines();
        StartCoroutine(DoFade());

    }


    public void set_Bag(KBEngine.Avatar avatar)
    {
        for (int i = 0; i < Items.childCount; i++)
        {
            Destroy(Items.GetChild(i).gameObject);
        }


        List<object> bag = avatar.Bag;
        for (byte i = 0; i < bag.Count; i++)
        {
            var obj = Instantiate(ItemPrefab, Items) as GameObject;
            BagItem bitems = obj.GetComponent<BagItem>();
            bitems.Index = i;
            bitems.Id = (byte)bag[i];
        }

    }

    IEnumerator DoFade()
    {
        CanvasGroup[] BagUI = GameObject.Find("BagMenu").GetComponentsInChildren<CanvasGroup>();
        Button[] btnBag = GameObject.Find("btnBag").GetComponents<Button>();
            BagUI[0].interactable = false;
            btnBag[0].interactable = true;
         while (BagUI[0].alpha > 0)
         {
             BagUI[0].alpha -= Time.deltaTime / 1;
             yield return null;
         }
        yield return null;
        
    }
    IEnumerator DeFade()
    {
        CanvasGroup[] BagUI = GameObject.Find("BagMenu").GetComponentsInChildren<CanvasGroup>();
        Button[] btnBag = GameObject.Find("btnBag").GetComponents<Button>();

            BagUI[0].interactable = true;
            btnBag[0].interactable = false;
         while (BagUI[0].alpha < 1)
         {
             BagUI[0].alpha += Time.deltaTime / 1;
             yield return null;
         }
         
        yield return null;
    }
    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
