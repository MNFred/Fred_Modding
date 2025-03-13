using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardAmpUp : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/AmpUp.png"));
		helper.Content.Cards.RegisterCard("AmpUp", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.common,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "AmpUp", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
        recycle = true,
		cost = 1,
		artTint = "ffffff",
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AAttack{damage = GetDmg(s,1)},
			new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, targetPlayer = true, mode = AStatusMode.Set, statusAmount = 0},
			new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, targetPlayer = true, statusAmount = 1}
		],
		Upgrade.B => [
            new AAttack{damage = GetDmg(s,0)},
            new AAttack{damage = GetDmg(s,0)},
			new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, targetPlayer = true, mode = AStatusMode.Set, statusAmount = 0},
			new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, targetPlayer = true, statusAmount = 1}
		],
		_ => [
            new AAttack{damage = GetDmg(s,0)},
			new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, targetPlayer = true, mode = AStatusMode.Set, statusAmount = 0},
			new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, targetPlayer = true, statusAmount = 1}
		],
	};
}