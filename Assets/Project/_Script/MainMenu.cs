using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetCharacter(0);
    }

    public void SetCharacter(int index)
    {
        switch (index)
        {
            case 0:
            {
                GameManager.Instance.selectedCharacter = GameManager.Instance.characterStats.Find(element => element.characterType.GetType().Name == nameof(Character1));
                break;
            }
            case 1:
            {
                GameManager.Instance.selectedCharacter = GameManager.Instance.characterStats.Find(element => element.characterType.GetType().Name == nameof(Character2));
                break;
            }
            default:
            {
                GameManager.Instance.selectedCharacter = GameManager.Instance.characterStats.Find(element => element.characterType.GetType().Name == nameof(Character1));
                break;
            }
        }
        
    }

    public void Begin(string levelname)
    {
        GameManager.Instance.BeginLevel(levelname);
    }
}
