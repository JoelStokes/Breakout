using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DropShadow : MonoBehaviour
{
    public Vector2 ShadowOffset;
    public Material ShadowMaterial;
    public bool parentlessRotation = false;
    public bool parentlessPosition = false;

    SpriteRenderer spriteRenderer;
    GameObject shadowGameobject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //create a new gameobject to be used as drop shadow
        shadowGameobject = new GameObject("Shadow");

        //create a new SpriteRenderer for Shadow gameobject
        SpriteRenderer shadowSpriteRenderer = shadowGameobject.AddComponent<SpriteRenderer>();

        //set the shadow gameobject's sprite to the original sprite
        shadowSpriteRenderer.sprite = spriteRenderer.sprite;
        //set the shadow gameobject's material to the shadow material we created
        shadowSpriteRenderer.material = ShadowMaterial;

        //update the sorting layer of the shadow to always lie behind the sprite
        shadowSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;

        shadowGameobject.transform.SetParent(transform);
        shadowGameobject.transform.localRotation = Quaternion.Euler(0,0,0);
        shadowGameobject.transform.localScale = new Vector3(1,1,1);
        shadowGameobject.transform.localPosition = new Vector3(ShadowOffset.x, ShadowOffset.y, shadowGameobject.transform.position.z);

        if (parentlessRotation || parentlessPosition){
            shadowGameobject.transform.parent = null;
        }
    }

    private void LateUpdate() {
        if (parentlessRotation){
            shadowGameobject.transform.rotation = transform.rotation;
        }

        if (parentlessPosition){
            shadowGameobject.transform.position = new Vector3(transform.position.x + ShadowOffset.x, transform.position.y + ShadowOffset.y, shadowGameobject.transform.position.z);
        }
    }
}