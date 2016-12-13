using UnityEngine;
using System.Collections;
using KBEngine;
public class PlantUnit : MonoBehaviour
{
    public IsoObject Iso;
    public Plant Plant;
    public SpriteRenderer Sprite;

    public ParticleSystem WaterEffect;

    public SpriteRenderer Stolen;
    // Use this for initialization
    void Start()
    {
        Iso.Position = Plant.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTap(TapGesture gesture)
    {
        if (Plant.MaxLevel == Plant.Level)
            Plant.ReqTake();
        else
        {
            Vector3 position = Isometric.screenToIsoPoint(gesture.Position, 0);
            player p = ((KBEngine.Avatar)KBEngine.KBEngineApp.app.player()).Player;
            p.position = position;
        }
    }
}
