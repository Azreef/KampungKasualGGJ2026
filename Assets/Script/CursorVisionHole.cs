using UnityEngine;

public class CursorVisionHole : MonoBehaviour
{
    [SerializeField] private Material visionMaterial;

    void Update()
    {
        Vector2 mouse = Input.mousePosition;

        Vector2 uv = new Vector2(
            mouse.x / Screen.width,
            mouse.y / Screen.height
        );

        visionMaterial.SetVector("_HoleCenter",
            new Vector4(uv.x, uv.y, 0, 0));
    }
}
