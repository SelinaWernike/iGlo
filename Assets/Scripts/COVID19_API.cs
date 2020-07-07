using UnityEngine;
using System;

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
        string webURL = URL + "country/" + location + "/status/confirmed/live?from=" + startDate + "T00:00:00Z" + "&to=" + endDate + "T23:59:59Z";
        UnityEngine.Debug.Log(webURL);
        return toData(await Utility.RequestAsync<RootObject>(webURL, "{\"results\":", "}"));
    }

    public async Task<DataObject[]> specificRequest(string location)
    {
        string webURL = URL + "live/country/" + location + "/status/confirmed";
        UnityEngine.Debug.Log(webURL);
        return toData(await Utility.RequestAsync<RootObject>(webURL, "{\"results\":", "}"));
    }

    public async Task<DataObject[][]> dateRequest(string startDate, string endDate) {
        DataObject[][] result = new DataObject[100][];
        int i = 0;
        foreach (string countryCode in GeocodeAPI.cache.Keys)
        {
            if(i == 99) {
                break;
            }
            DataObject[] requestAnswer = await specificRequest(countryCode, startDate, endDate);
            if(requestAnswer.Length != 0) {
            DataObject[] homogenArray = new DataObject[365];
            Array.Copy(requestAnswer,0,homogenArray,0,requestAnswer.Length);
            result[i] = homogenArray;
            }
            i++;
        }
        foreach(DataObject[] obj in result) {
            if(obj != null) {
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

    private DataObject[] toData(RootObject response)
    {
        DataObject[] obj = new DataObject[response.results.Length];
        for (int i = 0; i < response.results.Length; i++)
        {
            obj[i] = new DataObject(response.results[i].Lat, response.results[i].Lon, response.results[i].Country, response.results[i].Cases, "Personen", DateTime.Parse(response.results[i].Date));
        }
        return obj;
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

