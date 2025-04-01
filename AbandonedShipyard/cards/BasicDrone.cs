using System.Collections.Generic;
using System.Reflection;
using Fred.AbandonedShipyard;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.AbandonedShipyard.cards;
internal sealed class BasicDroneCard : Card, IAbandonedCard
{
    private static ISpriteEntry TopArt = null!;
    private static ISpriteEntry BottomArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        TopArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Drone_Top.png"));
        BottomArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Drone_Bottom.png"));
		helper.Content.Cards.RegisterCard("BasicDroneCard", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = Deck.colorless,
				rarity = Rarity.common,
				upgradesTo = [Upgrade.A, Upgrade.B],
                dontOffer = true
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BasicDroneCard", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        floppable = true,
        art = flipped ? BottomArt.Sprite : TopArt.Sprite,
        exhaust = upgrade == Upgrade.B ? true : false,
		cost = upgrade == Upgrade.B ? 2 : 1,
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new ASpawn{
                thing = new AttackDrone{targetPlayer = false, bubbleShield = true},
                disabled = flipped
            },
            new ADummyAction(),
            new ADummyAction(),
            new ASpawn{
                thing = new ShieldDrone{targetPlayer = true, bubbleShield = true},
                disabled = !flipped
            }
		],
		Upgrade.B => [
            new ASpawn{
                thing = new AttackDrone{targetPlayer = false, upgraded = true},
                offset = -1,
                disabled = flipped
            },
            new ASpawn{
                thing = new AttackDrone{targetPlayer = false, upgraded = true},
                disabled = flipped
            },
            new ADummyAction(),
            new ASpawn{
                thing = new ShieldDrone{targetPlayer = true, bubbleShield = true},
                disabled = !flipped
            },
            new ASpawn{
                thing = new ShieldDrone{targetPlayer = true, bubbleShield = true},
                offset = 1,
                disabled = !flipped
            }
		],
		_ => [
            new ASpawn{
                thing = new AttackDrone{targetPlayer = false},
                disabled = flipped
            },
            new ADummyAction(),
            new ADummyAction(),
            new ASpawn{
                thing = new ShieldDrone{targetPlayer = true},
                disabled = !flipped
            },
		],
	};
}