using UnityEngine;

namespace FahimsUtils
{
    public class UtilsClass
    {

        #region Mouse World Position on the screen 

        // The mosue positon with the Z-index = 0
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPositon, Camera worldCamera)
        {

            return worldCamera.ScreenToWorldPoint(screenPositon);
        }

        #endregion
    }
}