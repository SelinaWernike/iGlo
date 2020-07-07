using System;
public class DataObject
{
    private float latitude;
    private float longitude;
    private string country;
    private float value;
    private string unit;
    public DateTime date;

    public DataObject(float lat, float log, string country, float value, string unit, DateTime date)
    {
        this.latitude = lat;
        this.longitude = log;
        this.country = country;
        this.value = value;
        this.unit = unit;
        this.date = date;
    }

    public DataObject () {
        
    }

    public float getLatitude()
    {
        return latitude;
    }

    public float getLongitude()
    {
        return longitude;
    }

    public string getCountry()
    {
        return country;
    }

    public float getValue()
    {
        return value;
    }

    public string getUnit()
    {
        return unit;
    }

    public DateTime getDate(){
        return date;
    }

    public override string ToString()
    {
        return "Koordinaten: " + latitude + ", " + longitude + " Land: " + country + " Werte: " + value + unit;
    }

}
