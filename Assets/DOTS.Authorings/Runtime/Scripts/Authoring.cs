using Unity.Entities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DOTS.Authorings
{
    public abstract class Authoring : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private Authoring _prefab;

        public Authoring Prefab => _prefab;

        public Entity PrefabEntity { get; private set; }
        public Entity Entity { get; private set; }

        public virtual void Bake(Entity entity, IBaker baker) { }
        public virtual void Initialize(Entity entity, EntityManager entityManager) { }

        protected virtual void Start()
        {
            if (!_prefab || !AuthoringDatabase.Prefabs.TryGetValue(_prefab, out var prefabEntity))
            {
                Debug.LogError("Cannot initialize entity", gameObject);
                return;
            }

            var world = World.DefaultGameObjectInjectionWorld;
            var entityManager = world.EntityManager;
            PrefabEntity = prefabEntity;
            Entity = entityManager.Instantiate(PrefabEntity);
            entityManager.SetName(Entity, name);
            entityManager.AddComponentObject(Entity,
                new AuthoringInstanceData { PrefabEntity = PrefabEntity, Object = this });

            Initialize(Entity, entityManager);
        }

        public virtual void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (!this || EditorApplication.isUpdating ||
                PrefabUtility.GetPrefabAssetType(this) == PrefabAssetType.NotAPrefab)
            {
                return;
            }

            _prefab = PrefabUtility.GetCorrespondingObjectFromSource(this);
#endif
        }

        public virtual void OnAfterDeserialize() { }
    }
}