using StateSyncServer.LogicScripts.Util;
using System;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills.Base
{
    public class RegSkillClass
    {
        public static RegSkillClass Instance;

        public Dictionary<int, Type> dic = new Dictionary<int, Type>();
        public static RegSkillClass GetInstance()
        {
            if (Instance == null)
            {
                Instance = new RegSkillClass();
                Instance.Init();
            }
            return Instance;
        }

        private void Init()
        {
            dic.Add(SkillEnum.BASE_ATK, typeof(BaseAttack));
            dic.Add(SkillEnum.BASE_JUMP, typeof(BaseJump));
            dic.Add(SkillEnum.BASE_ROLL, typeof(BaseRoll));

/*            dic.Add(SkillEnum.GAILUN_ATK, typeof(GaiLunAttack));
            dic.Add(SkillEnum.ZHIMINGDAJI, typeof(ZhiMingDaJiSkill));
            dic.Add(SkillEnum.YONGQI, typeof(YongQiSkill));
            dic.Add(SkillEnum.SHENPAN, typeof(ShenPanSkill));
            dic.Add(SkillEnum.ZHENGYI, typeof(ZhengYiSkill));


            dic.Add(SkillEnum.CHAOFUHE, typeof(ChaoFuHeSkill));
            dic.Add(SkillEnum.FUWENJINGGU, typeof(FuWenJingGuSkill));
            dic.Add(SkillEnum.FASHUYONGDONG, typeof(FaShuYongDongSkill));
            dic.Add(SkillEnum.QUJINZHEYUE, typeof(QuJinZheYueSkill));

            dic.Add(SkillEnum.LIQING_ATK, typeof(LiQingAttack));
            dic.Add(SkillEnum.TIANYINBO, typeof(TianYingBoSkill));
            dic.Add(SkillEnum.JINZHONGZHAO, typeof(JinZhongZhaoSkill));
            dic.Add(SkillEnum.CUIJINDUANGU, typeof(CuiJingDuanGuSkill));
            dic.Add(SkillEnum.SHENLONGBAIWEI, typeof(ShengLongBaiWeiSkill));*/

        }

        public Type GetType(int key)
        {
            if (!dic.ContainsKey(key))
            {
                CommonUtils.Logout("regClass not found" + key + ", pls check register");
                return null;
            }
            Type t = dic[key];
            return t;
        }
    }
}