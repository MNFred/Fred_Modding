using System.Collections.Generic;
using System.Reflection;
using Fred.AbandonedShipyard;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.AbandonedShipyard.cards;
internal sealed class BasicMissileCard : Card, IAbandonedCard
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Basic_Missile.png"));
		helper.Content.Cards.RegisterCard("BasicMissileCard", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = Deck.colorless,
				rarity = Rarity.common,
				upgradesTo = [Upgrade.A, Upgrade.B],
                dontOffer = true
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BasicMissileCard", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
		cost = upgrade == Upgrade.A ? 0 : 1,
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new ASpawn{
                thing = new Missile{missileType = MissileType.normal, targetPlayer = false}
            }
		],
		Upgrade.B => [
            new ASpawn{
                thing = new Missile{missileType = MissileType.heavy, targetPlayer = false}
            }
		],
		_ => [
            new ASpawn{
                thing = new Missile{missileType = MissileType.normal, targetPlayer = false}
            }
		],
	};
}