﻿using System;
using UI.Animators.WindowScaler;
using UI.Controls.Buttons.Selectable;
using UI.Windows.Base.WindowView;
using UnityEngine;

namespace UI.Windows.Implementations.DeathWindow
{
    public class DeathWindowView : BaseWindowView
    {
        [SerializeField] private SelectableButton _respawnButton;
        [SerializeField] private SelectableButton _mainMenuButton;

        private GameObject _background;
        private WindowScaler _windowScaler;
        
        public void Construct(DeathWindowConfig config, GameObject background)
        {
            _respawnButton.Construct(config.RespawnButtonConfig);
            _mainMenuButton.Construct(config.MainMenuButtonConfig);
            
            _windowScaler = new WindowScaler(transform, config.WindowScalerConfig);
            
            _background = background;
            EnableAnimators();
        }
        
        public override async void Open()
        {
            _background.SetActive(true);
            base.Open();
            await _windowScaler.ScaleOnWindowOpen();
        }

        public override async void Close()
        {
            await _windowScaler.ScaleOnWindowClosed();
            _background.SetActive(false);
            base.Close();
        }
        
        public event Action RespawnButtonClicked;
        public event Action MainMenuButtonClicked;

        protected override void SubscribeControls()
        {
            _respawnButton.Cliked += OnRespawnButtonClick;
            _mainMenuButton.Cliked += OnMainMenuButtonClick;
        }

        protected override void UnsubscribeControls()
        {
            _respawnButton.Cliked -= OnRespawnButtonClick;
            _mainMenuButton.Cliked -= OnMainMenuButtonClick;
        }

        protected override void EnableAnimators() => 
            _windowScaler?.Enable();

        protected override void DisableAnimators() => 
            _windowScaler?.Disable();

        private void OnRespawnButtonClick() => 
            RespawnButtonClicked?.Invoke();

        public void OnMainMenuButtonClick() =>
            MainMenuButtonClicked?.Invoke();
    }
}