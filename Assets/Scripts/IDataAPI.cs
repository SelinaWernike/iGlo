using System.Threading.Tasks;

public interface IDataAPI
{
    Task<DataObject[]> simpleRequest();
    Task<DataObject[]> specificRequest(string location, string startDate, string endDate);
    Task<DataObject[][]> dateRequest(string startDate, string endDate);

    string getName();
    string getDescription();
}
