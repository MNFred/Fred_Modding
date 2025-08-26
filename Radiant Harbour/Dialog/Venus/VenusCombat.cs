using System.Linq;

namespace FredAndRadience.Radiant_Shipyard;

internal static class VenusCombat
{
	internal static void Inject()
	{
        DB.story.all[$"Venus_RunStart_Dizzy"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.dizzy.Key() ],
			oncePerRun = true,
            lookup = new() { "Venus_StartRun" },
			oncePerRunTags = new() { "Venus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "This ship has a really old-school look. I like it.",
					loopTag = "neutral"
				},
			}
		};
        DB.story.all[$"Venus_RunStart_Isaac"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.goat.Key() ],
			oncePerRun = true,
            lookup = new() { "Venus_StartRun" },
			oncePerRunTags = new() { "Venus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "This ship seems more designed for atmosphere than space...",
					loopTag = "squint"
				},
                new CustomSay()
                {
                    who = Deck.riggs.Key(),
                    Text = "It still handles fine!",
                    loopTag = "neutral"
                }
			}
		};
        DB.story.all[$"Venus_RunStart_Books"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.shard.Key() ],
			oncePerRun = true,
            lookup = new() { "Venus_StartRun" },
			oncePerRunTags = new() { "Venus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "This ship is pretty! I like its wings!",
					loopTag = "paws"
				},
			}
		};
        DB.story.all[$"Venus_RunStart_Drake"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.eunice.Key() ],
			oncePerRun = true,
            lookup = new() { "Venus_StartRun" },
			oncePerRunTags = new() { "Venus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Seems like a straightforward enough ship. Let's see what we can do.",
					loopTag = "sly"
				},
			}
		};
    }
}