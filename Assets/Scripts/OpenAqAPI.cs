using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Globalization;

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
        Results res1 = simpleRequest(52.47f, 13.22f, "o3");
        Debug.Log(res1.value);
        FullResponse res2 = timeRequest(52.47f, 13.22f, "o3","2020-01-01","2020-12-31" );
        Debug.Log(res2.results[1].value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

public Results simpleRequest(float lat, float lon, string parameter) {
    string url = URL + "?coordinates=" + lat.ToString("G", CultureInfo.InvariantCulture) + "," + lon.ToString("G", CultureInfo.InvariantCulture) + "&parameter=" + parameter;
    Debug.Log(url);
    WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()).results[0];
}

public  FullResponse timeRequest(float lat, float lon, string parameter, string startDate, string endDate) {
    string url = URL + "?coordinates=" + lat.ToString("G", CultureInfo.InvariantCulture) + "," + lon.ToString("G", CultureInfo.InvariantCulture) + "&date_from=" + startDate + "&date_to=" + endDate + "&parameter=" + parameter;
    Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return JsonUtility.FromJson<FullResponse>(reader.ReadToEnd());
}
    
}
