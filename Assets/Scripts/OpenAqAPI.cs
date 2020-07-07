using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;

[Serializable]
public class FullResponse
{
    public Results[] results;
}

[Serializable]
public class Results
{
    public Date date;
    public float value;
    public string unit;
    public CoordinatesAQ coordinates;
    public string country;
}

[Serializable]
public class Date 
{
    public string utc;
}

[Serializable]
public class CoordinatesAQ
{
    public float latitude;
    public float longitude;
}

public class OpenAqAPI : MonoBehaviour, IDataAPI
{

    const string NAME = "Ozon Werte";
    const string DESCRIPTION = URL + "\nOzin Werte angegeben in ug/m^3";
    private const string URL = "https://api.openaq.org/v1/measurements";
    public static readonly string[] locations = {

    "AD0944A","US Diplomatic Post: Dubai","AT4S235"
    ,"Memorial Park", "Lukavac", "BG0057A","Goose Bay", "Kensington Park"
    , "CH0044A", "Puchuncaví","市城管局"
    ,"淄博市", "MED-MIRA - Tanque Miraflores", "CY0006A"
    , "CZ0SMBO","DERP013", "DEBE010", "DK0054A"
    , "EE0018A", "ES-92", "FI00208","FR04004", "Camden Kerbside"
    , "GR0020A", "HU0037A","IE0111A", "Lodhi Road, Delhi - IMD"
    , "IT1128A", "IT-1", "LT00002", "LU0101A", "LV0007A"
    , "MK0035A", "MT00004", "Camarones", "Posterholt-Vlodropperweg"
    ,"Sofienbergparken", "CAMPO DE MARTE", "Osieczów","PT01047"
    ,"RO0155A","RS0033A", "SE0022A", "Residence for Dept. of Primary Industries and Mines"
    ,"Los Angeles - N. Mai", "pm25", "US-624","Prishtine - IHMK"
    };

    public async Task<DataObject[]> specificRequest(string location)
    {
        string url = URL + "?country=" + location + "&parameter=o3&limit=250";
        Debug.Log(url);
        return toData(await Utility.RequestAsync<FullResponse>(url));
    }

    public async Task<DataObject[]> specificRequest(string location, string startDate, string endDate)
    {
        string url = String.Format("{0}?{1}&date_from={2}&date_to={3}&parameter=o3&limit=250&order_by=date", URL, location, startDate, endDate);
        Debug.Log(url);
        return toData(await Utility.RequestAsync<FullResponse>(url));
    }

    public async Task<DataObject[]> simpleRequest()
    {
        DateTime now = DateTime.Now;
        DataObject[] result = { };
        foreach (string location in locations)
        {
            string request = "location=" + location;
            DataObject[] partResult = await specificRequest(request, now.ToString("yyyy-MM-dd") + "T00:00:00Z", now.ToString("yyyy-MM-dd") + "T23:59:59Z");
            if(partResult != null) {

            int resultLength = result.Length;
            if (resultLength == 0)
            {
                result = partResult;
                continue;
            }
            if (partResult.Length == 0)
            {
                continue;
            }
            Array.Resize<DataObject>(ref result, resultLength + partResult.Length);
            Array.Copy(partResult, 0, result, resultLength, partResult.Length);
            }
        }
        return result;
    }

    public async Task<DataObject[][]> dateRequest(string startDate, string endDate)
    {
        DataObject[][] result = new DataObject[locations.Length][];
        int counter = 0;
        for (int i = 0; i < locations.Length; i++)
        {
            string request = "location=" + locations[i];
            DataObject[] requestAnswer = await specificRequest(request, startDate, endDate);
           if (requestAnswer != null)
           {
            if (requestAnswer.Length != 0)
            {
                DataObject[] homogenArray = new DataObject[365];
                Array.Copy(requestAnswer, 0, homogenArray, 0, requestAnswer.Length);
                result[counter] = homogenArray;
                counter++;
                continue;
            }
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

    public string getName()
    {
        return NAME;
    }

    public string getDescription()
    {
        return DESCRIPTION;
    }

    /*
      converts a FullResponse-object into a DataObject  
    */
    private DataObject[] toData(FullResponse response)
    {
        if (response != null)
        {
            if(response.results.Length != 0) {

            List<DataObject> dataList = new List<DataObject>();
            DateTime date = DateTime.Parse(response.results[0].date.utc);
            dataList.Add(new DataObject(response.results[0].coordinates.latitude, response.results[0].coordinates.longitude, response.results[0].country, response.results[0].value, response.results[0].unit, date));
            Debug.Log(date);
            for (int i = 1; i < response.results.Length; i++)
            {
                if (!DateTime.Equals(date.Date, DateTime.Parse(response.results[i].date.utc).Date))
                {
                    date = DateTime.Parse(response.results[i].date.utc);
                    Debug.Log(date);
                    dataList.Add(new DataObject(response.results[i].coordinates.latitude, response.results[i].coordinates.longitude, response.results[i].country, response.results[i].value, response.results[i].unit, date));
                }
            }
            return dataList.ToArray();
            }
        }
        return null;
        
    }

}
