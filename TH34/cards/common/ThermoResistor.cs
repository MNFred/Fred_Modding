using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardThermoResistor : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/ThermoResistor.png"));
		helper.Content.Cards.RegisterCard("ThermoResistor", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.common,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ThermoResistor", "name"]).Localize,
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
			new AStatus{status = Status.tempShield, targetPlayer = true, statusAmount = 1},
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, targetPlayer = true, statusAmount = 1}
		],
		Upgrade.B => [
			new AStatus{status = Status.tempShield, targetPlayer = true, statusAmount = 3},
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, targetPlayer = true, statusAmount = 1},
		],
		_ => [
			new AStatus{status = Status.tempShield, targetPlayer = true, statusAmount = 1},
			new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, targetPlayer = true, statusAmount = 1}
		],
	};
}