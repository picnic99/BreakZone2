using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Net.Protocols
{
    enum ProtocolId
    {
/*        CLENT_LOGIN_IN_REQ = 10001,//登录请求
        CLENT_LOGIN_IN_REP = 10002,//登录返回
        CLENT_ADD_ROOM_REQ = 10003,//加入房间请求
        CLENT_ADD_ROOM_REP = 10004,//加入房间返回
        CLENT_PLAYER_OPT_REQ = 10005,//角色操作请求
        CLENT_PLAYER_OPT_REP = 10006,//角色操作返回
        CLENT_GET_GAME_DATA_REQ = 10007,//获取游戏数据请求
        CLENT_GET_GAME_DATA_REP = 10008,//获取游戏数据返回
        CLENT_GAME_DATA_NTF = 10009,//游戏数据同步通知
        CLENT_PLAYER_ADD_NTF = 10010,//有新玩家加入房间通知
        CLENT_BULLET_ADD_NTF = 10011,//有弹道新增*/

        CLIENT_GAME_PLAYER_OPT_REQ = 10012,//玩家操作
        CLIENT_GAME_PLAYER_OPT_REP = 10013,//玩家操作返回
    }
}
