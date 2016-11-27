using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GpsSystem : MonoBehaviour
{
    public double Latitude;
    public double Longitude;
    private AndroidJavaClass unity;

    void Awake()
    {
        if (unity == null)
        {
             //unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        }
    }

    public void SetText(string text)
    {
        string[] words = text.Split(new char[] { ',' });
        Latitude = System.Convert.ToDouble(words[0].Substring(4));
        Longitude = System.Convert.ToDouble(words[1].Substring(5));
    }
    public Vector3 GetVector3()
    {
        return new Vector3(System.Convert.ToSingle(Longitude), System.Convert.ToSingle(Latitude),1);
    }



}
