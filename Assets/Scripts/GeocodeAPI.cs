using UnityEngine;
using System;
using System.Net;
using System.IO;

[Serializable]
public class ForwardResponse
{
    public Result[] results;
}

[Serializable]
public class ReverseResponse
{
    public ReverseResult[] results;
}

[Serializable]
public class ReverseResult
{
    public Bounds bounds;
    public Geometry geometry;
    public Components components;
}

[Serializable]
public class Components
{
    public string country;
    public string country_code;
}

[Serializable]
public class Result
{
    public Bounds bounds;
    public Geometry geometry;
}

[Serializable]
public class Bounds
{
    public Geometry northeast;

    public Geometry southwest;
}

[Serializable]
public class Geometry
{
    public float lat;

    public float lng;
}

[Serializable]
public class ResultCache : SerializableDictionary<String, Result> { }

public class GeocodeAPI : MonoBehaviour
{
    private const string FORWARD_URL = "https://api.opencagedata.com/geocode/v1/json?key={0}&q={1}&countrycode={2}&no_annotations=1&limit=1&language=en";
    private const string REVERSE_URL = "https://api.opencagedata.com/geocode/v1/json?key={0}&q={1},{2}&no_annotations=1&limit=1&language=en";
    private const string API_KEY = "01905856ecea4d39992467e82a283703";
    private const string CACHE_PATH = "./caches/geocode.cache";

    public static ResultCache cache;
    private bool cacheCreated;

    private void Awake()
    {
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

    private void OnApplicationQuit()
    {
        if (cacheCreated)
        {
            File.WriteAllText(CACHE_PATH, JsonUtility.ToJson(cache));
        }
    }

    public Result Forward(string countryName, string countryCode)
    {
        if (!cache.ContainsKey(countryCode))
        {
            WebRequest request = WebRequest.Create(String.Format(FORWARD_URL, API_KEY, countryName, countryCode));
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            Result result = JsonUtility.FromJson<ForwardResponse>(reader.ReadToEnd()).results[0];
            cache.Add(countryCode, result);
        }
        return cache[countryCode];
    }

    public ReverseResult Reverse(float latitude, float longitude)
    {
        WebRequest request = WebRequest.Create(String.Format(REVERSE_URL, API_KEY, latitude, longitude));
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        ReverseResponse respone = JsonUtility.FromJson<ReverseResponse>(reader.ReadToEnd());
        // return null if coordinates do not correspond to country (e.g. international waters)
        return respone.results.Length == 0 || respone.results[0].components.country == null ? null : respone.results[0];
    }
}
