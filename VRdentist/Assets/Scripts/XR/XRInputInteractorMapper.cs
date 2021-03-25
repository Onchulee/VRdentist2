using com.dgn.XR.Extensions;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInputInteractorMapper : Singleton<XRInputInteractorMapper>
{
    public XRInputReceiver inputReceiver_left;
    public XRInputReceiver inputReceiver_right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public XRInputReceiver GetXRInputReceiver(XRBaseInteractor baseInteractor) {
        XRBaseInteractor[] interactors;

        interactors = XRInteractorsManager.Instance.GetInteractors(XRInteractorsManager.Controller.Left);
        foreach (XRBaseInteractor interactor in interactors) {
            if(baseInteractor == interactor) return inputReceiver_left;
        }

        interactors = XRInteractorsManager.Instance.GetInteractors(XRInteractorsManager.Controller.Right);
        foreach (XRBaseInteractor interactor in interactors)
        {
            if (baseInteractor == interactor) return inputReceiver_right;
        }

        return null;
    }
}
