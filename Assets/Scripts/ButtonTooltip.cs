using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltip = "";

    [SerializeField]
    public UIManager.Buildings definition;

    public Vector3 offset = new Vector3(20, -15, 0);

    UIManager _ui;
    bool hovering = false;

	// Use this for initialization
	void Start () {
        _ui = FindObjectOfType<UIManager>();
	}

    private void Update()
    {
        if (hovering) _ui.SetMouseTooltipPosition(offset);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.ShowMouseTooltip(tooltip);
        _ui.SetMouseTooltipPosition(offset);
        _ui.ShowLowPanelTooltip(definition);
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.HideMouseTooltip();
        _ui.HideLowPanelTooltip();
        hovering = false;
    }

}
