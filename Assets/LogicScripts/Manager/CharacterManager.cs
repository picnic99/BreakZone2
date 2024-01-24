using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>, Manager
{
    public void Init()
    {

    }

    public Character CreateCharacter(int id) {
        //创建一个角色
        //原生模型只需要包括模型即可
        //此处将添加游戏中所需要的所以模块
        //IK
        //armTarget
        //...
        return null;
    }

    public ShowCharacter AddShowRole(CharacterVO vo)
    {
        var obj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.modelName);
        var character = new ShowCharacter(vo, obj);
        return character;
    }

}