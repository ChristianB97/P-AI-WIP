using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerStorage>().WithId("Player").FromInstance(new PlayerStorage(50));
    }
}