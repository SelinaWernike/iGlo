using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;

[Serializable]
public class FullResponse {
    public Results[] results;
}

[Serializable]
public class Results {
    public float value;
    public string unit;
    public string country;
}



public class OpenAqAPI : MonoBehaviour
{

    private const string URL = "https://api.openaq.org/v1/measurements";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

public Results simpleRequest(float lat, float lon, string parameter) {
    WebRequest request = WebRequest.Create(URL + "?coordinates=" + lat + "," + lon + "&parameter=" + parameter);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()).results[0];
}

public Results simpleRequest(float lat, float lon, string parameter, string startDate, string endDate) {
        WebRequest request = WebRequest.Create(URL + "?coordinates=" + lat + "," + lon + "&date_from=" + startDate + "&date_to=" + endDate + "&parameter=" + parameter);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()).results[0];
}
    
}
