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


public class COVID19_API : MonoBehaviour
{
    private const string URL = "https://api.covid19api.com/";
    // Start is called before the first frame update
    void Start()
    {
      
        RootObject anfrage1 = AccurateDataRequest("portugal");
        Debug.Log(anfrage1.results[0].Deaths);
        RootObject  anfrage2= AccurateDataRequest("portugal", "2020-03-21T13:13:30Z");
        Debug.Log(anfrage2.results[0].Deaths);

        Response anfrage3 = dataRequest();
        Debug.Log(anfrage3.Date);
        Debug.Log(anfrage3.Countries[1].NewRecovered);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public RootObject  AccurateDataRequest(string location, string time) {
         string webURL = URL + "live/country/" + location +"/status/confirmed/date/" + time;
          Debug.Log(webURL);
        WebRequest covidRequest = WebRequest.Create(webURL);
        covidRequest.Timeout=10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        string res = reader.ReadToEnd();
        return JsonUtility.FromJson<RootObject >("{\"results\":" + res + "}");
    }

   public RootObject  AccurateDataRequest(string location) {
       string webURL = URL + "live/country/" + location +"/status/confirmed";
        Debug.Log(webURL);
        WebRequest covidRequest = WebRequest.Create(webURL);
        covidRequest.Timeout=10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        string res = reader.ReadToEnd();
        Debug.Log(res);
        return JsonUtility.FromJson<RootObject >("{\"results\":" + res + "}");
   } 

   public Response dataRequest() {
       string webURL = URL + "summary";
       Debug.Log(webURL);
       WebRequest covidRequest = WebRequest.Create(webURL);
        covidRequest.Timeout=10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        return JsonUtility.FromJson<Response>(reader.ReadToEnd());
   }
}
