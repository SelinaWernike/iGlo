using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryObj
{
        public string country;
        public string countryCode;
        public float lat;
        public float lng;

        public CountryObj(string country, string countryCode, float lat, float lng) {
            this.country = country;
            this.countryCode = countryCode;
            this.lat = lat;
            this.lng = lng;
        }
    
}
