using UnityEngine;
using System.Collections;
using KBEngine;

public class World : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
        KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
        KBEngine.Event.registerOut("updatePosition", this, "updatePosition");
        int[] keys = new int[KBEngineApp.app.entities.Keys.Count + 10];
        KBEngineApp.app.entities.Keys.CopyTo(keys, 0);
        for (int i = 0; i < keys.Length; i++)
        {
            Entity entity = null;
            if (KBEngineApp.app.entities.TryGetValue(keys[i], out entity))
            {
                onEnterWorld(entity);
            }
        }
    }
    public GameObject AccountPrefab;
    // Update is called once per frame
    public void onLeaveWorld(Entity entity)
    {
    }
    public void onEnterWorld(Entity entity)
    {

        if (entity is Account)
        {
            Account account = entity as Account;
            account.renderObj=  Instantiate(AccountPrefab);
            account.Player.entity = account;
        }
    }
    public void updatePosition(KBEngine.Entity entity)
    {
        if (entity is Account )
        {
            (entity as Account).Player.iso.Position = entity.position;
        }

    }
}
