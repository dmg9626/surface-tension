using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// List of our surfaces as enums
    public enum materialType
    {
        NONE,
        BOUNCE,
        SLIP,
        STICK
    }

	/// <summary>
	/// Reference to player
	/// </summary>
	public Player player;

    /// <summary>
    /// Mapping of materials to movement speeds
    /// </summary>
    /// <returns></returns>
    public Dictionary<materialType, SurfaceSpeeds> speedMapping = new Dictionary<materialType, SurfaceSpeeds> {
        { materialType.NONE, new SurfaceSpeeds {
            defaultSpeed = 4f,
            upSlopeSpeed = 2.5f,
            pushSpeed = 1.5f,
            pullSpeed = 1f
        }},
        { materialType.SLIP, new SurfaceSpeeds {
            defaultSpeed = 8f,
            upSlopeSpeed = 6.5f,
            pushSpeed = 5.5f,
            pullSpeed = 5.5f
        }},
        { materialType.BOUNCE, new SurfaceSpeeds {
            defaultSpeed = 4f,
            upSlopeSpeed = 2.5f,
            pushSpeed = 1.5f,
            pullSpeed = 1.5f
        }},
        { materialType.STICK, new SurfaceSpeeds {
            defaultSpeed = 2f,
            upSlopeSpeed = .5f,
            pushSpeed = 0,
            pullSpeed = 0
        }}
    };

    /// <summary>
    /// Mapping of materials to player trail color
    /// </summary>
    public Dictionary<materialType, materialTrail> colorMapping = new Dictionary<materialType, materialTrail>() {
        { materialType.BOUNCE, new materialTrail {
            startColor = new Color32(141, 249, 77, 255),
            endColor = new Color32(255, 233, 50, 255),
            startAlpha = 1F,
            endAlpha = .66F
        } },
        { materialType.SLIP, new materialTrail {
            startColor = new Color32(36, 236, 255, 255),
            endColor = new Color32(0, 41, 255, 255), // location: 52.6%
            // Additional color key: 158, 249, 253
            startAlpha = 1F,
            endAlpha = .83F // restore this value to default when adding additional color key
        } },
        { materialType.STICK, new materialTrail {
            startColor = Color.yellow,
            endColor = Color.yellow,
            startAlpha = 1F,
            endAlpha = .66F
        } },
        { materialType.NONE, new materialTrail {
            startColor = Color.white,
            endColor = Color.white,
            startAlpha = 0F,
            endAlpha = 0f
        } }
    };

    /// <summary>
    /// Mapping of materialType to material
    /// </summary>
    public Dictionary<materialType, Material> materialMapping;

    /// <summary>
    /// Mapping of materialType to preview overlay material
    /// </summary>
    public Dictionary<materialType, Material> previewMaterialMapping;

    /// <summary>
    /// Data for creating player trail particle effect for a material
    /// </summary>
    public struct materialTrail {
        public Color startColor;
        public Color endColor;
        public float startAlpha;
        public float endAlpha;
    }

    /// <summary>
    /// Data for plaeyr movement speeds on a material
    /// </summary>
    public struct SurfaceSpeeds {
        public float defaultSpeed;
        public float upSlopeSpeed;
        public float pushSpeed;
        public float pullSpeed;
    };

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

    /// <summary>
    /// Bounce material preview
    /// </summary>
    public Material bouncePreviewMaterial;

    /// <summary>
    /// Slick material preview
    /// </summary>
    public Material slickPreviewMaterial;

    /// <summary>
    /// Sticky material preview
    /// </summary>
    public Material stickyPreviewMaterial;

    /// <summary>
    /// Selection highlight material
    /// </summary>
    public Material highlightMaterial;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        // Initialize material mapping
        materialMapping = new Dictionary<materialType, Material>() {
            {  materialType.BOUNCE, bounceMaterial },
            {  materialType.SLIP, slickMaterial },
            {  materialType.STICK, stickyMaterial },
            {  materialType.NONE, nullMaterial }
        };

        // Initialize preview material mapping
        previewMaterialMapping = new Dictionary<materialType, Material>() {
            {  materialType.BOUNCE, bouncePreviewMaterial },
            {  materialType.SLIP, slickPreviewMaterial },
            {  materialType.STICK, stickyPreviewMaterial },
            {  materialType.NONE, null }
        };
    }

    /// <summary>
    /// Returns material of given type
    /// </summary>
    /// <param name="type">Type of material</param>
    /// <param name="preview">Returns transparent material overlay preview if true</param>
    public Material GetMaterial(GameController.materialType type, bool preview = false)
    {
        if(!preview)
        {
            return materialMapping[type];
        }
        else {
            return previewMaterialMapping[type];
        }
    }
}
