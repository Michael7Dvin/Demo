﻿using Cysharp.Threading.Tasks;
using UI.Windows;
using UI.Windows.Base.Window;

namespace UI.Services.Operating
{
    public interface IWindowsOperator
    {
        UniTask<IWindow> OpenWindow(WindowType type);
        void CloseWindow(WindowType type);
    }
}