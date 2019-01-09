namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedBattleResultType : SharedVariable<BattleResultType>
    {
        public static implicit operator SharedBattleResultType(BattleResultType value) { return new SharedBattleResultType { mValue = value }; }
    }
}
