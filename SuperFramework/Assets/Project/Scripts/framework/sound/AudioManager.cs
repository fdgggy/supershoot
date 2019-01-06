using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private GroupAudio battleAudio = null;

    public static AudioManager Instance
    {
        get
        {
            return SingleComponent<AudioManager>.Instance();
        }
    }

    public void PlayBattleAudio(string audioName, bool isLoop = false)
    {
        if (battleAudio == null)
        {
            battleAudio = TComponent<GroupAudio>.Instance();
        }
        battleAudio.PlayAudio(audioName, isLoop);
    }

    public void ClearBattleAudio()
    {
        if (battleAudio != null)
        {
            battleAudio.Clear();
            battleAudio = null;
        }
    }
}
