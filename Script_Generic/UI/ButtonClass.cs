using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClass : MonoBehaviour
{
    protected private Button button;
    [SerializeField] private Instancer instancer;
    [field: SerializeField] public string scene;
    [field: SerializeField] public TextMeshProUGUI buttonText { get; set; }

    /// <summary>
    /// Ž©“®‚ÅButtonOnClick‚ð’Ç‰Á
    /// </summary>
    public virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonOnClick);
        buttonText = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void Initialize()
    {
        
    }
    public virtual void ButtonOnClick()
    {
        ButtonSelectNull();
    }

    public void ButtonSelectNull()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void Instance()
    {
        instancer.Instance();
    }
    public void CloseCanvas(GameObject closeCanvas)
    {
        Destroy(closeCanvas);
    }
    
    public void SceneLoad()
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}