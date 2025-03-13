using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardCleverTech : Card, ITH34Card
{
    private static ISpriteEntry TopArt = null!;
    private static ISpriteEntry BottomArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        TopArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/CleverTech_Top.png"));
        BottomArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/CleverTech_Bottom.png"));
		helper.Content.Cards.RegisterCard("CleverTech", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.uncommon,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CleverTech", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        floppable = true,
        artTint = "ffffff",
        art = flipped ? TopArt.Sprite : BottomArt.Sprite,
		cost = upgrade == Upgrade.A ? 0 : 1
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set, disabled = !flipped},
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 1, targetPlayer = true, mode = AStatusMode.Set, disabled = !flipped},
            new ADummyAction(),
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set, disabled = flipped},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 1, targetPlayer = true, mode = AStatusMode.Set, disabled = flipped},
		],
		Upgrade.B => [
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set, disabled = !flipped},
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 2, targetPlayer = true, mode = AStatusMode.Set, disabled = !flipped},
            new ADummyAction(),
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set, disabled = flipped},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 2, targetPlayer = true, mode = AStatusMode.Set, disabled = flipped},
		],
		_ => [
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set, disabled = !flipped},
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 1, targetPlayer = true, mode = AStatusMode.Set, disabled = !flipped},
            new ADummyAction(),
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set, disabled = flipped},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 1, targetPlayer = true, mode = AStatusMode.Set, disabled = flipped},
		],
	};
}