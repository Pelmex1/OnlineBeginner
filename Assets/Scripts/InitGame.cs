using System;
using System.Collections.Generic;
using CustomEventBus;
using UnityEngine;

namespace Game.Level
{
    public class InitGame : MonoBehaviour
    {
        [SerializeField] private GameMenu _gameMenu;
        [SerializeField] private MapGenerator _spawnController;
        [SerializeField] private InGameScene _inGameScene;
        [SerializeField] ParticleSystem[] _fireworks = new ParticleSystem[2];

        private EventBus _eventBus;

        private List<IDisposable> _disposables = new();

        private void Awake()
        {
            _eventBus = new EventBus();

            RegisterServices();
            Init();
            AddDisposables();
        }
        private void OnEnable() {
            _eventBus.Subscribe<EndGame>(InitEndGame);
        }
        private void OnDisable() {
            _eventBus.Unsubscribe<EndGame>(InitEndGame);
        }
        private void InitEndGame(EndGame endGame)
        {
            endGame.Init(_fireworks);
        }
        private void RegisterServices()
        {
            ServiceLocator.Initialize();
            ServiceLocator.Current.Register(_eventBus);
/*             ServiceLocator.Current.Register<GameMenu>(_gameMenu);
            ServiceLocator.Current.Register<PlayerWalk>(_player);
            ServiceLocator.Current.Register<MapGenerator>(_spawnController);
            ServiceLocator.Current.Register<EndGame>(_endGame); */
        }

        private void Init()
        {
            _gameMenu.Init();
            _spawnController.Init();
            _inGameScene.Init();
        }

        private void AddDisposables()
        {
            //_disposables.Add(_scoreController);
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}