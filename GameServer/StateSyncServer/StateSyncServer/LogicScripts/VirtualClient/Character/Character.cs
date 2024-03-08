using Msg;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Actions;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Bridge;
using StateSyncServer.LogicScripts.VirtualClient.Buffs;
using StateSyncServer.LogicScripts.VirtualClient.Buffs.newBuff;
using StateSyncServer.LogicScripts.VirtualClient.Enum;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using CharacterManager = StateSyncServer.LogicScripts.Manager.CharacterManager;
using EventDispatcher = StateSyncServer.LogicScripts.VirtualClient.Manager.EventDispatcher;

namespace StateSyncServer.LogicScripts.VirtualClient.Characters
{

    /// <summary>
    /// ��ɫ������
    /// 
    /// </summary>
    public class Character: Instance
    {
        public int playerId;

        public GameInstance instance;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public CharacterBaseInfo baseInfo { get; set; }
        /// <summary>
        /// ��Ӫ
        /// </summary>
        public CharacterState state = CharacterState.NEUTRAL;

        //��¼��ɫ������ֵ
        public Property property { get; set; }
        //���������
        public PhysicController physic { get; set; }

        public Transform Trans => instance.trans;
        public int InstanceId => instance.InstanceId;
        //�¼�������
        public EventDispatcher eventDispatcher;
        //״̬�� ֻ�����ɫ״̬֮����л�
        public FSM fsm { get; set; }
        //���������
        public InputManager input { get; set; }

        //�洢��ɫ��ǰ����ִ�е�Skill ��Щ���ּ��� ��һ������ô�������
        public List<Skill> SkillBehaviour;
        //�洢��ɫ��ǰ����ִ�е�BUFF
        public List<Buff> BuffBehaviour;
        //��ɫ�Ļ������� ������ ����id ��� XXX��
        public CharacterVO characterData;
        //TODO����һ����Ҫ�� �����ƶ� �����Ǳ������ʩ��E������תʱ����������ROOTMOTION�Ķ�����ô���ܿ��������ı��ɫ ϣ����ʱ�����ɳ��������������ƶ�
        public bool KeepMove;
        public bool canRotate = true;

        public AnimCoverData animCoverData;

        public LogicScripts.VO.Player player;

        public Character(CharacterVO vo,int playerId, CharacterBaseInfo baseInfo = null)
        {
            characterData = vo;
            this.playerId = playerId;
            player = PlayerManager.GetInstance().FindPlayer(playerId);
            this.baseInfo = baseInfo;
            if (baseInfo == null)
            {
                this.baseInfo = new CharacterBaseInfo();
            }

            instance = new GameInstance(this);

            InitCharacter();
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        public virtual void InitCharacter()
        {
            input = new InputManager(this);
            eventDispatcher = new EventDispatcher();
            SkillBehaviour = new List<Skill>();
            BuffBehaviour = new List<Buff>();
            property = new Property(this);
            physic = new PhysicController(this);
            fsm = new FSM(this);
            animCoverData = new AnimCoverData(this);
            if (characterData.id == 99) property.hp.AddExAddValue(999999);
            AddEventListener();
        }

        /// <summary>
        /// ���ݳ�ʼ��
        /// </summary>
        public void InitData()
        {
            //todo
            instance.trans.Position = Vector3.Zero;
            instance.trans.Rot = 0;
            instance.trans.Scale = Vector3.One;

        }

        public Vector2 GetPlayerInput()
        {
            if(player != null)
            {
                return player.input;
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Ӧ����ҵĲ���
        /// </summary>
        public void ApplyOpt(GamePlayerOptReq opt)
        {
            input.ApplyOpt(opt);
        }

        public void AddEventListener()
        {

        }

        public void RemoveEventListener()
        {

        }

        /// <summary>
        /// �Ƿ��Ѿ�����
        /// </summary>
        private bool hadDie = false;
        /// <summary>
        /// ÿִ֡��
        /// </summary>
        public void Tick()
        {
            physic?.OnUpdate();
            fsm?.OnUpdate();
            //�Ƿ�����
            if (property.IsDie())
            {
                if (!hadDie)
                {
                    hadDie = true;
                    CharacterManager.GetInstance().Event(CharacterManager.CHARACTER_DIE, this);
                }
                return;
            }

            //skill��buff����
            for (int i = 0; i < SkillBehaviour.Count; i++)
            {
                var skill = SkillBehaviour[i];
                skill.OnUpdate();
            }

            for (int i = 0; i < BuffBehaviour.Count; i++)
            {
                var buff = BuffBehaviour[i];
                buff.OnUpdate();
            }
        }

        public void AddSkillBehaviour(Skill skill)
        {
            SkillBehaviour.Add(skill);
            skill.OnEnter();
        }

        //��Ҫ֪���Ƴ���ԭ�� ����ʱ�䵽���Զ��Ƴ� ���߱���ѣ�δ���Ƴ� ֮���
        public void RemoveSkillBehaviour(Skill skill)
        {
            SkillBehaviour.Remove(skill);
            skill.OnExit();
        }

        /// <summary>
        /// �Ƿ��������
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public Skill GetSkill(int skillId)
        {
            for (int i = 0; i < SkillBehaviour.Count; i++)
            {
                var skill = SkillBehaviour[i];
                if (skill.skillData.Id == skillId) return skill;
            }
            return null;
        }

        public void AddBuffBehaviour(Buff buff)
        {
            BuffBehaviour.Add(buff);
            buff.OnEnter();
        }

        public void RemoveBuffBehaviour(Buff buff)
        {
            BuffBehaviour.Remove(buff);
            buff.OnExit();
        }

        public TagBuffComponent HasTagBuff(TagType tag)
        {
            for (int i = 0; i < BuffBehaviour.Count; i++)
            {
                var buff = BuffBehaviour[i];
                if (buff is BuffGroup)
                {
                    TagBuffComponent tagbuff = ((BuffGroup)buff).HasTagBuff(tag);
                    if (tagbuff != null)
                        return tagbuff;
                }
            }
            return null;
        }

        public void BuffAsk(BaseAction action)
        {
            for (int i = 0; i < BuffBehaviour.Count; i++)
            {
                var buff = BuffBehaviour[i];
                if (buff is BuffGroup)
                {
                    ((BuffGroup)buff).BuffAsk(action);
                }
            }
        }

        public void OnDestory()
        {
            RemoveEventListener();
            //EventDispatcher.GetInstance().Event(EventDispatcher.CHARACTER_DESTORY, this);
            fsm.OnDestory();
            fsm = null;
            physic.OnDestory();
            physic = null;
            //������
            for (int i = 0; i < SkillBehaviour.Count; i++)
            {
                var item = SkillBehaviour[i];
                item.OnDestroy();
            }
            this.SkillBehaviour.Clear();
            this.SkillBehaviour = null;
            //����BUFF
            for (int i = 0; i < BuffBehaviour.Count; i++)
            {
                var item = BuffBehaviour[i];
                item.OnDestroy();
            }
            this.BuffBehaviour.Clear();
            this.BuffBehaviour = null;
        }
    }
}