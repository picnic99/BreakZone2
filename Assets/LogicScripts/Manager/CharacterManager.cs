using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>, Manager
{
    public void Init()
    {

    }

    public Character CreateCharacter(int id) {
        CharacterVO vo = CharacterConfiger.GetInstance().GetCharacterById(id);
        return CreateCharacter(vo);
    }
    public Character CreateCharacter(CharacterVO vo)
    {
        GameObject characterObj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.character.ModePath);
        return new Character(vo, characterObj); ;
    }

}