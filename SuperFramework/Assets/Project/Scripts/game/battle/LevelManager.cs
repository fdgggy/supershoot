using UnityEngine;
using BehaviorDesigner.Runtime;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance
    {
        get
        {
            return SingleComponent<LevelManager>.Instance();
        }
    }

    public void Init(string aiName)
    {
        ExternalBehavior eb = (ExternalBehavior)Resources.Load<ExternalBehavior>(aiName);

        var bt = gameObject.AddComponent<BehaviorTree>();
        bt.StartWhenEnabled = false;
        bt.ExternalBehavior = eb;
    }
}
