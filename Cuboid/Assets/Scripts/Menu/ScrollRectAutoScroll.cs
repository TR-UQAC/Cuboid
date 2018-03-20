using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public float scrollSpeed = 10f;
    private bool mouseOver = false;

    private List<Selectable> m_Selectables = new List<Selectable>();
    private ScrollRect m_ScrollRect;


    private Vector2 m_NextScrollPosition = Vector2.up;
    void OnEnable() {
        if (m_ScrollRect) {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }
    }
    void Awake() {
        m_ScrollRect = GetComponent<ScrollRect>();
    }
    void Start() {
        if (m_ScrollRect) {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }
        ScrollToSelected(true);
    }
    void Update() {
        // Scroll via input.
        InputScroll();
        if (!mouseOver) {
            // Lerp scrolling code.
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.deltaTime);
        } else {
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }

        m_NextScrollPosition.y = Mathf.Clamp(m_NextScrollPosition.y, 0, 1);
        m_ScrollRect.normalizedPosition = m_NextScrollPosition;


    }
    void InputScroll() {
        if (m_Selectables.Count > 0) {
            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButton("Horizontal") || Input.GetButton("Vertical") || CrossPlatformInputManager.GetAxis("Vertical") >0.5f || CrossPlatformInputManager.GetAxis("Vertical") < -0.5f) {
                ScrollToSelected(true);
            }
        }

        Debug.Log(CrossPlatformInputManager.GetAxis("Vertical"));
    }
    void ScrollToSelected(bool quickScroll) {
        int selectedIndex = -1;
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;

        if (selectedElement) {
            selectedIndex = m_Selectables.IndexOf(selectedElement);
        }
        if (selectedIndex > -1) {
            if (quickScroll) {
                m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
                //Debug.Log(m_ScrollRect.normalizedPosition.y);
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            } else {
                m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
            }
        }

        Debug.Log(m_ScrollRect.normalizedPosition.y);
    }
    public void OnPointerEnter(PointerEventData eventData) {
        mouseOver = true;
    }
    public void OnPointerExit(PointerEventData eventData) {
        mouseOver = false;
        ScrollToSelected(false);
    }
}