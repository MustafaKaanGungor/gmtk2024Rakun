using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*public enum Scene {
    MainMenu
}*/

public class SceneLoader : MonoBehaviour {
    public delegate void OnSceneLoadDelegate();
    public static event OnSceneLoadDelegate OnSceneLoad;

    public delegate void OnSceneLoadCompleteDelegate();
    public static event OnSceneLoadCompleteDelegate OnSceneLoadComplete;

    public static SceneLoader instance = null;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadNextScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        OnSceneLoadEvent();
    }

    public  void LoadSceneWithIndex(int index) {
        SceneManager.LoadScene(index);
        OnSceneLoadEvent();
    }

    public  void LoadSceneWithName(string name) {
        SceneManager.LoadScene(name);
        OnSceneLoadEvent();
    }

    public IEnumerator LoadAsyncSceneWithName(string name, float delay, Action preLoadCallBack = null, Action onCompleteCallBack = null) {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        OnSceneLoadEvent();
        if (preLoadCallBack != null)
            preLoadCallBack();
        yield return new WaitForSeconds(delay);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }

        Debug.LogWarning("scene load completed");

        OnSceneLoadCompleteEvent();

        if (onCompleteCallBack != null)
            onCompleteCallBack();
    }

    public void OnSceneLoadEvent() {
        if (OnSceneLoad != null) {
            OnSceneLoad();
        }
    }


    public void OnSceneLoadCompleteEvent() {
        if (OnSceneLoadComplete != null) {
            OnSceneLoadComplete();
        }
    }

    public void Quit() {
        Application.Quit();
    }
}