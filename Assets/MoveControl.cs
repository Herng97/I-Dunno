using UnityEngine;
using System.Collections;

public class MoveControl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void OnTap(TapGesture gesture)
    {
        Vector3 position = Isometric.screenToIsoPoint(gesture.Position, 0);
        player p = ((KBEngine.Avatar)KBEngine.KBEngineApp.app.player()).Player;
        p.position = position;
    }
}
