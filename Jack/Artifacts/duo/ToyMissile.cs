using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class ToyMissile : Artifact, IJackArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      if (ModEntry.Instance.TyApi is not { } TyApi)
        return;
      helper.Content.Artifacts.RegisterArtifact("ToyMissile", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_tysasha.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ToyMissile", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ToyMissile", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, TyApi.TyDeck]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [new TTGlossary("action.spawn"),
      ..ModEntry.Instance.TyApi!.WildTrait.Configuration.Tooltips!(DB.fakeState, null)];
    }
    public override void OnCombatStart(State state, Combat combat)
    {
      foreach(Card card in state.deck)
      {
        if(card.GetActions(state, combat).Any(a => a is ASpawn))
        {
          ModEntry.Instance.Helper.Content.Cards.SetCardTraitOverride(state, card, ModEntry.Instance.TyApi!.WildTrait, true, false);
        }
      }
    }
    public override void OnPlayerRecieveCardMidCombat(State state, Combat combat, Card card)
    {
      if(card.GetActions(state, combat).Any(a => a is ASpawn))
      {
        ModEntry.Instance.Helper.Content.Cards.SetCardTraitOverride(state, card, ModEntry.Instance.TyApi!.WildTrait, true, false);
      }
    }
}
