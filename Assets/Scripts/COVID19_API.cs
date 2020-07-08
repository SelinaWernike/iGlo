using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;


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
    public float Cases;
    public string Date;
}

/*
Shows new cases from multiple countries
*/

public class COVID19_API : MonoBehaviour, IDataAPI
{
    private const string NAME = "COVID-19 Infizierte";
    private const string DESCRIPTION = "https://api.covid19api.com/ \nAPI stellt alle Neuinfizierungen an einem Tag pro Land dar.";

    private const string URL = "https://api.covid19api.com/";
    private GeocodeAPI geocode;
    
    private void Awake()
    {
        geocode = GetComponent<GeocodeAPI>();
    }





    public async Task<DataObject[]> specificRequest(string location, string startDate, string endDate)
    {
        string webURL = URL + "country/" + location + "/status/confirmed/live?from=" + startDate + "&to=" + endDate;
        UnityEngine.Debug.Log(webURL);
        return toData(await Utility.RequestAsync<RootObject>(webURL, "{\"results\":", "}"), startDate, endDate);
    }



    public async Task<DataObject[][]> dateRequest(string startDate, string endDate)
    {
        DataObject[][] result = new DataObject[100][];
        int i = 0;
        foreach (string countryCode in GeocodeAPI.cache.Keys)
        {
            if (i == 99)
            {
                break;
            }
            DataObject[] requestAnswer = await specificRequest(countryCode, startDate, endDate);
                if(requestAnswer != null) {
                result[i] = requestAnswer;
            i++;
            }
        }
        foreach (DataObject[] obj in result)
        {
            if (obj != null)
            {
                return result;
            }
            }
            return null;
    
    }


    public async Task<DataObject[]> simpleRequest()
    {
        string webURL = URL + "summary";
        return toData(await Utility.RequestAsync<Response>(webURL));
    }

    public string getName()
    {
        return NAME;
    }

    public string getDescription()
    {
        return DESCRIPTION;
    }

    /*
    converts a RootObject into a DataObject[]
    */
    private DataObject[] toData(RootObject response, string startDate, string endDate)
    {
        if (response != null)
        {
            DateTime date = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);
            TimeSpan span = end.Date.Subtract(date.Date);
            DataObject[] dataArray = new DataObject[(int) span.Days + 1];
            int counter = 0;
            for (int i = 0; i < dataArray.Length; i++)
            {
                 if(response.results.Length - 1 < counter) {
                    break;
                }
               if(DateTime.Equals(date.Date, DateTime.Parse(response.results[counter].Date).Date)) {
                   DataObject dataAus = new DataObject(response.results[counter].Lat, response.results[counter].Lon, response.results[counter].Country, response.results[counter].Cases, "Personen", date);
                   dataArray[i] = dataAus;
                   
                   while(DateTime.Equals(date.Date, DateTime.Parse(response.results[counter].Date).Date)) {
                       counter++;
                       if(response.results.Length - 1 < counter) {
                    break;
                }
                   }
               }
               date = date.AddDays(1f); 
            }
            return dataArray;
        }else
        {
            return null;
        }
    }

    /*
    converts a Response-Object into a DataObject[]
    */
    private DataObject[] toData(Response response)
    {
        DataObject[] obj = new DataObject[response.Countries.Length];
        for (int i = 0; i < response.Countries.Length; i++)
        {
            Result res = geocode.Forward(response.Countries[i].Country, response.Countries[i].CountryCode);
            obj[i] = new DataObject(res.geometry.lat, res.geometry.lng, response.Countries[i].Country, response.Countries[i].TotalConfirmed, "Personen", DateTime.Parse(response.Date));
        }
        return obj;
    }
}

