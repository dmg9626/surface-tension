using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// List of our surfaces as enums
    public enum material
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
    public Dictionary<material, SurfaceSpeeds> speedMapping = new Dictionary<material, SurfaceSpeeds> {
        { material.NONE, new SurfaceSpeeds {
            defaultSpeed = 4f,
            upSlopeSpeed = 2.5f,
            pushSpeed = 1.5f,
            pullSpeed = 1f
        }},
        { material.SLIP, new SurfaceSpeeds {
            defaultSpeed = 8f,
            upSlopeSpeed = 6.5f,
            pushSpeed = 5.5f,
            pullSpeed = 5.5f
        }},
        { material.BOUNCE, new SurfaceSpeeds {
            defaultSpeed = 4f,
            upSlopeSpeed = 2.5f,
            pushSpeed = 1.5f,
            pullSpeed = 1.5f
        }},
        { material.STICK, new SurfaceSpeeds {
            defaultSpeed = 2f,
            upSlopeSpeed = .5f,
            pushSpeed = 0,
            pullSpeed = 0
        }}
    };

    /// <summary>
    /// Mapping of materials to player trail color
    /// </summary>
    public Dictionary<material, materialTrail> colorMapping = new Dictionary<material, materialTrail>() {
        { material.BOUNCE, new materialTrail {
            startColor = Color.green,
            endColor = Color.green,
            startAlpha = 1F,
            endAlpha = 1f
        } },
        { material.SLIP, new materialTrail {
            startColor = Color.blue,
            endColor = Color.blue,
            startAlpha = 1F,
            endAlpha = 1f
        } },
        { material.STICK, new materialTrail {
            startColor = Color.yellow,
            endColor = Color.yellow,
            startAlpha = 1F,
            endAlpha = 1f
        } },
        { material.NONE, new materialTrail {
            startColor = Color.white,
            endColor = Color.white,
            startAlpha = 0F,
            endAlpha = 0f
        } }
    };

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
    }
}
