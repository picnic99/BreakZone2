using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using System;
using System.Collections.Generic;
using System.Numerics;

public enum RangeType
{
    FRIEND,//队友
    ENEMY,//敌人
    All,//全部
    OTHER,//其它 道具或者宝箱之类的
}
/// <summary>
/// 范围扫描器
/// 主要想用来做 技能范围扫描 确定哪些角色或者物体是需要进行互动的？ 如哪些角色在治疗或者伤害范围内
/// 或者我附近是否有队友或者敌人 又或者道具宝箱之类的
/// </summary>
public class RangeScan
{
    private Character character;

    public RangeScan(Character character)
    {
        this.character = character;
        InitDrawer();
    }

    public void InitDrawer()
    {
        if (this.character == null || this.character.trans == null) return;
        EventDispatcher.GetInstance().On(EventDispatcher.MAIN_ROLE_CHANGE, UpdateRangeScan);
        HideRange();
    }

    public void UpdateRangeScan(object[] args)
    {
        if (character == GameContext.CurRole) character.state = CharacterState.FRIEND;
        else character.state = CharacterState.ENEMY;
    }

    /// <summary>
    /// 圆形范围检测
    /// </summary>
    /// <param name="pos">起始位置</param>
    /// <param name="dir">方向</param>
    /// <param name="degreeH">水平角度</param>
    /// <param name="degreeV">垂直角度</param>
    /// <param name="radius">半径</param>
    /// <param name="type">检测类型</param>
    /// <returns></returns>
    public Character[] CheckShphere(Vector3 pos, Vector3 dir, string degree, float radius, RangeType type)
    {
        float[] degs = new float[2]; int i = 0;
        foreach (var item in degree.Split(',')) degs[i++] = Convert.ToSingle(item);
        var left_border = Quaternion.AngleAxis(degs[0], Vector3.up) * dir;
        var right_border = Quaternion.AngleAxis(degs[1], Vector3.up) * dir;
        List<Character> result = new List<Character>();
        foreach (var target in GameContext.AllCharacter)
        {
            if (target != character)
            {
                var self2Target = target.trans.position - pos;
                var left_crs = Vector3.Cross(self2Target.normalized, left_border.normalized);
                var right_crs = Vector3.Cross(self2Target.normalized, right_border.normalized);
                var angle = Vector3.SignedAngle(dir, self2Target, new Vector3(0,1,0));
                if ((angle >= degs[0] && angle <= degs[1] && left_crs.y * right_crs.y <= 0) || Math.Abs(degs[0]) + Math.Abs(degs[1]) >= 360)
                {
                    Vector3 targetPos = target.trans.position; targetPos.y = 0;
                    Vector3 characterPos = pos; characterPos.y = 0;
                    if (Vector3.Distance(targetPos, characterPos) <= radius)
                    {
                        result.Add(target);
                    }
                }
            }

        }
        return result.ToArray();
    }

    /// <summary>
    /// 围绕某点旋转指定角度
    /// </summary>
    /// <param name="position">自身坐标</param>
    /// <param name="center">旋转中心</param>
    /// <param name="axis">围绕旋转轴</param>
    /// <param name="angle">旋转角度</param>
    /// <returns></returns>
    public Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
    {
        return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
    }
}