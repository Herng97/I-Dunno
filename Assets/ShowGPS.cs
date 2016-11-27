using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowGPS : MonoBehaviour
{
    public GpsSystem gps;

    void Update()
    {
        if (gps == null)
        {
            return;
        }
        GetComponent<Text>().text = "Lat:" + gps.Latitude + ", Lot:" + gps.Longitude;
    }
}
