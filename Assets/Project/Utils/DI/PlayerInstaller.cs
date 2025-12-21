using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private float playerSpeed = 5f;
    public override void InstallBindings()
    {
        BindWeaponSlotController();
        BindPlayerMovementService();
    }

    private void BindWeaponSlotController()
    {
        Container
            .Bind<WeaponSlotController>()
            .FromComponentInHierarchy()
            .AsSingle();
    }

    private void BindPlayerMovementService()
    {
        Container
            .Bind<IMovementService>()
            .To<PlayerMovement>()
            .AsTransient()
            .WithArguments(playerSpeed);
    }
}