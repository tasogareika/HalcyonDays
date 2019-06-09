using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LocationBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Text displayText;
    [SerializeField] private Sprite hoverImg;
    private Sprite baseImg;
    private Image thisImage;

    private void Start()
    {
        thisImage = GetComponent<Image>();
        displayText.enabled = false;
        baseImg = thisImage.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        thisImage.sprite = hoverImg;
        GameFlags.singleton.thisLocation = eventData.pointerCurrentRaycast.gameObject;
        GameFlags.singleton.moveMenuTextToggle();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        thisImage.sprite = baseImg;
        GameFlags.singleton.thisLocation = eventData.pointerCurrentRaycast.gameObject;
        GameFlags.singleton.moveMenuTextToggle();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        displayText.enabled = false;
        GameObject o = eventData.pointerCurrentRaycast.gameObject;
        int n2 = GameFlags.singleton.locationButtons.IndexOf(o);
        string n = o.name;
        if (!DialogueManager.singleton.checkLocation(n)) //only move if selected spot is different location
        {
            if (n == "LivingRoom")
            {
                n = "Living";
            }
            n = n.ToLower();
            n = n + ",false,0";
            GameObject[] itemBtnArray = GameObject.FindGameObjectsWithTag("Items");
            foreach (var b in itemBtnArray)
            {
                b.GetComponent<Button>().interactable = false;
                b.SetActive(false);
            }
            thisImage.sprite = baseImg;
            GameFlags.singleton.movementMenu.SetActive(false);
            DialogueManager.singleton.toggleLocationShift(n);
        } else
        {
            MoveButton.singleton.closeMoveMenu();
        }
    }
}
