using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CHECS
{
    public static class AuthoringDatabase
    {
        private static Dictionary<Authoring, Entity> _prefabs;

        public static IReadOnlyDictionary<Authoring, Entity> Prefabs
        {
            get
            {
                if (_prefabs == null)
                {
                    Initialize();
                }

                return _prefabs;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnLoad()
        {
            _prefabs = null;
        }

        private static void Initialize()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            var entityManager = world.EntityManager;
            var query = entityManager.CreateEntityQuery(new EntityQueryBuilder(Allocator.Temp)
                                                        .WithAll<Prefab, AuthoringPrefabData>()
                                                        .WithOptions(EntityQueryOptions.IncludePrefab));

            _prefabs = new Dictionary<Authoring, Entity>();
            foreach (var prefabEntity in query.ToEntityArray(Allocator.Temp).ToArray())
            {
                var authoringData = entityManager.GetComponentData<AuthoringPrefabData>(prefabEntity);
                if (authoringData.ObjectPrefab && authoringData.ObjectPrefab.TryGetComponent(out Authoring authoring))
                {
                    _prefabs[authoring] = prefabEntity;
                }
            }
        }
    }
}