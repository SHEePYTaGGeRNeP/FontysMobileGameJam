namespace Assets.Scripts.Helpers.Components
{
    using System;
    using System.Collections;
    using System.IO;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CameraScreenShotter : MonoBehaviour
    {

        public void TakeScreenshot(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                filename = string.Format("Screenshot {0} - {1}", SceneManager.GetActiveScene().name, this.name);
            this.StartCoroutine(this.ReallyTakeScreenshot(filename));
        }


        private IEnumerator ReallyTakeScreenshot(string filename)
        {
            // We should only read the screen buffer after rendering is complete
            //yield return new WaitForEndOfFrame();

            // Create a texture the size of the screen, RGB24 format
            Camera cameraToSave = this.GetComponent<Camera>();
            int width = cameraToSave.pixelWidth;  //Screen.width;
            int height = cameraToSave.pixelHeight;//Screen.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            // Initialize and render
            RenderTexture rt = new RenderTexture(width, height, 24);
            cameraToSave.targetTexture = rt;
            cameraToSave.Render();
            RenderTexture.active = rt;

            // Read pixels
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);

            // Clean up
            cameraToSave.targetTexture = null;
            RenderTexture.active = null; // added to avoid errors 
            DestroyImmediate(rt);


            // Read screen contents into the texture
            //tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            //tex.Apply();

            // Encode texture into PNG
            byte[] bytes = tex.EncodeToPNG();
            UnityEngine.Object.DestroyImmediate(tex);

            string path = string.Format("{0}/{1}-{2}.png", Application.dataPath, filename, DateTime.Now.ToString("dd-MM-yy HH-mm-ss"));
            File.WriteAllBytes(path, bytes);
            LogHelper.Log(typeof(CameraScreenShotter), "screenshot saved in " + path);

            yield return null;
        }
    }
}
