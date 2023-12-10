
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


public final class DemoPrimitiveTypesTable extends AbstractBean {
    public DemoPrimitiveTypesTable(JsonObject _buf) { 
        x1 = _buf.get("x1").getAsBoolean();
        x2 = _buf.get("x2").getAsByte();
        x3 = _buf.get("x3").getAsShort();
        x4 = _buf.get("x4").getAsInt();
        x5 = _buf.get("x5").getAsLong();
        x6 = _buf.get("x6").getAsFloat();
        x7 = _buf.get("x7").getAsDouble();
        s1 = _buf.get("s1").getAsString();
        s2 = _buf.get("s2").getAsString();
        v2 = cfg.vec2.deserialize(_buf.get("v2").getAsJsonObject());
        v3 = cfg.vec3.deserialize(_buf.get("v3").getAsJsonObject());
        v4 = cfg.vec4.deserialize(_buf.get("v4").getAsJsonObject());
        t1 = _buf.get("t1").getAsLong();
    }

    public static DemoPrimitiveTypesTable deserialize(JsonObject _buf) {
            return new cfg.test.DemoPrimitiveTypesTable(_buf);
    }

    public final boolean x1;
    public final byte x2;
    public final short x3;
    public final int x4;
    public final long x5;
    public final float x6;
    public final double x7;
    public final String s1;
    public final String s2;
    public final cfg.vec2 v2;
    public final cfg.vec3 v3;
    public final cfg.vec4 v4;
    public final long t1;

    public static final int __ID__ = -370934083;
    
    @Override
    public int getTypeId() { return __ID__; }

    @Override
    public String toString() {
        return "{ "
        + "(format_field_name __code_style field.name):" + x1 + ","
        + "(format_field_name __code_style field.name):" + x2 + ","
        + "(format_field_name __code_style field.name):" + x3 + ","
        + "(format_field_name __code_style field.name):" + x4 + ","
        + "(format_field_name __code_style field.name):" + x5 + ","
        + "(format_field_name __code_style field.name):" + x6 + ","
        + "(format_field_name __code_style field.name):" + x7 + ","
        + "(format_field_name __code_style field.name):" + s1 + ","
        + "(format_field_name __code_style field.name):" + s2 + ","
        + "(format_field_name __code_style field.name):" + v2 + ","
        + "(format_field_name __code_style field.name):" + v3 + ","
        + "(format_field_name __code_style field.name):" + v4 + ","
        + "(format_field_name __code_style field.name):" + t1 + ","
        + "}";
    }
}

