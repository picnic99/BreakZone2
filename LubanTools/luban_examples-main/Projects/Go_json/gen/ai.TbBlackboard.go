
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

package cfg;


type AiTbBlackboard struct {
    _dataMap map[string]*AiBlackboard
    _dataList []*AiBlackboard
}

func NewAiTbBlackboard(_buf []map[string]interface{}) (*AiTbBlackboard, error) {
    _dataList := make([]*AiBlackboard, 0, len(_buf))
    dataMap := make(map[string]*AiBlackboard)

    for _, _ele_ := range _buf {
        if _v, err2 := NewAiBlackboard(_ele_); err2 != nil {
            return nil, err2
        } else {
            _dataList = append(_dataList, _v)
            dataMap[_v.Name] = _v
        }
    }
    return &AiTbBlackboard{_dataList:_dataList, _dataMap:dataMap}, nil
}

func (table *AiTbBlackboard) GetDataMap() map[string]*AiBlackboard {
    return table._dataMap
}

func (table *AiTbBlackboard) GetDataList() []*AiBlackboard {
    return table._dataList
}

func (table *AiTbBlackboard) Get(key string) *AiBlackboard {
    return table._dataMap[key]
}


