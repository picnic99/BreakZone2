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
    /// 角色管理器
    /// 
    /// </summary>
    public class Character : GameInstance
    {
        /// <summary>
        /// 基础信息
        /// </summary>
        public CharacterBaseInfo baseInfo { get; set; }
        /// <summary>
        /// 角色游戏中的数据
        /// </summary>
        public CharacterGameData GameData { get; private set; }
        /// <summary>
        /// 阵营
        /// </summary>
        public CharacterState state = CharacterState.NEUTRAL;
        /// <summary>
        /// 角色属性值
        /// </summary>
        public Property property { get; set; }
        /// <summary>
        /// 物理控制器
        /// </summary>
        public PhysicController physic { get; set; }
        /// <summary>
        /// 事件管理
        /// </summary>
        public _EventDispatcher eventDispatcher { get; set; }
        /// <summary>
        /// 状态机
        /// </summary>
        public FSM fsm { get; set; }
        /// <summary>
        /// 输入控制器
        /// </summary>
        public InputController input { get; set; }
        /// <summary>
        /// 存储角色当前正在执行的Skill 有些脱手技能 不一定有那么快结束掉
        /// </summary>
        public List<Skill> SkillBehaviour { get; set; }
        /// <summary>
        /// 存储角色当前正在执行的BUFF
        /// </summary>
        public List<Buff> BuffBehaviour { get; set; }
        /// <summary>
        /// 角色的基础数据 从配置中读取
        /// </summary>
        public CharacterVO characterData{ get; set; }
        //TODO（不一定需要） 保持移动 考虑是比如盖伦施法E技能旋转时，假如我是ROOTMOTION的动画那么不能靠动画来改变角色 希望此时可以由程序继续控制玩家移动
        public bool KeepMove { get; set; }
        public bool canRotate = true;
        /// <summary>
        /// 动画覆盖数据 考虑移动到动画管理器中
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
        /// 应用玩家的操作
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
        /// 初始化容器
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
        /// 数据初始化
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
        /// 是否已经死亡
        /// </summary>
        private bool hadDie = false;
        /// <summary>
        /// 每帧执行
        /// </summary>
        public void Tick()
        {
            //physic?.OnUpdate();
            fsm?.OnUpdate();
            base.Tick();
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