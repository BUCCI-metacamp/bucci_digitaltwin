using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Factory;
using System;

public class SecenChange : MonoBehaviour
{
    public event Action<bool> ResetStart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadPlayScene(int scenIndex)  
    {
        SceneManager.LoadScene(scenIndex, LoadSceneMode.Single);
    }

    public void ObjectSetActive(GameObject obj)
    {
        bool check = obj.activeSelf;
        string str = string.Format("{0}", check);       
        obj.SetActive(!check);
    }   

    public void OpenWebsite()
    {
        Application.OpenURL("http://158.247.241.162/");
    }

    public void FastSpeed(float speed) 
    {

        Time.timeScale = speed;
    }

    public void ResetButton()
    {
        MainValue.Instance.sucessCount = 0;
        // ���� �ִ� ��� Chip ������Ʈ ã��
        Chip[] chips = FindObjectsOfType<Chip>();
        foreach (Chip chip in chips)
        {
            // Chip�� �پ� �ִ� ���� ������Ʈ ����
            Destroy(chip.gameObject);
        }

        ResetStart?.Invoke(false);
    }


}
