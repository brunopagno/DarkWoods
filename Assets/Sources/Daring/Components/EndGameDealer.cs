using System;
using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameDealer : MonoBehaviour
{
    private IMessageService _messageService;
    private Animator _animator;

    public bool ReadyToMakeStuffHappen = false;
    private SceneMutreta _sceneMutreta;
    private bool _won;

    public void Awake()
    {
        _messageService = ServiceHolder.Instance.Get<IMessageService>();
        _sceneMutreta = ServiceHolder.Instance.Get<SceneMutreta>();
        _messageService.AddHandler<EndGameMessage>(OnEndGameMessage);
        _animator = GetComponent<Animator>();
    }

    private void OnEndGameMessage(EndGameMessage mess)
    {
        if (mess.Victory)
        {
            _animator.SetTrigger("win");
        }
        else
        {
            _animator.SetTrigger("defeat");
        }
        
        _won = mess.Victory;
        ReadyToMakeStuffHappen = true;
    }

    public void Update()
    {
        if (ReadyToMakeStuffHappen)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (_won)
                {
                    _sceneMutreta.GoToNextScene();
                }
                else
                {
                    _sceneMutreta.ReloadScene();
                }
            }
        }
    }
}
