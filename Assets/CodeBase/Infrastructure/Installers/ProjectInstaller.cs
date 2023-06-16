﻿using Gameplay.Hero;
using Gameplay.PlayerCamera;
using Infrastructure.GameFSM;
using Infrastructure.GameFSM.States;
using Infrastructure.LevelLoading.LevelServicesProviding;
using Infrastructure.Services.AssetProviding.Addresses;
using Infrastructure.Services.AssetProviding.Common;
using Infrastructure.Services.AssetProviding.ForCamera;
using Infrastructure.Services.AssetProviding.UI;
using Infrastructure.Services.Input.Service;
using Infrastructure.Services.Instantiating;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Pause;
using Infrastructure.Services.ResourcesLoading;
using Infrastructure.Services.SceneLoading;
using Infrastructure.Services.StaticDataProviding;
using Infrastructure.Services.Updater;
using UI;
using UI.Services.Factory;
using UI.Services.Mediating;
using UI.Services.WindowsOperating;
using UI.Windows.Factory;
using UnityEngine;
using Zenject;
using IInstantiator = Infrastructure.Services.Instantiating.IInstantiator;

namespace Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private AllAssetsAddresses _allAssetsAddresses;
        [SerializeField] private ScenesData _scenesData;
        
        [SerializeField] private HeroConfig _heroConfig;
        [SerializeField] private PlayerCameraConfig _playerCameraConfig;
        [SerializeField] private UIConfig _uiConfig;

        public override void InstallBindings()
        {
            BindServices();
            BindProvidingServices();
            BindGameStateMachine();
            BindUI();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();

            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<InitializationState>().AsSingle();
            Container.Bind<MainMenuState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameplayState>().AsSingle();
            Container.Bind<QuitState>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<ICustomLogger>().To<CustomLogger>().AsSingle();
            Container.Bind<IInputService>().To<InputService>().AsSingle();
            Container.Bind<IInstantiator>().To<Instantiator>().AsSingle();
            Container.Bind<IPauseService>().To<PauseService>().AsCached();
            Container.Bind<IResourcesLoader>().To<ResourcesLoader>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<Updater>().AsSingle();
        }

        private void BindProvidingServices()
        {
            Container
                .Bind<IStaticDataProvider>()
                .To<StaticDataProvider>()
                .AsSingle()
                .WithArguments(_allAssetsAddresses, _scenesData, _heroConfig, _uiConfig, _playerCameraConfig);
            
            Container.Bind<ILevelServicesProvider>().To<LevelServicesProvider>().AsSingle();

            Container.Bind<ICommonAssetsProvider>().To<CommonAssetsProvider>().AsSingle();
            Container.Bind<ICameraAssetsProvider>().To<CameraAssetsProvider>().AsSingle();
            Container.Bind<IUIAssetsProvider>().To<UIAssetsProvider>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<IMediator>().To<Mediator>().AsSingle();
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();
            Container.Bind<IWindowsService>().To<WindowsService>().AsSingle();
        }
    }
}