using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldMenuBehaviour : MonoBehaviour {
    public GameObject worldMenu;
    public Toggle timeLapseToggle;

    public bool worldGlow = false;
    public bool timeLapseIsOn = false;

    public GameObject dateInput;
    public GameObject endDateInput;
    public GameObject timeLapseSlider;

    public GameObject globe2Button;

    void Start() {

        endDateInput.SetActive(false);
        timeLapseSlider.SetActive(false);

        globe2Button.SetActive(false);
    }

    void Update() {
        if (timeLapseIsOn) {
            setTimeLapseDates();
        }
    }

    public void IncreaseDay(string defIn) {
        InputField dayInput;
        if (defIn.Equals("start")) {
            dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
        } else {
            dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
        }
        int day = int.Parse(dayInput.text);
        if (checkButtonInput("day", day + 1)) {
            day += 1;
            dayInput.text = day.ToString();
        }
    }

    public void ReduceDay(string defIn) {
        InputField dayInput;
        if (defIn.Equals("start")) {
            dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
        } else {
            dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
        }
        int day = int.Parse(dayInput.text);
        if (checkButtonInput("day", day - 1)) {
            day -= 1;
            dayInput.text = day.ToString();
        }
    }

    public void IncreaseMonth(string defIn) {
        InputField monthInput;
        if (defIn.Equals("start")) {
            monthInput = GameObject.Find("InputFieldMonth").GetComponent<InputField>();
        } else {
            monthInput = GameObject.Find("InputFieldMonthEnd").GetComponent<InputField>();
        }
        int month = int.Parse(monthInput.text);
        if (checkButtonInput("month", month + 1)) {
            month += 1;
            monthInput.text = month.ToString();
        }
    }

    public void ReduceMonth(string defIn) {
        InputField monthInput;
        if (defIn.Equals("start")) {
            monthInput = GameObject.Find("InputFieldMonth").GetComponent<InputField>();
        } else {
            monthInput = GameObject.Find("InputFieldMonthEnd").GetComponent<InputField>();
        }
        int month = int.Parse(monthInput.text);
        if (checkButtonInput("month", month - 1)) {
            month -= 1;
            monthInput.text = month.ToString();
        }
    }

    public void IncreaseYear(string defIn) {
        InputField yearInput;
        if (defIn.Equals("start")) {
            yearInput = GameObject.Find("InputFieldYear").GetComponent<InputField>();
        } else {
            yearInput = GameObject.Find("InputFieldYearEnd").GetComponent<InputField>();
        }
        int year = int.Parse(yearInput.text);
        if (checkButtonInput("year", year + 1)) {
            year += 1;
            yearInput.text = year.ToString();
        }
    }

    public void ReduceYear(string defIn) {
        InputField yearInput;
        if (defIn.Equals("start")) {
            yearInput = GameObject.Find("InputFieldYear").GetComponent<InputField>();
        } else {
            yearInput = GameObject.Find("InputFieldYearEnd").GetComponent<InputField>();
        }
        int year = int.Parse(yearInput.text);
        if (checkButtonInput("year", year - 1)) {
            year -= 1;
            yearInput.text = year.ToString();
        }
    }

    public void AddWorld() {
        InputField globeCountInput = GameObject.Find("InputFieldGlobeCount").GetComponent<InputField>();
        int globeCount = int.Parse(globeCountInput.text);
        if (checkButtonInput("worlds", globeCount + 1)) {
            globeCount += 1;
            globeCountInput.text = globeCount.ToString();

            globe2Button.SetActive(true);

            // GameObject earthClone = Instantiate(GameObject.Find("Earth"));

            // move cam to view both
        }
    }

    public void RemoveWorld() {
        InputField globeCountInput = GameObject.Find("InputFieldGlobeCount").GetComponent<InputField>();
        int globeCount = int.Parse(globeCountInput.text);
        if (checkButtonInput("worlds", globeCount - 1)) {
            globeCount -= 1;
            globeCountInput.text = globeCount.ToString();

            globe2Button.SetActive(false);

            // Destroy(GameObject.Find("earthClone"));

            // move cam back to single view
        }
    }

    public void onWorldButtonClick() {
        string worldButton = EventSystem.current.currentSelectedGameObject.name;
        if (worldButton.Equals("Welt 1")) {
            // set worldGlow pos behind earth 1
            if (worldGlow) {
                worldGlow = false;
            } else {
                worldGlow = true;
            }
        } else {
            // set worldGlow pos behind earth 2
            if (worldGlow) {
                worldGlow = false;
            } else {
                worldGlow = true;
            }
        }
    }

    public void addTimeLapse() {
        if (timeLapseToggle.isOn) {
            timeLapseIsOn = true;

            Text dateInputLabel = GameObject.Find("DateInputLabel").GetComponent<Text>();
            dateInputLabel.text = "Start Datum:";

            endDateInput.SetActive(true);
            timeLapseSlider.SetActive(true);

            setTimeLapseDates();
        }

        if (!timeLapseToggle.isOn) {
            timeLapseIsOn = false;

            Text dateInputLabel = GameObject.Find("DateInputLabel").GetComponent<Text>();
            dateInputLabel.text = "Datum:";

            endDateInput.SetActive(false);
            timeLapseSlider.SetActive(false);
        }
    }

    public void setTimeLapseDates() {
        Text startDate = GameObject.Find("StartDate").GetComponent<Text>();
        startDate.text = getDate("start");

        Text endDate = GameObject.Find("EndDate").GetComponent<Text>();
        endDate.text = getDate("end");

    }

    public string getDate(string defDate) {

        InputField dayInput;
        InputField monthInput;
        InputField yearInput;
        string date = "";

        if (defDate.Equals("start")) {
            dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
            monthInput = GameObject.Find("InputFieldMonth").GetComponent<InputField>();
            yearInput = GameObject.Find("InputFieldYear").GetComponent<InputField>();
            date = (dayInput.text + "." + monthInput.text + "." + yearInput.text).ToString();

        } else if (defDate.Equals("end")) {
            dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
            monthInput = GameObject.Find("InputFieldMonthEnd").GetComponent<InputField>();
            yearInput = GameObject.Find("InputFieldYearEnd").GetComponent<InputField>();
            date = (dayInput.text + "." + monthInput.text + "." + yearInput.text).ToString();

        }

        return date;
    }

    public bool checkButtonInput(string buttontype, int number) {
        bool isValidNumber = false;

        switch (buttontype) {
            case "day":
                if (number > 0 && number < 32) {
                    isValidNumber = true;
                }
                break;
            case "month":
                if (number > 0 && number < 13) {
                    isValidNumber = true;
                }
                break;
            case "year":
                if (number > 1000 && number < 2021) {
                    isValidNumber = true;
                }
                break;
            case "worlds":
                if (number > 0 && number < 3) {
                    isValidNumber = true;
                }
                break;
        }
        return isValidNumber;
    }

    public void checkKeyboardInput(string inputFieldType) {
        int number;

        InputField dayInput;
        InputField monthInput;
        InputField yearInput;

        switch (inputFieldType) {
            case "day":
                dayInput = GameObject.Find("InputFieldDay").GetComponent<InputField>();
                number = int.Parse(dayInput.text);
                number = Mathf.Clamp(number, 1, 31);
                dayInput.text = number.ToString();
                break;

            case "dayEnd":
                dayInput = GameObject.Find("InputFieldDayEnd").GetComponent<InputField>();
                number = int.Parse(dayInput.text);
                number = Mathf.Clamp(number, 1, 31);
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

            case "worlds":
                InputField globeCountInput = GameObject.Find("InputFieldGlobeCount").GetComponent<InputField>();
                number = int.Parse(globeCountInput.text);
                number = Mathf.Clamp(number, 1, 2);
                globeCountInput.text = number.ToString();
                break;
        }
    }
}
