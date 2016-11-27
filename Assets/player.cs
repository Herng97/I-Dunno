using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class player : MonoBehaviour {

    public IsoObject iso;
    public GpsSystem GPS;
    Vector3 StartPoint;// = new Vector3(120.6499191f,24.1791539f,1);
    public Text Diff;
	// Use this for initialization


    // Update is called once per frame
    void Update()
    {
        if (GPS.GetVector3() != new Vector3(0,0,1))
        {
            if (StartPoint == Vector3.zero)
                StartPoint = new Vector3(120.6465792f, 24.1790243f, 1);
                //StartPoint = new Vector3(120.6483793f, 24.1791002f, 1);
            Diff.text = ((GPS.GetVector3() - StartPoint) * 20000).ToString();
            iso.Position = Vector3.MoveTowards(iso.Position, (GPS.GetVector3() - StartPoint) * 20000, 1 * Time.deltaTime);
        }

    }
}
