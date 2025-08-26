using System.Linq;

namespace FredAndRadience.Radiant_Shipyard;

internal static class HadesCombat
{
	internal static void Inject()
	{
        DB.story.all[$"Hades_RunStart_Dizzy"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.dizzy.Key() ],
			oncePerRun = true,
            lookup = new() { "Hades_StartRun" },
			oncePerRunTags = new() { "Hades_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "So, the cannon consumes shields to power up? It's like I was made for this ship!",
					loopTag = "neutral"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.goat.Key(),
                            Text = "Yeah, no kidding.",
                            loopTag = "shy"
                        },
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "Hopefully it doesn't get us blown up.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "Then get to work.",
                            loopTag = "sly"
                        },
                        new CustomSay()
                        {
                            who = "comp",
                            Text = "Do your thing, Dizzy!",
                            loopTag = "neutral"
                        },
                        new CustomSay()
                        {
                            who = Deck.peri.Key(),
                            Text = "We'll see how we do.",
                            loopTag = "nap"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Hades_RunStart_Books"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.shard.Key() ],
			oncePerRun = true,
            lookup = new() { "Hades_StartRun" },
			oncePerRunTags = new() { "Hades_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "If only the cannon could be charged with crystals!",
					loopTag = "squint"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "You can use your crystals to make shields, right?",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "You can use your crystals to make shields, right?",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = "comp",
                            Text = "You can use your crystals to make shields, right?",
                            loopTag = "squint"
                        },
                    ]
                }
			}
		};
        DB.story.all[$"Hades_RunStart_Max"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.hacker.Key() ],
			oncePerRun = true,
            lookup = new() { "Hades_StartRun" },
			oncePerRunTags = new() { "Hades_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "I don't like how vulnerable this ship's core makes us.",
					loopTag = "squint"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "Relax. We'll be fine.",
                            loopTag = "neutral"
                        },
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "I'll make sure to keep us safe.",
                            loopTag = "neutral"
                        },
                        new CustomSay()
                        {
                            who = Deck.shard.Key(),
                            Text = "It's magic! We'll figure it out somehow!",
                            loopTag = "paws"
                        },
                        new CustomSay()
                        {
                            who = Deck.peri.Key(),
                            Text = "It's risky, but I'm sure we can manage.",
                            loopTag = "neutral"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Hades_RunStart_Isaac"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.goat.Key() ],
			oncePerRun = true,
            lookup = new() { "Hades_StartRun" },
			oncePerRunTags = new() { "Hades_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Man, my shield drones could do some really good work here.",
					loopTag = "neutral"
				},
			}
		};
        DB.story.all[$"Hades_RunStart_Peri"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.peri.Key() ],
			oncePerRun = true,
            lookup = new() { "Hades_StartRun" },
			oncePerRunTags = new() { "Hades_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "This ship's core puts us at greater risk, but I should be able to work with it.",
					loopTag = "neutral"
				},
			}
		};
    }
}