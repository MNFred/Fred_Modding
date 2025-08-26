using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using FMOD;
using FredAndRadience.Radiant_Shipyard.actions;
using FSPRO;
using Nanoray.PluginManager;
using Nickel;

namespace FredAndRadience.Radiant_Shipyard;
internal sealed class CardChargeCannon : Card, IRadiantCard
{
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
		helper.Content.Cards.RegisterCard("ChargeCannon", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.Hades_Deck.Deck,
				rarity = Rarity.common,
                dontOffer = true
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ChargeCannon", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new()
    {
        cost = 0,
        temporary = true,
        retain = true,
        exhaust = true
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            
		],
		Upgrade.B => [
            
		],
		_ => [
            new AVariableHint{status = Status.shield},
            new AStatus{status = ModEntry.Instance.Elec_Charge.Status, xHint = 1, statusAmount = s.ship.Get(Status.shield), targetPlayer = true},
            new AStatus{status = Status.shield, statusAmount = 0, mode = AStatusMode.Set, targetPlayer = true}
		],
	};
}