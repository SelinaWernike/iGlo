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



public class OpenAqAPI : MonoBehaviour, IDataAPI<FullResponse,FullResponse>
{

    private const string URL = "https://api.openaq.org/v1/measurements";
    // Start is called before the first frame update
    void Start()
    {
        FullResponse res1 = specificRequest("IN");
        Debug.Log(res1.results[1].value);
        FullResponse res2 = specificRequest("AU","2020-01-01","2020-12-31" );
        Debug.Log(res2.results[1].value);
        FullResponse res3 = simpleRequest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

public FullResponse  specificRequest(string location) {
    string url = URL + "?country=" + location + "&parameter=o3";
    Debug.Log(url);
    WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return JsonUtility.FromJson<FullResponse>(reader.ReadToEnd());
}

public  FullResponse specificRequest(string location, string startDate, string endDate) {
    string url = URL + "?country=" + location + "&date_from=" + startDate + "&date_to=" + endDate + "&parameter=o3";
    Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return JsonUtility.FromJson<FullResponse>(reader.ReadToEnd());
}

public FullResponse simpleRequest() {
    string url = URL + "?parameter=o3";
    Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return JsonUtility.FromJson<FullResponse>(reader.ReadToEnd());
}
    
}
