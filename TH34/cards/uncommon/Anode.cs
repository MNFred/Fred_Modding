using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardAnode : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/Anode.png"));
		helper.Content.Cards.RegisterCard("Anode", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.uncommon,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Anode", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
        retain = true,
		artTint = "ffffff",
        exhaust = upgrade == Upgrade.A ? false : true,
		cost = 1
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 1, targetPlayer = true, mode = AStatusMode.Set}
		],
		Upgrade.B => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 2, targetPlayer = true}
		],
		_ => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 1, targetPlayer = true, mode = AStatusMode.Set}
		],
	};
}