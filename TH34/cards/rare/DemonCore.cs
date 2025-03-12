using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardDemonCore : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/DemonCore.png"));
		helper.Content.Cards.RegisterCard("DemonCore", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.rare,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "DemonCore", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
        exhaust = true,
		retain = true,
		artTint = "ffffff",
		cost = upgrade == Upgrade.A ? 1 : 2,
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 3, targetPlayer = true, mode = AStatusMode.Set},
		],
		Upgrade.B => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 4, targetPlayer = true, mode = AStatusMode.Set},
		],
		_ => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 3, targetPlayer = true, mode = AStatusMode.Set},
		],
	};
}