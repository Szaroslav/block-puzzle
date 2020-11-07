using UnityEngine;

public class PauseScreenBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("In"))
            GameManager.ins.PauseGame();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Out"))
            GameManager.ins.UnpauseGame();
    }
}
