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
internal sealed class CardShipPartShuffle : Card, IRadiantCard
{
    public static void Register(IPluginPackage<IModManifest> package,IModHelper helper) {
		helper.Content.Cards.RegisterCard("ShipPartShuffle", new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.Changeling_Deck.Deck,
				rarity = Rarity.common,
                dontOffer = true
			},
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ShipPartShuffle", "name"]).Localize,
		});
	}
    public override CardData GetData(State state) => new() {
        description = ModEntry.Instance.Localizations.Localize(["card", "ShipPartShuffle", "description",upgrade.ToString()]),
        cost = 0,
        retain = true,
		recycle = true,
		temporary = true
	};
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => [
            
		],
		Upgrade.B => [
            
		],
		_ => [
            new AShuffleShip{targetPlayer = true}
		],
	};
}