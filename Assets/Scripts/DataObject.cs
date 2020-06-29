public class DataObject
{
    private float latitude;
    private float longitude;
    private string country;
    private float value;
    private string unit;

    public DataObject(float lat, float log, string country, float value, string unit)
    {
        this.latitude = lat;
        this.longitude = log;
        this.country = country;
        this.value = value;
        this.unit = unit;
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

    public override string ToString()
    {
        return "Koordinaten: " + latitude + ", " + longitude + " Land: " + country + " Werte: " + value + unit;
    }
}
