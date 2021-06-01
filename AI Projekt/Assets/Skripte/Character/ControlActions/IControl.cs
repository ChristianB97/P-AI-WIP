using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControl
{
    void Move(int horizontalAxis, int verticalAxis);
    void Special1();
    void Special2();
}
