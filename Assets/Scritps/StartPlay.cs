using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPlay : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer Vp;
    AsyncOperation async;
    bool isVideoCompleate=false;
  public void StartVideo()
  {
        if (PlayerPrefs.HasKey("isVideoCompleate"))
        {
            isVideoCompleate = bool.Parse(PlayerPrefs.GetString("isVideoCompleate"));
        }
        if (!isVideoCompleate)
        {
            Vp.isLooping = false;
            Vp.Play();
            Screen.orientation = ScreenOrientation.Landscape;
            Debug.Log(Vp.clip.length);
            async = SceneManager.LoadSceneAsync(1);
            async.allowSceneActivation = false;
        }
        else
        {
            SceneManager.LoadScene(1);
        }
  }

    private void Update()
    {


        if (Vp.clip.length - Vp.time <= 0.1f)
        {
            Screen.orientation = ScreenOrientation.Portrait;
            isVideoCompleate = true;
            PlayerPrefs.SetString("isVideoCompleate", isVideoCompleate.ToString());
            async.allowSceneActivation = true;

        }
    }   
    
}
