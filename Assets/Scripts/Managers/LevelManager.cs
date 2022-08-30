using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] public int CurrentLevel = 1;
    [SerializeField] public int LevelCount = 3;
    [SerializeField] public int RoadLenght = 12;
    [SerializeField] public int EmptyRoadLength = 2;
    [SerializeField] public Difficulty Difficulty = Difficulty.Easy;
    private void Awake()
    {
        Instance = this;
    }
}
