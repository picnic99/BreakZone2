using DG.Tweening;
using System;
using UnityEngine;

/// <summary>
/// 天音波
/// 2段技能
/// 非瞬发技 1段施放法球 2段飞踢被法球名字的人
/// 目前存在问题：高度差问题，施放需要穿越物体？
/// 存在多次一段而无二段
/// </summary>
public class TianYingBoSkill : Skill
{
    public static string HUIYINJI_TRIGGERED = "TianYingBoSkill_HUIYINJI_TRIGGERED";

    private Character target;

    public TianYingBoSkill()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (StageNum == 0)
        {
            PlayAnim(skillData.GetAnimKey(0));
            stateDurationTime = curAnimLength;
            skillDurationTime = 3f;
        }else if (StageNum == 1)
        {
            PlayAnim(skillData.GetAnimKey(1),0.3f);
            stateDurationTime = 99f;
            skillDurationTime = 99f;
        }
    }
    public override void OnTrigger()
    {
        base.OnTrigger();
        if (StageNum == 1)
        {
            //创建技能实体
            new TianYingBo(this, BeTrigger1);
        }
        else if (StageNum == 2)
        {
            //第二段为族弟啊触发
            CanTriggerAgain = false;
            new HuiYinJi(this, target, BeTrigger2);
        }
    }

    public override void OnBack()
    {
        base.OnBack();
        //该技能需要特殊处理 或者后续统一处理掉
        //为什么这么写？ 1、需求是后摇时可以通过其它状态打断 最好能支持配置
        //2、改技能需要留有时间处理后续内容，如球飞的过程击中敌人以及二段踢等 按理来说 该技能虽然脱手 状态也消失了 但技能任然需要存在
        //EndState();
    }

    /// <summary>
    /// 第一段触发 检测目标
    /// </summary>
    /// <param name="target"></param>
    public void BeTrigger1(Character[] target)
    {
        DoDamage(target, 50);
        this.target = target[0];
        skillDurationTime = 3f;
        CanTriggerAgain = true;
    }

    /// <summary>
    /// 第二段触发 飞向目标
    /// </summary>
    /// <param name="target"></param>
    public void BeTrigger2()
    {
        DoDamage(this.target, 100);
        var dir = (character.trans.forward).normalized;
        dir.y = 0;
        //target.physic.MoveTo(dir * 0.06f, 0.2f);
        target.physic.Move(dir.normalized * 0.2f, 0.1f);
        skillDurationTime = 0f;
    }
}

class TianYingBo : SkillInstance
{
    Transform triggerEffect;
    Action<Character[]> call;

    public TianYingBo(Skill skill, Action<Character[]> call)
    {
        this.instancePath = "Skill/TianYinBo";
        this.RootSkill = skill;
        this.instanceObj = ResourceManager.GetInstance().GetObjInstance<GameObject>(instancePath);
        this.durationTime = 1f;
        this.call = call;

        this.Init();
    }

    public override void InitTransform()
    {
        //根据玩家是否瞄准 初始化飞行的方向
        this.instanceObj.transform.forward = CameraManager.GetInstance().state == CameraState.ARM ? Camera.main.transform.forward : this.RootSkill.character.trans.forward; //CameraManager.GetInstance().curCam.transform.forward;//  character.trans.forward;
        this.instanceObj.transform.position = this.RootSkill.character.trans.position;
        //击中后的特效
        triggerEffect = this.instanceObj.transform.Find("triggerEffect");
    }

    public override void AddBehaviour()
    {
        //飞行道具位移
        TimeManager.GetInstance().AddFrameLoopTimer(this, 0, durationTime,
        //每帧执行
        () =>
        {
            this.instanceObj.transform.position += this.instanceObj.transform.forward * Time.deltaTime * 20f;
        },
        //结束时回调
        () =>
        {
            End();
        });
    }

    public override void InvokeEnterTrigger(Character target)
    {
        //通知技能击中
        call.Invoke(new Character[] { target });

        //展示击中特效
        triggerEffectObj = GameObject.Instantiate<GameObject>(triggerEffect.gameObject, target.trans);
        triggerEffectObj.SetActive(true);

        this.RootSkill.EventDispatcher.On(TianYingBoSkill.HUIYINJI_TRIGGERED, RemoveTriggerEffect);
        TimeManager.GetInstance().AddOnceTimer(triggerEffectObj, this.RootSkill.skillDurationTime, () =>
        {
            if (triggerEffectObj != null)
            {
                RemoveTriggerEffect(null);
            }
        });
    }
    private GameObject triggerEffectObj;
    private void RemoveTriggerEffect(object[] args)
    {
        GameObject.Destroy(triggerEffectObj);
        this.RootSkill.EventDispatcher.Off(TianYingBoSkill.HUIYINJI_TRIGGERED, RemoveTriggerEffect);
    }

    public override void End()
    {
        this.RootSkill.EventDispatcher.Off(TianYingBoSkill.HUIYINJI_TRIGGERED, RemoveTriggerEffect);
        base.End();
    }
}

class HuiYinJi: SkillInstance
{
    Character target;
    Action call;

    public HuiYinJi(Skill skill,Character target, Action flyEndCall)
    {
        this.RootSkill = skill;
        this.target = target;
        this.call = flyEndCall;
        this.durationTime = 3f;
        this.maxTriggerTarget = 1;
        this.needTriggerCheck = false;

        this.Init();
    }

    public override void InitTransform()
    {

    }

    public override void AddBehaviour()
    {
        var character = RootSkill.character;
        character.trans.forward = target.trans.position - character.trans.position;
        character.canRotate = false;
        CameraManager.GetInstance().ShowFeatureCam(character);
        character.physic.MoveFollow(target, 20f, OnFlyEnd);
    }

    bool triggered = false;
    private void OnFlyEnd()
    {
        if (triggered) return;
        RootSkill.EventDispatcher.Event(TianYingBoSkill.HUIYINJI_TRIGGERED);
        call.Invoke();
        triggered = true;
        RootSkill.character.canRotate = true;
        CameraManager.GetInstance().ShowMainCam(RootSkill.character);
        End();
    }
}