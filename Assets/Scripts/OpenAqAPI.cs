using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Globalization;

[Serializable]
public class FullResponse
{
    public Results[] results;
}

[Serializable]
public class Results
{
    public float value;
    public string unit;
    public CoordinatesAQ coordinates;
    public string country;
}

[Serializable]
public class CoordinatesAQ
{
    public float latitude;
    public float longitude;
}

public class OpenAqAPI : MonoBehaviour, IDataAPI
{

    private const string URL = "https://api.openaq.org/v1/measurements";

    public string GetAPIName()
    {
        return "Ozone";
    }

    public DataObject[] specificRequest(string location)
    {
        string url = URL + "?country=" + location + "&parameter=o3&limit=250";
        Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return toData(JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()));
    }

    public DataObject[] specificRequest(string location, string startDate, string endDate)
    {
        string url = URL + "?country=" + location + "&date_from=" + startDate + "&date_to=" + endDate + "&parameter=o3&limit=250";
        Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return toData(JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()));
    }

     public string getName() {
        return null;
    }

    public string getDescription() {
        return null;
    }


    public DataObject[] simpleRequest()
    {
        string url = URL + "?parameter=o3&limit=250";
        Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return toData(JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()));
    }

    private DataObject[] toData(FullResponse response)
    {
        DataObject[] obj = new DataObject[response.results.Length];
        for (int i = 0; i < response.results.Length; i++)
        {
            obj[i] = new DataObject(response.results[i].coordinates.latitude, response.results[i].coordinates.longitude, response.results[i].country, response.results[i].value, response.results[i].unit);
        }
        return obj;
    }

}
