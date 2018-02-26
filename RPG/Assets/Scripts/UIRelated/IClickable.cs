using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public interface IClickable  {

    Image MyIcon {
        get;
        set;
    }

    int MyCount {
        get;
    }

    Text MyStackSize {
        get;
    }
}
