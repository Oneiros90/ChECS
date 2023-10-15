using Unity.Entities;
using UnityEngine;

namespace CHECS
{
    public class AuthoringPrefabData : IComponentData
    {
        public Authoring ObjectPrefab;
    }
    
    public class AuthoringInstanceData : IComponentData
    {
        public Entity PrefabEntity;
        public Authoring Object;
    }
}