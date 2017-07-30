using System;
using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMutreta
{
    private int _currentScene = 0;
    private string[] _scenes = {
        "scene_demo",
        "scene_first"
    };

    public void StartGame()
    {
        ServiceHolder.Instance.Get<IMessageService>().Clear();
        SceneManager.LoadScene(_scenes[0]);
    }

    private string _endingScene = "ending";

    public void ReloadScene()
    {
        ServiceHolder.Instance.Get<IMessageService>().Clear();
        SceneManager.LoadScene(_scenes[_currentScene]);
    }

    public void GoToNextScene()
    {
        ServiceHolder.Instance.Get<IMessageService>().Clear();
        _currentScene += 1;
        if (_scenes.Length <= _currentScene)
        {
            SceneManager.LoadScene(_endingScene);
        }
        else
        {
            SceneManager.LoadScene(_scenes[_currentScene]);
        }
    }
}
