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
    /// Whether or not the player can change the material of this surface
    /// </summary>
    public bool changeable;

    /// <summary>
    /// Type of material on this surface
    /// </summary>
    public GameController.material type;

    /// <summary>
    /// Reference to player
    /// </summary>
    protected Player player;

    /// <summary>
    /// Bouncy material
    /// </summary>
    public Material bounceMaterial;

    /// <summary>
    /// Slick material
    /// </summary>
    public Material slickMaterial;

    /// <summary>
    /// Sticky material
    /// </summary>
    public Material stickyMaterial;

    /// <summary>
    /// Null material
    /// </summary>
    public Material nullMaterial;

    void Start()
    {
        if(GetComponent<Tilemap>() != null) {
            Debug.Log(name + ": found tilemap, won't allow material changing on this surface");
            // changeable = false;
        }
        else {
            ChangeMaterial(type);
        }
        player = GameObject.FindWithTag("GameController").GetComponent<GameController>().player;
    }

    /// <summary>
    /// Set tiling parameters on material to work with associated material texture
    /// </summary>
    /// <param name="material">Material type</param>
    void SetMaterialTiling(GameController.material material)
    {
        // Slip material has regular tiling
        if(material.Equals(GameController.material.SLIP)) {
            GetComponent<Renderer>().material.mainTextureScale = transform.localScale;
        }
        // All other materials are in weird grid form (material texture image should be changed so we don't have to do this)
        else {
            GetComponent<Renderer>().material.mainTextureScale = transform.localScale / 3;
        }
        
    }

    /// <summary>
    /// Initializes surface with associated move speeds from surfaceSpeeds (called from derived class)
    /// </summary>
    protected void InitializeSurfaceSpeeds(GameController.material materialType)
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
            if (Input.GetMouseButton(0))
            {
                ChangeMaterial(player.equippedMaterial);   
            }
            // Right click
            else if (Input.GetMouseButton(1))
            {
                ChangeMaterial(GameController.material.NONE);
            }
        }
    }

    /// <summary>
    /// Changes appearance and behavior of material
    /// </summary>
    void ChangeMaterial(GameController.material material)
    {
        type = material;

        // Change appearance of material
        switch(material)
        {
            case GameController.material.BOUNCE:
                GetComponent<Renderer>().material = bounceMaterial;
                break;
            case GameController.material.SLIP:
                GetComponent<Renderer>().material = slickMaterial;
                break;
            case GameController.material.STICK:
                GetComponent<Renderer>().material = stickyMaterial;
                break;
            case GameController.material.NONE:
                GetComponent<Renderer>().material = nullMaterial;
                break;
        }
        
        // Set tiling to fit to surface dimensions
        SetMaterialTiling(material);

        // Set move speeds
        InitializeSurfaceSpeeds(material);
    }
}