using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;




[Serializable]
public class Response {
    public GlobalC Global;
    public Countries[] Countries;
    public string Date;


}

[Serializable]
public class GlobalC {
    public int NewConfirmed;
    public int TotalConfirmed;
    public int NewDeaths;
    public int TotalDeaths;
    public int NewRecovered;
    public int TotalRecovered;
}
[Serializable]
public class Countries {
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
public class RootObject {
    public ShortResponse[] results;
}

[Serializable]
public class ShortResponse {
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
    private const string URL = "https://api.covid19api.com/";
    private GeocodeAPI geocode;
   
    public DataObject[] specificRequest(string location,  string startDate, string endDate) {
         string webURL = URL + "live/country/" + location +"/status/confirmed?from" + startDate + "&to=" + endDate;
          Debug.Log(webURL);
        WebRequest covidRequest = WebRequest.Create(webURL);
        covidRequest.Timeout=10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        string res = reader.ReadToEnd();
        return toData(JsonUtility.FromJson<RootObject >("{\"results\":" + res + "}"));
    }

   public DataObject[]  specificRequest(string location) {
       string webURL = URL + "live/country/" + location +"/status/confirmed";
        Debug.Log(webURL);
        WebRequest covidRequest = WebRequest.Create(webURL);
        covidRequest.Timeout=10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        string res = reader.ReadToEnd();
        Debug.Log(res);
        return toData(JsonUtility.FromJson<RootObject >("{\"results\":" + res + "}"));
   } 

   public DataObject[] simpleRequest() {
       string webURL = URL + "summary";
       Debug.Log(webURL);
       WebRequest covidRequest = WebRequest.Create(webURL);
        covidRequest.Timeout=10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        return toData(JsonUtility.FromJson<Response>(reader.ReadToEnd()));
   }

   private DataObject[] toData(RootObject response) {
       DataObject[] obj = new DataObject[response.results.Length];
        for(int i = 0; i < response.results.Length; i++) {
            if(i == response.results.Length - 1) {
            obj[i] = new DataObject(response.results[i].Lat, response.results[i].Lon,response.results[i].Country, response.results[i].Confirmed, "person", true);
        }
        obj[i] = new DataObject(response.results[i].Lat, response.results[i].Lon,response.results[i].Country, response.results[i].Confirmed, "person", false );
        }
        return obj;
           
   }

   private DataObject[] toData(Response response) {
        geocode = GetComponent<GeocodeAPI>();
       DataObject[] obj = new DataObject[response.Countries.Length];
       for(int i = 0; i < response.Countries.Length; i++) {
            Result res = geocode.Forward(response.Countries[i].Country, response.Countries[i].CountryCode);
            Debug.Log(res.geometry.lat + ", " + res.geometry.lng);
            if(i == response.Countries.Length - 1) {
            obj[i] = new DataObject(res.geometry.lat, res.geometry.lng,response.Countries[i].Country, response.Countries[i].NewConfirmed, "person", true);
        }
        obj[i] = new DataObject(res.geometry.lat, res.geometry.lng,response.Countries[i].Country, response.Countries[i].NewConfirmed, "person", false );
        }
        return obj;
   }
}
