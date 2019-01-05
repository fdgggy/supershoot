public struct EntityInfo
{
    public int EntityId;
    public string Name;
    public string PrefabName;
    public int Level;
    public string AI;
    public CampType Camp;
    public float MoveSpeed;
    public float RunSpeed;
    public float FieldOfView;
    public float FieldDistance;
    public string WeaponIds;
}
public enum CampType
{
    Enemy = 0,
    Player = 1,
    Neutral = 2
}