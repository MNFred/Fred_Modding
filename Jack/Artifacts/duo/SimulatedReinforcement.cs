using System.Collections.Generic;
using System.Reflection;
using Fred.Jack.cards;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class SimulatedReinforcement : Artifact, IJackArtifact
{
  public int temporaryPlayed = 0;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      helper.Content.Artifacts.RegisterArtifact("SimulatedReinforcement", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_legato.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SimulatedReinforcement", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SimulatedReinforcement", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, Deck.colorless]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [new TTCard{card = new PrecisionCard{singleUseOverride = true}}];
    }
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
      if(card.GetDataWithOverrides(state).temporary)
      {
        temporaryPlayed++;
        if(temporaryPlayed == 2)
        {
          combat.QueueImmediate(new AAddCard{card = new PrecisionCard{singleUseOverride = true}, amount = 1, destination = CardDestination.Hand});
          Pulse();
        }
      }
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        temporaryPlayed = 0;
    }
}
