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
        //skill存在时间
        stateDurationTime = 1f;
        skillDurationTime = 2f;
    }

    public override void OnEnter()
    {
        if(StageNum == 0)
        {
            PlayAnim(skillData.GetAnimKey(0));
        }
        base.OnEnter();
    }
    public override void OnTrigger()
    {
        base.OnTrigger();
        if (StageNum == 1)
        {
            //创建技能实体
            new TianYingBo(this, character, BeTrigger1);
        }
        else if (StageNum == 2)
        {
            //第二段为族弟啊触发
            CanTriggerAgain = false;
            new HuiYinJi(this, character, target, BeTrigger2);
        }
    }

    public override void OnBack()
    {
        base.OnBack();
        //该技能需要特殊处理 或者后续统一处理掉
        //为什么这么写？ 1、需求是后摇时可以通过其它状态打断 最好能支持配置
        //2、改技能需要留有时间处理后续内容，如球飞的过程击中敌人以及二段踢等 按理来说 该技能虽然脱手 状态也消失了 但技能任然需要存在
        EndState();
    }

    /// <summary>
    /// 第一段触发
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
    /// 第二段触发
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

class TianYingBo
{
    Skill skill;
    Character character;
    GameObject skillInstance;
    Transform triggerEffect;
    ColliderHelper collider;
    Action<Character[]> call;
    string path = "Skill/TianYinBo";
    float skillDurationTime = 1f;
    bool Triggered = false;
    public TianYingBo(Skill skill, Character character, Action<Character[]> call)
    {
        this.skill = skill;
        this.character = character;
        this.call = call;
        skillInstance = ResourceManager.GetInstance().GetObjInstance<GameObject>(path);
        this.InitTransform();
        this.AddBehaviour();
    }

    private void InitTransform()
    {
        skillInstance.transform.forward = CameraManager.GetInstance().state == CameraState.ARM ? Camera.main.transform.forward : character.trans.forward; //CameraManager.GetInstance().curCam.transform.forward;//  character.trans.forward;
        skillInstance.transform.position = character.trans.position;
        triggerEffect = skillInstance.transform.Find("triggerEffect");
        collider = skillInstance.GetComponent<ColliderHelper>();
    }

    private void AddBehaviour()
    {
        collider.SetInfo();
        //碰撞到角色时的回调
        collider.OnTriggerEnterCall += DoTrigger;

        //飞行道具位移
        TimeManager.GetInstance().AddFrameLoopTimer(this, 0, skillDurationTime,
        () =>
        {
            skillInstance.transform.position += skillInstance.transform.forward * Time.deltaTime * 20f;
        },
        () =>
        {
            if (!Triggered)
            {
                End();
            }
        });
    }

    private void DoTrigger(Collider col)
    {
        var target = GameContext.GetCharacterByObj(col.gameObject);
        if (target == null) return;
        if (target.state != collider.info.TriggerType)
        {
            return;
        }

        Triggered = true;
        collider.OnTriggerEnterCall -= DoTrigger;
        End();

        //通知技能击中
        call.Invoke(new Character[] { target });

        //展示击中特效
        triggerEffectObj = GameObject.Instantiate<GameObject>(triggerEffect.gameObject, col.transform);
        triggerEffectObj.SetActive(true);

        skill.EventDispatcher.On(TianYingBoSkill.HUIYINJI_TRIGGERED, RemoveTriggerEffect);
        TimeManager.GetInstance().AddOnceTimer(triggerEffectObj, skill.skillDurationTime, () =>
        {
            if (triggerEffectObj != null)
            {
                RemoveTriggerEffect(null);
            }
        });
        skillDurationTime = 0;
    }

    private GameObject triggerEffectObj;
    private void RemoveTriggerEffect(object[] args)
    {
        GameObject.Destroy(triggerEffectObj);
        skill.EventDispatcher.Off(TianYingBoSkill.HUIYINJI_TRIGGERED, RemoveTriggerEffect);
    }

    private void End()
    {
        TimeManager.GetInstance().RemoveAllTimer(this);
        GameObject.Destroy(skillInstance);
    }
}

class HuiYinJi
{
    Skill skill;
    Character character;
    Character target;
    Action call;
    //string path = "Skill/ChaoFuHe";
    float skillDurationTime = 3f;
    public HuiYinJi(Skill skill, Character character, Character target, Action call)
    {
        this.skill = skill;
        this.character = character;
        this.target = target;
        this.call = call;
        this.InitTransform();
        this.AddBehaviour();
    }

    private void InitTransform()
    {

    }

    private void AddBehaviour()
    {
        character.trans.forward = target.trans.position - character.trans.position;
        character.canRotate = false;
        CameraManager.GetInstance().ShowFeatureCam(character);
        character.physic.MoveFollow(target, 20f, DoTrigger);
        skill.PlayAnim(skill.skillData.GetAnimKey(1), 0.7f);
    }

    bool triggered = false;
    private void DoTrigger()
    {
        if (triggered) return;
        skill.EventDispatcher.Event(TianYingBoSkill.HUIYINJI_TRIGGERED);
        call.Invoke();
        skillDurationTime = 0;
        triggered = true;
        character.canRotate = true;
        CameraManager.GetInstance().ShowMainCam(character);
        TimeManager.GetInstance().RemoveAllTimer(this);
        skill.OnBack();
    }
}