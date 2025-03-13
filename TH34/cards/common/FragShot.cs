using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.cards;
internal sealed class CardFragShot : Card, ITH34Card
{
    private static ISpriteEntry MainArt = null!;
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
        MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/FragShot.png"));
		helper.Content.Cards.RegisterCard("FragShot", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.TH34_Deck.Deck,
				rarity = Rarity.common,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FragShot", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        art = MainArt.Sprite,
		artTint = "ffffff",
		cost = upgrade == Upgrade.A ? 0 : 1
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            new AAttack{damage = GetDmg(s,0)},
            new AAttack{damage = GetDmg(s,1)},
            new AAttack{damage = GetDmg(s,0)}
		],
		Upgrade.B => [
            new AAttack{damage = GetDmg(s,0)},
            new AAttack{damage = GetDmg(s,1)},
            new AAttack{damage = GetDmg(s,1)},
            new AAttack{damage = GetDmg(s,0)}
		],
		_ => [
            new AAttack{damage = GetDmg(s,0)},
            new AAttack{damage = GetDmg(s,1)},
            new AAttack{damage = GetDmg(s,0)}
		],
	};
}