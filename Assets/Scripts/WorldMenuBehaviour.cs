using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class WorldMenuBehaviour : MonoBehaviour
{
    public GameObject earth;
    public Toggle timeLapseToggle;

    public bool timeLapseIsOn = false;
    public string globeKey;

    public GameObject dateInput;
    public GameObject endDateInput;
    public GameObject timeLapseSlider;

    public GameObject globe2Button;
    public GameObject apiController;

    public GameObject addWorldButton;
    public GameObject removeWorldButton;

    public GameObject startDateError;
    public GameObject endDateError;

    private Vector3 earthPos;
    private GameObject selectedEarth;
    private List<ISelecionChangeObserver> observers = new List<ISelecionChangeObserver>();

    void Start()
    {
        setDateOnStartup();

        endDateInput.SetActive(false);
        timeLapseSlider.SetActive(false);

        globe2Button.SetActive(false);

        removeWorldButton.SetActive(false);

        startDateError.SetActive(false);
        endDateError.SetActive(false);

        earthPos = earth.transform.position;

        earth.GetComponent<Renderer>().material.SetShaderPassEnabled("Always", false);
        SetSelectedEarthNoPass(earth);

        globeKey = "Globe1";
    }

    void Update()
    {
        if (timeLapseIsOn)
        {
            setTimeLapseDates();
        }
    }

    public void IncreaseDay(string defIn)
    {
        InputField dayInput;
        if (defIn.Equals("start"))
        {
            dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
        }
        else
        {
            dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
        }
        int day = int.Parse(dayInput.text);
        if (checkButtonInput("day", day + 1))
        {
            day += 1;
            day = clampDayByMonth(defIn, "button", day);
            dayInput.text = day.ToString();
        }
        checkForDateError(defIn);
    }

    public void ReduceDay(string defIn)
    {
        InputField dayInput;
        if (defIn.Equals("start"))
        {
            dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
        }
        else
        {
            dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
        }
        int day = int.Parse(dayInput.text);
        if (checkButtonInput("day", day - 1))
        {
            day -= 1;
            day = clampDayByMonth(defIn, "button", day);
            dayInput.text = day.ToString();
        }
        checkForDateError(defIn);
    }

    public void IncreaseMonth(string defIn)
    {
        InputField monthInput;
        InputField dayInput;
        if (defIn.Equals("start"))
        {
            monthInput = GameObject.Find("InputFieldMonth").GetComponent<InputField>();
            dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
        }
        else
        {
            monthInput = GameObject.Find("InputFieldMonthEnd").GetComponent<InputField>();
            dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
        }
        int day = int.Parse(dayInput.text);
        int month = int.Parse(monthInput.text);
        if (checkButtonInput("month", month + 1))
        {
            month += 1;
            monthInput.text = month.ToString();

            day = clampDayByMonth(defIn, "button", day);
            dayInput.text = day.ToString();
        }
        checkForDateError(defIn);
    }

    public void ReduceMonth(string defIn)
    {
        InputField monthInput;
        if (defIn.Equals("start"))
        {
            monthInput = GameObject.Find("InputFieldMonth").GetComponent<InputField>();
        }
        else
        {
            monthInput = GameObject.Find("InputFieldMonthEnd").GetComponent<InputField>();
        }
        int month = int.Parse(monthInput.text);
        if (checkButtonInput("month", month - 1))
        {
            month -= 1;
            monthInput.text = month.ToString();
        }
        checkForDateError(defIn);
    }

    public void IncreaseYear(string defIn)
    {
        InputField yearInput;
        if (defIn.Equals("start"))
        {
            yearInput = GameObject.Find("InputFieldYear").GetComponent<InputField>();
        }
        else
        {
            yearInput = GameObject.Find("InputFieldYearEnd").GetComponent<InputField>();
        }
        int year = int.Parse(yearInput.text);
        if (checkButtonInput("year", year + 1))
        {
            year += 1;
            yearInput.text = year.ToString();
        }
        checkForDateError(defIn);
    }

    public void ReduceYear(string defIn)
    {
        InputField yearInput;
        if (defIn.Equals("start"))
        {
            yearInput = GameObject.Find("InputFieldYear").GetComponent<InputField>();
        }
        else
        {
            yearInput = GameObject.Find("InputFieldYearEnd").GetComponent<InputField>();
        }
        int year = int.Parse(yearInput.text);
        if (checkButtonInput("year", year - 1))
        {
            year -= 1;
            yearInput.text = year.ToString();
        }
        checkForDateError(defIn);
    }

    public void AddWorld()
    {
        addWorldButton.SetActive(false);
        removeWorldButton.SetActive(true);
        globe2Button.SetActive(true);

        GameObject earthClone = Instantiate(earth);
        Texture texture = earthClone.GetComponent<Renderer>().material.mainTexture;
        Texture copied = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, texture.mipmapCount, true);
        Graphics.CopyTexture(texture, copied);
        earthClone.GetComponent<Renderer>().material.mainTexture = copied;

        earth.transform.position = new Vector3(earthPos.x - 0.25f, earthPos.y, earthPos.z + 0.1f);
        earthClone.transform.position = new Vector3(earthPos.x + 0.25f, earthPos.y, earthPos.z + 0.1f);
        earth.GetComponent<Renderer>().material.SetShaderPassEnabled("Always", true);
        earthClone.GetComponent<Renderer>().material.SetShaderPassEnabled("Always", false);
        SetSelectedEarthNoPass(earth);
    }

    public void RemoveWorld()
    {
        addWorldButton.SetActive(true);
        removeWorldButton.SetActive(false);
        globe2Button.SetActive(false);
        earth.GetComponent<Renderer>().material.SetShaderPassEnabled("Always", false);
        SetSelectedEarthNoPass(earth);
        Destroy(GameObject.Find("Earth(Clone)"));
        earth.transform.position = earthPos;
    }

    public void AddSelectionChangeObserver(ISelecionChangeObserver observer)
    {
        observers.Add(observer);
    }

    public GameObject GetSelectedEarth()
    {
        return selectedEarth;
    }

    public void SetSelectedEarth(GameObject selected)
    {
        if (selectedEarth != selected)
        {
            selectedEarth.GetComponent<Renderer>().material.SetShaderPassEnabled("Always", false);
            selected.GetComponent<Renderer>().material.SetShaderPassEnabled("Always", true);
            SetSelectedEarthNoPass(selected);
        }
    }

    public void SetSelectedEarthNoPass(GameObject selected)
    {
        if (selectedEarth != selected)
        {
            foreach (ISelecionChangeObserver observer in observers)
            {
                observer.onChange(selected);
            }
            selectedEarth = selected;
        }
    }

    public void onWorldButtonClick()
    {
        string worldButton = EventSystem.current.currentSelectedGameObject.name;
        GameObject world2 = GameObject.Find("Earth(Clone)");
        if (world2 != null)
        {
            switch (worldButton)
            {
                case "Globe1Button":
                    SetSelectedEarth(earth);
                    break;
                case "Globe2Button":
                    SetSelectedEarth(world2);
                    break;
            }
        }
    }

    public void addTimeLapse()
    {
        if (timeLapseToggle.isOn)
        {
            timeLapseIsOn = true;

            Text dateInputLabel = GameObject.Find("DateInputLabel").GetComponent<Text>();
            dateInputLabel.text = "Start Datum:";

            endDateInput.SetActive(true);
            timeLapseSlider.SetActive(true);

            setTimeLapseDates();
        }

        if (!timeLapseToggle.isOn)
        {
            timeLapseIsOn = false;

            Text dateInputLabel = GameObject.Find("DateInputLabel").GetComponent<Text>();
            dateInputLabel.text = "Datum:";

            endDateInput.SetActive(false);
            timeLapseSlider.SetActive(false);
        }
    }

    public void setTimeLapseDates()
    {
        Text startDate = GameObject.Find("StartDate").GetComponent<Text>();
        startDate.text = getDate("start");

        Text endDate = GameObject.Find("EndDate").GetComponent<Text>();
        endDate.text = getDate("end");

    }

    public System.DateTime getCurrentDate() {
        if(timeLapseIsOn) {
            DateTime startDate = DateTime.Parse(getDate("start"));
            float days = timeLapseSlider.GetComponent<Slider>().value;
            return startDate.AddDays(days);
        } else {
            return System.DateTime.Parse(getDate("start"));
        }
    }

    public string getDate(string inputFields)
    {

        InputField dayInput;
        InputField monthInput;
        InputField yearInput;
        string date = "";

        if (inputFields.Equals("start"))
        {
            dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
            monthInput = GameObject.Find("InputFieldMonth").GetComponent<InputField>();
            yearInput = GameObject.Find("InputFieldYear").GetComponent<InputField>();
            date = (dayInput.text + "." + monthInput.text + "." + yearInput.text).ToString();

        }
        else if (inputFields.Equals("end"))
        {
            dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
            monthInput = GameObject.Find("InputFieldMonthEnd").GetComponent<InputField>();
            yearInput = GameObject.Find("InputFieldYearEnd").GetComponent<InputField>();
            date = (dayInput.text + "." + monthInput.text + "." + yearInput.text).ToString();
        }

        return date;
    }

    public void setDateOnStartup() {
        InputField dayInputStart = GameObject.Find("InputFieldDay").GetComponent<InputField>();
        InputField monthInputStart = GameObject.Find("InputFieldMonth").GetComponent<InputField>();
        InputField yearInputStart = GameObject.Find("InputFieldYear").GetComponent<InputField>();

        InputField dayInputEnd = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
        InputField monthInputEnd = GameObject.Find("InputFieldMonthEnd").GetComponent<InputField>();
        InputField yearInputEnd = GameObject.Find("InputFieldYearEnd").GetComponent<InputField>();

        int[] currentDate = getSystemDate();

        dayInputStart.text = currentDate[0].ToString();
        dayInputEnd.text = currentDate[0].ToString();

        monthInputStart.text = currentDate[1].ToString();
        monthInputEnd.text = currentDate[1].ToString();

        yearInputStart.text = currentDate[2].ToString();
        yearInputEnd.text = currentDate[2].ToString();
    }

    public int[] getSystemDate()
    {
        DateTime localDate = DateTime.Now;
        int[] currentDate = new int[] { localDate.Day, localDate.Month, localDate.Year };

        return currentDate;
    }

    public bool checkButtonInput(string buttontype, int number)
    {
        bool isValidNumber = false;

        int[] currentDate = getSystemDate();

        switch (buttontype)
        {
            case "day":
                if (number > 0 && number <= 31)
                {
                    isValidNumber = true;
                }
                break;
            case "month":
                if (number > 0 && number <= 12)
                {
                    isValidNumber = true;
                }
                break;
            case "year":
                if (number > 2016 && number <= currentDate[2])
                {
                    isValidNumber = true;
                }
                break;
        }
        return isValidNumber;
    }

    public void checkKeyboardInput(string inputFieldType)
    {
        int number;

        InputField dayInput;
        InputField monthInput;
        InputField yearInput;

        string inputFields;

        switch (inputFieldType)
        {
            case "day":
                inputFields = "start";
                dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
                number = clampDayByMonth(inputFields, "keyboard", 1);
                dayInput.text = number.ToString();
                break;

            case "dayEnd":
                inputFields = "end";
                dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
                number = clampDayByMonth(inputFields, "keyboard", 1);
                dayInput.text = number.ToString();
                break;

            case "month":
                monthInput = GameObject.Find("InputFieldMonth").GetComponent<InputField>();
                number = int.Parse(monthInput.text);
                number = Mathf.Clamp(number, 1, 12);
                monthInput.text = number.ToString();
                break;

            case "monthEnd":
                monthInput = GameObject.Find("InputFieldMonthEnd").GetComponent<InputField>();
                number = int.Parse(monthInput.text);
                number = Mathf.Clamp(number, 1, 12);
                monthInput.text = number.ToString();
                break;

            case "year":
                yearInput = GameObject.Find("InputFieldYear").GetComponent<InputField>();
                number = int.Parse(yearInput.text);
                number = Mathf.Clamp(number, 1, 2020);
                yearInput.text = number.ToString();
                break;

            case "yearEnd":
                yearInput = GameObject.Find("InputFieldYearEnd").GetComponent<InputField>();
                number = int.Parse(yearInput.text);
                number = Mathf.Clamp(number, 1, 2020);
                yearInput.text = number.ToString();
                break;
        }
    }

    public void onApplyButtonClick()
    {
        DateTime start = DateTime.Parse(getDate("start"));
        Debug.Log(start.ToString());
        List<IDataAPI> dataList = apiController.GetComponent<ScrollButtonControl>().getApiList();
        if (timeLapseIsOn)
        {
            DateTime end = DateTime.Parse(getDate("end"));
            Debug.Log(end.ToString());
            TimeSpan span = end.Subtract(start);
            Debug.Log(span.Days);
            timeLapseSlider.GetComponent<Slider>().maxValue = span.Days;
            apiController.GetComponent<ScrollButtonControl>().saveTimeSpanData(start, end);
        } else {
          apiController.GetComponent<ScrollButtonControl>().drawSingleDay(start);
        }
    }

    public string[] getCurrentInput(string inputFields)
    {
        string[] currentInput = new string[4];

        InputField dayInput;
        InputField monthInput;
        InputField yearInput;

        switch (inputFields)
        {
            case "start":
                dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
                monthInput = GameObject.Find("InputFieldMonth").GetComponent<InputField>();
                yearInput = GameObject.Find("InputFieldYear").GetComponent<InputField>();

                currentInput[0] = inputFields;
                currentInput[1] = dayInput.text;
                currentInput[2] = monthInput.text;
                currentInput[3] = yearInput.text;
                break;

            case "end":
                dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
                monthInput = GameObject.Find("InputFieldMonthEnd").GetComponent<InputField>();
                yearInput = GameObject.Find("InputFieldYearEnd").GetComponent<InputField>();

                currentInput[0] = inputFields;
                currentInput[1] = dayInput.text;
                currentInput[2] = monthInput.text;
                currentInput[3] = yearInput.text;
                break;
        }

        return currentInput;
    }

    public void checkForDateError(string inputFields)
    {
        string[] currentInput = getCurrentInput(inputFields);
        int[] currentDate = getSystemDate();

        int inputDay = int.Parse(currentInput[1]);
        int inputMonth = int.Parse(currentInput[2]);
        int inputYear = int.Parse(currentInput[3]);

        int currentDay = currentDate[0];
        int currentMonth = currentDate[1];
        int currentYear = currentDate[2];

        if (inputYear < 2017 && inputYear > currentYear || inputYear == 2017 && inputMonth < 8 || inputYear == currentYear && inputMonth > currentMonth || inputYear == 2017 && inputMonth == 8 && inputDay < 11 || inputYear == currentYear && inputMonth == currentMonth && inputDay > currentDay)
        {
            switch (inputFields)
            {
                case "start":
                    startDateError.SetActive(true);
                    break;
                case "end":
                    endDateError.SetActive(true);
                    break;
            }
        }
        else
        {
            switch (inputFields)
            {
                case "start":
                    startDateError.SetActive(false);
                    break;
                case "end":
                    endDateError.SetActive(false);
                    break;
            }
        }
    }

    public int clampDayByMonth(string inputFields, string inputType, int inputNumber)
    {
        int minDay = 1;
        int maxDay = 31;

        int inputDay = 1;

        string[] currentInput = getCurrentInput(inputFields);
        int inputMonth = int.Parse(currentInput[2]);
        int inputYear = int.Parse(currentInput[3]);

        switch (inputType)
        {
            case "button":
                inputDay = inputNumber;
                break;
            case "keyboard":
                inputDay = int.Parse(currentInput[1]);
                break;
        }

        switch (inputMonth)
        {
            case 2:
                if (inputYear == 2020)
                {
                    maxDay = 29;
                }
                else
                {
                    maxDay = 28;
                }
                break;
            case 4:
            case 6:
            case 9:
            case 11:
                maxDay = 30;
                break;
            default:
                break;
        }
        int clampedDay = Mathf.Clamp(inputDay, minDay, maxDay);

        return clampedDay;
    }
}
