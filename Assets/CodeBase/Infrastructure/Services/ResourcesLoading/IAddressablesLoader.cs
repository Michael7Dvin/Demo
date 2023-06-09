using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.ResourcesLoading
{
    public interface IAddressablesLoader
    {
        UniTask<GameObject> LoadGameObject(AssetReferenceGameObject assetReference);
        UniTask<T> LoadComponent<T>(AssetReferenceGameObject assetReference) where T : Component;
        void ClearCache();
    }
}