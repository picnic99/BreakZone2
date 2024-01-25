using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace CustomPlayable
{
    public class AnimClipData
    {
        public string animName;
        public AnimNode able;

        public AnimClipData(string animName, AnimNode able)
        {
            this.animName = animName;
            this.able = able;
        }
    }

    public class AnimMixNode : AnimNode
    {
        protected AnimationMixerPlayable mixer;
        /// <summary>
        /// 记录动画数据  animKey - {inputIndex , animationClip}
        /// </summary>
        //private Dictionary<string, AnimClipData> datas = new Dictionary<string, AnimClipData>();

        protected Dictionary<int, AnimClipData> datas = new Dictionary<int, AnimClipData>();
        /// <summary>
        /// 待递减的下标 除开当前动画与目标动画下标  原切换动画未完成时 又切换到新动画时 将上一个未切换完成的动画下标存储此处
        /// 跟随帧递减 直到为0抛出
        /// </summary>
        public List<int> dels = new List<int>();

        /// <summary>
        /// 当前播放的动画下标
        /// </summary>
        public int curIndex = -1;
        /// <summary>
        /// 下一个要播放的动画下标
        /// </summary>
        public int targetIndex = -1;

        /// <summary>
        /// 剩余的切换时间
        /// </summary>
        public float translateTime = 0;

        /// <summary>
        /// 当前动画权重变化速率
        /// </summary>
        public float curSpeed;
        /// <summary>
        /// 待递减动画变化速率
        /// </summary>
        public float delSpeed;

        /// <summary>
        /// 当前是否在切换中
        /// </summary>
        public bool isTranlating;

        public AnimMixNode(PlayableGraph graph, playableAnimator animator) : base(graph, animator)
        {
            mixer = AnimationMixerPlayable.Create(graph);
            AddRootInput(mixer);
            if(GameContext.GameMode == GameMode.DEBUG)
            {
                DebugManager.Instance?.AddAnimMonitor(this);
            }
        }

        protected bool ExistAnim(string animName)
        {
            bool result = false;
            foreach (var item in datas)
            {
                if (item.Value.animName == animName)
                {
                    result = true;
                }
            }
            return result;
        }

        public override void Execute(Playable playable, FrameData info)
        {
            //节点未启用
            if (enable == false) return;

            if (!isTranlating) return;

            if (curIndex < 0 || targetIndex < 0) return;

            CheckWeight();
            //帧时长
            float deltaTime = info.deltaTime * rootAnimtor.Speed;

            //剩余切换时长大于0
            if (translateTime >= 0)
            {
                translateTime -= deltaTime;
                //待递减权重总和
                float delTotalWeight = 0;
                for (int i = 0; i < dels.Count; i++)
                {
                    var del = dels[i];

                    var weight = AddWeight(del, -delSpeed * deltaTime);

                    if (weight <= 0)
                    {
                        dels.Remove(del);
                        GetAnimClipData(del).able.Disable();
                    }
                    else
                    {
                        delTotalWeight += weight;
                    }
                }
                var curW = AddWeight(curIndex, -curSpeed * deltaTime);
                float tw = 1 - delTotalWeight - curW;
                SetWeight(targetIndex, tw);
                return;
            }

            GetAnimClipData(curIndex).able.Disable();
            //GetAnimClipData(targetIndex).able.Enable();
            curIndex = targetIndex;
            targetIndex = -1;
            isTranlating = false;

        }

        public void PlayAnim(string animName, float time, bool isPercent = true)
        {
            TranslateTo(animName, time, isPercent);
        }

        public void StopAnim(string animName, float time)
        {

        }

        public bool CheckWeight()
        {
            float totalW = 0;
            for (int i = 0; i < datas.Count; i++)
            {
                totalW += GetWeight(i);
            }
            if (totalW > 1)
            {
                Debug.LogError(this.GetType().Name + "总动画权重超过1 请检查！！！");
                return false;
            }
            return true;
        }

        public void ClearAllWeight()
        {
            foreach (var item in datas)
            {
                SetWeight(item.Key, 0);
            }
        }

        protected int GetAnimClipDataIndex(string animKey)
        {
            int index = -1;
            foreach (var item in datas)
            {
                if (item.Value.animName == animKey)
                {
                    index = item.Key;
                }
            }
            return index;
        }
        protected AnimClipData GetAnimClipData(int index)
        {
            AnimClipData data = null;
            if (datas.ContainsKey(index))
            {
                data = datas[index];
            }
            return data;
        }

         public virtual float[] EnableTargetAnim(string targetAnimName)
        {
            int tarIndex;
            float animLen;

            //查找目标动画对应的input下标 若没有则新建
            var data = GetAnimClipDataIndex(targetAnimName);

            if (data != -1)
            {
                tarIndex = data;
                var animData = GetAnimClipData(tarIndex);
                animLen = ((AnimClipNode)animData.able).GetClipPlayable().GetAnimationClip().length;
                animData.able.Enable();
            }
            else
            {
                //mixer.SetInputCount(mixer.GetInputCount() + 1);
                var clip = ResourceManager.GetInstance().GetAnimatinClip(targetAnimName);
                AnimClipNode able = new AnimClipNode(graph, rootAnimtor, clip);
                tarIndex = AddInput(able.GetPlayable());
                able.Enable();
                datas.Add(tarIndex, new AnimClipData(targetAnimName, able));
                animLen = clip.length;
            }

            return new float[] { tarIndex,animLen };
        }

        /// <summary>
        /// 切换动画
        /// </summary>
        /// <param name="targetAnimName">欲切换的动画</param>
        /// <param name="time">时间</param>
        /// <param name="isPercent">是否百分比</param>
        public virtual void TranslateTo(string targetAnimName, float time, bool isPercent = true)
        {
            int tarIndex = -1;
            float animLen = 0;
            Debug.Log("TranslateTo:" + targetAnimName);

            float[] data = EnableTargetAnim(targetAnimName);
            if (data != null && data.Length >= 2)
            {
                tarIndex = Convert.ToInt32(data[0]);
                animLen = data[1];
            }
            //若当前无播放的动画 则直接播放该动画 权重设为1
            if (curIndex == -1)
            {
                curIndex = tarIndex;
                GetAnimClipData(curIndex).able.Enable();
                SetWeight(curIndex, 1);
                return;
            }

            //若目标动画的权重为1 则无需转换
            if (mixer.GetInputWeight(tarIndex) == 1)
            {
                return;
            }

            //若当前真正转换中 且上一个目标动画不为此次转换的动画
            if (isTranlating && tarIndex != targetIndex)
            {
                //极限情况 在目标下标未当前下标时且权重大于上个目标权重有BUG
                //这种情况直接交换即可
                if(curIndex == tarIndex)
                {
                    curIndex = targetIndex;
                }else if (GetWeight(curIndex) > GetWeight(targetIndex))
                {
                    //当前动画权重 大于 目标动画权重
                    //将上一个目标动画存入待递减动画数组
                    dels.Add(targetIndex);
                }
                else
                {
                    //当前动画权重 小于 目标动画权重
                    //将当前动画存入待递减动画数组
                    //再将原目标动画设为当前动画
                    //该处理 可避免切换过于突兀
                    dels.Add(curIndex);
                    curIndex = targetIndex;
                }
            }

            //目标动画下标改变
            targetIndex = tarIndex;

            //若目标动画下标存在待递减动画数值中 则移除
            if (dels.IndexOf(targetIndex) != -1)
            {
                dels.Remove(targetIndex);
            }

            //计算切换时间
            if (isPercent)
            {
                //处理百分比情况
                float animLength = animLen;
                time = animLength * time;
            }

            //设置待切换时间
            translateTime = time;
            //计算当前动画权重改变速率 当前权重需要N秒内变为0
            curSpeed = GetWeight(curIndex) / time;
            //递减数组的动画权重改变速率 需要快于当前动画变化速率 此处快两倍 保证递减数组能先归0
            delSpeed = 2 * curSpeed;
            //递减中
            isTranlating = true;
        }


        public float GetWeight(int index)
        {
            if (index < 0) return 0;
            return mixer.GetInputWeight(index);
        }

        public float AddWeight(int index, float delta)
        {
            float w = GetWeight(index) + delta;
            w = Mathf.Clamp(w, 0, 1);
            mixer.SetInputWeight(index, w);
            return w;
        }

        public void SetWeight(int index, float w)
        {
            w = Mathf.Clamp(w, 0, 1);
            mixer.SetInputWeight(index, w);
        }

        public override int AddInput(Playable able)
        {
            mixer.AddInput(able, 0);
            return mixer.GetInputCount() - 1;
        }
    }
}