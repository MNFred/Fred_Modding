using Fred.Jack;
using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class RemoteActivation : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_SteelRecycle.png"));
      helper.Content.Cards.RegisterCard("RemoteActivation", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RemoteActivation", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        description = ModEntry.Instance.Localizations.Localize(["card", "RemoteActivation", "description", upgrade.ToString()]),
        art = ThisArt.Sprite,
        retain = upgrade == Upgrade.A ? true : false,
        cost = 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ProtectCockpit{upgradeB = false}
      ],
      Upgrade.B => [
        new ProtectCockpit{upgradeB = true}
      ],
      _ => [
        new ProtectCockpit{upgradeB = false}
      ],
    };
  }
}
public class ProtectCockpit : CardAction
{
  public bool upgradeB;
    public override List<Tooltip> GetTooltips(State s)
    {
      List<Tooltip> list = new List<Tooltip>();
      list.Add(new TTGlossary("action.spawn"));
      list.Add(new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::BlankMissile")
			{
				Icon = ModEntry.Instance.BlankRocket_Icon.Sprite,
				TitleColor = Colors.midrow,
				Title = ModEntry.Instance.Localizations.Localize(["midrow", "BlankMissile", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["midrow", "BlankMissile", "description"])
			});
      foreach(Part part in s.ship.parts)
    {
      if(part.type == PType.cockpit)
      {
        part.hilight = true;
      }
    }
      return list;
    }
    public override void Begin(G g, State s, Combat c)
    {
      c.QueueImmediate(from cockpits in s.ship.parts.Select((Part part, int x) => new {part, x}) where cockpits.part.type == PType.cockpit select new ASpawn{fromX = cockpits.x, thing = new BlankMissile{targetPlayer = false, bubbleShield = upgradeB}});
    }
}