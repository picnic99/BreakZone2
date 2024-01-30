using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LimitTimePracticeUI : UIBase
{
    private TextMeshProUGUI timeLabel { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "timeLabel"); } }
    private GameObject startBtn { get { return UIBase.GetBind<GameObject>(Root, "startBtn"); } }
    private GameObject cancelBtn { get { return UIBase.GetBind<GameObject>(Root, "cancelBtn"); } }

    private float remainTime = 20;

    private Character testCrt;

    private bool isStart = false;
    private bool isEnd = false;

    public LimitTimePracticeUI()
    {
        uiPath = RegPrefabs.LimitTimePracticeUI;
        layer = UILayers.MIDDLE;
        //GameContext.IsCrtReceiveInput = false;
    }

    public override void OnLoad()
    {
        base.OnLoad();
        AddClick(startBtn, OnStartClick);
        AddClick(cancelBtn, OnCancelClick);
    }

    public void OnStartClick(object[] args)
    {
        //�ƶ���ɫ��ָ��λ������ָ������
        GameContext.CurRole.physic.Move(Vector3.zero - GameContext.CurRole.trans.position, 0.01f);
        if (testCrt != null) CharacterManager.GetInstance().RemoveCharacter(testCrt);
        testCrt = CharacterManager.GetInstance().CreateTestCharacter();
        testCrt.trans.parent = GameObject.Find("SceneRoot").transform;
        testCrt.physic.Move(testCrt.trans.position - new Vector3(0, 0, 5f), 0.01f);
        //����ʱ3/2/1 �ڼ�����޷��ٿ�
        int time = 3;
        TimeManager.GetInstance().AddTimeByDurationCtn(this, 4, 4, () =>
        {
            if (time == 0)
            {
                isStart = true;
                testCrt.state = CharacterState.ENEMY;
                startBtn.SetActive(false);
                cancelBtn.SetActive(false);
                return;
            }
            timeLabel.text = "" + time--;
        });
        //��ʼ�� 20�뵹��ʱ ��ʱ��¼20ʱ�� ��ɫ�ĸ�����ֵ
    }
    public void OnCancelClick(object[] args)
    {
        UIManager.GetInstance().CloseUI(RegUIClass.LimitTimePracticeUI);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isStart && !isEnd)
        {
            if (remainTime > 0)
            {
                timeLabel.text = "ʣ��ʱ��:" + remainTime.ToString("F2");
                remainTime -= Time.deltaTime;
            }
            else
            {
                isEnd = true;
                timeLabel.text = "����";
                TimeManager.GetInstance().AddOnceTimer(this, 1, () =>
                  {
                      remainTime = 20;

                      isStart = false;
                      isEnd = false;

                      startBtn.SetActive(true);
                      cancelBtn.SetActive(true);

                      //�������㵯��
                      timeLabel.text = "���˺�:" + (testCrt.property.hp.baseValue - testCrt.property.hp.finalValue);
                  });
            }

        }
    }

    public override void OnUnLoad()
    {
        base.OnUnLoad();
        if (testCrt != null) CharacterManager.GetInstance().RemoveCharacter(testCrt);
    }
}
