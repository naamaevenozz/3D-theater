using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;  
    [SerializeField] private Button button;       

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(LoadScene);
        else
            Debug.LogError("LoadSceneButton: No Button component found!");
    }

    private void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("LoadSceneButton: sceneToLoad is empty!");
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
