using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Timers;

namespace StateSyncServer.LogicScripts.VirtualClient.Bridge
{
    public enum PhysicActionType
    {
        /// <summary>
        /// 叠加
        /// </summary>
        ADD,
        /// <summary>
        /// 覆盖
        /// </summary>
        OVERRIDE
    }

    /// <summary>
    /// 物理行动
    /// </summary>
    public class PhysicAction
    {
        /// <summary>
        /// 总位移
        /// </summary>
        public Vector3 totalOffset;
        /// <summary>
        /// 期望时间
        /// </summary>
        public float time;
        /// <summary>
        /// 物理行动类型
        /// </summary>
        public PhysicActionType type;

        private float endTime;

        public bool IsEnd
        {
            get
            {
                return true; // Time.time >= endTime;
            }
        }

        public Vector3 FrameOffset
        {
            get
            {
                return totalOffset / time;
            }

            private set { }
        }

        public PhysicAction(Vector3 totalOffset, float time, PhysicActionType type)
        {
            this.totalOffset = totalOffset;
            this.time = time;
            this.type = type;
            //endTime = Time.time + this.time;
        }
    }


    public class PhysicController
    {
        public Character character;
        //private Rigidbody rb;
        //public CharacterController cc;
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
        //private VectorValue deltaPosValue = new VectorValue(Vector3.zero);
        private Vector3 totalMove = Vector3.Zero;

        private List<PhysicAction> actionList = new List<PhysicAction>();

        //变换矩阵
        private Matrix4x4 TransformMatrix = Matrix4x4.Identity;

        public PhysicController(Character character)
        {
            this.character = character;
/*            cc = this.character.Trans.GetComponent<CharacterController>();
            cc.enabled = true;*/
        }

        /// <summary>
        /// 添加一个物理行为
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(PhysicAction action)
        {
            if (action.type == PhysicActionType.OVERRIDE)
            {
                actionList.Clear();
            }
            actionList.Add(action);
        }

        /// <summary>
        /// 检查物理行为
        /// </summary>
        public void CheckAction()
        {
            //已经结束的行为
            List<PhysicAction> dirtyActions = new List<PhysicAction>();

            for (int i = 0; i < actionList.Count; i++)
            {
                var item = actionList[i];
                if (item.IsEnd)
                {
                    dirtyActions.Add(item);
                }
            }

            //移除行为
            for (int j = 0; j < dirtyActions.Count; j++)
            {
                actionList.Remove(dirtyActions[j]);
            }
        }


        public void ClearAllAction()
        {
            actionList.Clear();
        }

        /// <summary>
        /// 移除物理行为的某个轴向的偏移
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void RemoveActionAxis(bool x, bool y, bool z)
        {
            foreach (var item in actionList)
            {
                if (x)
                {
                    item.totalOffset.X = 0;
                }
                else if (y)
                {
                    item.totalOffset.Y = 0;
                }
                else if (z)
                {
                    item.totalOffset.Z = 0;
                }
            }
        }

        /// <summary>
        /// 获取当前帧物理行为总偏移量
        /// </summary>
        /// <returns></returns>
        public Vector3 GetFrameAction()
        {
            CheckAction();
            Vector3 result = Vector3.Zero;
            foreach (var item in actionList)
            {
                result += item.FrameOffset;
            }
            return result;
        }

        /// <summary>
        /// 给定目标位置和所需时间进行位移
        /// </summary>
        /// <param name="posOffset">位移量</param>
        /// <param name="durationTime">位移时间</param>
        /// <param name="isOverride">是否覆盖位移</param>
        public void Move(Vector3 posOffset, float durationTime, bool isOverride = false, bool canCtrl = true)
        {
            CanControl = canCtrl;
            AddAction(new PhysicAction(posOffset, durationTime, PhysicActionType.ADD));
        }


        /// <summary>
        /// 跳跃
        /// </summary>
        /// <param name="force"></param>
        public void Jump(float force = 8)
        {
            Vector3 offset = new Vector3(0, force, 0);
            AddAction(new PhysicAction(offset, 1, PhysicActionType.ADD));
            GravityOffset = 0;
            IsJump = true;
        }

        /// <summary>
        /// 停止移动
        /// </summary>
        public void StopMove()
        {
            ClearAllAction();
        }
        /// <summary>
        /// 冻结移动
        /// </summary>
        public void FreezonMove(float maxTime)
        {
            StopMove();
            CanMove = false;
            TimeManager.GetInstance().AddOnceTimer(this, maxTime, () =>
            {
                GravityOffset = 0;
                CanMove = true;
            });

        }

        /// <summary>
        /// 击飞
        /// </summary>
        /// <param name="height"></param>
        /// <param name="durationTime"></param>
        public void AtkFly(float height, float durationTime)
        {
            //Move(Vector3.up * height, durationTime, true, false);
        }

        /// <summary>
        /// 击退
        /// </summary>
        public void AtkBack(Vector3 offset, float durationTime)
        {

        }

        /// <summary>
        /// 跟随目标位移 按速度来 默认每秒位移1m
        /// </summary>
        /// <param name="speed"></param>
        public void MoveFollow(Character target, float speed = 1, Action endCall = null)
        {
/*            Action fun = null;
            Timer t = null;
            fun = () =>
            {
                Vector3 dir = target.Trans.position - character.Trans.position;
                cc.Move(dir.normalized * speed * Time.deltaTime);
                if (Vector3.Distance(target.Trans.position, character.Trans.position) <= character.physic.cc.radius + target.physic.cc.radius + 0.2f || Math.Abs(cc.velocity.magnitude) <= 0.1f)
                {
                    TimeManager.GetInstance().RemoveTimer(this, t);
                    endCall();
                }
            };

            t = TimeManager.GetInstance().AddLoopTimer(this, fun, 0f,Global.FixedFrameTimeS);*/
        }

        public void OnUpdate()
        {
            return;
            Vector3 actionOffset = GetFrameAction();
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
                    GravityOffset -= gravity * Global.FixedFrameTimeS * multiply;
                }

                if (actionOffset.Y >= GravityOffset)
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
                    RemoveActionAxis(false, true, false);
                }
                //重力偏移重置
                //GravityOffset = 0;
            }


            if (!CanMove) return;

            Vector3 dir = Vector3.Zero;
            //滞空移动
            if (CanControl && !isGround && actionOffset.Length() > 0)
            {
                dir = GameContext.GetDirByInput(character, false) * 5f;
            }

            //当前帧总偏移 = （行为总偏移 + 重力偏移 + 滞空移动偏移） * 当前帧时间 * 倍数
            totalMove = (actionOffset + new Vector3(0, GravityOffset, 0) + dir) * Global.FixedFrameTimeS * multiply;

            //cc.Move(totalMove);

        }

        /// <summary>
        /// 检查是否在地面上
        /// </summary>
        private void CheckIsGround()
        {
/*            lastIsGround = isGround;
            var raycastAll = Physics.OverlapBox(character.Trans.position, new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, 1 << LayerMask.NameToLayer("Build"));
            if (raycastAll.Length > 0)
            {
                // 在地面
                isGround = true;
            }
            else
            {
                // 离地
                isGround = false;
            }*/
        }

        public void OnDestory()
        {

        }
    }
}