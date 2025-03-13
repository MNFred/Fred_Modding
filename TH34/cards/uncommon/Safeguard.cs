using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardSafeguard : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/Safeguard.png"));
		helper.Content.Cards.RegisterCard("Safeguard", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.uncommon,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Safeguard", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
        exhaust = true,
		artTint = "ffffff",
		cost = upgrade == Upgrade.A ? 0 : 1
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = 2, targetPlayer = true}
		],
		Upgrade.B => [
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = 3, targetPlayer = true}
		],
		_ => [
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = 2, targetPlayer = true}
		],
	};
}