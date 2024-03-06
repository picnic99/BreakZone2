using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.Character;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skill.Base;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.State;
using StateSyncServer.LogicScripts.VirtualClient.States;
using System;
using UnityEngine;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills.GaiLun
{
    public class GaiLunAttack : BaseAttack
    {
        public string effectPath = "";

        public GaiLunAttack()
        {
            CanTriggerAgain = true;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (Character.GetSkill(SkillEnum.ZHIMINGDAJI) != null)
            {
                durationTime = 0;
                return;
            }
            int index = StageNum + 1;
            TimeManager.GetInstance().AddOnceTimer(this, skillData.GetFrontTime(StageNum), () =>
              {
                  var ins = new GaiLunAtkInstance(this, index, DoDamage);
                  instanceList.Add(ins);
                  AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "atk", character.playerId, character.InstanceId);
              });
        }

        public void DoDamage(Character target)
        {
            if (target.IsDestroyed) return;
            AddState(target, character, StateType.Injure);
            DoDamage(target, character.property.Atk);
            /*        AudioManager.GetInstance().Play("sword_damage1", false);
                    AudioManager.GetInstance().Play("sword_damage2", false);*/

            //顿帧
            character.characterAnimator.SetSpeed(0);
            TimeManager.GetInstance().AddOnceTimer(this, 0.02f, () =>
            {
                //character.anim.speed = 1;
                character.characterAnimator.SetSpeed(1);
            });
            var dir = character.trans.forward.normalized;
            dir.y = 0;
            target.physic.Move(dir.normalized * 1f, 0.2f);
            character.eventDispatcher.Event(CharacterEvent.ATK, new Character[] { target });
        }
    }

    class GaiLunAtkInstance : SkillInstance
    {
        public GameObject curAtk;

        Action<Character> call;
        public GaiLunAtkInstance(Skill skill, int index, Action<Character> call)
        {
            RootSkill = skill;
            instancePath = "GaiLunAtk";
            durationTime = 1f;
            maxTriggerTarget = 99;
            IsEndRemoveObj = false;
            this.call = call;
            instanceObj = ResourceManager.GetInstance().GetSkillInstance(instancePath);
            instanceObj.transform.SetParent(skill.character.trans);
            curAtk = instanceObj.transform.Find("atk" + index).gameObject;
            curAtk.SetActive(true);
            Init();
            CameraManager.GetInstance().HorizontalShake(0.05f);
        }

        public override void SetCollider(string layerName, CharacterState TriggerType)
        {
            collider = curAtk.transform.Find("collider").GetComponent<ColliderHelper>();
            SetTriggerInfo(layerName, TriggerType);
            collider.OnTriggerEnterCall += OnEnterTrigger;
        }

        public override void OnEnterTrigger(Collider col)
        {
            base.OnEnterTrigger(col);
            Vector3 v = col.ClosestPointOnBounds(RootSkill.character.GetWeapon().transform.position);
            var bloodEffect = ResourceManager.GetInstance().GetEffectInstance("Common/BloodEffect");
            bloodEffect.transform.position = v;
            TimeManager.GetInstance().AddOnceTimer(this, 0.5f, () =>
            {
                GameObject.Destroy(bloodEffect);
            });
        }

        public override void InvokeEnterTrigger(Character target)
        {
            call.Invoke(target);
            //特效顿帧
            var lzEffect = curAtk.GetComponentsInChildren<ParticleSystem>();
            foreach (var item in lzEffect)
            {
                item.Pause();
            }
            TimeManager.GetInstance().AddOnceTimer(this, 0.02f, () =>
            {
                foreach (var item in lzEffect)
                {
                    item.Play();
                }
            });
        }

        public override void InitTransform()
        {
            if (instanceObj == null || RootSkill.character.trans == null) return;
            instanceObj.transform.forward = RootSkill.character.trans.forward;
            instanceObj.transform.position = RootSkill.character.trans.position + RootSkill.character.trans.forward * 1f;
        }

        public override void AddBehaviour()
        {
            TimeManager.GetInstance().AddLoopTimer(this, 0.02f, () =>
            {
                if (durationTime <= 0)
                {
                    End();
                    return;
                }
                durationTime -= 0.02f;
            });
        }
    }
}