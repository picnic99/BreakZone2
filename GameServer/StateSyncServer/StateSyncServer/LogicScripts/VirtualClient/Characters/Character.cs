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
using StateSyncServer.LogicScripts.VO;
using System.Collections.Generic;
using System.Numerics;
using _EventDispatcher = StateSyncServer.LogicScripts.VirtualClient.Manager._EventDispatcher;

namespace StateSyncServer.LogicScripts.VirtualClient.Characters
{
    /// <summary>
    /// ��ɫ������
    /// 
    /// </summary>
    public class Character : GameInstance
    {
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public CharacterBaseInfo baseInfo { get; set; }
        /// <summary>
        /// ��ɫ��Ϸ�е�����
        /// </summary>
        public CharacterGameData GameData { get; private set; }
        /// <summary>
        /// ��Ӫ
        /// </summary>
        public CharacterState state = CharacterState.NEUTRAL;
        /// <summary>
        /// ��ɫ����ֵ
        /// </summary>
        public Property property { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        public PhysicController physic { get; set; }
        /// <summary>
        /// �¼�����
        /// </summary>
        public _EventDispatcher eventDispatcher { get; set; }
        /// <summary>
        /// ״̬��
        /// </summary>
        public FSM fsm { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        public InputController input { get; set; }
        /// <summary>
        /// �洢��ɫ��ǰ����ִ�е�Skill ��Щ���ּ��� ��һ������ô�������
        /// </summary>
        public List<Skill> SkillBehaviour { get; set; }
        /// <summary>
        /// �洢��ɫ��ǰ����ִ�е�BUFF
        /// </summary>
        public List<Buff> BuffBehaviour { get; set; }
        /// <summary>
        /// ��ɫ�Ļ������� �������ж�ȡ
        /// </summary>
        public CharacterVO characterData{ get; set; }
        //TODO����һ����Ҫ�� �����ƶ� �����Ǳ������ʩ��E������תʱ����������ROOTMOTION�Ķ�����ô���ܿ��������ı��ɫ ϣ����ʱ�����ɳ��������������ƶ�
        public bool KeepMove { get; set; }
        public bool canRotate = true;
        /// <summary>
        /// ������������ �����ƶ���������������
        /// </summary>
        public AnimCoverData animCoverData { get; set; }

        public Character(CharacterVO vo, int playerId, CharacterBaseInfo baseInfo = null)
        {
            characterData = vo;
            _playerId = playerId;
            this.baseInfo = baseInfo;
            if (baseInfo == null) this.baseInfo = new CharacterBaseInfo();
            this.GameData = new CharacterGameData();
            InitCharacter();
        }

        /// <summary>
        /// Ӧ����ҵĲ���
        /// </summary>
        public void ApplyOpt(GamePlayerOptCmdReq optCmd)
        {
            //todo
            input.ApplyOpt(optCmd);
        }

        public void ApplyInput(GamePlayerInputCmdReq inputCmd)
        {
            input.ApplyInput(inputCmd);
        }

        public void GetGameData()
        {
            this.GameData.PlayerId = PlayerId;
            this.GameData.CrtId = characterData.id;
            this.GameData.Rot = Trans.Rot;
            this.GameData.PosX = Trans.Position.X;
            this.GameData.PosY = Trans.Position.Y;
            this.GameData.PosZ = Trans.Position.Z;
            this.GameData.CurAnimKey = Anim.curAnimKey;
            this.GameData.AnimTime = Anim.animTime;
            //this.GameData.States =
            foreach (var item in fsm.myState.states)
            {
                this.GameData.States.Add(item.stateData.stateName);
            }
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        public virtual void InitCharacter()
        {
            input = new InputController(this);
            eventDispatcher = new _EventDispatcher();
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
            Trans.Position = Vector3.Zero;
            Trans.Rot = 0;
            Trans.Scale = Vector3.One;
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
            //physic?.OnUpdate();
            fsm?.OnUpdate();
            base.Tick();
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