using My;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
public class FrontCanvas : SingletonDontDestroy<FrontCanvas>
{
    [SerializeField] private bool debugMode;
    [SerializeField] SceneEditor sceneEditor;
    [SerializeField] GameObject[] editors;
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
        sceneEditor = GameObject.FindWithTag("SceneEditor").GetComponent<SceneEditor>();
     }

    private void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        FindSceneEditor();
    }
}