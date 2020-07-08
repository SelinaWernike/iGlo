using System.Threading.Tasks;

public interface IDataAPI
{
    /*
a request for the current date
@return DataObject[] array with data for all locations
*/
    Task<DataObject[]> simpleRequest();

    /*
request for a single location 
@param location Requested Location
returns DataObject[] an array with all Dates for one location
*/
   // Task<DataObject[]> specificRequest(string location);

 /*   request for a specific location and a specific start and end date
@param location Requested Location
@param startDate 
@param endDate
@returns DataObject[] an Array with DataObjects
*/
    Task<DataObject[]> specificRequest(string location, string startDate, string endDate);

    /* 
request for all locations for a specified timespan
@return DataObject[][] a array for every location with the specified timespan
*/
    Task<DataObject[][]> dateRequest(string startDate, string endDate);

    string getName();
    string getDescription();
}
