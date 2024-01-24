using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Operation : MonoBehaviour
{
    [SerializeField] private PlayerInput input;



    public string sceneName;
    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
    }

    private void NextScene()
    {
        SceneManager.LoadScene(sceneName);
    }


    #region PlayerInputÇ…ìoò^Ç≥ÇÍÇÈä÷êî
    private void OnPositive(InputValue inputValue)
    {
        NextScene();
    }

    private void OnNegative(InputValue inputValue)
    {

        NextScene();
    }

    private void OnPause(InputValue inputValue)
    {
        NextScene();

    }
    #endregion
}