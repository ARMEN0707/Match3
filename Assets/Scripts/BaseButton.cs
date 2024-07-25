using System;
using FuryLion;
using FuryLion.UI;

public class BaseButton : Element, IClickable
{
    public event Action Click;

    public void OnClick()
    {
        if(SoundManager.GlobalSoundsVolume == 1)
            SoundManager.PlaySound(Sounds.Sound.Click);

        Click?.Invoke();
    }
}
