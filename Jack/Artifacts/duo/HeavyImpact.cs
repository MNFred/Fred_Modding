using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fred.Jack.Midrow;
using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class HeavyImpact : Artifact, IJackArtifact
{
  public bool fromMissile = false;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      if (ModEntry.Instance.DynaApi is not { } dynaApi)
        return;
      helper.Content.Artifacts.RegisterArtifact("HeavyImpact", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_dyna.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "HeavyImpact", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "HeavyImpact", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, dynaApi.DynaDeck.Deck]);
      ModEntry.Instance.Harmony.Patch(
			  original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.NormalDamage)),
			  postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_NormalDamage_Postfix))
		  );
      ModEntry.Instance.Harmony.Patch(
			  original: AccessTools.DeclaredMethod(typeof(AMissileHit), nameof(AMissileHit.Update)),
			  prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AMissileHit_Update_Prefix))
		  );
      ModEntry.Instance.Harmony.Patch(
			  original: AccessTools.DeclaredMethod(typeof(AMissileHit), nameof(AMissileHit.Update)),
			  finalizer: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AMissileHit_Update_Finalizer))
		  );
    }
    private static void AMissileHit_Update_Prefix(G g, State s, Combat c)
    {
      if (s.EnumerateAllArtifacts().OfType<HeavyImpact>().FirstOrDefault() is not { } artifact)
        return;
      artifact.fromMissile = true;
    }
    private static void AMissileHit_Update_Finalizer(G g, State s, Combat c)
    {
      if (s.EnumerateAllArtifacts().OfType<HeavyImpact>().FirstOrDefault() is not { } artifact)
        return;
      artifact.fromMissile = false;
    }
    private static void Ship_NormalDamage_Postfix( State s, Combat c, int incomingDamage, int? maybeWorldGridX)
    {
      if (s.EnumerateAllArtifacts().OfType<HeavyImpact>().FirstOrDefault() is not { } artifact)
        return;
      int worldX1 = 0;
      int worldX2 = 0;
      if(maybeWorldGridX.HasValue)
      {
        worldX1 = maybeWorldGridX.Value - 1;
        worldX2 = maybeWorldGridX.Value + 1;
      }
      foreach(Part part in c.otherShip.parts)
      {
        if(part != null && part.type != PType.empty && ModEntry.Instance.DynaApi!.GetStickedCharge(s,c, part) != null && (c.otherShip.GetPartAtWorldX(worldX1) == part || c.otherShip.GetPartAtWorldX(worldX2) == part))
        {
          if(artifact.fromMissile)
            ModEntry.Instance.DynaApi.TriggerChargeIfAny(s,c,part,false);
        }
      }
    }
}