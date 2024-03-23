using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.CSkill
{
    class CZhiMingDaJiSkill : CSkillBase
    {

        public override void DoBehaviour()
        {
            if(stageNum == 0)
            {
                ShowWeaponEffect();
            }
            else if(stageNum == 1)
            {
                ReleaseFlyWheel();
            }
        }

        public void ShowWeaponEffect()
        {
            var obj = ResourceManager.GetInstance().GetSkillInstance("ZhiMingDaJi_XuLi");
            //AudioManager.GetInstance().Play("sword_power", false);
            //AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "weaponFillPower");
            obj.transform.parent = Player.Crt.GetWeaponObj().transform;
            obj.transform.localPosition = new Vector3(-0.084F, 0, 0.033F);
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = new Vector3(0.05F, 0.05F, 0.05F);
        }

        public void ReleaseFlyWheel()
        {
            AnimManager.GetInstance().PlayAnim(Player.Crt.CharacterAnimator, "GAILUN_SWORD_HARD_ATK");

            CreateWheel(0);
            CreateWheel(15);
            CreateWheel(-15);

        }

        public void CreateWheel(int rot)
        {
            var instanceObj = ResourceManager.GetInstance().GetSkillInstance("ZhiMingDaJi");

            instanceObj.transform.position = CrtObj.transform.position + CrtObj.transform.forward * 1f;
            instanceObj.transform.forward = CrtObj.transform.forward;
            instanceObj.transform.RotateAround(instanceObj.transform.position, Vector3.up, rot);
            instanceObj.SetActive(false);

            var durationTime = 2f;
            bool isBack = false;

            Action moveCall = null;
            moveCall = () =>
            {
                if (!instanceObj.activeSelf) instanceObj.SetActive(true);

                if (durationTime <= 0)
                {
                    if (isBack && Vector3.Distance(instanceObj.transform.position, CrtObj.transform.position) <= 0.5f)
                    {
                        TimeManager.GetInstance().RemoveTimer(instanceObj, moveCall);
                        MonoBridge.GetInstance().DestroyOBJ(instanceObj);
                        return;
                    }
                }
                if (durationTime <= 1)
                {
                    if (!isBack)
                    {
                        isBack = true;
                    }
                    instanceObj.transform.position -= instanceObj.transform.forward * Time.deltaTime * 30f;
                    var scale = instanceObj.transform.localScale - Vector3.one * Time.deltaTime * 3f;
                    instanceObj.transform.localScale = scale.x < 0.2f ? new Vector3(0.2f, 0.2f, 0.2f) : scale;
                    instanceObj.transform.forward = -(CrtObj.transform.position - instanceObj.transform.position);
                }
                else
                {
                    instanceObj.transform.position += instanceObj.transform.forward * Time.deltaTime * 30f;
                    instanceObj.transform.localScale += Vector3.one * Time.deltaTime * 3f;
                }

                durationTime -= Time.deltaTime;
            };

            TimeManager.GetInstance().AddLoopTimer(instanceObj, 0.25f, moveCall);
        }
    }
}
