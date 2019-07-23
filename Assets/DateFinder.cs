using UnityEngine;
using System.Collections;
using System.Linq;

public class DateFinder : MonoBehaviour
{
    private new KMAudio audio;
    private KMBombModule bombModule;
    [HideInInspector] public KMSelectable[] buttons;

    [HideInInspector] public TextMesh displayDate;
    [HideInInspector] public TextMesh buttonText;

    private int[] days = new int[31];
    private int[] years = new int[100];
    private int[] centuries = new int[29];

    private string[] months1 = new string[] { "January", "October" };
    private string[] months2 = new string[] { "May" };
    private string[] months3 = new string[] { "August" };
    private string[] months4 = new string[] { "February", "March", "November" };
    private string[] months5 = new string[] { "June" };
    private string[] months6 = new string[] { "September", "December" };
    private string[] months7 = new string[] { "April", "July" };
    private string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    private string[] monthsAbbr = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    private int[] years1 = new int[] { 00, 06, 17, 23, 28, 34, 45, 51, 56, 62, 73, 79, 84, 90 };
    private int[] years2 = new int[] { 01, 07, 12, 18, 29, 35, 40, 46, 57, 63, 68, 74, 85, 91, 96 };
    private int[] years3 = new int[] { 02, 13, 19, 24, 30, 41, 47, 52, 58, 69, 75, 80, 86, 97 };
    private int[] years4 = new int[] { 03, 08, 14, 25, 31, 36, 42, 53, 59, 64, 70, 81, 87, 92, 98 };
    private int[] years5 = new int[] { 09, 15, 20, 26, 37, 43, 48, 54, 65, 71, 76, 82, 93, 99 };
    private int[] years6 = new int[] { 04, 10, 21, 27, 32, 38, 49, 55, 60, 66, 77, 83, 88, 94 };
    private int[] years7 = new int[] { 05, 11, 16, 22, 33, 39, 44, 50, 61, 67, 72, 78, 89, 95 };

    private int[] centuries1 = new int[] { 0, 7, 14, 17, 21, 25 };
    private int[] centuries2 = new int[] { 1, 8, 15 };
    private int[] centuries3 = new int[] { 2, 9, 18, 22, 26 };
    private int[] centuries4 = new int[] { 3, 10 };
    private int[] centuries5 = new int[] { 4, 11, 19, 23, 27 };
    private int[] centuries6 = new int[] { 5, 12, 16, 20, 24, 28 };
    private int[] centuries7 = new int[] { 6, 13 };

