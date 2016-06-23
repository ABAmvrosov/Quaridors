using UnityEngine;
using UnityEngine.UI;

public class MouseToolTip : MonoBehaviour {
    [SerializeField] private GameObject _mouseToolTip;
    private float _xOffset = 20f;
    private float _yOffset = 20f;

    public void ShowToolTip(string toolTip) {
        _mouseToolTip.transform.position = Input.mousePosition - new Vector3(_xOffset, _yOffset, 0.0f);
        _mouseToolTip.GetComponent<Text>().text = toolTip;
        _mouseToolTip.SetActive(true);
    }

    public void HideToolTip() {
        _mouseToolTip.SetActive(false);
    }
}
