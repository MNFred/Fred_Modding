using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardBlastProcessing : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/BlastProcessing.png"));
		helper.Content.Cards.RegisterCard("BlastProcessing", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.common,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BlastProcessing", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
		artTint = "ffffff",
		cost = 2
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AAttack{damage = GetDmg(s,6)},
            new AVariableHint{status = ModEntry.Instance.MinusChargeStatus.Status},
            new AStatus{status = Status.tempShield, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status), xHint = 1, targetPlayer = true}
		],
		Upgrade.B => [
            new AAttack{damage = GetDmg(s,4)},
            new AVariableHint{status = ModEntry.Instance.MinusChargeStatus.Status},
            new AStatus{status = Status.tempShield, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)*2, xHint = 2, targetPlayer = true}
		],
		_ => [
            new AAttack{damage = GetDmg(s,4)},
            new AVariableHint{status = ModEntry.Instance.MinusChargeStatus.Status},
            new AStatus{status = Status.tempShield, statusAmount = s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status), xHint = 1, targetPlayer = true}
		],
	};
}