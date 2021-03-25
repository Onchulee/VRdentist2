using com.dgn.UnityAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DentalRemoteController : MonoBehaviour
{
    public DentalChairController chairController;
    [ReadOnly]
    [SerializeField]
    private XRInputReceiver holder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (holder && chairController) {
            if (holder.GetKey(XRInputReceiver.KEY.PrimaryButton)) {
                chairController.LiftDown();
            }
            if (holder.GetKey(XRInputReceiver.KEY.SecondaryButton))
            {
                chairController.LiftUp();
            }
            if (holder.GetPrimary2DAxis().x > 0.5f)
            {
                chairController.BlendDown();
            }
            if (holder.GetPrimary2DAxis().x < -0.5f)
            {
                chairController.BlendUp();
            }
        }
    }

    public void OnGrabbed(XRBaseInteractor baseInteractor)
    {
        holder = XRInputInteractorMapper.Instance.GetXRInputReceiver(baseInteractor);
    }

    public void OnReleased(XRBaseInteractor baseInteractor)
    {
        holder = null;
    }
}
