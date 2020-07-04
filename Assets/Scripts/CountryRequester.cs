using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;


[Serializable]
public class CountryRequest {
    public CountryList[] countries;
}

[Serializable]
public class CountryList {
    public string Slug;
    public string ISO2;
}
public class CountryRequester : MonoBehaviour
{
    public GameObject earth;
    private List<CountryObj> allCountries = new List<CountryObj>();
    private GeocodeAPI geocode;

    // Start is called before the first frame update
    void Start()
    {
        geocode = earth.GetComponent<GeocodeAPI>();
        WebRequest covidRequest = WebRequest.Create("https://api.covid19api.com/countries");
        covidRequest.Timeout=10000;
        WebResponse Answer = covidRequest.GetResponse();
        StreamReader reader = new StreamReader(Answer.GetResponseStream());
        string res = reader.ReadToEnd();
        Debug.Log(res);
        CountryRequest countryArray = JsonUtility.FromJson<CountryRequest>("{\"countries\":" + res + "}");
        toList(countryArray);
    }

   private void toList(CountryRequest array) {
       foreach (CountryList country in array.countries)
       {
           Debug.Log(country.Slug);
           //Ausnahmen:
           if(!country.ISO2.Equals("AN") ) {

           Result res = geocode.Forward(country.Slug, country.ISO2);
           allCountries.Add(new CountryObj(country.Slug, country.ISO2,res.geometry.lat,res.geometry.lng));
           Debug.Log(country.Slug + ": lat: " + res.geometry.lat + res.geometry.lng);
           }
       }
   }

   public List<CountryObj> getAllCountries() {
       return allCountries;
   }
}
