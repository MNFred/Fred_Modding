using System.Collections.Generic;
using System.Reflection;
using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class DOMEArtifact : Artifact, IJackArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      helper.Content.Artifacts.RegisterArtifact("DOMEArtifact", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_goat.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "DOMEArtifact", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "DOMEArtifact", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, Deck.goat]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [new TTCard{card = new CardDOME()}];
    }
    public override void OnReceiveArtifact(State state)
    {
      state.deck.Add(new CardDOME());
    }
}
internal sealed class CardDOME : Card, IJackCard
  {
    private static ISpriteEntry MainArt = null!;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
        return;
      MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_DOME.png"));
      helper.Content.Cards.RegisterCard("CardDOME", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CardDOME", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = MainArt.Sprite,
        cost = upgrade == Upgrade.B ? 2 : 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{
          thing = new DOME()
        },
        new AStatus{status = Status.droneShift, statusAmount = 1, targetPlayer = true}
        ],
      Upgrade.B => [
        new ASpawn{
          thing = new Asteroid{bubbleShield = true},
          offset = -1
        },
        new ASpawn{
          thing = new DOME()
        },
        new ASpawn{
          thing = new Asteroid{bubbleShield = true},
          offset = 1
        },
      ],
      _ => [
        new ASpawn{
          thing = new DOME()
        },
      ],
    };
  }