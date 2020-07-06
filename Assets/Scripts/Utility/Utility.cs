using UnityEngine;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System;

public static class Utility
{

    private static bool setupDone;

    public static async Task<T> RequestAsync<T>(string url, string prefix = "", string postfix = "")
    {
        if (!setupDone)
        {
            // HTTP 1.1 only allows us to use 2 connections per client :(
            ServicePointManager.DefaultConnectionLimit = 2;
            ServicePointManager.MaxServicePoints = 20;
            setupDone = true;
        }
        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        request.KeepAlive = true; // keep connections alive for some time, reduces handshake overhead
        request.Pipelined = true; // pipeling allows for multiple requests to be performed without waiting for responses
        using (WebResponse response = await request.GetResponseAsync())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string content = await reader.ReadToEndAsync();
                return JsonUtility.FromJson<T>(prefix + content + postfix);
            }
        }
    }
}