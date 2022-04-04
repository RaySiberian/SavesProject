using UnityEngine;

public class Resource : MonoBehaviour
{
    public SerializableResource SerializableResource;
    public ResourceType Type;

    public enum ResourceType
    {
        Wood,
        Stone
    }
    
}