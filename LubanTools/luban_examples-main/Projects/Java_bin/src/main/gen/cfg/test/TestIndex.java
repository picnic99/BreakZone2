
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

package cfg.test;

import luban.*;


public final class TestIndex extends AbstractBean {
    public TestIndex(ByteBuf _buf) { 
        id = _buf.readInt();
        {int n = Math.min(_buf.readSize(), _buf.size());eles = new java.util.ArrayList<cfg.test.DemoType1>(n);for(int i = 0 ; i < n ; i++) { cfg.test.DemoType1 _e;  _e = cfg.test.DemoType1.deserialize(_buf); eles.add(_e);}}
    }

    public static TestIndex deserialize(ByteBuf _buf) {
            return new cfg.test.TestIndex(_buf);
    }

    public final int id;
    public final java.util.ArrayList<cfg.test.DemoType1> eles;

    public static final int __ID__ = 1941154020;
    
    @Override
    public int getTypeId() { return __ID__; }

    @Override
    public String toString() {
        return "{ "
        + "(format_field_name __code_style field.name):" + id + ","
        + "(format_field_name __code_style field.name):" + eles + ","
        + "}";
    }
}

