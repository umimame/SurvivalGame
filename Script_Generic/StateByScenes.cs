using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using My;

namespace My
{
    public class StateByScenes : MonoBehaviour
    {
        [SerializeField] private List<string> scenes = new List<string>();
        [field: SerializeField] public GameState state { get; set; }
        private void Start()
        {
            Initialize();
            SceneManager.sceneLoaded += OnStateChangeBySceneLoaded;
        }

        private void Update()
        {

        }

        public void OnStateChangeBySceneLoaded(Scene scene, LoadSceneMode mode)
        {
            int scenesIndex = -1;
            for (int i = 0; i < scenes.Count; ++i)
            {
                if (scene.name == scenes[i])
                {
                    scenesIndex = i;
                    break;
                }
            }

            switch (scenesIndex)
            {
                case 0:
                    state = GameState.Title;
                    break;
                case 1:
                    state = GameState.CharaSelect;
                    break;
                case 2:
                    state = GameState.GameStart;
                    break;
                case 3:
                    state = GameState.Result;
                    break;

                default:
                    Debug.LogWarning("例外シーン");
                    state = GameState.Non;
                    break;
            }
        }

        public void Initialize()
        {
            int scenesIndex = -1;
            for (int i = 0; i < scenes.Count; ++i)
            {
                if (SceneManager.GetActiveScene().name == scenes[i])
                {
                    scenesIndex = i;
                    break;
                }
            }

            switch (scenesIndex)
            {
                case 0:
                    state = GameState.Title;
                    break;
                case 1:
                    state = GameState.CharaSelect;
                    break;
                case 2:
                    state = GameState.GameStart;
                    break;
                case 3:
                    state = GameState.Result;
                    break;

                default:
                    Debug.LogWarning("例外シーン");
                    state = GameState.Non;
                    break;
            }
        }


    }

    public enum GameState
    {
        Title,
        CharaSelect,
        GameStart,
        InGame,
        GameEnd,
        Result,
        Non,
    }
}
public enum SceneState
{

    Start,
    Idol,
    Next,
}