using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionSelectPlayer : MonoBehaviour
{
    [SerializeField] private CharacterData whipPlayerData; 
    [SerializeField] private CharacterData gunPlayerData; 

    public void OnWhipPlayerButtonClick()
    {
        Player player = Player.GetInstance();
        if (player != null)
        {
            player.SetCharacterData(whipPlayerData);
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Player instance is null!");
        }
    }

    public void OnGunPlayerButtonClick()
    {
        Player player = Player.GetInstance();
        if (player != null)
        {
            player.SetCharacterData(gunPlayerData);
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Player instance is null!");
        }
    }
}
