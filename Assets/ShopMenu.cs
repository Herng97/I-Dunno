using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {
    bool run=true;
    // Use this for initialization
    void Start () {
	}
	
    public void RunShop ()
    {
        if (run)
        {
            transform.GetChild(5).DOLocalMoveX(0, 0.5f).SetDelay(0.5f);
            run = false;
        }
        else
        {
            transform.GetChild(5).DOLocalMoveX(-520, 0.5f).SetDelay(0.5f);
            run = true;
        }
        StartCoroutine(DoFade());

    }
    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DoFade()
    {
        CanvasGroup[] ShopUI = GameObject.Find("ShopMenu").GetComponentsInChildren<CanvasGroup>();
        Button[] BagButton = GameObject.Find("btnBag").GetComponents<Button>();
        if (run)
        {
            while (ShopUI[0].alpha > 0)
            {                
                ShopUI[0].alpha -= Time.deltaTime / 1;    
                yield return null;
            }
            ShopUI[0].interactable = false;
            BagButton[0].interactable = true;
            yield return null;
        }
        else
        {
            while (ShopUI[0].alpha < 1)
            {                  
                ShopUI[0].alpha += Time.deltaTime / 1;    
                yield return null;
            }
            ShopUI[0].interactable = true;
            BagButton[0].interactable = false;
            yield return null;
        }
    }

}
