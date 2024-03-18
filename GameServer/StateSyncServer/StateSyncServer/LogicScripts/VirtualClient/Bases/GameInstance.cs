using Msg;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Bridge;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Bases
{
    /// <summary>
    /// 对应游戏中的一个实例
    /// </summary>
    public class GameInstance
    {
        public Instance parent { get; set; }
        /// <summary>
        /// 实例的唯一ID
        /// </summary>
        public int InstanceId { get; set; }
        /// <summary>
        /// 对应的预制ID
        /// </summary>
        public int PrefabId { get; set; }

        public string PrefabName { get; set; }
        /// <summary>
        /// 实例的类型
        /// </summary>
        private int InstanceType { get; set; }
        /// <summary>
        /// 变换组件
        /// </summary>
        public Transform trans { get; set; }
        /// <summary>
        /// 动画组件
        /// </summary>
        public Animator anim { get; set; }
        /// <summary>
        /// 有物体进入范围时调用
        /// </summary>
        public Action<GameInstance[]> enterCall { get; set; }
        /// <summary>
        /// 有物体停留时调用
        /// </summary>
        public Action<GameInstance[]> stayCall { get; set; }
        /// <summary>
        /// 有物体离开范围时调用
        /// </summary>
        public Action<GameInstance[]> exitCall { get; set; }
        /// <summary>
        /// 检测器
        /// </summary>
        public Collider col { get; set; }
        /// <summary>
        /// 是否可视
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        private GameInstaneInfo info;


        public GameInstance(Instance obj)
        {
            parent = obj;

            trans = new Transform((Character)obj);
            anim = new Animator((Character)obj);
            info = new GameInstaneInfo();

            SendCreateAction();
        }

        public GameInstaneInfo GetGameInstaneInfo()
        {
            info.PlayerId = parent.PlayerId;
            info.InstanceId = 1;
            info.Parent = 1; //等于其它的实例ID
            info.FollowType = "Root"; // 自定义其它参数 只要客户端能够识别即可
            info.Offset = new Vec3() { X = 0, Y = 0, Z = 0 };
            info.Rot = 0;
            info.PrefabKey = "Skill/GaiLunAtk";
            info.StageNum = 1;
            info.DurationTime = 2;
            info.IsAutoDestroy = true;
            return info;
        }

        public int GetInstanceType()
        {
            if(parent is Character)
            {
                return InstanceTypeEnum.CHARACTER;
            }
            return 0;
        }

        public void SetInstanceType(int type)
        {
            this.InstanceType = type;
        }

        public void SetCollider(Collider col)
        {
            this.col = col;
/*            this.col.enterCall += OnTriggerEnter;
            this.col.stayCall += OnTriggerStay;
            this.col.exitCall += OnTriggerExit;*/
        }

        public void SetActive(bool b)
        {

        }

        public void Tick()
        {
            CheckCollider();
        }

        private void CheckCollider()
        {
            col.Check();
        }

        public void OnTriggerEnter(GameInstance[] targets)
        {
            enterCall(targets);
        }

        public void OnTriggerStay(GameInstance[] targets)
        {
            stayCall(targets);
        }
        public void OnTriggerExit(GameInstance[] targets)
        {
            exitCall(targets);
        }

        public void SendCreateAction()
        {
            ActionManager.GetInstance().SendGameInstanceCreateNtf(GetGameInstaneInfo());
        }

    }
}
