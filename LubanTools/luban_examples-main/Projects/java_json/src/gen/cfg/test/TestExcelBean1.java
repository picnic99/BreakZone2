
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


/**
 * 这是个测试excel结构
 */
public final class TestExcelBean1 extends AbstractBean {
    public TestExcelBean1(JsonObject _buf) { 
        x1 = _buf.get("x1").getAsInt();
        x2 = _buf.get("x2").getAsString();
        x3 = _buf.get("x3").getAsInt();
        x4 = _buf.get("x4").getAsFloat();
    }

    public static TestExcelBean1 deserialize(JsonObject _buf) {
            return new cfg.test.TestExcelBean1(_buf);
    }

    /**
     * 最高品质
     */
    public final int x1;
    /**
     * 黑色的
     */
    public final String x2;
    /**
     * 蓝色的
     */
    public final int x3;
    /**
     * 最差品质
     */
    public final float x4;

    public static final int __ID__ = -1738345160;
    
    @Override
    public int getTypeId() { return __ID__; }

    @Override
    public String toString() {
        return "{ "
        + "(format_field_name __code_style field.name):" + x1 + ","
        + "(format_field_name __code_style field.name):" + x2 + ","
        + "(format_field_name __code_style field.name):" + x3 + ","
        + "(format_field_name __code_style field.name):" + x4 + ","
        + "}";
    }
}

