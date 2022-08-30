using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private Transform PlayerCameras;
    [SerializeField] private Vector3 CameraOffSet = new Vector3(0f,9f,-10f);
    [SerializeField] public GameObject VictoryStateCameraPos;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameManager.GameState obj)
    {
        if(obj == GameManager.GameState.PlayerTurn)
        {
            transform.position = new Vector3(transform.position.x, 0, -6f);
            PlayerAnimator.Play("Running");
            Camera.main.transform.parent = null;
            VictoryStateCameraPos.transform.parent = transform;
            Camera.main.transform.rotation = Quaternion.Euler(17, 0, 0);

        }
        else if(obj == GameManager.GameState.Lose)
        {
            PlayerAnimator.Play("Idle");
        }
        else if(obj == GameManager.GameState.Victory)
        {
            PlayerAnimator.Play("Victory");
            Camera.main.transform.parent = VictoryStateCameraPos.transform;
            VictoryStateCameraPos.transform.parent = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.PlayerTurn)
        {
            PlayerCameras.position = transform.position + CameraOffSet;
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
