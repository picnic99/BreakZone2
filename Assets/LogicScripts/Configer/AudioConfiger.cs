using System.Collections.Generic;

public class AudioConfiger : Singleton<AudioConfiger>, Manager
{
    private List<AudioVO> List;

    public void Init()
    {
        if (List == null) List = new List<AudioVO>();

        foreach (var item in Configer.Tables.TbAudio.DataList)
        {
            var vo = new AudioVO();
            vo.audio = item;
            List.Add(vo);
        }
    }
    public AudioVO GetAudioData(int crtId, int skillId, int stateId, string keyword = "")
    {
        var vo = List.Find((item) => { return item.GetKeyStr() == GetKeyStr(crtId,skillId,stateId,keyword); });
        return vo;
    }
    public string GetKeyStr(int crtId = 0,int skillId = 0, int stateId = 0, string keyword = "")
    {
        return $"{crtId}_{skillId}_{stateId}_{keyword}";
    }
}