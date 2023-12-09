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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            kill += 5;
            score += 100;
            apple += 1;
            orange += 2;
            grape += 3;
        }
    }

    public int GetScore() { return score; }
    public int GetApple() { return apple; }
    public int GetOrange() { return orange; }
    public int GetGrape() { return grape; }
    public int GetKill() { return kill; }
    public void SetScore()
    {
        score = 0;
        kill = 0;
        apple = 0;
        orange = 0;
        grape = 0;
    }

    // �V���O���g���̉����i�j���j���\�b�h
    public static void ReleaseSingleton()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }

    // �V�[�����؂�ւ��Ƃ��ɌĂ΂�郁�\�b�h
    private void OnSceneChanged(Scene current, Scene next)
    {
        if (next.name == "TitleScene")
        {
            ReleaseSingleton(); // �^�C�g����ʂɑJ�ڂ���Ƃ��ɃV���O���g��������
        }
    }

    private void OnEnable()
    {
        // �V�[�����؂�ւ�邽�т� OnSceneChanged ���\�b�h���Ă΂��悤�ɂ���
        //SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void OnDisable()
    {
        // �C�x���g�̉���
        //SceneManager.sceneLoaded -= OnSceneChanged;
    }
}
