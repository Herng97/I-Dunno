using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

public class player : MonoBehaviour
{
    public ParticleSystem WaterEffect;

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
    public Vector3 position;
    // Update is called once per frame
    void Update()
    {

        if (iso.Position == position)
        {
            Anim.SetBool("isWalk", false);
        }
        else
        {
            Vector3 target = Vector3.MoveTowards(iso.Position, position, 1.8f * Time.deltaTime);
            Anim.SetBool("isWalk", true);
            Anim.SetFloat("x", target.x - iso.Position.x);
            Anim.SetFloat("y", target.y - iso.Position.y);

            iso.Position = target;
        }
        if (entity.isPlayer())
        {
            KBEngine.Event.fireIn("UpdatePlayer", iso.Position);
        }


    }

    public void OnWater()
    {

        WaterEffect.Play();
        if (entity.isPlayer())
        {
            RaycastHit[] hit = Physics.SphereCastAll(IsoObject.projectionMatrix.MultiplyPoint(iso.Position - Vector3.back), 3, (Isometric.vectorToIsoDirection(IsoDirection.Up)), 3);
            print(hit.Length);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.tag == "Plant")
                {
                    hit[i].transform.GetComponent<PlantUnit>().Plant.ReqWater();
                }
            }
        }
    }
}
