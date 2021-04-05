using com.dgn.UnityAttributes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class EquipmentPickController : MonoBehaviour
{
    
    [ReadOnly]
    [SerializeField]
    private XRInputReceiver holder;

    [Header("Guideline UI")]
    public Transform pivotUI;
    enum DisplayUI { None, pickGuide, usageGuide }
    private DisplayUI currentDisplay;
    public Image pickGuideUI;
    public Image usageGuideUI;

    // Mask Images
    public Image maskA;
    public Image maskB;
    public Image maskThumbStick;
    public Image maskThumbStick_Left;
    public Image maskThumbStick_Right;

    // Tracking Input
    private bool hasInputA;
    private bool hasInputB;
    private bool hasInputThumbStick_Left;
    private bool hasInputThumbStick_Right;
    private bool hasPickup;

    // Start is called before the first frame update
    void Start()
    {
        currentDisplay = DisplayUI.None;
        pickGuideUI.fillAmount = 0;
        usageGuideUI.fillAmount = 0;
        maskA.fillAmount = 1;
        maskB.fillAmount = 1;
        maskThumbStick.fillAmount = 1;
        maskThumbStick_Left.fillAmount = 1;
        maskThumbStick_Right.fillAmount = 1;
        hasInputA = false;
        hasInputB = false;
        hasInputThumbStick_Left = false;
        hasInputThumbStick_Right = false;
        hasPickup = false;
    }

    void Update()
    {
        UpdateUIDisplay();
        ListenInput();
    }

    private void UpdateUIDisplay()
    {
        // Check status
        currentDisplay = DisplayUI.None;
        if (IsMainCameraFocus())
        {
            if (holder)
            {
                currentDisplay = DisplayUI.usageGuide;
            }
            else if (hasPickup == false)
            {
                currentDisplay = DisplayUI.pickGuide;
            }
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

        UpdateInputUI();

        // Face UI To Camera
        if (pivotUI)
        {
            pivotUI.LookAt(Camera.main.transform, Vector3.up);
        }
    }

    private void UpdateInputUI()
    {
        UpdateEachInputUI(hasInputA, maskA);
        UpdateEachInputUI(hasInputB, maskB);
        UpdateEachInputUI(hasInputThumbStick_Left, maskThumbStick_Left);
        UpdateEachInputUI(hasInputThumbStick_Right, maskThumbStick_Right);
        UpdateEachInputUI(hasInputThumbStick_Left && hasInputThumbStick_Right, maskThumbStick);
    }

    private void UpdateEachInputUI(bool hasInput, Image targetImg)
    {
        if (hasInput && targetImg.fillAmount > 0)
        {
            if (currentDisplay == DisplayUI.usageGuide)
            {
                FillImage(targetImg, -Time.deltaTime);
            }
            else
            {
                targetImg.fillAmount = 0;
            }
        }
    }

    private void FillImage(Image targetImg, float amount)
    {
        if (targetImg)
            targetImg.fillAmount = Mathf.Clamp01(targetImg.fillAmount + amount);
    }

    private void DisplayNoneUI()
    {
        if (pickGuideUI.fillAmount > 0)
        {
            FillImage(pickGuideUI, -Time.deltaTime);
        }
        if (usageGuideUI.fillAmount > 0)
        {
            FillImage(usageGuideUI, -Time.deltaTime);
        }
    }

    private void DisplayPickGuide()
    {
        if (usageGuideUI.fillAmount > 0)
        {
            FillImage(usageGuideUI, -Time.deltaTime);
        }
        else
        {
            FillImage(pickGuideUI, +Time.deltaTime);
        }
    }

    private void DisplayUsageGuide()
    {
        if (pickGuideUI.fillAmount > 0)
        {
            FillImage(pickGuideUI, -Time.deltaTime);
        }
        else
        {
            FillImage(usageGuideUI, +Time.deltaTime);
        }
    }

    private void ListenInput()
    {
        if (holder)
        {
            if (holder.GetKey(XRInputReceiver.KEY.PrimaryButton))
            {
                hasInputA = true;
              
            }
            if (holder.GetKey(XRInputReceiver.KEY.SecondaryButton))
            {
                hasInputB = true;
               
            }
            if (holder.GetPrimary2DAxis().x > 0.5f)
            {
                hasInputThumbStick_Right = true;
               
            }
            if (holder.GetPrimary2DAxis().x < -0.5f)
            {
                hasInputThumbStick_Left = true;
                
            }
        }
    }

    private bool IsMainCameraFocus()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool isInFrontRange = screenPoint.z > 0
            && Vector3.Distance(Camera.main.transform.position, transform.position) < 5f;
        bool isInWidthRange = screenPoint.x > 0.3f && screenPoint.x < 0.7f;
        bool isInHeightRange = screenPoint.y > 0.2f && screenPoint.y < 0.8f;
        return isInFrontRange && isInWidthRange && isInHeightRange;
    }

    public void OnGrabbed(XRBaseInteractor baseInteractor)
    {
        holder = XRInputInteractorMapper.Instance.GetXRInputReceiver(baseInteractor);
        hasPickup = true;
    }

    public void OnReleased(XRBaseInteractor baseInteractor)
    {
        holder = null;
    }
}
