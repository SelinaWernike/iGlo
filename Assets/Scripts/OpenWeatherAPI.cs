using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;

[Serializable]
public class fullResponse {
    public ResponseOW[] ow;
}
[Serializable]
public class ResponseOW {
    public DateTime timestamp;
    public Location location;
    public float data;

    public string ToString() {
        return "ozonschicht: " + data + " " + location;
    }

}
[Serializable]
public class Location {
    public float latitude;
    public float longitude;

    public string ToString() {
        return latitude + "/" + longitude;
    }

}
public class OpenWeatherAPI : MonoBehaviour
{
    private const string URL = "http://api.openweathermap.org/pollution/v1/o3/{0}/{1}.json?appid={2}";
    private const string OW_KEY = "e46233973b41a40536ceec511043881d";
    // Start is called before the first frame update
    void Start()
    {
        ResponseOW anfrage1 = dataRequest("0.0,10.0", "2020-06-10Z");
        Debug.Log(anfrage1);
        ResponseOW anfrage2 = dataRequest("0.0,10.0");
        Debug.Log(anfrage2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ResponseOW dataRequest(string location, string time) {
        //String.Format(URL,location,time,OW_KEY
        WebRequest pollutionRequest = WebRequest.Create("http://api.openweathermap.org/pollution/v1/o3/0.0,10.0/2020-06-10Z.json?appid=e46233973b41a40536ceec511043881d");
        pollutionRequest.Timeout=10000;
        WebResponse owAnswer = pollutionRequest.GetResponse();
        StreamReader reader = new StreamReader(owAnswer.GetResponseStream());
        return JsonUtility.FromJson<fullResponse>(reader.ReadToEnd()).ow[0];
    }

   public ResponseOW dataRequest(string location) {
        WebRequest pollutionRequest = WebRequest.Create(String.Format(URL,location,System.DateTime.Now.ToString("YYYY-MM-DDZ"),OW_KEY));
        pollutionRequest.Timeout=10000;
        WebResponse owAnswer = pollutionRequest.GetResponse();
        StreamReader reader = new StreamReader(owAnswer.GetResponseStream());
        return JsonUtility.FromJson<fullResponse>(reader.ReadToEnd()).ow[0];
   } 
}
