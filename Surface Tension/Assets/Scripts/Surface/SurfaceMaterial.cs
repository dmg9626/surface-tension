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
            ChangeMaterial(type);
            Debug.Log(name + " setting second material null");
            GetComponent<Renderer>().materials[1] = null;
        }
        else {
            Debug.Log(name + ": found tilemap, won't allow material changing on this surface");
            InitializeSurfaceSpeeds(type);
            // changeable = false;
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
                ChangeMaterial(player.equippedMaterial);   
            }
            // Right click
            else if (Input.GetMouseButtonDown(1))
            {
                ChangeMaterial(GameController.materialType.NONE);
            }
            if(type != player.equippedMaterial) {
                // PreviewMaterial(gameController.GetMaterial(player.equippedMaterial, true));
                // SetMaterialTiling(GameController.materialType.NONE, GetComponent<Renderer>().materials[1]);
                PreviewMaterial(gameController.highlightMaterial);
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
            Debug.Log("Removing material preview");
            // Restore material (sets overlay material to null)
            PreviewMaterial(gameController.GetMaterial(GameController.materialType.NONE, true));
        }
    }

    /// <summary>
    /// Creates transparent preview overlay of equipped material
    /// </summary>
    /// <param name="materialType">Material type</param>
    void PreviewMaterial(Material previewMaterial)
    {
        // Get preview material
        // Material previewMaterial = gameController.GetMaterial(materialType, true);
        
        // Set preview material to 2nd slot in Renderer.materials
        Material[] materials = GetComponent<Renderer>().materials;
        materials[1] = previewMaterial;
        GetComponent<Renderer>().materials = materials;

        // SetMaterialTiling(GameController.materialType.NONE, GetComponent<Renderer>().materials[1]);
    }

    /// <summary>
    /// Changes appearance and behavior of material
    /// </summary>
    /// <param name="materialType">Material type</param>
    void ChangeMaterial(GameController.materialType materialType)
    {
        Debug.Log("Changing material to " + materialType);

        type = materialType;

        GetComponent<Renderer>().material = gameController.GetMaterial(materialType);
        
        // Set tiling to fit to surface dimensions
        SetMaterialTiling(materialType, GetComponent<Renderer>().material);

        // Set move speeds
        InitializeSurfaceSpeeds(materialType);
    }
}