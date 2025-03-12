using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardTurboMode : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/TurboMode.png"));
		helper.Content.Cards.RegisterCard("TurboMode", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.common,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "TurboMode", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
		artTint = "ffffff",
		cost = upgrade == Upgrade.A ? 1 : 2
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AStatus{status = Status.shield, statusAmount = 2, targetPlayer = true},
			new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, targetPlayer = true, mode = AStatusMode.Set, statusAmount = 0},
			new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, targetPlayer = true, mode = AStatusMode.Set, statusAmount = 1}
		],
		Upgrade.B => [
			new AStatus{status = Status.shield, statusAmount = 3, targetPlayer = true},
            
			new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, targetPlayer = true, mode = AStatusMode.Set, statusAmount = 0},
			new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, targetPlayer = true, mode = AStatusMode.Set, statusAmount = 1}
		],
		_ => [
			new AStatus{status = Status.shield, statusAmount = 2, targetPlayer = true},
			new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, targetPlayer = true, mode = AStatusMode.Set, statusAmount = 0},
			new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, targetPlayer = true, mode = AStatusMode.Set, statusAmount = 1}
		],
	};
}