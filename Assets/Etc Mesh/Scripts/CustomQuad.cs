using UnityEngine;

namespace vnc.Utilities
{
    public class CustomQuad : MonoBehaviour
    {
        public Rect rect;
        public Material customMaterial;

        public void Create(Vector3 rectPos, Vector3 rectSize)
        {
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Mesh mesh = quad.GetComponent<MeshFilter>().mesh;

            var vertices = new Vector3[]
            {
                    rectPos, //
                    rectPos + rectSize,
                    new Vector2(rectPos.x + rectSize.x, rectPos.y),
                    new Vector2(rectPos.x, rectPos.y + rectSize.y)
            };
            mesh.vertices = vertices;

            // Fix collider
            DestroyImmediate(quad.GetComponent<MeshCollider>());
            quad.AddComponent<MeshCollider>();

            if (customMaterial != null)
            {
                var renderer = quad.GetComponent<MeshRenderer>();
                renderer.material = customMaterial;
            }
        }
    }
}

