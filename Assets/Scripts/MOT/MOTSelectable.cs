using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MOTSelectable : MonoBehaviour
{
    private bool isSelected = false;
    private Renderer rend;
    private XRSimpleInteractable interactable;

    private bool isTarget = false;
    public void SetIsTarget(bool value) => isTarget = value;
    public bool WasSelected => isSelected;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        interactable = GetComponent<XRSimpleInteractable>();
        if (interactable == null)
            interactable = gameObject.AddComponent<XRSimpleInteractable>();

        interactable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (isSelected) return;

        isSelected = true;
        rend.material.color = Color.yellow;

        MOTManager.Instance.RegisterSelection(this);
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnSelectEntered);
    }
}
