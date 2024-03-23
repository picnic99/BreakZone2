using Assets.LogicScripts.Client.Entity;
using Assets.LogicScripts.Client.Manager;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.CSkill
{
    abstract class CSkillBase
    {
        protected int PlayerId;
        public Player Player => PlayerManager.GetInstance().FindPlayer(PlayerId);
        protected GameObject CrtObj => Player.Crt.CrtObj;

        protected int skillId;

        protected int stageNum;

        public virtual void Init(SkillDataInfo info)
        {
            PlayerId = info.PlayerId;
            skillId = info.SkillId;
            stageNum = info.StageNum;
        }

        public abstract void DoBehaviour();
    }
}
