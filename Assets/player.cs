using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

public class player : MonoBehaviour
{

    public IsoObject iso;
    public KBEngine.Avatar entity;
    public GpsSystem GPS;
    Vector3 StartPoint;// = new Vector3(120.6499191f,24.1791539f,1);
    public Animator Anim;
    public Vector3 TargetPosition;
    // Use this for initialization
    void Start()
    {
        if (entity.isPlayer())
        {
            Camera.main.GetComponent<ProCamera2D>().AddCameraTarget(this.transform);
            GPS = GameObject.Find("Canvas").GetComponent<GpsSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (entity.isPlayer())
        {

            if (GPS.GetVector3() != new Vector3(0, 0, 1))
            {
                if (StartPoint == Vector3.zero)
                    StartPoint = new Vector3(120.6465792f, 24.1790243f, 1);

                TargetPosition = (GPS.GetVector3() - StartPoint) * 20000;
                if (Vector3.Distance(iso.Position, TargetPosition) > 5)
                    iso.Position = TargetPosition;
                else
                {

                    if (Vector3.Distance(iso.Position, TargetPosition) < 0.5f)
                    {
                        Anim.SetBool("isWalk", false);
                    }
                    else
                    {
                        Vector3 target = Vector3.MoveTowards(iso.Position, TargetPosition, Time.deltaTime);
                        Anim.SetBool("isWalk", true);
                        Anim.SetFloat("x", target.x - iso.Position.x);
                        Anim.SetFloat("y", target.y - iso.Position.y);

                        iso.Position = target;

                    }
                }
                KBEngine.Event.fireIn("UpdatePlayer", iso.Position);
            }


        }

    }
}
