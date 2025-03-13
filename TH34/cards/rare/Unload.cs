using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardUnload : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    private static ISpriteEntry ArtA = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/Unload.png"));
        ArtA = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/UnloadA.png"));
		helper.Content.Cards.RegisterCard("Unload", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.rare,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Unload", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = upgrade == Upgrade.A ? ArtA.Sprite : MainArt.Sprite,
		artTint = "ffffff",
		cost = 2
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AAttack{damage = GetDmg(s,2)},
            new AAttack{damage = GetDmg(s,2)},
            new AAttack{damage = GetDmg(s,2)},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set},
            new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 1, targetPlayer = true, mode = AStatusMode.Set}
		],
		Upgrade.B => [
            new AAttack{damage = GetDmg(s,2)},
            new AAttack{damage = GetDmg(s,2)},
            new AAttack{damage = GetDmg(s,2)},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = -1, targetPlayer = true}
		],
		_ => [
            new AAttack{damage = GetDmg(s,2)},
            new AAttack{damage = GetDmg(s,2)},
            new AAttack{damage = GetDmg(s,2)},
            new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set}
		],
	};
}