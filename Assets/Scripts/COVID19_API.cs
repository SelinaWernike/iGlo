using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


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
        return toData(await Utility.RequestAsync<RootObject>(webURL, "{\"results\":", "}"));
    }


    public async Task<DataObject[]> specificRequest(string location)
    {
        string webURL = URL + "live/country/" + location + "/status/confirmed";
        return toData(await Utility.RequestAsync<RootObject>(webURL, "{\"results\":", "}"));
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
            if (requestAnswer.Length != 0)
            {
                DataObject[] homogenArray = new DataObject[365];
                if (requestAnswer.Length >= 365)
                {
                    Array.Copy(requestAnswer, 0, homogenArray, 0, 365);
                }
                else
                {
                    Array.Copy(requestAnswer, 0, homogenArray, 0, requestAnswer.Length);
                }
                result[i] = homogenArray;
            }
            i++;
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
    private DataObject[] toData(RootObject response)
    {
        if (response != null)
        {
            List<DataObject> dataList = new List<DataObject>();
            DateTime date = DateTime.Parse(response.results[0].Date);
            dataList.Add(new DataObject(response.results[0].Lat, response.results[0].Lon, response.results[0].Country, response.results[0].Cases, "Personen", date));
            for (int i = 1; i < response.results.Length; i++)
            {
                if (!DateTime.Equals(date.Date, DateTime.Parse(response.results[i].Date).Date))
                {
                    date = DateTime.Parse(response.results[i].Date).Date;
                    Debug.Log(date);
                    dataList.Add(new DataObject(response.results[i].Lat, response.results[i].Lon, response.results[i].Country, response.results[i].Cases, "Personen", date));
                }
            }
            return dataList.ToArray();
        }
        return null;
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

