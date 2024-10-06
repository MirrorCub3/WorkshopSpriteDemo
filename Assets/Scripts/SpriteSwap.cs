using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class SpriteSwap : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button pixelButton;
    [SerializeField] private Button rasterButton;

    [Header("Sprite Libraries")]
    [SerializeField] private SpriteLibrary playerSL;
    [SerializeField] private SpriteResolver playerSR;
    [SerializeField] private SpriteLibraryAsset pixelSprites;
    [SerializeField] private SpriteLibraryAsset rasterSprites;

    private void Awake()
    {
        pixelButton.onClick.AddListener(OnPixelButtonPressed);
        rasterButton.onClick.AddListener(OnRasterButtonPressed);
    }

    private void OnPixelButtonPressed()
    {
        playerSL.spriteLibraryAsset = pixelSprites;
    }

    private void OnRasterButtonPressed()
    {
        playerSL.spriteLibraryAsset = rasterSprites;
    }
}
