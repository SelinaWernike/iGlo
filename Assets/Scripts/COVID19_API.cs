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
        return toData(await Utility.RequestAsync<RootObject>(webURL, "{\"results\":", "}"), DateTime.Parse(startDate), DateTime.Parse(endDate));
    }

    public async Task<DataObject[][]> dateRequest(string startDate, string endDate)
    {
        DataObject[][] result = new DataObject[100][];
        int i = 0;
        foreach (string countryCode in GeocodeAPI.cache.Keys)
        {
            if (i == 100)
            {
                break;
            }
            DataObject[] requestAnswer = await specificRequest(countryCode, startDate, endDate);
            result[i++] = requestAnswer;
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
        UnityEngine.Debug.Log(webURL);
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

    private DataObject[] toData(RootObject response, DateTime start, DateTime end)
    {
        int days = end.Subtract(start).Days + 1;
        List<DataObject> obj = new List<DataObject>(days);
        for (int i = 0; i < response.results.Length; i++)
        {
            // COVID-19 API sometimes gets confused and adds unwanted dates
            DateTime current = DateTime.Parse(response.results[i].Date).Date;
            if (current >= start && current <= end)
            {
                obj.Add(new DataObject(response.results[i].Lat, response.results[i].Lon, response.results[i].Country, response.results[i].Cases, "Personen", current));
            }
        }
        if (obj.Count > days)
        {
            int exceedAmount = obj.Count - days;
            Debug.Log("Exceeded requested data size by " + exceedAmount);
            obj.RemoveRange(days, exceedAmount);
        }
        return obj.ToArray();
    }

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

