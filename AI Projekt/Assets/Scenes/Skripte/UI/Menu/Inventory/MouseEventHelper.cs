using UnityEngine;

public class MouseEventHelper
{
    public ItemUI MouseDragItemUI { get; private set; }
    public ItemUI MouseEnterItemUI { get; private set; }

    public void DragObject(ItemUI dragItemUI)
    {
        MouseDragItemUI = dragItemUI;
    }

    public void EndDragObject()
    {
        if (MouseDragItemUI != null)
        {
            MouseDragItemUI = null;
        }     
    }

    public void EnterObject(ItemUI enterItemUI)
    {
        MouseEnterItemUI = enterItemUI;
    }

    public void ExitObject()
    {
        MouseEnterItemUI = null;
    }
}
