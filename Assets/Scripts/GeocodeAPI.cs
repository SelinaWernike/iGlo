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

public class GeocodeAPI : MonoBehaviour
{
    private const string FORWARD_URL = "https://api.opencagedata.com/geocode/v1/json?key={0}&q={1}&countrycode={2}&no_annotations=1&limit=1";
    private const string API_KEY = "01905856ecea4d39992467e82a283703";
    private VisualizeDataScript visualizer;

    private void Start()
    {
        visualizer = GetComponent<VisualizeDataScript>();
    }

    private void Update()
    {
        // TODO: example code, has to be removed when done!
        if (Input.GetKeyDown(KeyCode.N))
        {
            Result result = Forward("Deutschland", "de");
            visualizer.Visualize(result.geometry.lat, result.geometry.lng, 50, false);
            result = Forward("Frankreich", "fr");
            visualizer.Visualize(result.geometry.lat, result.geometry.lng, 100, false);
            result = Forward("Polen", "pl");
            visualizer.Visualize(result.geometry.lat, result.geometry.lng, 0, true);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            visualizer.ClearDrawings();
        }
    }

    public Result Forward(string countryName, string countryCode)
    {
        WebRequest request = WebRequest.Create(String.Format(FORWARD_URL, API_KEY, countryName, countryCode));
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return JsonUtility.FromJson<ForwardResponse>(reader.ReadToEnd()).results[0];
    }
}
