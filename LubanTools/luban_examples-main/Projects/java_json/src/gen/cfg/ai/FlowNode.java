
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

package cfg.ai;

import luban.*;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;


public abstract class FlowNode extends cfg.ai.Node {
    public FlowNode(JsonObject _buf) { 
        super(_buf);
        { com.google.gson.JsonArray _json0_ = _buf.get("decorators").getAsJsonArray(); decorators = new java.util.ArrayList<cfg.ai.Decorator>(_json0_.size()); for(JsonElement _e0 : _json0_) { cfg.ai.Decorator _v0;  _v0 = cfg.ai.Decorator.deserialize(_e0.getAsJsonObject());  decorators.add(_v0); }   }
        { com.google.gson.JsonArray _json0_ = _buf.get("services").getAsJsonArray(); services = new java.util.ArrayList<cfg.ai.Service>(_json0_.size()); for(JsonElement _e0 : _json0_) { cfg.ai.Service _v0;  _v0 = cfg.ai.Service.deserialize(_e0.getAsJsonObject());  services.add(_v0); }   }
    }

    public static FlowNode deserialize(JsonObject _buf) {
        switch (_buf.get("$type").getAsString()) {
            case "Sequence": return new cfg.ai.Sequence(_buf);
            case "Selector": return new cfg.ai.Selector(_buf);
            case "SimpleParallel": return new cfg.ai.SimpleParallel(_buf);
            case "UeWait": return new cfg.ai.UeWait(_buf);
            case "UeWaitBlackboardTime": return new cfg.ai.UeWaitBlackboardTime(_buf);
            case "MoveToTarget": return new cfg.ai.MoveToTarget(_buf);
            case "ChooseSkill": return new cfg.ai.ChooseSkill(_buf);
            case "MoveToRandomLocation": return new cfg.ai.MoveToRandomLocation(_buf);
            case "MoveToLocation": return new cfg.ai.MoveToLocation(_buf);
            case "DebugPrint": return new cfg.ai.DebugPrint(_buf);
            default: throw new SerializationException();
        }
    }

    public final java.util.ArrayList<cfg.ai.Decorator> decorators;
    public final java.util.ArrayList<cfg.ai.Service> services;


    @Override
    public String toString() {
        return "{ "
        + "(format_field_name __code_style field.name):" + id + ","
        + "(format_field_name __code_style field.name):" + nodeName + ","
        + "(format_field_name __code_style field.name):" + decorators + ","
        + "(format_field_name __code_style field.name):" + services + ","
        + "}";
    }
}

