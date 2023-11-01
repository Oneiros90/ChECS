using Unity.Entities;
using UnityEngine;

namespace DOTS.Authorings
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