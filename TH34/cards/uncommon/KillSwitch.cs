using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardKillSwitch : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/KillSwitch.png"));
		helper.Content.Cards.RegisterCard("KillSwitch", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.uncommon,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "KillSwitch", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
		artTint = "ffffff",
        infinite = upgrade == Upgrade.B ? true : false,
		retain = true,
		cost = 0
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
		],
		Upgrade.B => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AEndTurn()
		],
		_ => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AEndTurn()
		],
	};
}