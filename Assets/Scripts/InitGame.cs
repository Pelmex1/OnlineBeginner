/*using CustomEventBus;
using System;
using Game.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public class InitGame : MonoBehaviour
    {
        [SerializeField] private UserInterface _userInterface;
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private Player _player;
        [SerializeField] private SpawnController _spawnController;

        private EventBus _eventBus;
        private ScoreController _scoreController;

        private List<IDisposable> _disposables = new();

        private void Awake()
        {
            _eventBus = new EventBus();
            _scoreController = new ScoreController();

            RegisterServices();
            Init();
            AddDisposables();
        }

        private void RegisterServices()
        {
            ServiceLocator.Initialize();
            ServiceLocator.Current.Register(_eventBus);
            ServiceLocator.Current.Register(_scoreController);
            ServiceLocator.Current.Register<SceneController>(_sceneController);
            ServiceLocator.Current.Register<UserInterface>(_userInterface);
            ServiceLocator.Current.Register<Player>(_player);
            ServiceLocator.Current.Register<SpawnController>(_spawnController);
        }

        private void Init()
        {
            _scoreController.Init();
            _sceneController.Init();
            _userInterface.Init();
            _player.Init();
            _spawnController.Init();
        }

        private void AddDisposables()
        {
            _disposables.Add(_scoreController);
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}*/