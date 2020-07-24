using UnityEngine;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Collections.Generic;

public static class Utility
{

    private static bool setupDone;

    private static Dictionary<string, long> rateLimits = new Dictionary<string, long>();

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
        // some APIs unfortunatly limit the number of requests in a certain time window
        await waitForRateLimit(request.Host);
        request.KeepAlive = true; // keep connections alive for some time, reduces handshake overhead
        request.Pipelined = true; // pipeling allows for multiple requests to be performed without waiting for responses
        using (WebResponse response = await request.GetResponseAsync())
        {
            updateRateLimit(request.Host, response);
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string content = await reader.ReadToEndAsync();
                return JsonUtility.FromJson<T>(prefix + content + postfix);
            }
        }
    }

    private static async Task waitForRateLimit(string host)
    {
        if (rateLimits.ContainsKey(host))
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long differenceSeconds = Math.Max(rateLimits[host] - now - 1, 0);
            long differenceMilliseconds = 1000 - DateTimeOffset.UtcNow.Millisecond;
            await Task.Delay((int)(differenceSeconds * 1000 + differenceMilliseconds));
        }
    }

    private static void updateRateLimit(string host, WebResponse response)
    {
        WebHeaderCollection headers = response.Headers;
        string remainingString = headers["X-Ratelimit-Remaining"];
        if (remainingString != null)
        {
            long remaining = long.Parse(remainingString);
            if (remaining == 0)
            {
                rateLimits[host] = long.Parse(headers["X-Ratelimit-Reset"]);
                Debug.Log("We have to wait until: " + rateLimits[host] + ", Current Time: " + DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }
            else
            {
                rateLimits.Remove(host);
            }
        }
    }
}
