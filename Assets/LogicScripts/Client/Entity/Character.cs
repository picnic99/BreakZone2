using Assets.LogicScripts.Client.Manager;
using Assets.LogicScripts.Client.VO;
using UnityEngine;

namespace Assets.LogicScripts.Client.Entity
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class Character : EventDispatcher
    {
        public static string JUMP_END = "JUMP_END";

        /// <summary>
        /// 具体控制的角色
        /// </summary>
        public GameObject CrtObj { get; set; }

        private Binding bind;
        /// <summary>
        /// 角色的动画状态机
        /// </summary>
        public CharacterAnimator CharacterAnimator { get; set; }
        /// <summary>
        /// 角色的数据
        /// </summary>
        public CharacterDataVO CrtData { get; set; }

        public PhysicController physic { get; set; }

        public CharacterVO CrtVO { get; set; }

        public bool IsSelf { get; set; }

        public int playerId;

        public Character(int CrtId, int playerId)
        {
            if (CrtId > 0)
            {
                var vo = CharacterConfiger.GetInstance().GetCharacterById(CrtId);
                CrtVO = vo;
                this.playerId = playerId;
                CrtObj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.character.ModePath);
                var anim = CrtObj.GetComponent<Animator>();
                CrtObj.TryGetComponent<Binding>(out bind);
                CharacterAnimator = new CharacterAnimator();
                CharacterAnimator.Init(anim, this);
                //初始化角色位置
                CrtObj.transform.position = Vector3.zero;
                CrtObj.transform.rotation = Quaternion.identity;
                CrtData = new CharacterDataVO();
                physic = new PhysicController(this);
                //初始化角色动画 默认播放Idle
                AnimManager.GetInstance().PlayAnim(CharacterAnimator, "DEFAULT_IDLE");
                MonoBridge.GetInstance().AddCall(OnUpdate);

                float t = 0.15f;
                TimeManager.GetInstance().AddLoopTimer(this, 0, () => {
                    if(t<= 0)
                    {
                        t = 0.15f;
                        ActionManager.GetInstance().Send_GamePlayerPosSyncReq(CrtObj.transform.position);
                    }
                    t -= Time.deltaTime;
                });
            }
        }

        public GameObject GetWeaponObj()
        {
            if (bind != null)
            {
                if (bind.weapons.Count > 0)
                {
                    return bind.weapons[0];
                }
            }
            return null;
        }

        public void SetActive(bool b)
        {
            if (CrtObj != null)
            {
                CrtObj.SetActive(b);
            }
        }


        public Vector3 targetPos = Vector3.zero;
        public float threshold = 0.1f;
        public bool InLerp = false;
        public void ApplyTransform(Vector3 pos, float rot)
        {
            //平滑位移
            targetPos = pos;

            /*            if (InLerp) InLerp = false;

                        if (Vector3.Distance(CrtObj.transform.position, targetPos) <= threshold)
                        {
                            CrtObj.transform.position = targetPos;
                        }
                        else
                        {
                            InLerp = true;
                        }*/
            CrtObj.transform.position = targetPos;
            CrtObj.transform.rotation = Quaternion.AngleAxis(rot, Vector3.up);
        }
        public void ApplyAnim()
        {

        }

        public void Clear()
        {
            MonoBridge.GetInstance().DestroyOBJ(CrtObj);
            CharacterAnimator.OnDestroy();
        }

        public void OnUpdate()
        {
/*            if (InLerp)
            {
                CrtObj.transform.position = Vector3.Lerp(CrtObj.transform.position, targetPos, 0.1f);
            }*/
        }

        public void CorrectPos(Vector3 pos)
        {
            CrtObj.transform.position = Vector3.Lerp(CrtObj.transform.position, pos, 0.2f);
        }
    }
}