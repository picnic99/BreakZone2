
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

package cfg.test;

import luban.*;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;


public final class TestRow extends AbstractBean {
    public TestRow(JsonObject _buf) { 
        x = _buf.get("x").getAsInt();
        y = _buf.get("y").getAsBoolean();
        z = _buf.get("z").getAsString();
        a = cfg.test.Test3.deserialize(_buf.get("a").getAsJsonObject());
        { com.google.gson.JsonArray _json0_ = _buf.get("b").getAsJsonArray(); b = new java.util.ArrayList<Integer>(_json0_.size()); for(JsonElement _e0 : _json0_) { int _v0;  _v0 = _e0.getAsInt();  b.add(_v0); }   }
    }

    public static TestRow deserialize(JsonObject _buf) {
            return new cfg.test.TestRow(_buf);
    }

    public final int x;
    public final boolean y;
    public final String z;
    public final cfg.test.Test3 a;
    public final java.util.ArrayList<Integer> b;

    public static final int __ID__ = -543222164;
    
    @Override
    public int getTypeId() { return __ID__; }

    @Override
    public String toString() {
        return "{ "
        + "(format_field_name __code_style field.name):" + x + ","
        + "(format_field_name __code_style field.name):" + y + ","
        + "(format_field_name __code_style field.name):" + z + ","
        + "(format_field_name __code_style field.name):" + a + ","
        + "(format_field_name __code_style field.name):" + b + ","
        + "}";
    }
}

