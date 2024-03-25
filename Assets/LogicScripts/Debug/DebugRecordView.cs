using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugRecordView
{
    public GameObject tmp_record;
    public Button clearBtn;
    public List<DebugRecordItem> records;

    public DebugRecordView(GameObject tmp_record,GameObject recordView)
    {
        this.tmp_record = tmp_record;
        clearBtn = recordView.transform.Find("recordOpt/clearBtn").GetComponent<Button>();
        clearBtn.onClick.AddListener(RemoveAll);
        records = new List<DebugRecordItem>();
        AddEventListener();
    }

    private void AddEventListener()
    {
        _EventDispatcher.GetInstance().On(_EventDispatcher.ADD_RECORD, AddRecord);
    }

    private void RemoveEventListener()
    {
        _EventDispatcher.GetInstance().Off(_EventDispatcher.ADD_RECORD, AddRecord);
    }

    private void AddRecord(object[] msg)
    {
        string str = (string)msg[0];
        GameObject obj = GameObject.Instantiate<GameObject>(tmp_record, tmp_record.transform.parent);
        obj.SetActive(true);
        DebugRecordItem item = new DebugRecordItem(obj);
        item.UpdateMsg(str);
        records.Add(item);
        LayoutRebuilder.ForceRebuildLayoutImmediate(tmp_record.transform.parent.GetComponent<RectTransform>());
    }

    private void RemoveAll()
    {
        foreach (var item in records)
        {
            GameObject.Destroy(item.Root);
        }
        records.Clear();
    }
}
