using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollusionController : MonoBehaviour
{
    [SerializeField] private Animator PlayerAnimator;
    private AnimatorClipInfo[] _currentClipInfo;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Obstacle")
        {
            _currentClipInfo = PlayerAnimator.GetCurrentAnimatorClipInfo(0);
            if(_currentClipInfo[0].clip.name != "PlayerDamageTaken")
            {
                PlayerAnimator.Play("PlayerDamageTaken");
                GameManager.Instance.HPLoss();
            }
        }
        if(collision.collider.tag == "Finish")
        {
            collision.transform.gameObject.SetActive(false);
            GameManager.Instance.UpdateGameState(GameManager.GameState.Victory);
        }
    }
}
