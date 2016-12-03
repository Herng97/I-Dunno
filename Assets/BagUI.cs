using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BagUI : MonoBehaviour
{
    bool run = true;
    // Use this for initialization
    void Start () {
	
	}
    public void RunBag()
    {
        if (run)
        {
            transform.GetChild(3).DOLocalMoveY(0, 0.5f).SetDelay(0.5f);
            run = false;
        }
        else
        {
            transform.GetChild(3).DOLocalMoveY(-600, 0.5f).SetDelay(0.5f);
            run = true;
        }
        StartCoroutine(DoFade());
    }
    // Update is called once per frame
    void Update () {
	
	}

    IEnumerator DoFade()
    {
        CanvasGroup[] BagUI = GameObject.Find("BagMenu").GetComponentsInChildren<CanvasGroup>();
        Button[] btnBag = GameObject.Find("btnBag").GetComponents<Button>();
        if (run)
        {
            while (BagUI[0].alpha > 0)
            {
                BagUI[0].alpha -= Time.deltaTime / 1;
                yield return null;
            }
            BagUI[0].interactable = false;
            btnBag[0].interactable = true;
            yield return null;
        }
        else
        {
            while (BagUI[0].alpha < 1)
            {
                BagUI[0].alpha += Time.deltaTime / 1;
                yield return null;
            }
            BagUI[0].interactable = true;
            btnBag[0].interactable = false;
            yield return null;
        }
    }
}
