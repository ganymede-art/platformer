using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using System.Linq;

public class TimeHighLogic : MonoBehaviour, IPersistenceLoadable
{
    // Constants.
    private const float HOURS_IN_DAY = 24.0F;

    private const float EARLY_HOUR = 0.0F;
    private const float DAWN_HOUR = 4.0F;
    private const float MORNING_HOUR = 8.0F;
    private const float AFTERNOON_HOUR = 12.0F;
    private const float DUSK_HOUR = 16.0F;
    private const float LATE_HOUR = 20.0F;
    private const float END_HOUR = 24.0F;

    // Private fields.
    private float hour;
    private int day;
    private (float hour, PeriodType periodType)[] periods;

    private PeriodType periodOfDay;
    private DayType dayOfWeek;

    // Public properties.
    public float Hour => hour;
    public int Day => day;
    public static TimeHighLogic G => GameHighLogic.G.TimeHighLogic;
    public PeriodType PeriodOfDay => periodOfDay;
    public DayType DayOfWeek => dayOfWeek;

    private void Awake()
    {
        periods = new (float hour, PeriodType periodType)[]
        {
            (EARLY_HOUR,PeriodType.Early),
            (DAWN_HOUR,PeriodType.Dawn),
            (MORNING_HOUR,PeriodType.Morning),
            (AFTERNOON_HOUR,PeriodType.Afternoon),
            (DUSK_HOUR,PeriodType.Dusk),
            (LATE_HOUR,PeriodType.Late),
        };

        hour = TIME_INITIAL_HOUR_OFFSET;
        day = TIME_INITIAL_DAY_OFFSET;
    }

    public void ModifyTime(float changeHoursAmount)
    {
        hour += changeHoursAmount;

        if(hour >= HOURS_IN_DAY)
        {
            hour -= HOURS_IN_DAY;
            day++;
        }
        else if(hour < 0)
        {
            hour += HOURS_IN_DAY;
            day--;
        }

        RecalculatePeriod();
        RecalculateDayOfWeek();
    }

    private void RecalculatePeriod()
    {
        periodOfDay = GetPeriodFromHour(hour);
    }

    private void RecalculateDayOfWeek()
    {
        dayOfWeek = (DayType)(day % 7);
    }

    public void LoadFromPersistence(PersistenceHighLogic.PersistenceInfo pi)
    {
        hour = pi.hour;
        day = pi.day;

        RecalculatePeriod();
        RecalculateDayOfWeek();
    }

    public static PeriodType GetPeriodFromHour(float newHour)
    {
        return
            (newHour >= EARLY_HOUR && newHour < DAWN_HOUR) ? PeriodType.Early :
            (newHour >= DAWN_HOUR && newHour < MORNING_HOUR) ? PeriodType.Dawn :
            (newHour >= MORNING_HOUR && newHour < AFTERNOON_HOUR) ? PeriodType.Morning :
            (newHour >= AFTERNOON_HOUR && newHour < DUSK_HOUR) ? PeriodType.Afternoon :
            (newHour >= DUSK_HOUR && newHour < LATE_HOUR) ? PeriodType.Dusk :
            (newHour >= LATE_HOUR && newHour < END_HOUR) ? PeriodType.Late :
            PeriodType.Late;
    }

    public static float GetPeriodProgress(PeriodType periodType, float hour)
    {
        float startHour = GetPeriodStartHour(periodType);
        float finishHour = GetPeriodFinishHour(periodType);
        float periodProgress = Mathf.InverseLerp(startHour, finishHour, hour);
        return periodProgress;
    }

    public static float GetPeriodStartHour(PeriodType periodType)
    {
        return periodType switch
        {
            PeriodType.Early => EARLY_HOUR,
            PeriodType.Dawn => DAWN_HOUR,
            PeriodType.Morning => MORNING_HOUR,
            PeriodType.Afternoon => AFTERNOON_HOUR,
            PeriodType.Dusk => DUSK_HOUR,
            PeriodType.Late => LATE_HOUR,
            _ => EARLY_HOUR,
        };
    }

    public static float GetPeriodFinishHour(PeriodType periodType)
    {
        return periodType switch
        {
            PeriodType.Early => DAWN_HOUR,
            PeriodType.Dawn => MORNING_HOUR,
            PeriodType.Morning => AFTERNOON_HOUR,
            PeriodType.Afternoon => DUSK_HOUR,
            PeriodType.Dusk => LATE_HOUR,
            PeriodType.Late => END_HOUR,
            _ => DAWN_HOUR,
        };
    }

    private void OnGUI()
    {
        string debug
            = $"\nHour: {hour}"
            + $"\nPeriod: {periodOfDay}"
            + $"\nDay: {day}/{dayOfWeek}";
        GUI.Label(new Rect(0, 500, 500, 500), debug);
    } 
}
