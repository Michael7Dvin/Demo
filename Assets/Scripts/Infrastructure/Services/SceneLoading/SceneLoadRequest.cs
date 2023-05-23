﻿using System;

namespace Infrastructure.Services.SceneLoading
{
    public struct SceneLoadRequest
    {
        public SceneLoadRequest(string sceneName, Action onLoad)
        {
            SceneName = sceneName;
            OnLoad = onLoad;
        }

        public string SceneName { get; }
        public Action OnLoad { get; }
    }
}