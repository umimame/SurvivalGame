using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPlayer : MonoBehaviour
{
    [SerializeField] int kill, score, apple, orange, grape;
    private static ResultPlayer instance; // �V���O���g���C���X�^���X
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("�V���O���g���ɂ�����");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("ResultPlayer�ŃV���O���g����j��������");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            Debug.Log("AddNownOwnoW");
            kill += 5;
            score += 100;
            apple += 1;
            orange += 2;
            grape += 3;
        }
    }
    //public int GetScore() { return score; }
    //public int GetApple() { return apple; }
    //public int GetOrange() { return orange; }
    //public int GetGrape() { return grape; }
    //public int GetKill() { return kill; }    
    // �V���O���g���̉����i�j���j���\�b�h
    public void ReleaseSingleton()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }      
}
