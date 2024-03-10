﻿using Assets.LogicScripts.Client.Common;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.Entity
{
    /// <summary>
    /// 玩家
    /// </summary>
    class Player
    {
        public int playerId;
        /// <summary>
        /// 上一次进入的场景
        /// </summary>
        public int lastStaySceneId = 1;
        public int SceneId => lastStaySceneId;
        /// <summary>
        /// 从上一次选择的角色
        /// </summary>
        public int lastSelectCrtId = 1;
        public int CrtId => lastSelectCrtId;
        /// <summary>
        /// 最后停留的退出
        /// </summary>
        public Vector3 lastStayPos = Vector3.zero;

        public GamePlayerBaseInfo info;

        public Transform Trans => _crt != null ? _crt.CrtObj.transform : null;

        private Character _crt;
        public Character Crt
        {
            get
            {
                if(_crt == null)
                {
                    if(playerId != 0 && CrtId != 0)
                    {
                        _crt = Assets.LogicScripts.Client.Manager.CharacterManager.GetInstance().CreateCharacter(CrtId, playerId);
                        if (playerId == Global.SelfPlayerId)
                        {
                            _crt.IsSelf = true;
                            CameraManager.GetInstance().SetTarget(_crt.CrtObj.transform);
                        }
                    }
                }
                return _crt;
            }
        }

        public void UpdateBaseInfo(GamePlayerBaseInfo info)
        {
            this.info = info;
            Crt.ApplyTransform(new Vector3(info.Pos.X,info.Pos.Y,info.Pos.Z),info.Rot);
        }

        public void Clear()
        {
            info = null;
            _crt.Clear();
            _crt = null;
        }
    }
}
