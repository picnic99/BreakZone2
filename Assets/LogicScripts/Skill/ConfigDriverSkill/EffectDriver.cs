
using UnityEngine;

public class EffectDriver
{
    public static int POS_TYPE_FOLLOW = 1;
    public static int POS_TYPE_SOMEPOS = 2;

    public float inTime;
    public string effectName;
    public float drationTime;
    public int posType;
    public Vector3 pos;
    public Vector3 rotate;
    public Vector3 scale;

    public EffectDriver(float inTime, string effectName, float drationTime, int posType, Vector3 pos, Vector3 rotate, Vector3 scale)
    {
        this.inTime = inTime;
        this.effectName = effectName;
        this.drationTime = drationTime;
        this.posType = posType;
        this.pos = pos;
        this.rotate = rotate;
        this.scale = scale;
    }
}