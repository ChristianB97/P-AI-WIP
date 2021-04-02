using Zenject;
using UnityEngine;

public class Events_TimeManager_Installer : MonoInstaller
{
public override void InstallBindings()
    {
        Container.BindInterfacesTo<Events_TimeManager>().AsSingle();
    }
}
