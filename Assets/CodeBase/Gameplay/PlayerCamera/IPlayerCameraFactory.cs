using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.PlayerCamera
{
    public interface IPlayerCameraFactory
    {
        UniTask WarmUp();
        UniTask<Camera> Create(Transform hero);
    }
}