using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Rendering;

public class SlotView : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;

    public event System.Action<int> Clicked; // "someone clicked me - here's my index"
    public event System.Action<int, int> Moved; // (fromIndex, toIndex)
    private int index;
    private static int draggedFrom = -1; // shared accross all slots during one drag

    public void SetIndex(int i) => index = i;

    public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke(index);

    public void OnBeginDrag(PointerEventData e) 
    {
        draggedFrom = index;
        icon.color = new Color(1, 1, 1, 0.5f); // fade the source for feedback
    }

    public void OnDrag(PointerEventData e) // must exist, even empty, or  OnDrop won't fire
    {
        
    }

    public void OnEndDrag(PointerEventData e)
    {
        icon.color = Color.white; // restore
        draggedFrom = -1;
    }

    public void OnDrop(PointerEventData e) // fires on the slot you released OVER
    {
        if(draggedFrom >= 0 && draggedFrom != index)
            Moved?.Invoke(draggedFrom, index);
    }

    public void Render(ItemStack stack)
    {
        if(stack == null) // empty stack
        {
            icon.enabled = false;
            countText.text = "";
            return;    
        }

        icon.enabled = true; 
        icon.color = Color.white; // reset opacity in case a drag faded it
        icon.sprite = stack.Item.Icon; // the sprite from our Flyweight asset
        countText.text = stack.Count > 1 ? stack.Count.ToString() : ""; // hide  "1"
    }
}
