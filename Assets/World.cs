using UnityEngine;
using System.Collections;
using KBEngine;

public class World : MonoBehaviour
{

    // Use this for initialization
    void Start()
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
                entity.callPropertysSetMethods();
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

        if (entity is KBEngine.Avatar)
        {
            KBEngine.Avatar avatar = entity as KBEngine.Avatar;
            avatar.renderObj=  Instantiate(AccountPrefab);
            avatar.Player.entity = (KBEngine.Avatar)avatar;
        }
    }
    public void updatePosition(KBEngine.Entity entity)
    {

        if (entity is KBEngine.Avatar)
        {
            print(entity.position);
            (entity as KBEngine.Avatar).Player.iso.Position = entity.position;
        }

    }
}
