using System.Linq;
using Unity.Entities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DOTS.Authorings
{
    public class AuthoringsBaker : MonoBehaviour
    {
        [SerializeField]
        private Authoring[] _authorings = { };

#if UNITY_EDITOR
        [ContextMenu("Add all authorings in project")]
        private void AddAllAuthoringsInProject()
        {
            _authorings = AssetDatabase.FindAssets($"t:{nameof(GameObject)}").Select(AssetDatabase.GUIDToAssetPath)
                                       .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                                       .Select(p => p.GetComponent<Authoring>()).Where(a => a).ToArray();
            EditorUtility.SetDirty(this);
        }
#endif

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