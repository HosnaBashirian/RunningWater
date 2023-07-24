using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using _Scripts.Controller.Player;
using MyBox;
using UnityEngine;

namespace _Scripts.Utils
{
    [RequireComponent(typeof(Camera))]
    public class CameraCapture : MonoBehaviour
    {
        [SerializeField] private List<PlayerSkin> skins;

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                CamCapture();
            }
        }

        public async void CamCapture()
        {
            foreach (var skin in skins)
            {
                await Task.Delay(100);
                skins.ForEach(x => x.gameObject.SetActive(false));
                skin.gameObject.SetActive(true);
                await Task.Delay(100);
                
                Camera Cam = GetComponent<Camera>();

                RenderTexture currentRT = RenderTexture.active;
                RenderTexture.active = Cam.targetTexture;

                Cam.Render();

                Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
                Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
                Image.Apply();
                RenderTexture.active = currentRT;

                var Bytes = Image.EncodeToPNG();
                Destroy(Image);

                File.WriteAllBytes(
                    $"C:\\Users\\Sina\\Documents\\Work\\Personal\\RunningWater\\Assets\\Sprites\\Characters\\{skin.key}.png",
                    Bytes);
            }
        }
    }
}