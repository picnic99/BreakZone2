using System.Collections.Generic;

namespace LogicScripts.Client.Manager
{
    public class CharacterManager:Manager<CharacterManager>
    {
        public Dictionary<int, Character> CharactersDic = new Dictionary<int, Character>();
    }
}