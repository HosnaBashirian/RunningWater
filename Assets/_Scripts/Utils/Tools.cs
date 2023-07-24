using UnityEditor;
using UnityEngine;

namespace _Scripts.Utils
{
    public class Tools : MonoBehaviour
    {
        [MenuItem("Tools/Screenshot")]
        public static void ScreenShot()
        {
            ScreenCapture.CaptureScreenshot("C:\\Users\\Sina\\Documents\\Work\\Personal\\RunningWater\\Assets\\Screenshots\\Temp.png");
        }
    }
}