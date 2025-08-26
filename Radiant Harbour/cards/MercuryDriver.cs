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
internal sealed class CardMercuryDriver : Card, IRadiantCard
{
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
		helper.Content.Cards.RegisterCard("MercuryDriver", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.Mercury_Deck.Deck,
				rarity = Rarity.common,
                dontOffer = true
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "MercuryDriver", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        cost = 0,
        temporary = true,
        recycle = true
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            
		],
		Upgrade.B => [
            
		],
		_ => [
            new AStatus{status = Status.drawNextTurn, statusAmount = 1, targetPlayer = true}
		],
	};
}