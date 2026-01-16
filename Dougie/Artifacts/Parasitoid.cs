using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Dougie.cards;
using System.Linq;
using Dougie.features;
using Dougie.Midrow;

namespace Dougie.Artifacts;
public class Parasitoid : Artifact, IDougieArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Parasitoid", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DougieDeck.Deck,
                pools = [ArtifactPool.Boss],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/Parasitoid.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Parasitoid", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Parasitoid", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        new GlossaryTooltip($"trait.{ModEntry.Instance.Package.Manifest.UniqueName}::Symbiotic")
		{
			Icon = ModEntry.Instance.SymbioticIcon.Sprite,
			TitleColor = Colors.cardtrait,
			Title = ModEntry.Instance.Localizations.Localize(["trait", "Symbiotic", "name"]),
			Description = ModEntry.Instance.Localizations.Localize(["trait", "Symbiotic", "description"])
		},
        new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell")
        {
            Icon = ModEntry.Instance.CellColonyIcon.Sprite,
            TitleColor = Colors.midrow,
            Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]),
            Description = string.Format("Will block 1 shot before being destroyed.")
        },
      ];
    }
    public override void OnReceiveArtifact(State state)
    {
        state.ship.baseEnergy += 1;
    }
    public override void OnRemoveArtifact(State state)
    {
        state.ship.baseEnergy -= 1;
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        combat.QueueImmediate(new OopsAllSymbiotic{timer = 0});
        List<int> validSpaces = new List<int>();
        for (int i = state.ship.x - 1; i < state.ship.x + state.ship.parts.Count() + 1; i++)
        {
            if (!combat.stuff.ContainsKey(i))
            {
                validSpaces.Add(i);
            }
        }
        List<int> finalSpaces = validSpaces.Shuffle(state.rngActions).Take(2).ToList();
        foreach (int x in finalSpaces)
        {
            combat.stuff.Add(x, new CellColony
            {
                x = x,
                xLerped = x
            });
        }
        if (finalSpaces.Count > 0)
        {
            Pulse();
        }
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        combat.QueueImmediate(new OopsAllSymbiotic{timer = 0});
    }
    public override void OnPlayerRecieveCardMidCombat(State state, Combat combat, Card card)
    {
        card.SetSymbiotic(true);
        ModEntry.Instance.Helper.Content.Cards.SetCardTraitOverride(state, card, ModEntry.Instance.SymbioticTrait, true, true);
    }
}
public class OopsAllSymbiotic : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(Card cardDeck in s.deck)
        {
            cardDeck.SetSymbiotic(true);
            ModEntry.Instance.Helper.Content.Cards.SetCardTraitOverride(s, cardDeck, ModEntry.Instance.SymbioticTrait, true, true);
        }
        foreach(Card cardDiscard in c.discard)
        {
            cardDiscard.SetSymbiotic(true);
            ModEntry.Instance.Helper.Content.Cards.SetCardTraitOverride(s, cardDiscard, ModEntry.Instance.SymbioticTrait, true, true);
        }
        foreach(Card cardExhaust in c.exhausted)
        {
            cardExhaust.SetSymbiotic(true);
            ModEntry.Instance.Helper.Content.Cards.SetCardTraitOverride(s, cardExhaust, ModEntry.Instance.SymbioticTrait, true, true);
        }
    }
}