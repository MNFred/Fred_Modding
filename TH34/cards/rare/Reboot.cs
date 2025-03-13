using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardReboot : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/Reboot.png"));
		helper.Content.Cards.RegisterCard("Reboot", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.rare,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Reboot", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
		artTint = "ffffff",
		cost = upgrade switch
		{
			Upgrade.B => 4,
			Upgrade.A => 2,
			_ => 3
		},
        exhaust = upgrade == Upgrade.B ? false : true
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
			new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStunShip{targetPlayer = false},
            new AEndTurn()
		],
		Upgrade.B => [
			new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStunShip{targetPlayer = false},
            new AEndTurn()
		],
		_ => [
			new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStunShip{targetPlayer = false},
            new AEndTurn()
		],
	};
}