
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

package cfg;


type TestTbDetectCsvEncoding struct {
    _dataMap map[int32]*TestDetectEncoding
    _dataList []*TestDetectEncoding
}

func NewTestTbDetectCsvEncoding(_buf []map[string]interface{}) (*TestTbDetectCsvEncoding, error) {
    _dataList := make([]*TestDetectEncoding, 0, len(_buf))
    dataMap := make(map[int32]*TestDetectEncoding)

    for _, _ele_ := range _buf {
        if _v, err2 := NewTestDetectEncoding(_ele_); err2 != nil {
            return nil, err2
        } else {
            _dataList = append(_dataList, _v)
            dataMap[_v.Id] = _v
        }
    }
    return &TestTbDetectCsvEncoding{_dataList:_dataList, _dataMap:dataMap}, nil
}

func (table *TestTbDetectCsvEncoding) GetDataMap() map[int32]*TestDetectEncoding {
    return table._dataMap
}

func (table *TestTbDetectCsvEncoding) GetDataList() []*TestDetectEncoding {
    return table._dataList
}

func (table *TestTbDetectCsvEncoding) Get(key int32) *TestDetectEncoding {
    return table._dataMap[key]
}


