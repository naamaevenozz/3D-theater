using UnityEngine;

public class AspectRatioManager : MonoBehaviour
{
    [Header("Aspect Ratio Settings")]
    [SerializeField] private float targetAspectRatio = 16f / 9f;
    [SerializeField] private Color letterboxColor = Color.black;
    
    private Camera mainCamera;
    private int lastScreenWidth;
    private int lastScreenHeight;
    
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
        }
        
        if (mainCamera == null)
        {
            Debug.LogError("AspectRatioManager: No camera found! Please attach this script to a camera or ensure Camera.main is set.");
            return;
        }
        
        mainCamera.backgroundColor = letterboxColor;
        
        AdjustAspectRatio();
        
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }
    
    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            AdjustAspectRatio();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
        }
    }
    
    void AdjustAspectRatio()
    {
        if (mainCamera == null) return;
        
        float currentAspectRatio = (float)Screen.width / Screen.height;
        
        if (Mathf.Approximately(currentAspectRatio, targetAspectRatio))
        {
            mainCamera.rect = new Rect(0, 0, 1, 1);
        }
        else if (currentAspectRatio > targetAspectRatio)
        {
            float viewportWidth = targetAspectRatio / currentAspectRatio;
            float viewportX = (1f - viewportWidth) / 2f;
            
            mainCamera.rect = new Rect(viewportX, 0, viewportWidth, 1);
        }
        else
        {
            float viewportHeight = currentAspectRatio / targetAspectRatio;
            float viewportY = (1f - viewportHeight) / 2f;
            
            mainCamera.rect = new Rect(0, viewportY, 1, viewportHeight);
        }
        
        Debug.Log($"Aspect Ratio Manager: Screen {Screen.width}x{Screen.height} (ratio: {currentAspectRatio:F2}), Target ratio: {targetAspectRatio:F2}");
    }
    
    public void SetTargetAspectRatio(float newAspectRatio)
    {
        targetAspectRatio = newAspectRatio;
        AdjustAspectRatio();
    }
    
    public void SetLetterboxColor(Color newColor)
    {
        letterboxColor = newColor;
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = letterboxColor;
        }
    }
}
