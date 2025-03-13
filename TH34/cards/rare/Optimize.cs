using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardOptimize : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    private static ISpriteEntry ArtB = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/Optimize.png"));
        ArtB = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/OptimizeB.png"));
		helper.Content.Cards.RegisterCard("Optimize", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.rare,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Optimize", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = upgrade == Upgrade.B ? ArtB.Sprite : MainArt.Sprite,
		exhaust = true,
		artTint = "ffffff",
		cost = upgrade == Upgrade.A ? 1 : 2
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AStatus{status = ModEntry.Instance.OptimizeStatus.Status, statusAmount = 1, targetPlayer = true}
		],
		Upgrade.B => [
            new AStatus{status = ModEntry.Instance.OptimizeBStatus.Status, statusAmount = 1, targetPlayer = true}
		],
		_ => [
            new AStatus{status = ModEntry.Instance.OptimizeStatus.Status, statusAmount = 1, targetPlayer = true}
		],
	};
}