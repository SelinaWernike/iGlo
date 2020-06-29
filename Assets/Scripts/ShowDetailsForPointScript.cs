using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ShowDetailsForPointScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private GameObject details;
    [SerializeField]
    private GameObject dataMenu;
    [SerializeField]
    private GameObject worldMenu;

    private bool valid;
    private GeocodeAPI geocode;
    private List<IDataAPI> apiList;
    private WorldMenuBehaviour worldMenuBehaviour;

    private void Awake()
    {
        geocode = GetComponent<GeocodeAPI>();
        apiList = dataMenu.GetComponent<ScrollButtonControl>().getApiList();
        worldMenuBehaviour = worldMenu.GetComponent<WorldMenuBehaviour>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        valid = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        valid = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (valid)
        {
            RaycastResult raycast = eventData.pointerPressRaycast;
            Vector3 localPoint = raycast.gameObject.transform.InverseTransformPoint(raycast.worldPosition);
            Vector2 latLon = ToLatLong(localPoint);
            ReverseResult result = geocode.Reverse(latLon.x, latLon.y);
            string date = worldMenuBehaviour.getCurrentDate().ToString("yyyy-MM-dd");
            if (result == null)
            {
                details.SetActive(false);
            }
            else
            {
                string info = "Latitude: " + latLon.x + "\n";
                info += "Longitude: " + latLon.y + "\n";
                info += "Country: " + result.components.country + " (" + result.components.country_code + ")\n";
                foreach (IDataAPI api in apiList)
                {
                    info += api.GetAPIName() + ": ";
                    DataObject[] dataObjects = api.specificRequest(result.components.country, date + "T00:00:00Z", date + "T23:59:59Z");
                    info += dataObjects.Length == 0 ? "No data available!\n" : dataObjects[0].getValue() + " " + dataObjects[0].getUnit() + "\n";
                }
                details.GetComponent<Text>().text = info;
                details.GetComponent<RectTransform>().position = eventData.pressPosition;
                details.SetActive(true);
            }
        }
    }

    private Vector2 ToLatLong(Vector3 position)
    {
        position = Vector3.Normalize(position);
        float lat = (float)Mathf.Asin(position.y) * Mathf.Rad2Deg;
        float lon = -(float)Mathf.Atan2(position.x, position.z) * Mathf.Rad2Deg;
        return new Vector2(lat, lon);
    }
}
