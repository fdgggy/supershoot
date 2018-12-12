namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedCampType : SharedVariable<CampType>
    {
        public static implicit operator SharedCampType(CampType value) { return new SharedCampType { mValue = value }; }
    }
}