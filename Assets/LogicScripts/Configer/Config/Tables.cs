
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;

namespace cfg
{
public partial class Tables
{
    public TbCharacter TbCharacter {get; }
    public TbProperty TbProperty {get; }
    public TbSkill TbSkill {get; }
    public TbAnimation TbAnimation {get; }
    public TbState TbState {get; }

    public Tables(System.Func<string, JSONNode> loader)
    {
        TbCharacter = new TbCharacter(loader("tbcharacter"));
        TbProperty = new TbProperty(loader("tbproperty"));
        TbSkill = new TbSkill(loader("tbskill"));
        TbAnimation = new TbAnimation(loader("tbanimation"));
        TbState = new TbState(loader("tbstate"));
        ResolveRef();
    }
    
    private void ResolveRef()
    {
        TbCharacter.ResolveRef(this);
        TbProperty.ResolveRef(this);
        TbSkill.ResolveRef(this);
        TbAnimation.ResolveRef(this);
        TbState.ResolveRef(this);
    }
}

}
