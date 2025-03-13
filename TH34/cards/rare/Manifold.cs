using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardManifold : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/Manifold.png"));
		helper.Content.Cards.RegisterCard("Manifold", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.rare,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Manifold", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
		artTint = "ffffff",
		cost = 1,
		infinite = upgrade == Upgrade.A ? true : false
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AStatus{status = Status.tempShield, statusAmount = 1, targetPlayer = true},
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = 1, targetPlayer = true}
		],
		Upgrade.B => [
            new AStatus{status = Status.tempShield, statusAmount = 2, targetPlayer = true},
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = 1, targetPlayer = true}
		],
		_ => [
            new AStatus{status = Status.tempShield, statusAmount = 1, targetPlayer = true},
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = 1, targetPlayer = true}
		],
	};
}