using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using KBEngine;
using System;
using DG.Tweening;

public class World : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
        KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
        KBEngine.Event.registerOut("updatePosition", this, "updatePosition");
        KBEngine.Event.registerOut("set_IsWater", this, "set_IsWater");
        KBEngine.Event.registerOut("set_Level", this, "set_Level");
        KBEngine.Event.registerOut("set_otherTake", this, "set_otherTake");
        KBEngine.Event.registerOut("OnWater", this, "OnWater");
        KBEngine.Event.registerOut("OnAddCoin", this, "OnAddCoin");
        int[] keys = new int[KBEngineApp.app.entities.Keys.Count + 10];
        KBEngineApp.app.entities.Keys.CopyTo(keys, 0);
        for (int i = 0; i < keys.Length; i++)
        {
            Entity entity = null;
            if (KBEngineApp.app.entities.TryGetValue(keys[i], out entity))
            {

                onEnterWorld(entity);
                entity.callPropertysSetMethods();

            }
        }
    }
    public GameObject AccountPrefab;
    public GameObject ShopPrefab;
    public GameObject PlantPrefab;

    public Transform Canvas;
    public GameObject CoinPrefab;

    public void OnAddCoin(int value, Vector3 position)
    {
        GameObject obj = Instantiate(CoinPrefab, Canvas) as GameObject;

        obj.transform.localScale = new Vector3(0.001f, 0.001f, 1f);



        Text text = obj.GetComponent<Text>();
        text.text = (value >= 0 ? "+" : "-") + Math.Abs(value) + "c";
        text.DOKill();
        text.color = Color.white;
        text.DOFade(0, 0.2f).SetEase(Ease.OutCubic).SetDelay(1.5f).OnComplete(() => Destroy(obj));


        obj.transform.DOKill();
        obj.transform.position = IsoObject.projectionMatrix.MultiplyPoint(position)+new Vector3(0,1,0);
        obj.transform.DOScale(new Vector3(0.003f, 0.003f, 1f), 1f).SetEase(Ease.OutElastic);
        obj.transform.DOMoveY(obj.transform.position.y + 0.3f, 2f).SetEase(Ease.OutCubic);

    }
    public void set_otherTake(KBEngine.Plant plant)
    {
        plant.Unit.Stolen.gameObject.SetActive(plant.OtherTake);
    }
    public void set_IsWater(KBEngine.Plant plant)
    {
        if (plant.IsWater)
            plant.Unit.WaterEffect.Play();
        else
            plant.Unit.WaterEffect.Stop();
    }
    public void OnWater(KBEngine.Avatar avatar)
    {
        avatar.Player.OnWater();
    }

    public void set_Level(KBEngine.Plant plant)
    {
        Sprite sprite = Resources.Load<Sprite>("plant/" + plant.Id + "/" + plant.Level);
        print("plant/" + plant.Id + "/" + plant.Level);
        if (sprite != null)
        {
            plant.Unit.Sprite.sprite = sprite;
        }

    }
    public void onLeaveWorld(KBEngine.Entity entity)
    {

        if (entity.renderObj == null)
            return;

        UnityEngine.GameObject.Destroy((UnityEngine.GameObject)entity.renderObj);
        entity.renderObj = null;
    }
    public void onEnterWorld(Entity entity)
    {
        if (entity.renderObj != null)
        {
            return;
        }
        if (entity is KBEngine.Avatar)
        {

            KBEngine.Avatar avatar = entity as KBEngine.Avatar;
            avatar.renderObj = Instantiate(AccountPrefab);
            avatar.Player.entity = avatar;
            avatar.Player.position = avatar.position;
        }

        if (entity is KBEngine.Shop)
        {
            Shop shop = entity as Shop;
            shop.renderObj = Instantiate(ShopPrefab);
            shop.Unit.Shop = shop;
        }
        if (entity is KBEngine.Plant)
        {
            Plant plant = entity as Plant;
            plant.renderObj = Instantiate(PlantPrefab);
            plant.Unit.Plant = plant;
        }

    }
    public void updatePosition(KBEngine.Entity entity)
    {

        if (entity is KBEngine.Avatar)
        {
            (entity as KBEngine.Avatar).Player.position = entity.position;
        }

    }
}
