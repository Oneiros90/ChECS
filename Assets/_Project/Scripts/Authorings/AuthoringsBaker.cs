using Unity.Entities;
using UnityEngine;

namespace CHECS
{
    public class AuthoringsBaker : MonoBehaviour
    {
        [SerializeField]
        private Authoring[] _authorings = { };

        private sealed class Baker : Baker<AuthoringsBaker>
        {
            public override void Bake(AuthoringsBaker authoringsBaker)
            {
                foreach (var authoring in authoringsBaker._authorings)
                {
                    if (!authoring)
                    {
                        return;
                    }

                    var prefabEntity = CreateAdditionalEntity(TransformUsageFlags.Dynamic, entityName: authoring.name);
                    AddComponent<Prefab>(prefabEntity);
                    AddComponentObject(prefabEntity, new AuthoringPrefabData { ObjectPrefab = authoring });
                    authoring.Bake(prefabEntity, this);
                }
            }
        }
    }
}