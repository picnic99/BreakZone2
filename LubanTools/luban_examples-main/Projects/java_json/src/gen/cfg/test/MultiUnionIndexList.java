
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


public final class MultiUnionIndexList extends AbstractBean {
    public MultiUnionIndexList(JsonObject _buf) { 
        id1 = _buf.get("id1").getAsInt();
        id2 = _buf.get("id2").getAsLong();
        id3 = _buf.get("id3").getAsString();
        num = _buf.get("num").getAsInt();
        desc = _buf.get("desc").getAsString();
    }

    public static MultiUnionIndexList deserialize(JsonObject _buf) {
            return new cfg.test.MultiUnionIndexList(_buf);
    }

    public final int id1;
    public final long id2;
    public final String id3;
    public final int num;
    public final String desc;

    public static final int __ID__ = 1966847134;
    
    @Override
    public int getTypeId() { return __ID__; }

    @Override
    public String toString() {
        return "{ "
        + "(format_field_name __code_style field.name):" + id1 + ","
        + "(format_field_name __code_style field.name):" + id2 + ","
        + "(format_field_name __code_style field.name):" + id3 + ","
        + "(format_field_name __code_style field.name):" + num + ","
        + "(format_field_name __code_style field.name):" + desc + ","
        + "}";
    }
}

