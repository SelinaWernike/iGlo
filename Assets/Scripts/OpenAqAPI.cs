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
    public CoordinatesAQ coordinates;
    public string country;
}

[Serializable]
public class CoordinatesAQ {
    public float latitude;
    public float longitude;
}



public class OpenAqAPI : MonoBehaviour, IDataAPI
{

    private const string URL = "https://api.openaq.org/v1/measurements";
    // Start is called before the first frame update
    void Start()
    {
        DataObject[] res1 = specificRequest("IN");
        Debug.Log(res1[0].ToString());
        DataObject[] res2 = specificRequest("AU","2020-01-01","2020-12-31" );
        Debug.Log(res2[0].ToString());
        DataObject[] res3 = simpleRequest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

public DataObject[]  specificRequest(string location) {
    string url = URL + "?country=" + location + "&parameter=o3";
    Debug.Log(url);
    WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return toData(JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()));
}

public  DataObject[] specificRequest(string location, string startDate, string endDate) {
    string url = URL + "?country=" + location + "&date_from=" + startDate + "&date_to=" + endDate + "&parameter=o3";
    Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return toData(JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()));
}

public DataObject[] simpleRequest() {
    string url = URL + "?parameter=o3";
    Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return toData(JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()));
}

private DataObject[] toData(FullResponse response) {
    DataObject[] obj = new DataObject[response.results.Length];
    for(int i = 0; i < response.results.Length; i++) {
        if(i == response.results.Length - 1) {
            obj[i] = new DataObject(response.results[i].coordinates.latitude, response.results[i].coordinates.longitude,response.results[i].country, response.results[i].value, response.results[i].unit, true);
        }
        obj[i] = new DataObject(response.results[i].coordinates.latitude, response.results[i].coordinates.longitude,response.results[i].country, response.results[i].value, response.results[i].unit, false );
    }
    return obj;
}
    
}
