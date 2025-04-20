using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class TargetPractice : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_TargetPractise.png"));
      helper.Content.Cards.RegisterCard("TargetPractice", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "TargetPractice", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = upgrade == Upgrade.B ? 1 : 0,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AAttack{
          damage = GetDmg(s,1),
          piercing = true,
          status = ModEntry.Instance.LockOnStatus.Status,
          statusAmount = 1
        }
      ],
      Upgrade.B => [
        new AAttack{
          damage = GetDmg(s,0),
          status = ModEntry.Instance.LockOnStatus.Status,
          statusAmount = 2
        }
      ],
      _ => [
        new AAttack{
          damage = GetDmg(s,0),
          status = ModEntry.Instance.LockOnStatus.Status,
          statusAmount = 1
        }
      ],
    };
  }
}
