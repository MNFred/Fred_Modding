using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardCounterBalance : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/CounterBalance.png"));
		helper.Content.Cards.RegisterCard("CounterBalance", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.uncommon,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CounterBalance", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
		artTint = "ffffff",
		cost = 1
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AAttack{damage = GetDmg(s,2)},
            new AVariableHint{status = ModEntry.Instance.PlusChargeStatus.Status},
            new AStatus{status = Status.heat, statusAmount = -s.ship.Get(ModEntry.Instance.PlusChargeStatus.Status), xHint = -1, targetPlayer = true}
		],
		Upgrade.B => [
            new AAttack{damage = GetDmg(s,1),piercing = true},
            new AVariableHint{status = ModEntry.Instance.PlusChargeStatus.Status},
            new AStatus{status = Status.heat, statusAmount = -s.ship.Get(ModEntry.Instance.PlusChargeStatus.Status), xHint = -1, targetPlayer = true}
		],
		_ => [
            new AAttack{damage = GetDmg(s,1)},
            new AVariableHint{status = ModEntry.Instance.PlusChargeStatus.Status},
            new AStatus{status = Status.heat, statusAmount = -s.ship.Get(ModEntry.Instance.PlusChargeStatus.Status), xHint = -1, targetPlayer = true}
		],
	};
}