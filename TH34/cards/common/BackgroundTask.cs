using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardBackgroundTask : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/BackgroundTask.png"));
		helper.Content.Cards.RegisterCard("BackgroundTask", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.common,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BackgroundTask", "name"]).Localize,
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
            new AStatus{status = Status.shield, statusAmount = 1, targetPlayer = true},
            new AVariableHint{status = ModEntry.Instance.MinusChargeStatus.Status},
            new AStatus{status = Status.evade, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status), xHint = 1, targetPlayer = true},
            new AStatus{status = Status.shield, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status), xHint = 1, targetPlayer = true}
		],
		Upgrade.B => [
            new AVariableHint{status = ModEntry.Instance.MinusChargeStatus.Status},
            new AStatus{status = Status.evade, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status), xHint = 1, targetPlayer = true},
            new AStatus{status = Status.shield, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status), xHint = 1, targetPlayer = true},
			new AStatus{status = Status.tempShield, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status), xHint = 1, targetPlayer = true}
		],
		_ => [
            new AVariableHint{status = ModEntry.Instance.MinusChargeStatus.Status},
            new AStatus{status = Status.evade, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status), xHint = 1, targetPlayer = true},
            new AStatus{status = Status.shield, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status), xHint = 1, targetPlayer = true}
		],
	};
}