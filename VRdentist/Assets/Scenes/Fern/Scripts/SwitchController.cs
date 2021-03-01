using com.dgn.UnityAttributes;
using UnityEngine;
using UnityEngine.Events;

public class SwitchController : MonoBehaviour
{
    public enum Activation { TurnOff, SwitchA, SwitchB }
    public enum Axis { X, Y, Z }

    [System.Serializable]
    public struct MinMaxVector3
    {
        public Vector3 min;
        public Vector3 max;
    }

    [System.Serializable]
    public struct MinMax
    {
        public float min;
        public float max;
    }
    
    [SerializeField]
    [ReadOnly]
    protected Activation activation;

    [Header("Setup")]
    public UnityEvent onTurnOffEvent;
    public UnityEvent onTurnSwitchAEvent;
    public UnityEvent onTurnSwitchBEvent;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        activation = Activation.TurnOff;
        onTurnOffEvent.Invoke();
    }
    
    protected void ActivateSwitch(Activation activeSwitch) {
        if (activation == activeSwitch) return;
        activation = activeSwitch;
        switch (activeSwitch)
        {
            case Activation.SwitchA:
                onTurnSwitchAEvent.Invoke();
                break;
            case Activation.SwitchB:
                onTurnSwitchBEvent.Invoke();
                break;
            default:
                onTurnOffEvent.Invoke();
                break;
        }
    }
}
