using UnityEngine;

public class ActionDriver
{
    public static int ACTION_DAMAGE = 1;
    public static int ACTION_CARE = 2;

    public static int RANGE_CIRCLE = 1;
    public static int RANGE_BOX = 2;

    public float inTime;
    public float duration;
    public int actionType;
    public int value;
    public int rangeType;
    public Vector3 rangeParam;

    public ActionDriver(float inTime, float duration, int actionType, int value, int rangeType, Vector3 rangeParam)
    {
        this.inTime = inTime;
        this.duration = duration;
        this.actionType = actionType;
        this.value = value;
        this.rangeType = rangeType;
        this.rangeParam = rangeParam;
    }
}