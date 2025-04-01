using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;
using HarmonyLib;
using System.Collections.Generic;

namespace Fred.AbandonedShipyard;
public class LastStand : Artifact, IAbandonedArtifact
{
    private static ISpriteEntry ActiveSprite = null!;
	private static ISpriteEntry InactiveSprite = null!;
    public bool savedthisZone = false;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        ActiveSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Vanguard/LastStandActive.png"));
		InactiveSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Vanguard/LastStandNotActive.png"));
        helper.Content.Artifacts.RegisterArtifact("LastStand", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = ActiveSprite.Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "LastStand", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "LastStand", "description"]).Localize,
        });
        ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.DirectHullDamage)),
			prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_DirectHullDamage_Prefix))
		);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return StatusMeta.GetTooltips(Status.perfectShield,1);
    }
    public override Spr GetSprite()
		=> (savedthisZone ? InactiveSprite : ActiveSprite).Sprite;
    private static bool Ship_DirectHullDamage_Prefix(Ship __instance, State s, Combat c, int amt)
	{
		if (!__instance.isPlayerShip)
			return true;
		if (amt < __instance.hull)
			return true;
		if (__instance.Get(Status.perfectShield) > 0)
			return true;

		var artifact = s.EnumerateAllArtifacts().OfType<LastStand>().FirstOrDefault();
		if (artifact is null || artifact.savedthisZone)
			return true;
        __instance.Heal(99);
		c.QueueImmediate([
			new AStatus{targetPlayer = true,status = Status.perfectShield,statusAmount = 1,artifactPulse = artifact.Key()}
		]);
        artifact.savedthisZone = true;
		return false;
	}
    public override void OnCombatEnd(State state)
    {
        if(state.map.markers[state.map.currentLocation].contents is MapBattle contents)
        {
            if(contents.battleType == BattleType.Boss)
            {
                savedthisZone = false;
            }
        }
    }
}