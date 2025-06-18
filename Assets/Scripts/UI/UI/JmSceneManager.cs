using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JmSceneManager : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        Debug.Log("씬 로드됨: " + sceneName);

        switch (sceneName)
        {
            /*
            case "Title":
                var startBtn = GameObject.Find("GameStart")?.GetComponent<Button>();
                if (startBtn != null)
                    startBtn.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
                break;

            case "GameScene":
                var backBtn = GameObject.Find("BackButton")?.GetComponent<Button>();
                if (backBtn != null)
                    backBtn.onClick.AddListener(() => SceneManager.LoadScene("Title"));
                break;
            
                // 씬 추가될 때마다 여기에 case 추가!
            */
            //    현재 이름이랑 다르기 때문에 합칠 때 다시 활성화
        }
    }
}
