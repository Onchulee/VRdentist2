using com.dgn.UnityAttributes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class DentalRemoteController : MonoBehaviour
{
    public DentalChairController chairController;
    [ReadOnly]
    [SerializeField]
    private XRInputReceiver holder;


    [Header("Guideline UI")]
    public Transform pivotUI;
    enum DisplayUI { None, pickGuide, usageGuide}
    private DisplayUI currentDisplay;
    public Image pickGuideUI;
    public Image usageGuideUI;


    // Start is called before the first frame update
    void Start()
    {
        currentDisplay = DisplayUI.None;
        pickGuideUI.fillAmount = 0;
        usageGuideUI.fillAmount = 0;
    }
    
    void Update()
    {
        UpdateUIDisplay();
        ListenInput();
    }

    private void UpdateUIDisplay() {
        // Check status
        if (IsMainCameraFocus()) {
            if (holder)
            {
                currentDisplay = DisplayUI.usageGuide;
            }
            else {
                currentDisplay = DisplayUI.pickGuide;
            }
        } else {
            currentDisplay = DisplayUI.None;
        }
        // Display UI accordingly
        if (currentDisplay == DisplayUI.None)
        {
            DisplayNoneUI();
        }
        if (currentDisplay == DisplayUI.pickGuide && pickGuideUI.fillAmount < 1f)
        {
            DisplayPickGuide();
        }
        if (currentDisplay == DisplayUI.usageGuide && usageGuideUI.fillAmount < 1f)
        {
            DisplayUsageGuide();
        }

        // Face UI To Camera
        if (pivotUI)
        {
            pivotUI.LookAt(Camera.main.transform, Vector3.up);
        }
    }

    private bool IsMainCameraFocus() {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool isInFrontRange = screenPoint.z > 0 
            && Vector3.Distance(Camera.main.transform.position, transform.position) < 5f;
        bool isInWidthRange = screenPoint.x > 0.3f && screenPoint.x < 0.7f;
        bool isInHeightRange = screenPoint.y > 0.3f && screenPoint.y < 0.7f;
        return isInFrontRange && isInWidthRange && isInHeightRange;
    }

    private void DisplayNoneUI() {
        if (pickGuideUI.fillAmount > 0)
        {
            pickGuideUI.fillAmount = Mathf.Clamp01(pickGuideUI.fillAmount - Time.deltaTime);
        }
        if (usageGuideUI.fillAmount > 0)
        {
            usageGuideUI.fillAmount = Mathf.Clamp01(usageGuideUI.fillAmount - Time.deltaTime);
        }
    }

    private void DisplayPickGuide() {
        if (usageGuideUI.fillAmount > 0)
        {
            usageGuideUI.fillAmount = Mathf.Clamp01(usageGuideUI.fillAmount - Time.deltaTime);
        }
        else
        {
            pickGuideUI.fillAmount = Mathf.Clamp01(pickGuideUI.fillAmount + Time.deltaTime);
        }
    }

    private void DisplayUsageGuide()
    {
        if (pickGuideUI.fillAmount > 0)
        {
            pickGuideUI.fillAmount = Mathf.Clamp01(pickGuideUI.fillAmount - Time.deltaTime);
        }
        else
        {
            usageGuideUI.fillAmount = Mathf.Clamp01(usageGuideUI.fillAmount + Time.deltaTime);
        }
    }

    private void ListenInput() {
        if (holder && chairController)
        {
            if (holder.GetKey(XRInputReceiver.KEY.PrimaryButton))
            {
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
