using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObject 
{
    private float latitude;
    private float longitude;
    private string country;

    private float value;
    private string unit;
    private bool lastData;

    public DataObject(float lat, float log, string country, float value, string unit, bool lastData) {
        this.latitude = lat;
        this.longitude = log;
        this.country = country;
        this.value = value;
        this.unit = unit;
        this.lastData = lastData;
    } 

    public float getLatitude() {
        return latitude;
    }

    public float getLongitude() {
        return longitude;
    }

    public string getCountry() {
        return country;
    }

    public float getValue() {
        return value;
    }

    public string getUnit() {
        return unit;
    }

    public bool getLastData() {
        return lastData;
    }

    public string ToString() {
        return "Koordinaten: " + latitude + ", " + longitude + " Land: " + country + " Werte: " + value + unit;
    }
}
