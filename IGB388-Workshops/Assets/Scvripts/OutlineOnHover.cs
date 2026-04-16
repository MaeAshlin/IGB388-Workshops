using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public class OutlineOnHover : MonoBehaviour
{
    private Outline outline;
    private XRBaseInteractable interactable;
    private Transform playerTransform;
    private bool isHovered = false;
    private  Color grabDistanceColor = Color.white;
    private Color hoverColor = Color.yellow;

    private const float OUTLINE_WIDTH = 2.0f;
    private const float GRAB_LINE_SHOW_DISTANCE = 5.0f;


    void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline == null )
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
        }
        outline.OutlineWidth = OUTLINE_WIDTH;
       
        outline.enabled = false;

        interactable = GetComponent<XRBaseInteractable>();
        if ( interactable != null )
        {
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
        }
        XROrigin origin = FindFirstObjectByType<XROrigin>();
        if (origin !=null) 
        {
            playerTransform = origin.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (isHovered)
        {
            outline.OutlineColor = hoverColor;
            outline.enabled = true;
        }
        else if (distance <= GRAB_LINE_SHOW_DISTANCE && !interactable.isSelected)
        {
            outline.OutlineColor = grabDistanceColor;
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        isHovered = true;
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        isHovered = false;
    }

    void OnDestroy()
    {
       if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
        }
    }
}
