using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   private Transform _objToFollow = null;
   private float zPos = -10;
   private void Update()
   {
       if (_objToFollow != null)
       {
           transform.position = new Vector3(_objToFollow.position.x, _objToFollow.position.y, zPos);
       }
   }

   public void SetObject(GameObject gameObject)
   {
       _objToFollow = gameObject.transform;
   }
}
