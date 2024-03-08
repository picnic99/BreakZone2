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
    /// 角色管理器
    /// 
    /// </summary>
    public class Character: Instance
    {
        public int playerId;

        public GameInstance instance;
        /// <summary>
        /// 基础信息
        /// </summary>
        public CharacterBaseInfo baseInfo { get; set; }
        /// <summary>
        /// 阵营
        /// </summary>
        public CharacterState state = CharacterState.NEUTRAL;

        //记录角色属性数值
        public Property property { get; set; }
        //物理控制器
        public PhysicController physic { get; set; }

        public Transform Trans => instance.trans;
        public int InstanceId => instance.InstanceId;
        //事件管理器
        public EventDispatcher eventDispatcher;
        //状态机 只负责角色状态之间的切换
        public FSM fsm { get; set; }
        //输入管理器
        public InputManager input { get; set; }

        //存储角色当前正在执行的Skill 有些脱手技能 不一定有那么快结束掉
        public List<Skill> SkillBehaviour;
        //存储角色当前正在执行的BUFF
        public List<Buff> BuffBehaviour;
        //角色的基础数据 如名字 技能id 简介 XXX的
        public CharacterVO characterData;
        //TODO（不一定需要） 保持移动 考虑是比如盖伦施法E技能旋转时，假如我是ROOTMOTION的动画那么不能靠动画来改变角色 希望此时可以由程序继续控制玩家移动
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
        /// 初始化容器
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
        /// 数据初始化
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
        /// 应用玩家的操作
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
        /// 是否已经死亡
        /// </summary>
        private bool hadDie = false;
        /// <summary>
        /// 每帧执行
        /// </summary>
        public void Tick()
        {
            physic?.OnUpdate();
            fsm?.OnUpdate();
            //是否死亡
            if (property.IsDie())
            {
                if (!hadDie)
                {
                    hadDie = true;
                    CharacterManager.GetInstance().Event(CharacterManager.CHARACTER_DIE, this);
                }
                return;
            }

            //skill与buff更新
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

        //需要知道移除的原因 比如时间到了自动移除 或者被人眩晕打断移除 之类的
        public void RemoveSkillBehaviour(Skill skill)
        {
            SkillBehaviour.Remove(skill);
            skill.OnExit();
        }

        /// <summary>
        /// 是否包含技能
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
            //清理技能
            for (int i = 0; i < SkillBehaviour.Count; i++)
            {
                var item = SkillBehaviour[i];
                item.OnDestroy();
            }
            this.SkillBehaviour.Clear();
            this.SkillBehaviour = null;
            //清理BUFF
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