    private string[] weekDays = new string[] { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

    private int day, year, century, dayMonthResult, yearCenturyResult, x = 0, count = 0;
    private string month, date, currentWeekDay, yearString, centuryString, fullYear;

    private int _moduleId;
    private static int _moduleIdCounter = 1;

    bool isActive = false;

    public int stages = 3;

    int CurrentYearId()
    {
        int id = 0;
        foreach (int y in years1) { if (y == year) { id = 0; } }
        foreach (int y in years2) { if (y == year) { id = 1; } }
        foreach (int y in years3) { if (y == year) { id = 2; } }
        foreach (int y in years4) { if (y == year) { id = 3; } }
        foreach (int y in years5) { if (y == year) { id = 4; } }
        foreach (int y in years6) { if (y == year) { id = 5; } }
        foreach (int y in years7) { if (y == year) { id = 6; } }
        Debug.Log("Current year id:" + id);
        return id;
    }
    int CurrentCenturyId()
    {
        int id = 0;
        foreach (int c in centuries1) { if (c == century) { id = 7; } }
        foreach (int c in centuries2) { if (c == century) { id = 6; } }
        foreach (int c in centuries3) { if (c == century) { id = 5; } }
        foreach (int c in centuries4) { if (c == century) { id = 4; } }
        foreach (int c in centuries5) { if (c == century) { id = 3; } }
        foreach (int c in centuries6) { if (c == century) { id = 2; } }
        foreach (int c in centuries7) { if (c == century) { id = 1; } }
        Debug.Log("Current century id:" + id);
        return id;
    }

    private void Awake()
    {
        for (int i = 0; i < 31; i++) { days[i] = i + 1; } //Days' setup
        for (int i = 0; i < 100; i++) { years[i] = i; } //Years' setup
        for (int i = 0; i < 29; i++) { centuries[i] = i; } //Centuries' setup   

        yearCenturyResult = 0;
        _moduleId = _moduleIdCounter++;

        GenerateDate();

        audio = GetComponent<KMAudio>();
        bombModule = GetComponent<KMBombModule>();

        buttonText.text = weekDays[x];
    }

    private void Start()
    {
        bombModule.OnActivate += Activate;
        for (int i = 0; i < 3; i++) { int j = i; buttons[i].OnInteract += delegate () { OnPress(j); return false; }; }
    }

    void Activate()
    {
        isActive = true;
        displayDate.text = date;
    }

    void GenerateDate()
    {
        int rndDayInd = UnityEngine.Random.Range(1, days.Length);
        day = rndDayInd;

        int rndMonthInd = UnityEngine.Random.Range(0, months.Length);
        month = months[rndMonthInd];

        int rndYearInd = UnityEngine.Random.Range(0, years.Length);
        year = rndYearInd;

        int rndCentury = UnityEngine.Random.Range(0, centuries.Length);
        century = rndCentury;

        dayMonthResult = ((day + 6) % 7);
        foreach (string _month in months1) { if (_month == month) { dayMonthResult += 1; break; } }
        foreach (string _month in months2) { if (_month == month) { dayMonthResult += 2; break; } }
        foreach (string _month in months3) { if (_month == month) { dayMonthResult += 3; break; } }
        foreach (string _month in months4) { if (_month == month) { dayMonthResult += 4; break; } }
        foreach (string _month in months5) { if (_month == month) { dayMonthResult += 5; break; } }
        foreach (string _month in months6) { if (_month == month) { dayMonthResult += 6; break; } }
        foreach (string _month in months7) { if (_month == month) { dayMonthResult += 7; break; } }
        if (dayMonthResult > 7) { dayMonthResult -= 7; }

        yearCenturyResult = CurrentCenturyId() + CurrentYearId();
        if (yearCenturyResult > 7) { yearCenturyResult -= 7; }
        Debug.Log("Year century result: " + yearCenturyResult);

        centuryString = century.ToString();
        if (year < 10) { yearString = "0" + year.ToString(); } else yearString = year.ToString();
        fullYear = centuryString + yearString;

        int dayIndex = dayMonthResult + yearCenturyResult - 2;
        if (dayIndex > 6) { dayIndex -= 7; }
        if (dayIndex == -1) { dayIndex = 6; }
        currentWeekDay = weekDays[dayIndex];

        date = day + " " + monthsAbbr[rndMonthInd] + ",\n" + fullYear;
        Debug.LogFormat(@"[Date Finder #{0}] Generated date: {1}, which is on {2}", _moduleId, date.Replace("\n", " "), currentWeekDay);
    }

    void OnPress(int n)
    {
        if (!isActive) return;

        audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        GetComponent<KMSelectable>().AddInteractionPunch();

        if (n == 0) { x--; if (x < 0) { x += 7; } buttonText.text = weekDays[x]; }
        else if (n == 1) { x++; if (x > 6) { x -= 7; } buttonText.text = weekDays[x]; }

        if (n == 2) { Check(x); }
    }

    void Check(int n)
    {
        if (weekDays[n] == currentWeekDay) {
            if (count > stages - 2) {
                bombModule.HandlePass(); Debug.LogFormat(@"[Date Finder #{0}] Submitted date is correct.", _moduleId);
                isActive = false; displayDate.text = ""; return;
            } count++; GenerateDate(); Activate();
        } else { bombModule.HandleStrike(); Debug.LogFormat(@"[Date Finder #{0}] Submitted date is wrong.", _moduleId); return; }
    }

    private string TwitchHelpMessage = "!{0} submit Monday";

    IEnumerator ProcessTwitchCommand(string command)
    {
        //Remove submit and spaces from command
        command = command.ToLowerInvariant().Replace("submit", "").Replace(" ", "");
        var weekDaysShort = new string[0];
        //Avoid situation where empty or 1 character strings causes an exception.
        if (command.Length > 1)
        {
            command = command.Substring(0, 2);
            //take the first two characters of every value of the weekDays array
            weekDaysShort = weekDays.Select(x => x.Substring(0, 2).ToLowerInvariant()).ToArray();
            //report invalid command if the command is not a valid day
        }
        if (!weekDaysShort.Contains(command))
            yield break;
        var desiredDate = weekDaysShort.ToList().IndexOf(command);
        yield return null;
        while (x != desiredDate)
        {
            yield return new[] { buttons[1] };
            yield return "trycancel";
        }
        yield return new[] { buttons[2] };
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        for (int i = 0; i < 3; i++)
        {
            while (weekDays[x] != currentWeekDay)
            {
                buttons[1].OnInteract();
                yield return true;
            }
            buttons[2].OnInteract();
        }
    }
}