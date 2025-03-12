using System.Collections.Generic;
using System.Reflection;
using Fred.TH34.Artifacts;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardThetaProtocol : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    private static ISpriteEntry ArtB = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/ThetaProtocol.png"));
        ArtB = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/ThetaProtocolB.png"));
		helper.Content.Cards.RegisterCard("ThetaProtocol", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.common,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ThetaProtocol", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = upgrade == Upgrade.B ? ArtB.Sprite : MainArt.Sprite,
        exhaust = true,
		artTint = "ffffff",
		cost = upgrade == Upgrade.A ? 0 : 1
	};
	private static int HeartPresent(State s) 
	{
        int status = 1;
		foreach (object enumerateAllArtifact in s.EnumerateAllArtifacts())
      	{
			if (enumerateAllArtifact.GetType() == typeof (ArtifactMechanicalHeart))
				status = 2;
      	}
        return status;
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AVariableHint{status = ModEntry.Instance.PlusChargeStatus.Status},
			new AStatus{status = Status.tempShield, statusAmount = s.ship.Get(ModEntry.Instance.PlusChargeStatus.Status), targetPlayer = true, timer = 0, xHint = 0},
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = s.ship.Get(ModEntry.Instance.PlusChargeStatus.Status), xHint = 1, targetPlayer = true, timer = 0},
		],
		Upgrade.B => [
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, mode = AStatusMode.Set, targetPlayer = true},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 1, targetPlayer = true},
            new AVariableHint{status = ModEntry.Instance.PlusChargeStatus.Status},
			new AStatus{status = Status.tempShield, statusAmount = s.ship.Get(ModEntry.Instance.PlusChargeStatus.Status) + HeartPresent(s), targetPlayer = true, timer = 0, xHint = 1},
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = s.ship.Get(ModEntry.Instance.PlusChargeStatus.Status) + HeartPresent(s), xHint = 1, targetPlayer = true, timer = 0},
		],
		_ => [
            new AVariableHint{status = ModEntry.Instance.PlusChargeStatus.Status},
			new AStatus{status = Status.tempShield, statusAmount = s.ship.Get(ModEntry.Instance.PlusChargeStatus.Status), targetPlayer = true, timer = 0, xHint = 1},
            new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = s.ship.Get(ModEntry.Instance.PlusChargeStatus.Status), xHint = 1, targetPlayer = true, timer = 0},
		],
	};
}