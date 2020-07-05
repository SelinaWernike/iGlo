using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Globalization;

[Serializable]
public class FullResponse
{
    public Results[] results;
}

[Serializable]
public class Results
{
    public float value;
    public string unit;
    public CoordinatesAQ coordinates;
    public string country;
}

[Serializable]
public class CoordinatesAQ
{
    public float latitude;
    public float longitude;
}

public class OpenAqAPI : MonoBehaviour, IDataAPI {

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

    public DataObject[] specificRequest(string location)
    {
        string url = URL + "?country=" + location + "&parameter=o3&limit=250";
        Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return toData(JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()));
    }

    public DataObject[] specificRequest(string location, string startDate, string endDate)
    {
        string  url = String.Format("{0}?{1}&date_from={2}&date_to={3}&parameter=o3&limit=250", URL, location,startDate,endDate);
        Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return toData(JsonUtility.FromJson<FullResponse>(reader.ReadToEnd()));
    }



    public DataObject[] simpleRequest()
    {
        DateTime now = DateTime.Now;
        DataObject[] result = {};
        foreach (string location in locations)
        {
            string request = "location="  + location;
            DataObject[] partResult = specificRequest(request, now.ToString("yyyy-MM-dd"), now.ToString("yyyy-MM-dd"));
            int resultLength = result.Length;
            if(resultLength == 0) {
                result = partResult;
                continue;
            }
            if(partResult.Length == 0) {
                continue;
            }
            Array.Resize<DataObject>(ref result, resultLength + partResult.Length);
            Array.Copy(partResult,0,result,resultLength,partResult.Length);
        }
        return result;
    }

    public DataObject[][] dateRequest(string startDate, string endDate) {
        DataObject[][] result = new DataObject[locations.Length][];
        for(int i = 0;i < locations.Length;i++) {
            string request = "location="  + locations[i];
            DataObject[] requestAnswer = specificRequest(request, startDate, endDate);
            if(requestAnswer.Length != 0) {
            DataObject[] homogenArray = new DataObject[365];
            Array.Copy(requestAnswer,0,homogenArray,0,requestAnswer.Length);
            result[i] = homogenArray;
            } 
        }
       foreach(DataObject[] obj in result) {
            if(obj != null) {
                return result;
            }
        }
        return null;
    }

     public string getName() {
        return NAME;
    }

    public string getDescription() {
        return DESCRIPTION;
    }
    private DataObject[] toData(FullResponse response)
    {
        DataObject[] obj = new DataObject[response.results.Length];
        for (int i = 0; i < response.results.Length; i++)
        {
            obj[i] = new DataObject(response.results[i].coordinates.latitude, response.results[i].coordinates.longitude, response.results[i].country, response.results[i].value, response.results[i].unit);
        }
        return obj;
    }

    

}
