
using UnityEngine;
using Assets.LogicScripts.Client.Net.PB;
using Assets.LogicScripts.Client.Net.PB.Enum;
using Msg;
using Assets.LogicScripts.Utils;
using Assets.LogicScripts.Client.Entity;

namespace Assets.LogicScripts.Client.Manager
{
    /// <summary>
    /// 行为转发器
    /// </summary>
    class ActionManager : Manager<ActionManager>
    {

        public override void AddEventListener()
        {
            base.AddEventListener();
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_PLAYER_LOGIN_REP, LoginRepHandle);
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF, HandleBaseInfoNtf);
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_SELECT_ROLE_REP, SelectCrtHandle);
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_ENTER_SCENE_REP, EnterSceneHandle);
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_GAME_PLAYER_OPT_REP, HandlePlayerOptRep);
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_GAME_PLAYER_INPUT_NTF, HandlePlayerInput);
            EventDispatcher.GetInstance().On(ProtocolId.CLENT_SYNC_GAME_DATA_NTF, HandleSyncGameData);
            EventDispatcher.GetInstance().On(ProtocolId.CLIENT_ANIM_PLAY_NTF, HandleAnimPlay);
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_PLAYER_LOGIN_REP, LoginRepHandle);
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF, HandleBaseInfoNtf);
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_SELECT_ROLE_REP, SelectCrtHandle);
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_ENTER_SCENE_REP, EnterSceneHandle);
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_GAME_PLAYER_OPT_REP, HandlePlayerOptRep);
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_GAME_PLAYER_INPUT_NTF, HandlePlayerInput);
            EventDispatcher.GetInstance().Off(ProtocolId.CLENT_SYNC_GAME_DATA_NTF, HandleSyncGameData);
            EventDispatcher.GetInstance().Off(ProtocolId.CLIENT_ANIM_PLAY_NTF, HandleAnimPlay);

        }


        public void SendLoginReq(string username, string password, int type, int playerId = 0)
        {
            //入参校验

            var req = new PlayerLoginReq();
            req.Username = username;
            req.Password = password;
            req.Type = type;
            req.PlayerId = playerId;
            NetManager.GetInstance().SendProtocol(req);
        }

        public void SendSelectCrtReq(int crtId)
        {
            var req = new SelectCrtReq();
            req.CrtId = crtId;
            NetManager.GetInstance().SendProtocol(req);
        }

        public void SendEnterSceneReq(int sceneId = 1)
        {
            var req = new EnterSceneReq();
            req.SceneId = sceneId;
            NetManager.GetInstance().SendProtocol(req);
        }

        public void SelectCrtHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            SelectCrtRep rep = proto.GetDataInstance<SelectCrtRep>();

            var result = rep.Result;
            var crtId = rep.CrtId;
            if (result == ProtocolResultEnum.SUCCESS)
            {
                CommonUtils.Logout("选择角色" + crtId + "完毕");
                SendEnterSceneReq(PlayerManager.GetInstance().Self.lastStaySceneId);
            }
        }

        public void EnterSceneHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            EnterSceneRep rep = proto.GetDataInstance<EnterSceneRep>();

            var result = rep.Result;
            var SceneId = rep.SceneId;
            if (result == ProtocolResultEnum.SUCCESS)
            {
                CommonUtils.Logout("进入场景" + SceneId + "请求成功");
                GameSceneManager.Eventer.On(GameSceneManager.LOAD_SCENE, OnSceneEnterSuccess);
                //后续根据场景ID来定
                loadingScene = RegSceneClass.GameRoomScene;
                GameSceneManager.GetInstance().SwitchScene(loadingScene);
            }
        }
        private string loadingScene;
        public void OnSceneEnterSuccess(object[] args)
        {
            string sceneName = args[0] as string;
            if (sceneName == loadingScene)
            {
                //请求当前场景的所有游戏数据
                SendGetGameData();
            }
            GameSceneManager.Eventer.Off(GameSceneManager.LOAD_SCENE, OnSceneEnterSuccess);
        }

        public void SendGetGameData()
        {
            var req = new GetGameDataReq();
            NetManager.GetInstance().SendProtocol(req);
        }

        public void HandleGetGameData(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            GetGameDataRep rep = proto.GetDataInstance<GetGameDataRep>();
            if (rep.Result == ProtocolResultEnum.SUCCESS)
            {
                CommonUtils.Logout("数据同步请求成功!");
            }
        }

        public void HandleSyncGameData(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            SyncGameDataNtf ntf = proto.GetDataInstance<SyncGameDataNtf>();
            var CrtData = ntf.CrtData;

            /**
             * 同步原则
             * 1.服务器只同步实体的数据
             * 2.找到当前客户端的实体 若存在则同步数据 更新实体 若不存在 则因此实体
             */
            CharacterManager.GetInstance().HideAllCharacter();
            foreach (var item in CrtData)
            {
                var crt = CharacterManager.GetInstance().FindCharacter(item.PlayerId);

                if (crt == null)
                {
                    crt = CharacterManager.GetInstance().CreateCharacter(1, item.PlayerId);
                }

                crt.CrtData.Pos = new Vector3(item.PosX, item.PosY, item.PosZ);
                crt.CrtData.Rot = item.Rot;
                crt.SetActive(true);
                crt.ApplyCrtData();
            }

        }

        public void HandleAnimPlay(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GameAnimPlayNtf ntf = proto.GetDataInstance<GameAnimPlayNtf>();
            var PlayerId = ntf.PlayerId;
            var AnimName = ntf.AnimName;
            var crt = CharacterManager.GetInstance().FindCharacter(PlayerId);
            if (crt != null)
            {
                AnimManager.GetInstance().PlayAnim(crt.CharacterAnimator, AnimName);
            }
        }

        private void LoginRepHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            PlayerLoginRep rep = proto.GetDataInstance<PlayerLoginRep>();

            var result = rep.Result;
            var type = rep.Type;
            if (result == ProtocolResultEnum.SUCCESS)
            {
                if (type == LoginTypeEnum.LOGIN_IN)
                {
                    //登录
                    CommonUtils.Logout("登录成功!");
                }
                else if (type == LoginTypeEnum.LOGIN_Out)
                {
                    //退出登录
                    CommonUtils.Logout("登出成功!");
                    //清理数据
                    PlayerManager.GetInstance().ClearPlayer();
                    //返回到登录场景
                    GameSceneManager.GetInstance().SwitchScene(RegSceneClass.LoginScene);
                }
            }
            else
            {
                CommonUtils.Logout("PlayerLoginRep 操作失败");
            }
        }

        private void HandleBaseInfoNtf(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            PlayerBaseInfoNtf rep = proto.GetDataInstance<PlayerBaseInfoNtf>();
            var username = rep.Username;
            var playerId = rep.PlayerId;
            var lastStaySceneId = rep.LastStaySceneId;

            Vector3 lastStayPos = new Vector3(
                rep.LastStayPosX,
                rep.LastStayPosY,
                rep.LastStayPosZ);

            Player p = new Player();
            p.playerId = playerId;
            p.username = username;
            p.password = "";
            p.lastStayPos = lastStayPos;
            p.lastStaySceneId = lastStaySceneId;
            PlayerManager.GetInstance().Self = p;
            CommonUtils.Logout("玩家信息同步完成! 即将切换到选择角色");
            GameSceneManager.GetInstance().SwitchScene(RegSceneClass.SelectRoleScene);
        }


        public void SendPlayerInput(float x, float y)
        {
            if (x == 0 && y == 0) return;
            var req = new GamePlayerInputReq();
            req.InputX = x;
            req.InputY = y;
            //CommonUtils.Logout($"客户端:inputX = {x},inputY = {y}");
            NetManager.GetInstance().SendProtocol(req);
        }
        private void HandlePlayerInput(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GamePlayerInputNtf ntf = proto.GetDataInstance<GamePlayerInputNtf>();
            var PlayerId = ntf.PlayerId;
            var InputX = ntf.InputX;
            var InputY = ntf.InputY;
            var crt = CharacterManager.GetInstance().FindCharacter(PlayerId);
            if (crt != null)
            {
                crt.CrtData.Input = new Vector2(InputX, InputY);
            }
            //CommonUtils.Logout($"服务器:inputX = {InputX},inputY = {InputY}");
        }

        private void HandlePlayerOptRep(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GamePlayerOptRep rep = proto.GetDataInstance<GamePlayerOptRep>();
            if (rep.Result == ProtocolResultEnum.SUCCESS)
            {
                CommonUtils.Logout("玩家操作信息发送成功");
            }
        }




        /*        public void Handle(Protocol protocol)
                {
                    switch (protocol.protocolId)
                    {
                        case ProtocolId.CLIENT_PLAYER_LOGIN_REP:
                            Debug.Log("收到角色登录返回");
                            Handle_LoginRep(protocol);
                            break;
                        case ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF:
                            Debug.Log("收到玩家的基础信息");
                            Handle_PlayerBaseInfoNtf(protocol);
                            break;
                        case ProtocolId.CLIENT_SELECT_ROLE_REP:
                            Debug.Log("选择角色返回");

                            break;
                        case ProtocolId.CLIENT_ENTER_SCENE_REP:
                            Debug.Log("进入场景返回");
                            break;
                        case ProtocolId.CLENT_GET_GAME_DATA_REP:
                            Debug.Log("获取游戏数据返回");
                            break;
                        case ProtocolId.CLENT_SYNC_GAME_DATA_NTF:
                            Debug.Log("游戏数据同步通知");
                            break;
                        case ProtocolId.CLIENT_GAME_PLAYER_OPT_REP:
                            Debug.Log("玩家操作返回");
                            break;
                        case ProtocolId.CLIENT_OBJ_CREATE_NTF:
                            Debug.Log("物体创建");
                            break;
                        case ProtocolId.CLIENT_OBJ_TRANSFORM_NTF:
                            Debug.Log("物体变化");
                            break;
                        case ProtocolId.CLIENT_ANIM_PLAY_NTF:
                            Debug.Log("动画播放");
                            break;
                        case ProtocolId.CLIENT_AUDIO_PLAY_NTF:
                            Debug.Log("音频播放");
                            break;
                        case ProtocolId.CLIENT_PLAYER_PROPERTY_CHANGE_NTF:
                            Debug.Log("玩家属性发送改变");
                            break;
                        default:
                            break;
                    }
                }

                public void Send_LoginReq(string username,string password)
                {
                    if (!String.IsNullOrEmpty(username) || !String.IsNullOrEmpty(password))
                    {
                        //提示
                    }
                    var req = new PlayerLoginReq();
                    req.username = username;
                    req.password = password;
                    NetManager.GetInstance().SendProtocol(req);
                }
                private void Handle_LoginRep(Protocol proto)
                {
                    PlayerLoginRep action = proto as PlayerLoginRep;
                    int result = action.result;
                    int type = action.type;
                    if (result == (int)ProtocolResultEnum.SUCCESS)
                    {
                        if (type == 0)
                        {
                            //退出登录
                            //清理数据
                            //返回到登录场景
                        }
                        else if (type == 1)
                        {
                            //登录
                            //写入数据
                            //进入到选择角色场景
                        }
                    }
                }*/

        private void Handle_PlayerBaseInfoNtf(Protocol proto)
        {
            /*            PlayerBaseInfoNtf action = proto as PlayerBaseInfoNtf;
                        var baseInfo = action.baseInfo;
                        if (baseInfo != null)
                        {
                            Common.GameContext.playerBaseInfo = baseInfo;
                        }*/
        }
    }
}