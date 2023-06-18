﻿using TMPro;
using UI.Animators;
using UI.Animators.OnClickScaler;
using UnityEngine;

namespace UI.Elements.Buttons.SaveSlot
{
    public class SaveSlotButton : BaseButton
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        private OnClickScaler _onClickScalerAnimation;
        private ChangeTextOnSelect _changeTextOnSelectAnimation;
        private ChangeColorOnSelect _changeColorOnSelectAnimation;
        
        public void Construct(SaveSlotButtonConfig config)
        {
            _onClickScalerAnimation = new OnClickScaler(transform, Events, config.OnClickScalerConfig);

            _changeTextOnSelectAnimation = new ChangeTextOnSelect(_text,
                Events,
                config.OnSelectedText, 
                config.OnUnelectedText);
            
            _changeColorOnSelectAnimation = new ChangeColorOnSelect(_text,
                Events,
                config.SelectedColorGradient,
                config.UnselectedColorGradient);
            
            _onClickScalerAnimation.Enable();
            _changeTextOnSelectAnimation.Enable();
            _changeColorOnSelectAnimation.Enable();
        }

        private bool IsAnimationsCreated => _onClickScalerAnimation != null 
            && _changeTextOnSelectAnimation != null 
            && _changeColorOnSelectAnimation != null;

        protected override void OnEnable()
        {
            if (IsAnimationsCreated == true)
            {
                _onClickScalerAnimation.Enable();
                _changeTextOnSelectAnimation.Enable();
                _changeColorOnSelectAnimation.Enable();                
            }

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            _onClickScalerAnimation.Disable();
            _changeTextOnSelectAnimation.Enable();
            _changeColorOnSelectAnimation.Disable();
            base.OnDisable();
        }
    }
}