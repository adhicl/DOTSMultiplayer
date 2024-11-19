using UnityEngine;

[CreateAssetMenu(fileName = "Connection", menuName = "Scriptable Objects/Connection")]
public class Connection : ScriptableObject
{
    public string ipAddress;
    public ushort port;
    public string password;
}
