using System.Linq;

namespace FredAndRadience.Radiant_Shipyard;

internal static class MercuryCombat
{
	internal static void Inject()
	{
        DB.story.all[$"Mercury_RunStart_Drake_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.eunice.Key() ],
			oncePerRun = true,
            lookup = new() { "Mercury_StartRun" },
			oncePerRunTags = new() { "Mercury_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "What... Why are none of the weapons systems active? Ugh...",
					loopTag = "mad"
				},
			}
		};
        DB.story.all[$"Mercury_RunStart_Drake_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.eunice.Key() ],
			oncePerRun = true,
            lookup = new() { "Mercury_StartRun" },
			oncePerRunTags = new() { "Mercury_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Ugh. This ship makes me wish I was on the Fireball.",
					loopTag = "mad"
				},
			}
		};
        DB.story.all[$"Mercury_RunStart_Isaac_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.goat.Key() ],
			oncePerRun = true,
            lookup = new() { "Mercury_StartRun" },
			oncePerRunTags = new() { "Mercury_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Man. This ship's weird.",
					loopTag = "squint"
				},
			}
		};
        DB.story.all[$"Mercury_RunStart_Isaac_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.goat.Key() ],
			oncePerRun = true,
            lookup = new() { "Mercury_StartRun" },
			oncePerRunTags = new() { "Mercury_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Looks like I'll need some help getting my drones out.",
					loopTag = "shy"
				},
			}
		};
        DB.story.all[$"Mercury_RunStart_Peri_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.peri.Key() ],
			oncePerRun = true,
            lookup = new() { "Mercury_StartRun" },
			oncePerRunTags = new() { "Mercury_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Hm... This ship's design is unconventional.",
					loopTag = "nap"
				},
			}
		};
        DB.story.all[$"Mercury_RunStart_Peri_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.peri.Key() ],
			oncePerRun = true,
            lookup = new() { "Mercury_StartRun" },
			oncePerRunTags = new() { "Mercury_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "This ship could be potent with the right setup.",
					loopTag = "neutral"
				},
			}
		};
        DB.story.all[$"Mercury_RunStart_Books"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.shard.Key() ],
			oncePerRun = true,
            lookup = new() { "Mercury_StartRun" },
			oncePerRunTags = new() { "Mercury_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "This ship is strange. I like it!",
					loopTag = "neutral"
				},
			}
		};
        DB.story.all[$"Mercury_RunStart_BooksAndDizzy"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.shard.Key() , Deck.dizzy. Key()],
			oncePerRun = true,
            lookup = new() { "Mercury_StartRun" },
			oncePerRunTags = new() { "Mercury_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Hey Books, looks like your crystal shards are going to be extra helpful here.",
					loopTag = "crystal"
				},
                new CustomSay()
                {
                    who = Deck.shard.Key(),
                    Text = "Really?! I can't wait!",
                    loopTag = "stoked"
                }
			}
		};
    }
}