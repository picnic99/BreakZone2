using System;
using System.Collections;
using UnityEngine;

public class PhysicController
{
    public Character character;
    //private Rigidbody rb;
    private CharacterController cc;
    /// <summary>
    /// 重力加速度
    /// </summary>
    public readonly float gravity = 20f;
    /// <summary>
    /// 是否在地面上
    /// </summary>
    public bool isGround = true;
    /// <summary>
    /// 上一帧是否在地面上
    /// </summary>
    private bool lastIsGround = true;
    /// <summary>
    /// 玩家能否控制
    /// </summary>
    private bool CanControl = true;
    /// <summary>
    /// 玩家是否允许移动
    /// </summary>
    private bool CanMove = true;
    /// <summary>
    /// 是否在跳跃中
    /// </summary>
    private bool IsJump = false;
    /// <summary>
    /// 是否受重力影响
    /// </summary>
    private bool IsGravityEffect = true;

    public float multiply = 1f;
    /// <summary>
    /// 重力的偏移量 随着时间增加 重力加速度增加 该数值会不断提升，到达地面时会重置
    /// ？？？若还没到地面时 又来了一个升高咋处理？ 1.考虑这个偏移量 进行相减 但是速度小于这个偏移量时就无效了 最多顿挫一下， 2.不考虑偏移量直接置零然后重新下落
    /// </summary>
    private float GravityOffset = 0;
    /// <summary>
    /// 每帧的运动偏移量 XZ表示位移  Y表示高度
    /// </summary>
    //private Vector3 curDeltaPos = Vector3.zero;
    private VectorValue deltaPosValue = new VectorValue(Vector3.zero);
    private Vector3 totalMove = Vector3.zero;
    public PhysicController(Character character)
    {
        this.character = character;
        cc = this.character.trans.GetComponent<CharacterController>();
        DebugManager.Instance.AddMonitor(() => { return "DeltaPosValue：" + deltaPosValue.finalValue.ToString(); });
        DebugManager.Instance.AddMonitor(() => { return "IsJump：" + IsJump; });
        DebugManager.Instance.AddMonitor(() => { return "GravityOffset：" + GravityOffset; });
        DebugManager.Instance.AddMonitor(() => { return "isGround：" + isGround; });
        DebugManager.Instance.AddMonitor(() => { return "totalMove：" + totalMove.ToString(); });
        DebugManager.Instance.AddMonitor(() => { return "velocity：" + cc.velocity; });
    }

    public void Jump(float force = 8)
    {
        float yOffset = force;
        Vector3 offset = new Vector3(0, yOffset, 0);
        //curDeltaPos = offset;
        GravityOffset = 0;
        ValueModifier<Vector3> mod = deltaPosValue.AddModifier(offset);
        IsJump = true;
    }

    /// <summary>
    /// 给定目标位置和所需时间进行位移
    /// </summary>
    /// <param name="posOffset">位移量</param>
    /// <param name="durationTime">位移时间</param>
    /// <param name="isOverride">是否覆盖位移</param>
    public void Move(Vector3 posOffset, float durationTime, bool isOverride = false, bool canCtrl = true)
    {
        //此处可以曲线控制 如先快后慢 先慢后快 之类的
        Vector3 deltaPos = posOffset / durationTime;
        //curDeltaPos += deltaPos;
        if (isOverride)
        {
            this.StopMove();
        }

        CanControl = canCtrl;

        if (posOffset.y > 0)
        {
            GravityOffset = 0;
            IsGravityEffect = false;
            //CanControl = false;
            TimeManager.GetInstance().AddOnceTimer(this, durationTime, () =>
            {
                IsGravityEffect = true;
                //CanControl = true;
            });
        }

        ValueModifier<Vector3> mod = deltaPosValue.AddModifier(deltaPos);
        TimeManager.GetInstance().AddOnceTimer(this, durationTime, () =>
        {
            deltaPosValue.RemoveModifier(mod);
            if (!canCtrl)
            {
                CanControl = true;
            }
        });
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        deltaPosValue.ClearAll();
    }
    /// <summary>
    /// 冻结移动
    /// </summary>
    public void FreezonMove(float maxTime)
    {
        GravityOffset = 0;
        IsGravityEffect = false;
        StopMove();
        TimeManager.GetInstance().AddOnceTimer(this, maxTime, () =>
        {
            IsGravityEffect = true;
        });

    }


    public void AtkFly(float height, float durationTime)
    {
        Move(Vector3.up * height, durationTime, true, false);
    }

    /// <summary>
    /// 跟随目标位移 按速度来 默认每秒位移1m
    /// </summary>
    /// <param name="speed"></param>
    public void MoveFollow(Character target, float speed = 1, Action endCall = null)
    {
        Action fun = null;
        fun = () =>
        {
            Vector3 dir = target.trans.position - character.trans.position;
            cc.Move(dir.normalized * speed * Time.deltaTime);
            if (Vector3.Distance(target.trans.position, character.trans.position) <= character.physic.cc.radius + target.physic.cc.radius + 0.2f || Math.Abs(cc.velocity.magnitude) <= 0.1f)
            {
                TimeManager.GetInstance().RemoveTimer(this, fun);
                endCall();
            }
        };

        TimeManager.GetInstance().AddLoopTimer(this, 0f, fun);
    }

    public void OnUpdate()
    {
        CheckIsGround();
        if (!isGround)
        {
            //在空中时

            //刚起跳
            if (lastIsGround == true)
            {

            }
            //受到重力影响
            if (IsGravityEffect)
            {
                GravityOffset -= gravity * Time.deltaTime * multiply;
            }

            if (deltaPosValue.finalValue.y >= GravityOffset)
            {
                //上升状态
            }
            else if (IsGravityEffect)
            {
                //下降状态
            }
        }
        else
        {
            //在地面上

            //刚落地
            if (lastIsGround == false)
            {
                if (IsJump)
                {
                    //取消跳跃状态
                    IsJump = false;
                    EventDispatcher.GetInstance().Event(EventDispatcher.PLAYER_JUMPED);
                }
                deltaPosValue = new VectorValue(Vector3.zero);
            }
            //重力偏移重置
            GravityOffset = 0;
        }


        Vector3 dir = Vector3.zero;
        //滞空移动
        if (CanControl && deltaPosValue.finalValue.magnitude > 0)
        {
            dir = GameContext.GetDirByInput(character, false) * 5f;
        }

        totalMove = (deltaPosValue.finalValue + new Vector3(0, GravityOffset, 0) + dir) * Time.deltaTime * multiply;
        cc.Move(totalMove);

    }


    private void CheckIsGround()
    {
        lastIsGround = isGround;
        var raycastAll = Physics.OverlapBox(character.trans.position, new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, 1 << LayerMask.NameToLayer("Build"));
        if (raycastAll.Length > 0)
        {
            // 在地面
            isGround = true;
        }
        else
        {
            // 离地
            isGround = false;
        }
    }
}