
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


public final class ExcelFromJson extends AbstractBean {
    public ExcelFromJson(JsonObject _buf) { 
        x4 = _buf.get("x4").getAsInt();
        x1 = _buf.get("x1").getAsBoolean();
        x5 = _buf.get("x5").getAsLong();
        x6 = _buf.get("x6").getAsFloat();
        s1 = _buf.get("s1").getAsString();
        s2 = _buf.get("s2").getAsString();
        t1 = _buf.get("t1").getAsLong();
        x12 = cfg.test.DemoType1.deserialize(_buf.get("x12").getAsJsonObject());
        x13 = _buf.get("x13").getAsInt();
        x14 = cfg.test.DemoDynamic.deserialize(_buf.get("x14").getAsJsonObject());
        { com.google.gson.JsonArray _json0_ = _buf.get("k1").getAsJsonArray(); int __n0 = _json0_.size(); k1 = new int[__n0]; int __index0=0; for(JsonElement __e0 : _json0_) { int __v0;  __v0 = __e0.getAsInt();  k1[__index0++] = __v0; }   }
        { com.google.gson.JsonArray _json0_ = _buf.get("k8").getAsJsonArray(); k8 = new java.util.HashMap<Integer, Integer>(_json0_.size()); for(JsonElement _e0 : _json0_) { int _k0;  _k0 = _e0.getAsJsonArray().get(0).getAsInt(); int _v0;  _v0 = _e0.getAsJsonArray().get(1).getAsInt();  k8.put(_k0, _v0); }   }
        { com.google.gson.JsonArray _json0_ = _buf.get("k9").getAsJsonArray(); k9 = new java.util.ArrayList<cfg.test.DemoE2>(_json0_.size()); for(JsonElement _e0 : _json0_) { cfg.test.DemoE2 _v0;  _v0 = cfg.test.DemoE2.deserialize(_e0.getAsJsonObject());  k9.add(_v0); }   }
        { com.google.gson.JsonArray _json0_ = _buf.get("k15").getAsJsonArray(); int __n0 = _json0_.size(); k15 = new cfg.test.DemoDynamic[__n0]; int __index0=0; for(JsonElement __e0 : _json0_) { cfg.test.DemoDynamic __v0;  __v0 = cfg.test.DemoDynamic.deserialize(__e0.getAsJsonObject());  k15[__index0++] = __v0; }   }
    }

    public static ExcelFromJson deserialize(JsonObject _buf) {
            return new cfg.test.ExcelFromJson(_buf);
    }

    public final int x4;
    public final boolean x1;
    public final long x5;
    public final float x6;
    public final String s1;
    public final String s2;
    public final long t1;
    public final cfg.test.DemoType1 x12;
    public final int x13;
    public final cfg.test.DemoDynamic x14;
    public final int[] k1;
    public final java.util.HashMap<Integer, Integer> k8;
    public final java.util.ArrayList<cfg.test.DemoE2> k9;
    public final cfg.test.DemoDynamic[] k15;

    public static final int __ID__ = -1485706483;
    
    @Override
    public int getTypeId() { return __ID__; }

    @Override
    public String toString() {
        return "{ "
        + "(format_field_name __code_style field.name):" + x4 + ","
        + "(format_field_name __code_style field.name):" + x1 + ","
        + "(format_field_name __code_style field.name):" + x5 + ","
        + "(format_field_name __code_style field.name):" + x6 + ","
        + "(format_field_name __code_style field.name):" + s1 + ","
        + "(format_field_name __code_style field.name):" + s2 + ","
        + "(format_field_name __code_style field.name):" + t1 + ","
        + "(format_field_name __code_style field.name):" + x12 + ","
        + "(format_field_name __code_style field.name):" + x13 + ","
        + "(format_field_name __code_style field.name):" + x14 + ","
        + "(format_field_name __code_style field.name):" + k1 + ","
        + "(format_field_name __code_style field.name):" + k8 + ","
        + "(format_field_name __code_style field.name):" + k9 + ","
        + "(format_field_name __code_style field.name):" + k15 + ","
        + "}";
    }
}

