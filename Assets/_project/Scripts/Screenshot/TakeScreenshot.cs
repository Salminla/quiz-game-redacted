using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class TakeScreenshot : MonoBehaviour {
    
    public void Screenshot ()
    {
        StartCoroutine(UploadPNG());
        //Debug.log (encodedText);
    }

    public void PDFSave()
    {
        UploadPDF();
    }
    IEnumerator UploadPNG() {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();
 
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D( width, height, TextureFormat.RGB24, false );
 
        // Read screen contents into the texture
        tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
        tex.Apply();
 
        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy( tex );
 
        //string ToBase64String byte[]
        string encodedText = System.Convert.ToBase64String (bytes);
   
        var image_url = "data:image/png;base64," + encodedText;
 
        Debug.Log (image_url);
 
#if !UNITY_EDITOR
        openWindow(image_url);
#endif
    }
    void UploadPDF()
    {

        byte[] bytes = System.IO.File.ReadAllBytes(Application.dataPath + "/_project/Certificate.pdf");
        string encodedText = System.Convert.ToBase64String (bytes);
        var pdf_url = "data:application/pdf;base64," + encodedText;
        //var image_url = Application.dataPath + "/_project/Certificate.pdf";
 
        Debug.Log (pdf_url);
 
#if !UNITY_EDITOR
        openWindow(pdf_url);
#endif
    }
    [DllImport("__Internal")]
    private static extern void openWindow(string url);
 
}