
using UnityEngine;
using Assets.LogicScripts.Client.Net.PB;
using Assets.LogicScripts.Client.Net.PB.Enum;
using Msg;
using Assets.LogicScripts.Utils;
using Assets.LogicScripts.Client.Entity;
using Assets.LogicScripts.Client.Common;
using System;

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
            On(ProtocolId.CLIENT_PLAYER_LOGIN_REP, Handle_PlayerLoginRep);
            On(ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF, HandleBaseInfoNtf);
            On(ProtocolId.CLIENT_SELECT_ROLE_REP, Handle_SelectCrtRep);
            On(ProtocolId.CLIENT_ENTER_SCENE_REP, Handle_EnterSceneRep);
            On(ProtocolId.CLIENT_GAME_PLAYER_OPT_REP, HandlePlayerOptRep);
            On(ProtocolId.CLIENT_GAME_PLAYER_INPUT_NTF, Handle_GamePlayerInputNtf);
            On(ProtocolId.CLENT_SYNC_GAME_DATA_NTF, HandleSyncGameData);
            On(ProtocolId.CLIENT_ANIM_PLAY_NTF, HandleAnimPlay);
            On(ProtocolId.CLIENT_GAME_PLAYER_ENTER_SCENE_NTF, Handle_GamePlayerEnterSeneNtf);
            On(ProtocolId.CLIENT_GAME_SYNC_AOI_PLAYER_NTF, Handle_GameSyncAOIPlayerNtf);

            On(ProtocolId.CLIENT_GAME_PLAYER_OPT_CMD_REP, Handle_GamePlayerOptCmdRep);
            On(ProtocolId.CLIENT_GAME_PLAYER_INPUT_CMD_NTF, Handle_GamePlayerInputCmdNtf);
            On(ProtocolId.CLIENT_GAME_SYNC_STATE_CHANGE_NTF, Handle_GameSyncStateChangeNtf);
            On(ProtocolId.CLIENT_CAN_ENTER_SCENE_REP, Handle_CanEnterSceneRep);
            On(ProtocolId.CLIENT_OBJ_CREATE_NTF, Handle_GameInstanceCreateNtf);

        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            Off(ProtocolId.CLIENT_PLAYER_LOGIN_REP, Handle_PlayerLoginRep);
            Off(ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF, HandleBaseInfoNtf);
            Off(ProtocolId.CLIENT_SELECT_ROLE_REP, Handle_SelectCrtRep);
            Off(ProtocolId.CLIENT_ENTER_SCENE_REP, Handle_EnterSceneRep);
            Off(ProtocolId.CLIENT_GAME_PLAYER_OPT_REP, HandlePlayerOptRep);
            Off(ProtocolId.CLIENT_GAME_PLAYER_INPUT_NTF, Handle_GamePlayerInputNtf);
            Off(ProtocolId.CLENT_SYNC_GAME_DATA_NTF, HandleSyncGameData);
            Off(ProtocolId.CLIENT_ANIM_PLAY_NTF, HandleAnimPlay);
            Off(ProtocolId.CLIENT_GAME_PLAYER_ENTER_SCENE_NTF, Handle_GamePlayerEnterSeneNtf);
            Off(ProtocolId.CLIENT_GAME_SYNC_AOI_PLAYER_NTF, Handle_GameSyncAOIPlayerNtf);

            Off(ProtocolId.CLIENT_GAME_PLAYER_OPT_CMD_REP, Handle_GamePlayerOptCmdRep);
            Off(ProtocolId.CLIENT_GAME_PLAYER_INPUT_CMD_NTF, Handle_GamePlayerInputCmdNtf);
            Off(ProtocolId.CLIENT_GAME_SYNC_STATE_CHANGE_NTF, Handle_GameSyncStateChangeNtf);
            Off(ProtocolId.CLIENT_CAN_ENTER_SCENE_REP, Handle_CanEnterSceneRep);
            Off(ProtocolId.CLIENT_OBJ_CREATE_NTF, Handle_GameInstanceCreateNtf);



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

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="args"></param>
        private void Handle_PlayerLoginRep(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            PlayerLoginRep rep = proto.GetDataInstance<PlayerLoginRep>();

            var result = rep.Result;
            var playerId = rep.PlayerId;
            var type = rep.Type;

            if (result == ProtocolResultEnum.SUCCESS)
            {
                if (type == LoginTypeEnum.LOGIN_IN)
                {
                    //登录
                    CommonUtils.Logout("登录成功!");
                    //设置当前客户端的玩家ID
                    Global.SelfPlayerId = playerId;
                    //加入玩家到管理类
                    Player player = PlayerManager.GetInstance().AddPlayer(playerId);
                    //设置为Self
                    PlayerManager.GetInstance().Self = player;
                    //切换到选择角色场景中
                    GameSceneManager.GetInstance().SwitchScene(RegSceneClass.SelectRoleScene);
                }
                else if (type == LoginTypeEnum.LOGIN_Out)
                {
                    //退出登录
                    CommonUtils.Logout("登出成功!");
                    //将本地清理数据
                    Global.SelfPlayerId = 0;
                    PlayerManager.GetInstance().Self = null;
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

        public void SendSelectCrtReq(int crtId)
        {
            var req = new SelectCrtReq();
            req.CrtId = crtId;
            NetManager.GetInstance().SendProtocol(req);
        }

        /// <summary>
        /// 选择角色
        /// </summary>
        /// <param name="args"></param>
        public void Handle_SelectCrtRep(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            SelectCrtRep rep = proto.GetDataInstance<SelectCrtRep>();

            var result = rep.Result;
            var crtId = rep.CrtId;
            var sceneId = rep.SceneId;

            if (result == ProtocolResultEnum.SUCCESS)
            {
                if (PlayerManager.GetInstance().Self != null)
                {
                    CommonUtils.Logout("选择角色" + crtId + "完毕");
                    PlayerManager.GetInstance().Self.lastSelectCrtId = crtId;
                    //切换到玩家数据中
                    SendCanEnterSceneReq(sceneId);
                }
            }
            else
            {
                CommonUtils.Logout("选择角色失败");
            }
        }

        public void SendCanEnterSceneReq(int sceneId)
        {
            var req = new CanEnterSceneReq();
            req.SceneId = sceneId;
            NetManager.GetInstance().SendProtocol(req);
        }

        public void Handle_CanEnterSceneRep(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            CanEnterSceneRep rep = proto.GetDataInstance<CanEnterSceneRep>();

            var result = rep.Result;
            var SceneId = rep.SceneId;

            if (result == ProtocolResultEnum.SUCCESS)
            {
                CommonUtils.Logout("允许进入" + SceneId + "场景 开始切换");
                GameSceneManager.Eventer.On(GameSceneManager.LOAD_SCENE, OnSceneEnterSuccess);
                //后续根据场景ID来定
                loadingScene = RegSceneClass.GameRoomScene;
                this.SceneId = SceneId;
                GameSceneManager.GetInstance().SwitchScene(loadingScene);
            }
        }

        private string loadingScene;
        private int SceneId;
        public void OnSceneEnterSuccess(object[] args)
        {
            string sceneName = args[0] as string;
            if (sceneName == loadingScene)
            {
                Global.InGameScene = true;
            }
            SendEnterSceneReq(SceneId);
            GameSceneManager.Eventer.Off(GameSceneManager.LOAD_SCENE, OnSceneEnterSuccess);
        }

        public void SendEnterSceneReq(int sceneId = 1)
        {
            CommonUtils.Logout("场景" + SceneId + "切换完成");
            var req = new EnterSceneReq();
            req.SceneId = sceneId;
            NetManager.GetInstance().SendProtocol(req);
        }

        /// <summary>
        /// 进入场景
        /// </summary>
        /// <param name="args"></param>
        public void Handle_EnterSceneRep(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            EnterSceneRep rep = proto.GetDataInstance<EnterSceneRep>();

            var result = rep.Result;
            var SceneId = rep.SceneId;

            if (result == ProtocolResultEnum.SUCCESS)
            {
                CommonUtils.Logout("同步场景"+ SceneId + "数据成功");
            }
        }

        /// <summary>
        /// 通知 玩家进入场景
        /// </summary>
        /// <param name="args"></param>
        public void Handle_GamePlayerEnterSeneNtf(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GamePlayerEnterSeneNtf ntf = proto.GetDataInstance<GamePlayerEnterSeneNtf>();
            SceneManager.GetInstance().AddPlayerToScene(ntf.PlayerInfo);
        }

        public void SendGetGameSyncAOIPlayerReq()
        {
            NetManager.GetInstance().SendProtocol(new GetGameSyncAOIPlayerReq());
        }

        /// <summary>
        /// 同步角色AOI范围内的玩家信息
        /// </summary>
        /// <param name="args"></param>
        public void Handle_GameSyncAOIPlayerNtf(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GameSyncAOIPlayerNtf ntf = proto.GetDataInstance<GameSyncAOIPlayerNtf>();
            foreach (var item in ntf.PlayerInfos)
            {
                SceneManager.GetInstance().AddPlayerToScene(item);
            }
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
        }

        public void HandleAnimPlay(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GameAnimPlayNtf ntf = proto.GetDataInstance<GameAnimPlayNtf>();
            var playerId = ntf.InstanceId;
            var AnimName = ntf.AnimName;
            var p = PlayerManager.GetInstance().FindPlayer(playerId);
            if (p != null)
            {
                AnimManager.GetInstance().PlayAnim(p.Crt.CharacterAnimator, AnimName);
            }
        }


        //暂未使用
        private void HandleBaseInfoNtf(object[] args)
        {
            /*            Protocol proto = args[0] as Protocol;
                        PlayerBaseInfoNtf rep = proto.GetDataInstance<PlayerBaseInfoNtf>();
                        var username = rep.Username;
                        var playerId = rep.PlayerId;
                        var lastStaySceneId = rep.LastStaySceneId;

                        Vector3 lastStayPos = new Vector3(
                            rep.LastStayPos.X,
                            rep.LastStayPos.Y,
                            rep.LastStayPos.Z);

                        Player p = new Player();
                        p.playerId = playerId;
                        p.username = username;
                        p.password = "";
                        p.lastStayPos = lastStayPos;
                        p.lastStaySceneId = lastStaySceneId;
                        PlayerManager.GetInstance().Self = p;
                        CommonUtils.Logout("玩家信息同步完成! 即将切换到选择角色");
                        GameSceneManager.GetInstance().SwitchScene(RegSceneClass.SelectRoleScene);*/
        }

        public void SendPlayerInput(float x, float y)
        {
            if (x == 0 && y == 0) return;
            var req = new GamePlayerInputReq();
            req.InputX = x;
            req.InputY = y;
            NetManager.GetInstance().SendProtocol(req);
        }
        private void Handle_GamePlayerInputNtf(object[] args)
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

        public void Send_GamePlayerOptCmdReq(string cmd, int type, int param)
        {
            GamePlayerOptCmdReq req = new GamePlayerOptCmdReq();
            req.PlayerId = Global.SelfPlayerId;
            req.Cmd = cmd;
            req.Type = type;
            req.Param = param;
            NetManager.GetInstance().SendProtocol(req);
        }

        private void Handle_GamePlayerOptCmdRep(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GamePlayerOptCmdRep rep = proto.GetDataInstance<GamePlayerOptCmdRep>();

            int playerId = rep.PlayerId;
            int result = rep.Result;

            if (result != ProtocolResultEnum.SUCCESS)
            {
                CommonUtils.Logout("操作失败!");
            }
        }

        public void Send_GamePlayerInputCmdReq(float inputX, float inputY, float rot)
        {
            GamePlayerInputCmdReq req = new GamePlayerInputCmdReq();
            req.PlayerId = Global.SelfPlayerId;
            req.InputX = inputX;
            req.InputY = inputY;
            req.Rot = rot;
            NetManager.GetInstance().SendProtocol(req);
        }
        private void Handle_GamePlayerInputCmdNtf(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GamePlayerInputCmdNtf ntf = proto.GetDataInstance<GamePlayerInputCmdNtf>();

            Vector2 inputData = new Vector2(ntf.InputX, ntf.InputY);
            float Rot = ntf.Rot;
            int pId = ntf.PlayerId;

            //通知对应角色应用输入
            Player player = PlayerManager.GetInstance().FindPlayer(pId);
            if (player != null)
            {
                player.Crt.CrtData.Input = inputData;
            }
            player.Trans.rotation = Quaternion.Euler(0, Rot, 0);
        }

        private void Handle_GameSyncStateChangeNtf(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GameSyncStateChangeNtf ntf = proto.GetDataInstance<GameSyncStateChangeNtf>();

            GamePlayerBaseInfo info = ntf.PlayerInfo;
            Player p = PlayerManager.GetInstance().FindPlayer(info.PlayerId);
            p.UpdateBaseInfo(info);
        }

        private void Handle_GameInstanceCreateNtf(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GameInstanceCreateNtf ntf = proto.GetDataInstance<GameInstanceCreateNtf>();

            GameInstaneInfo info = ntf.Info;

            string prefabKey = info.PrefabKey;
            int stageNum = info.StageNum;
            Vector3 pos = new Vector3(info.Offset.X,info.Offset.Y,info.Offset.Z);
            float rot = info.Rot;
            InstanceManager.GetInstance().Event(InstanceManager.INSTANCE_CREATE, prefabKey, stageNum, pos, rot);
        }

    }
}