using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {
    bool run=true;
    // Use this for initialization
    void Start () {
	}
	
    void OShop()
    {
       // transform.DOMoveY(275, 0.5f).SetDelay(0.5f);
        run = false;
    }
    void CShop()
    {
       // transform.DOMoveY(-275, 0.5f).SetDelay(0.5f);
        run = true;
    }
    public void RunShop ()
    {
        if (run)
        {
         //   transform.GetChild(1).DOLocalMoveY(-30, 0.5f).SetDelay(0.5f);
            run = false;
            StartCoroutine(DoFade());
            //OShop();
        }
        else
        {
          //  transform.GetChild(1).DOLocalMoveY(-550, 0.5f).SetDelay(0.5f);
            run = true;
            StartCoroutine(DoFade());
            //CShop();
        }

    }
    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DoFade()
    {
        CanvasGroup[] ShopUI = GameObject.Find("ShopCanvas/ShopUI").GetComponentsInChildren<CanvasGroup>();
        Button[] ShopButton = GameObject.Find("ShopCanvas/btnShop").GetComponents<Button>();
        if (run)
        {
            while (ShopUI[0].alpha > 0)
            {                
                ShopUI[0].alpha -= Time.deltaTime / 1;    
                yield return null;
            }
            ShopUI[0].interactable = false;
            ShopButton[0].interactable = true;
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
            ShopButton[0].interactable = false;
            yield return null;
        }
    }

}
