using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using Unity.Jobs;
using Unity.Collections;



[Serializable]
public class Response
{
    public GlobalC Global;
    public Countries[] Countries;
    public string Date;


}

[Serializable]
public class GlobalC
{
    public int NewConfirmed;
    public int TotalConfirmed;
    public int NewDeaths;
    public int TotalDeaths;
    public int NewRecovered;
    public int TotalRecovered;
}
[Serializable]
public class Countries
{
    public string Country;
    public string CountryCode;
    public string Slug;
    public int NewConfirmed;
    public int TotalConfirmed;
    public int NewDeaths;
    public int TotalDeaths;
    public int NewRecovered;
    public int TotalRecovered;
    public string Date;

}

[Serializable]
public class RootObject
{
    public ShortResponse[] results;
}

[Serializable]
public class ShortResponse
{
    public string Country;
    public string CountryCode;
    public float Lat;
    public float Lon;
    public int Confirmed;
    public int Deaths;
    public int Recovered;
    public int Active;
    public string Date;
}

public class COVID19_API : MonoBehaviour, IDataAPI
{
    private const string NAME = "COVID-19 Infizierte";
    private const string DESCRIPTION = "https://api.covid19api.com/ \nAPI stellt alle Neuinfizierungen an einem Tag pro Land dar.";

    private const string URL = "https://api.covid19api.com/";
    private const string CACHE_PATH = "./caches/geocode.cache";
    private GeocodeAPI geocode;
     private static ResultCache cache;
    private bool cacheCreated;


    private void Awake()
    {
        geocode = GetComponent<GeocodeAPI>();
         if (cache == null)
        {
            cache = File.Exists(CACHE_PATH) ? JsonUtility.FromJson<ResultCache>(File.ReadAllText(CACHE_PATH)) : new ResultCache();
            cacheCreated = true;
        }
        else
        {
            cacheCreated = false;
        }
    }

    public string GetAPIName()
    {
        return "Covid-19";
    }

    public DataObject[] specificRequest( string location, string startDate, string endDate)
    {
        string webURL = URL + "live/country/" + location +  "/status/confirmed?from=" + startDate + "&to=" + endDate;
        Debug.Log(webURL);
        WebRequest covidRequest = WebRequest.Create(webURL);
        covidRequest.Timeout = 10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        string res = reader.ReadToEnd();
        return toData(JsonUtility.FromJson<RootObject>("{\"results\":" + res + "}"));
    }

    public DataObject[] specificRequest(string location)
    {
        string webURL = URL + "live/country/" + location + "/status/confirmed";
        Debug.Log(webURL);
        WebRequest covidRequest = WebRequest.Create(webURL);
        covidRequest.Timeout = 10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        string res = reader.ReadToEnd();
        Debug.Log(res);
        return toData(JsonUtility.FromJson<RootObject>("{\"results\":" + res + "}"));
    }

    public DataObject[][] dateRequest(string startDate, string endDate) {
        DataObject[][] result = new DataObject[cache.Count][];
        int i = 0;
        foreach (string countryCode in cache.Keys)
        {
            result[i] = specificRequest(countryCode,startDate,endDate);
            i++;
        }
        return result;
    }



    public DataObject[] simpleRequest()
    {
        string webURL = URL + "summary";
        Debug.Log(webURL);
        WebRequest covidRequest = WebRequest.Create(webURL);
        covidRequest.Timeout = 10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        return toData(JsonUtility.FromJson<Response>(reader.ReadToEnd()));
    }

    public string getName() {
        return NAME;
    }

    public string getDescription() {
    return DESCRIPTION;
    }

    private DataObject[] toData(RootObject response)
    {
        DataObject[] obj = new DataObject[response.results.Length];
        for (int i = 0; i < response.results.Length; i++)
        {
            obj[i] = new DataObject(response.results[i].Lat, response.results[i].Lon, response.results[i].Country, response.results[i].Confirmed, "infected");
        }
        return obj;
    }

    private DataObject[] toData(Response response)
    {
        DataObject[] obj = new DataObject[response.Countries.Length];
        for (int i = 0; i < response.Countries.Length; i++)
        {
            Result res = geocode.Forward(response.Countries[i].Country, response.Countries[i].CountryCode);
            Debug.Log(res.geometry.lat + ", " + res.geometry.lng);
            obj[i] = new DataObject(res.geometry.lat, res.geometry.lng, response.Countries[i].Country, response.Countries[i].NewConfirmed, "infected");
        }
        return obj;
    }
}

