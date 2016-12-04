using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using KBEngine;
using System.Collections.Generic;
using System;

public class ShopMenu : MonoBehaviour {
    public GameObject ItemPrefab;
    public Transform Items;
    public Text CoinText;
    // Use this for initialization
    void Awake () {
        KBEngine.Event.registerOut("OpenShop", this, "RunShop");
        set_Coin((KBEngine.Avatar)KBEngineApp.app.player());
        KBEngine.Event.registerOut("set_Coin", this, "set_Coin");
    }
    public void set_Coin(KBEngine.Avatar avatar)
    {
        CoinText.text = avatar.Coin.ToString();

    }
    public void RunShop(Shop shop)
    {
        for (int i = 0; i < Items.childCount; i++)
        {
            Destroy(Items.GetChild(i).gameObject);
        }
        List<object> sellitem = shop.Product;
        for (int i = 0; i < sellitem.Count; i++)
        {
            var product = sellitem[i] as Dictionary<string, object>;
            var obj = Instantiate(ItemPrefab, Items) as GameObject;
            ShopItems sitem = obj.GetComponent<ShopItems>();
            sitem.id =  product["id"];
            sitem.price = product["price"];

        }
        transform.GetChild(4).DOLocalMoveX(0, 0.5f);
        StartCoroutine(DeFade());
    }

    private GameObject Instantiate(GameObject itemPrefab, List<object> sellitem)
    {
        throw new NotImplementedException();
    }

    public void CloseShop()
    {
        transform.GetChild(4).DOLocalMoveX(-520, 0.5f);
        StartCoroutine(DoFade());

    }
    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DoFade()
    {
        CanvasGroup[] ShopUI = GameObject.Find("ShopMenu").GetComponentsInChildren<CanvasGroup>();
        Button[] BagButton = GameObject.Find("btnBag").GetComponents<Button>();
        ShopUI[0].interactable = false;
        BagButton[0].interactable = true;
        while (ShopUI[0].alpha > 0)
            {                
                ShopUI[0].alpha -= Time.deltaTime / 1;    
                yield return null;
            }
            yield return null;
        
    }
    IEnumerator DeFade()
    {
        CanvasGroup[] ShopUI = GameObject.Find("ShopMenu").GetComponentsInChildren<CanvasGroup>();
        Button[] BagButton = GameObject.Find("btnBag").GetComponents<Button>();
        ShopUI[0].interactable = true;
        BagButton[0].interactable = false;
        while (ShopUI[0].alpha < 1)
            {
                ShopUI[0].alpha += Time.deltaTime / 1;
                yield return null;
            }
            yield return null;
    }

}
