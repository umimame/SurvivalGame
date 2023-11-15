using My;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
public class FrontCanvas : SingletonDontDestroy<FrontCanvas>
{
    [field: SerializeField] public bool debugMode { get; private set; }
    [SerializeField] SceneOperator sceneEditor;
    [SerializeField] GameObject[] editors;
    [field: SerializeField] public PresetsByPlayerType presets { get; private set; }
    [field: SerializeField] public AudioSource source { get; private set; }
    private void Start()
    {
        source = GetComponent<AudioSource>();
        FindSceneEditor();
        SceneManager.sceneLoaded += SceneChanged;
    }
    private void Update()
    {
        debugMode = sceneEditor.debugMode;
    }

    private void FindSceneEditor()
    {
        sceneEditor = GameObject.FindWithTag(Tags.SceneOperator).GetComponent<SceneOperator>();
     }

    private void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        FindSceneEditor();

    }
}