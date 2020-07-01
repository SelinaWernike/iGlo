public interface IDataAPI
{
    DataObject[] simpleRequest();
    DataObject[] specificRequest(string location);
    DataObject[] specificRequest(string location, string startDate, string endDate);

    string getName();
    string getDescription();
}
