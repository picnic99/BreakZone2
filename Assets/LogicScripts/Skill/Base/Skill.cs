
using UnityEngine;

/// <summary>
/// ���ܵĻ���
/// ���Լ��ܱ���̳л���ʵ��
/// Ŀǰ�и��׵��ĸ�����ʾ��
///       OnUpdate
/// OnEnter > OnTrigger > OnBack> OnEnd > OnExit
/// 
/// ����˺��ḳ���Ӧ���ܻ�����
/// һ�����ܿ��ܻ���ɶ���˺� ÿ���˺���Ӧ��֧�����ò�ͬ���ܻ�����
/// 
/// </summary>
public abstract class Skill : Behaviour
{
    private EventDispatcher eventDispatcher;
    public EventDispatcher EventDispatcher {
        get
        {
            if (eventDispatcher == null) eventDispatcher = new EventDispatcher();
            return eventDispatcher;
        }
    }
    //�������� �����Ҫ��Ϣ ֧������
    public SkillVO skillData;
    //���ܳ���ʱ��
    public float skillDurationTime;
    //״̬����ʱ��
    public float stateDurationTime;
    //�Ƿ������ּ���
    //public bool isOutHandSkill = false;
    //ʩ����������
    protected bool releaseOver = false;
    //������Ϻ��Ƴ����ɵ�Buff
    protected bool IsExitRemoveBuff = false;
    //�����ܷ��ٴδ���
    public bool CanTriggerAgain = false;
    //��ǰ���ܽ׶�
    public int StageNum = 0;
    /// <summary>
    /// ����״̬ �˼���Ϊ״̬����
    /// </summary>
    public string belongState;
    //��ǰ��������״̬ ǰҡ ʩ�� ��ҡ
    public SkillInStateEnum skillState = SkillInStateEnum.FRONT;

    public float curAnimLength = 0;
    public Character Character
    {
        get { return character; }
        set
        {
            character = value;
            character.AddSkillBehaviour(this);
        }
    }

    /// <summary>
    /// ���ü���ʩ�Ž���
    /// �ô����˴�����
    /// </summary>
    protected virtual void SetReleaseOver()
    {
        releaseOver = true;
    }

    /// <summary>
    /// ����״̬����ʱ ֪ͨһ���Ƴ�״̬
    /// ���������Ҫ���ô˺��� 
    /// 1.���ܱ���ϵ���� ���ڴ󲿷ּ������ܹ�Ԥ��״̬�˳�ʱ���Ҳ���Ǽ���ʱ������ ״̬���Ǳ߻��Զ��Ƴ�״̬
    /// ���ֱ���ϵ���� ʱ��Ҫ֪ͨ״̬��
    /// 2.�޷�Ԥ���˳�ʱ��ļ��ܣ�������Ծ֮��� ��Ҫ�����˳�������֪ͨ״̬��
    /// </summary>
    protected virtual void EndState()
    {
        character.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.DoSkill);
    }

    /// <summary>
    /// �����Ƿ��Ƿ���ɣ�
    /// Ŀǰ�������������� һ���ǽ��뵽��ҡ״̬��һ���Ǽ���ʱ���ѵ�
    /// </summary>
    /// <returns></returns>
    public bool IsSkillReleaseOver()
    {
        return skillState == SkillInStateEnum.BACK || releaseOver;
    }

    public override void OnEnter()
    {
        // ˲ʱ����
        // �����������ʩ�Ų��˳�����״̬
        if (skillData.IsInstantSkill)
        {
            //ֱ�Ӵ��������߼�
            OnTrigger();
            //���̽����ҡ
            OnBack();
            //�׳��¼��Ƴ�����״̬
            EndState();
            return;
        }

        var cur_animName = skillData.GetAnimKeyBySkillIndex(StageNum);
        float animTime = AnimManager.GetInstance().GetAnimTime2(cur_animName);
        var animCfg = AnimConfiger.GetInstance().GetAnimByAnimKey(cur_animName);
        animTime = animTime * animCfg.animation.ValidLength;
        curAnimLength = animTime;
        if (animTime == 0) animTime = stateDurationTime;
        //ǰҡʱ�� ��ʱһ���ڲ���̧�ֶ��� ̧�ֶ�������ͨ��
        //��ʱ�Ȳ�����̧�ֶ����� ���ɣ�û�к��ʵ�ȡ��ʩ�Ű���
        //OnFront();   

        Debug.Log($"��ǰ����Ϊ��{skillData.skill.Name},ǰҡ��{skillData.GetFrontTime(StageNum)},��ҡ��{skillData.GetBackTime(StageNum)}");
        //���ܴ��� �Ѿ�����ʩ�ż�����
        TimeManager.GetInstance().AddOnceTimer(this, animTime * skillData.GetFrontTime(StageNum), () =>
        {
            OnTrigger();
        });

        //��ҡʱ�� ���ں�ҡʱ��Ļ�����ͨ���������ܴ�ϵ�ǰ��β���� ����ʩ����һ������ TODO��������Щ���ܿ��Դ����β����
        TimeManager.GetInstance().AddOnceTimer(this, animTime * skillData.GetBackTime(StageNum), () =>
        {
            OnBack();
        });
        
        //Ĭ��״̬ʱ����ڵ�ǰ�׶εĶ���ʱ��
        stateDurationTime = curAnimLength;
        //Ĭ�ϼ���ʱ�����״̬����ʱ��
        skillDurationTime = stateDurationTime;
    }

    public override void OnUpdate()
    {
        skillDurationTime -= Time.deltaTime;
        Debug.Log($"��ǰ����Ϊ��{skillData.skill.Name},ʣ��ʱ�䣺{skillDurationTime}");
        if (skillDurationTime <= 0)
        {
            OnEnd();
        }
    }

    /// <summary>
    /// ���ܱ����ʱ�Ĵ���
    /// </summary>
    public virtual void ForceStop()
    {
        OnEnd();
    }

    protected override void OnEnd()
    {

        SetReleaseOver();
        //���ܽ���ʱ �Ƴ�����ɫ������
        EndState();
        character.RemoveSkillBehaviour(this);
    }

    /// <summary>
    /// ���ܱ��Ƴ�
    /// </summary>
    public override void OnExit()
    {
        if (!releaseOver)
        {
            SetReleaseOver();
        }
        if (IsExitRemoveBuff)
        {
            BuffManager.GetInstance().RemoveAllBuffFromSkill(character, this);
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void OnTrigger()
    {
        skillState = SkillInStateEnum.TRIGGER;
        StageNum++;
    }
    /// <summary>
    /// ��ҡ
    /// </summary>
    public virtual void OnBack()
    {
        skillState = SkillInStateEnum.BACK;
    }

    public override string GetDesc()
    {
        return $"[{skillData.SkillName}]��ǰ����{skillState}״̬ ʣ��ʱ��{skillDurationTime};";
    }
}