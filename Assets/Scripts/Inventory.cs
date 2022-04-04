using System;

[Serializable]
public class Inventory
{
    public int Wood;
    public int Stone;

    public void AddItem(Resource.ResourceType type)
    {
        switch (type)
        {
            case Resource.ResourceType.Stone:
                Stone++;
                break;
            case Resource.ResourceType.Wood:
                Wood++;
                break;
        }
    }
}