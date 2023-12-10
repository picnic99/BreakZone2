
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

package cfg.ai;

import luban.*;


public abstract class Task extends cfg.ai.FlowNode {
    public Task(ByteBuf _buf) { 
        super(_buf);
        ignoreRestartSelf = _buf.readBool();
    }

    public static Task deserialize(ByteBuf _buf) {
        switch (_buf.readInt()) {
            case cfg.ai.UeWait.__ID__: return new cfg.ai.UeWait(_buf);
            case cfg.ai.UeWaitBlackboardTime.__ID__: return new cfg.ai.UeWaitBlackboardTime(_buf);
            case cfg.ai.MoveToTarget.__ID__: return new cfg.ai.MoveToTarget(_buf);
            case cfg.ai.ChooseSkill.__ID__: return new cfg.ai.ChooseSkill(_buf);
            case cfg.ai.MoveToRandomLocation.__ID__: return new cfg.ai.MoveToRandomLocation(_buf);
            case cfg.ai.MoveToLocation.__ID__: return new cfg.ai.MoveToLocation(_buf);
            case cfg.ai.DebugPrint.__ID__: return new cfg.ai.DebugPrint(_buf);
            default: throw new SerializationException();
        }
    }

    public final boolean ignoreRestartSelf;


    @Override
    public String toString() {
        return "{ "
        + "(format_field_name __code_style field.name):" + id + ","
        + "(format_field_name __code_style field.name):" + nodeName + ","
        + "(format_field_name __code_style field.name):" + decorators + ","
        + "(format_field_name __code_style field.name):" + services + ","
        + "(format_field_name __code_style field.name):" + ignoreRestartSelf + ","
        + "}";
    }
}

