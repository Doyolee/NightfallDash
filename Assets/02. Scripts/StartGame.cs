using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public PerkManager perk;
    public void GameStart()
    {
        LoadingSceneManager.LoadScene("GameScene");
    }
}
