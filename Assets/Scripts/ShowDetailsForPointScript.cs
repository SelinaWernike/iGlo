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
    private WorldMenuBehaviour worldMenuBehaviour;

    private void Start()
    {
        geocode = GetComponent<GeocodeAPI>();
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

    public async void OnPointerUp(PointerEventData eventData)
    {
        if (valid)
        {
            worldMenuBehaviour.SetSelectedEarth(gameObject);
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
                string info = "Breite: " + latLon.x + "\n";
                info += "Länge: " + latLon.y + "\n";
                info += "Land: " + result.components.country + " (" + result.components.country_code + ")\n";

                List<IDataAPI> apiList = dataMenu.GetComponent<ScrollButtonControl>().getApiList();
                foreach (IDataAPI api in apiList)
                {
                    info += api.getName() + ": ";
                    DataObject[] dataObjects;
                    if (api.getName().Equals("Ozon Werte"))
                    {
                        string location = "country=" + result.components.country_code.ToUpper();
                        dataObjects = await api.specificRequest(location, date, date);
                    }
                    else
                    {
                        string location = result.components.country;
                        dataObjects = await api.specificRequest(location, date + "T00:00:00Z", date + "T23:59:59Z");
                    }
                    bool dataPresent = dataObjects != null && dataObjects.Length == 1;
                    info += dataPresent ? dataObjects[0].getValue() + " " + dataObjects[0].getUnit() + "\n" : "Keine Daten verfügbar!\n";
                }
                Text text = details.GetComponentInChildren<Text>();
                text.text = info;
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
