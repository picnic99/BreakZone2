
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

package cfg;


import (
    "demo/luban"
)

import "errors"

type AiUeSetDefaultFocus struct {
    Id int32
    NodeName string
    KeyboardKey string
}

const TypeId_AiUeSetDefaultFocus = 1812449155

func (*AiUeSetDefaultFocus) GetTypeId() int32 {
    return 1812449155
}

func NewAiUeSetDefaultFocus(_buf *luban.ByteBuf) (_v *AiUeSetDefaultFocus, err error) {
    _v = &AiUeSetDefaultFocus{}
    { if _v.Id, err = _buf.ReadInt(); err != nil { err = errors.New("error"); return } }
    { if _v.NodeName, err = _buf.ReadString(); err != nil { err = errors.New("error"); return } }
    { if _v.KeyboardKey, err = _buf.ReadString(); err != nil { err = errors.New("error"); return } }
    return
}

