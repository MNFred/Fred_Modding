using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardPulseCannon : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/PulseCannon.png"));
		helper.Content.Cards.RegisterCard("PulseCannon", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.uncommon,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PulseCannon", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
		artTint = "ffffff",
		cost = 2
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AAttack{damage = GetDmg(s,1), timer = 0.1},
            new AStatus{status = Status.shield, statusAmount = 1, targetPlayer = true, timer = 0.1},
            new AAttack{damage = GetDmg(s,1), timer = 0.1},
            new AStatus{status = Status.shield, statusAmount = 1, targetPlayer = true, timer = 0.1},
            new AAttack{damage = GetDmg(s,1), timer = 0.1}
		],
		Upgrade.B => [
            new AStatus{status = Status.shield, statusAmount = 1, targetPlayer = true, timer = 0.1},
            new AAttack{damage = GetDmg(s,1), timer = 0.1},
            new AStatus{status = Status.shield, statusAmount = 1, targetPlayer = true, timer = 0.1},
            new AAttack{damage = GetDmg(s,1), timer = 0.1},
            new AStatus{status = Status.shield, statusAmount = 1, targetPlayer = true, timer = 0.1}
		],
		_ => [
            new AAttack{damage = GetDmg(s,1), timer = 0.1},
            new AStatus{status = Status.shield, statusAmount = 1, targetPlayer = true, timer = 0.1},
            new AAttack{damage = GetDmg(s,1), timer = 0.1},
            new AStatus{status = Status.shield, statusAmount = 1, targetPlayer = true, timer = 0.1}
		],
	};
}