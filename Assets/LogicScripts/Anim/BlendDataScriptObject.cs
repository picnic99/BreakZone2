
using UnityEngine;

[System.Serializable]
public struct BlendClip2D
{
    public AnimationClip clip;
    public Vector2 pos;
}


[CreateAssetMenu(fileName = "blend Data", menuName = "Create New Blend Data",order = 0)]
public class BlendDataScriptObject:ScriptableObject
{
    public BlendClip2D[] datas;
}