using Content.Shared.Mobs;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Toolshed.Commands.Math;

namespace Content.Client.UserInterface.Systems.GrainOverlays.Overlays;

public sealed class GrainOverlay : Overlay
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    public override bool RequestScreenTexture => true;

    private readonly ShaderInstance _grainShader;

    public MobState State = MobState.Alive;

    public GrainOverlay()
    {
        // TODO: Replace
        IoCManager.InjectDependencies(this);
        _grainShader = _prototypeManager.Index<ShaderPrototype>("FilmGrain").InstanceUnique();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (!_entityManager.TryGetComponent(_playerManager.LocalPlayer?.ControlledEntity, out EyeComponent? eyeComp))
            return;

        if (args.Viewport.Eye != eyeComp.Eye)
            return;

        var viewport = args.WorldAABB;
        var handle = args.WorldHandle;

        handle.UseShader(_grainShader);
        handle.DrawRect(viewport, Color.White);
        handle.UseShader(null);
    }
}
