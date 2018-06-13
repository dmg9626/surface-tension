using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SurfaceMaterial : MonoBehaviour
{
    /// <summary>
    /// Top speed the player can move on this surface
    /// </summary>
    public GameController.SurfaceSpeeds surfaceSpeeds;

    /// <summary>
    /// GameController
    /// </summary>
    public GameController gameController;

    /// <summary>
    /// Whether or not the player can change the material of this surface
    /// </summary>
    public bool changeable;

    /// <summary>
    /// Type of material on this surface
    /// </summary>
    public GameController.materialType type;

    /// <summary>
    /// Reference to player
    /// </summary>
    protected Player player;

    void Start()
    {
        // Find game controller and player
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        player = gameController.GetComponent<GameController>().player;

        if(GetComponent<Tilemap>() == null && tag == "Ground") {
            ChangeSurface(type);

            // stupid hacky way to keep renderer from displaying a Default Standard Material in the preview material slot (renderer.materials[1])
            // standard materials aren't included in the preloaded shaders in Project Settings > Graphics, so this is my best workaround here
            PreviewMaterial(gameController.GetMaterial(GameController.materialType.STICK));
        }
        else {
            InitializeSurfaceSpeeds(type);
        }
    }

    /// <summary>
    /// Set tiling parameters on material to work with associated material texture
    /// </summary>
    /// <param name="materialType">Material type</param>
    /// <param name="material">Material</param>
    void SetMaterialTiling(GameController.materialType materialType, Material material)
    {
        // Slip material has regular tiling
        if(materialType.Equals(GameController.materialType.SLIP)) {
            material.mainTextureScale = transform.localScale;
        }
        // All other materials are in weird grid form (material texture image should be changed so we don't have to do this)
        else {
            material.mainTextureScale = transform.localScale / 3;
        }
    }

    /// <summary>
    /// Initializes surface with associated move speeds from surfaceSpeeds (called from derived class)
    /// </summary>
    protected void InitializeSurfaceSpeeds(GameController.materialType materialType)
    {
        // Initialize surface speeds
		surfaceSpeeds = GameObject.FindWithTag("GameController").GetComponent<GameController>().speedMapping[materialType];
    }

    /// <summary>
    /// Right click reverts material to original color, left click assigns it the player's equipped color
    /// </summary>
    void OnMouseOver()
    {
        if(changeable) {
            // Left click
            if (Input.GetMouseButtonDown(0))
            {
                ChangeSurface(player.equippedMaterial);   
            }
            // Right click
            else if (Input.GetMouseButtonDown(1))
            {
                ChangeSurface(GameController.materialType.NONE);
            }
            if(type != player.equippedMaterial) {

                // Overlay highlight material on surface
                PreviewMaterial(gameController.GetMaterial(player.equippedMaterial, true));

                // Set texture tiling to 1, 1 to handle wonky highlight texture
                GetComponent<Renderer>().materials[1].mainTextureScale = new Vector2(1,1);
            }
        }
    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit()
    {
        if(changeable)
        {
            // Restore material (sets overlay material to null)
            PreviewMaterial(gameController.GetMaterial(GameController.materialType.STICK, true));

            Material[] materials = GetComponent<Renderer>().materials;

            SetMaterialTiling(GameController.materialType.SLIP, GetComponent<Renderer>().materials[1]);
        }
    }

    /// <summary>
    /// Creates transparent preview overlay of equipped material
    /// </summary>
    /// <param name="materialType">Material type</param>
    void PreviewMaterial(Material previewMaterial)
    {
        // Set preview material to 2nd slot in Renderer.materials
        Material[] materials = GetComponent<Renderer>().materials;
        if(previewMaterial == null) {
            Debug.Log("Setting preview material alpha = 0");
            Color color = materials[1].color;
            color.a = 0f;
        }
        else {
            materials[1] = previewMaterial;
        }
        
        GetComponent<Renderer>().materials = materials;

        // SetMaterialTiling(GameController.materialType.NONE, GetComponent<Renderer>().materials[1]);
    }

    /// <summary>
    /// Changes material being displayed on renderer
    /// </summary>
    /// <param name="material"></param>
    void ChangeMaterial(Material material)
    {
        Material[] materials = GetComponent<Renderer>().materials;
        materials[0] = material;
        GetComponent<Renderer>().materials = materials;
    }

    /// <summary>
    /// Changes appearance and behavior of surface to match given material type
    /// </summary>
    /// <param name="materialType">Material type</param>
    void ChangeSurface(GameController.materialType materialType)
    {
        // Set material type
        type = materialType;

        // Set material on renderer
        ChangeMaterial(gameController.GetMaterial(materialType));
        
        // Set tiling to fit to surface dimensions
        SetMaterialTiling(materialType, GetComponent<Renderer>().material);

        // Set move speeds
        InitializeSurfaceSpeeds(materialType);
    }
}