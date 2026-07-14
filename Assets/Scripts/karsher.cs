using UnityEngine;
using UnityEngine.InputSystem;

public class Karsher : MonoBehaviour
{
    [SerializeField] private float _raycastLength = 5f;
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _clickAction;

    private void Awake()
    {
        _clickAction = _playerInput.actions["Click"];
    }

    private void Update()
    {
        if (_clickAction.IsPressed())
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward * _raycastLength, Color.red);
        
            if (Physics.Raycast(transform.position, transform.forward, out hit, _raycastLength))
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();
                MeshCollider meshCollider = hit.collider as MeshCollider;
                Debug.Log(hit.transform.name);
                
                if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null || hit.collider.tag != "isDirty")
                {
                    Debug.Log("CANT");
                    return;
                }
                
                Texture2D tex = rend.sharedMaterial.mainTexture as Texture2D;
                Vector2 pixelUV = hit.textureCoord;
                Debug.Log($"x = {pixelUV.x}, y = {1-pixelUV.y}");//1-yUV car inversion sur blender, juste pour vérif les coordonnées
        
            }
            else Debug.Log("nothing");
        }
    }

    private void OnUse()
    {
        Debug.Log("Use");
    }

    private void OnMove(InputValue value)
    {
        Debug.Log("OnMove");
    }
}
