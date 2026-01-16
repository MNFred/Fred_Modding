namespace Fred.Jack;

internal static class CombatDialogue
{
	internal static ModEntry Instance => ModEntry.Instance;

	internal static void Inject()
	{
		string jack = Instance.Jack_Deck.UniqueName;
    string dizzy = Deck.dizzy.Key();
    string riggs = Deck.riggs.Key();
    string peri = Deck.peri.Key();
    string isaac = Deck.goat.Key();
    string drake = Deck.eunice.Key();
    string max = Deck.hacker.Key();
    string books = Deck.shard.Key();

		DB.story.all[$"CatWentMissing_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack, "comp" },
			oncePerCombat = true,
			oncePerCombatTags = new() { "CatWentMissing" },
      lastTurnPlayerStatuses = new() {Status.missingCat},
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "Our OS is gone, does anyone have a backup drive?",
					loopTag = "squint"
				},
        new CustomSay()
        {
          who = max,
          Text = "I do! Just let me get it.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"DizzyWentMissing_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack, dizzy },
			oncePerCombat = true,
			oncePerCombatTags = new() { "dizzyWentMissing" },
      lastTurnPlayerStatuses = new() {Status.missingDizzy},
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "Guys, the science dude just vanished.",
					loopTag = "squint"
				},
			}
		};
    DB.story.all[$"DrakeWentMissing_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack, drake },
			oncePerCombat = true,
			oncePerCombatTags = new() { "drakeWentMissing" },
      lastTurnPlayerStatuses = new() {Status.missingDrake},
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "Good riddance, I hate pirates.",
					loopTag = "happy"
				},
			}
		};
    DB.story.all[$"IsaacWentMissing_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack, isaac },
			oncePerCombat = true,
			oncePerCombatTags = new() { "isaacWentMissing" },
      lastTurnPlayerStatuses = new() {Status.missingIsaac},
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "Can we bring him back? He was useful.",
					loopTag = "question"
				},
			}
		};
    DB.story.all[$"MaxWentMissing_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack, max },
			oncePerCombat = true,
			oncePerCombatTags = new() { "maxWentMissing" },
      lastTurnPlayerStatuses = new() {Status.missingMax},
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "Oh well. He didn't contribute much anyway.",
					loopTag = "neutral"
				},
        new CustomSay()
        {
          who = peri,
          Text = "I beg to differ.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"RiggsWentMissing_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack, riggs },
			oncePerCombat = true,
			oncePerCombatTags = new() { "riggsWentMissing" },
      lastTurnPlayerStatuses = new() {Status.missingRiggs},
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "I don't wanna alarm anyone, but our pilot is gone.",
					loopTag = "shock"
				},
        new CustomSay()
        {
          who = dizzy,
          Text = "Well you certainly alarmed me.",
          loopTag = "intense"
        }
			}
		};
    DB.story.all[$"PeriWentMissing_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack, peri },
			oncePerCombat = true,
			oncePerCombatTags = new() { "periWentMissing" },
      lastTurnPlayerStatuses = new() {Status.missingPeri},
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "I think we got a mall cop M.I.A",
					loopTag = "squint"
				},
        new CustomSay()
        {
          who = drake,
          Text = "Hell yeah.",
          loopTag = "sly"
        }
			}
		};
    DB.story.all[$"BooksWentMissing_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack, books },
			oncePerCombat = true,
			oncePerCombatTags = new() { "booksWentMissing" },
      lastTurnPlayerStatuses = new() {Status.missingBooks},
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "I think the kid's gone. Is that normal?",
					loopTag = "question"
				},
        new CustomSay()
        {
          who = max,
          Text = "Sometimes.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"JackWentMissing"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerCombat = true,
			oncePerCombatTags = new() { "jackWentMissing" },
      lastTurnPlayerStatuses = new() {StatusMeta.deckToMissingStatus[ModEntry.Instance.Jack_Deck.Deck]},
			lines = new()
			{
				new SaySwitch()
        {
          lines = [
            new CustomSay()
            {
              who = dizzy,
              Text = "Whoa, the army guy is gone.",
              loopTag = "intense"
            },
            new CustomSay()
            {
              who = riggs,
              Text = "Jack?",
              loopTag = "nervous"
            },
            new CustomSay()
            {
              who = peri,
              Text = "We're down an officer.",
              loopTag = "squint"
            },
            new CustomSay()
            {
              who = isaac,
              Text = "We should probably bring him back.",
              loopTag = "neutral"
            },
            new CustomSay()
            {
              who = drake,
              Text = "Good riddance. I hate authority.",
              loopTag = "sly"
            },
            new CustomSay()
            {
              who = max,
              Text = "Do they teach you that at the military?",
              loopTag = "intense"
            },
            new CustomSay()
            {
              who = books,
              Text = "Can I disappear with him?",
              loopTag = "paws"
            },
            new CustomSay()
            {
              who = "comp",
              Text = "Hey! No sudden disappearing, someone bring him back!",
              loopTag = "squint"
            }
          ]
        }
			}
		};
    DB.story.all[$"WeDontOverlapWithEnemyAtAllButWeDoHaveASeekerToDealWith_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerCombat = true,
      shipsDontOverlapAtAll = true,
      doesNotHaveArtifacts = [
        "ChaffEmitters"
      ],
      anyDronesHostile = [
        "missile_seeker",
        "missile_shaker"
      ],
			oncePerCombatTags = new() { "NoOverlapBetweenShipsSeeker" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "We will have to brace ourself for that seeker.",
          loopTag = "serious"
        }
			}
		};
    DB.story.all[$"WeDontOverlapWithEnemyAtAllButWeDoHaveASeekerToNotDealWith_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerCombat = true,
      shipsDontOverlapAtAll = true,
      hasArtifacts = [
        "ChaffEmitters"
      ],
      anyDronesHostile = [
        "missile_seeker",
        "missile_shaker"
      ],
			oncePerCombatTags = new() { "NoOverlapBetweenShipsNoSeeker" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "We should be in the clear if we get far away enough from that seeker.",
          loopTag = "serious"
        }
			}
		};
    DB.story.all[$"WeDontOverlapWithEnemyAtAll_{jack}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerCombat = true,
      shipsDontOverlapAtAll = true,
			oncePerCombatTags = new() { "NoOverlapBetweenShips" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Running away? I call that a tactical retreat.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"NotJackMissedAShot_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerCombat = true,
      playerShotJustMissed = true,
			oncePerCombatTags = new() { "JackMissNotOwn" },
      doesNotHaveArtifacts = [
        "GrazerBeam",
        "Recalibrator"
      ],
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Alright. Who's in charge of weapons here?",
          loopTag = "question"
        },
        new CustomSay()
        {
          who = drake,
          Text = "Keep it to yourself.",
          loopTag = "squint"
        }
			}
		};
    DB.story.all[$"NotJackMissedAShot_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerCombat = true,
      playerShotJustMissed = true,
			oncePerCombatTags = new() { "JackMissNotOwn" },
      doesNotHaveArtifacts = [
        "GrazerBeam",
        "Recalibrator"
      ],
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Have you guys ever handled a weapon joystick before?",
          loopTag = "question"
        },
        new CustomSay()
        {
          who = peri,
          Text = "Cease complaining and help.",
          loopTag = "squint"
        }
			}
		};
    DB.story.all[$"JackMissedAShot_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerCombat = true,
      playerShotJustMissed = true,
      whoDidThat = ModEntry.Instance.Jack_Deck.Deck,
			oncePerCombatTags = new() { "JackMissOwn" },
      doesNotHaveArtifacts = [
        "GrazerBeam",
        "Recalibrator"
      ],
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Can you guys move? I can't see the reticle.",
          loopTag = "squint"
        },
        new CustomSay()
        {
          who = drake,
          Text = "Sure, whatever, prince.",
          loopTag = "mad"
        }
			}
		};
    DB.story.all[$"ManyTurns_{jack}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerRun = true,
      minTurnsThisCombat = 6,
      turnStart = true,
			oncePerCombatTags = new() { "manyTurns" },
      doesNotHaveArtifacts = [
        "GrazerBeam",
        "Recalibrator"
      ],
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Am I the only one who wants to be done with this already?",
          loopTag = "question"
        },
			}
		};
    DB.story.all[$"SignalCorruption_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerRun = true,
			oncePerCombatTags = new() { "SignalCorruption" },
      lastTurnPlayerStatuses = [
        ModEntry.Instance.LoseDroneshiftNextStatus.Status
      ],
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "The radar might start producing static, cover your ears.",
          loopTag = "neutral"
        },
        new CustomSay()
        {
          who = dizzy,
          Text = "What!?",
          loopTag = "intense"
        }
			}
		};
    DB.story.all[$"SignalCorruption_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerRun = true,
			oncePerCombatTags = new() { "SignalCorruption" },
      lastTurnPlayerStatuses = [
        ModEntry.Instance.LoseDroneshiftNextStatus.Status
      ],
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "I overclocked the radars, so they might go haywire in a minute.",
          loopTag = "neutral"
        },
        new CustomSay()
        {
          who = isaac,
          Text = "Are we even supposed to be doing that?",
          loopTag = "panic"
        }
			}
		};
    DB.story.all[$"OverheatDrakesFault_{jack}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack, drake },
			oncePerCombat = true,
      goingToOverheat = true,
      whoDidThat = Deck.eunice,
			oncePerCombatTags = new() { "OverheatDrakesFault" },
      lastTurnPlayerStatuses = [
        ModEntry.Instance.LoseDroneshiftNextStatus.Status
      ],
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "If that heat reaches the missile compartment...",
          loopTag = "shock"
        },
        new CustomSay()
        {
          who = drake,
          Text = "What a crybaby this soldier is.",
          loopTag = "sly"
        }
			}
		};
    DB.story.all[$"{jack}_LockOnWithJack_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerRun = true,
      whoDidThat = ModEntry.Instance.Jack_Deck.Deck,
			oncePerRunTags = new() { "LockOnWithJack" },
      lastTurnEnemyStatuses = [
        ModEntry.Instance.LockOnStatus.Status,
        ModEntry.Instance.ALockOnStatus.Status
      ],
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Smile for the reticle",
          loopTag = "serious"
        },
        new CustomSay()
        {
          who = riggs,
          Text = "What? Me? Do I look good from this angle?",
          loopTag = "nervous"
        }
			}
		};
    DB.story.all[$"{jack}_LockOnWithJack_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { jack },
			oncePerRun = true,
      whoDidThat = ModEntry.Instance.Jack_Deck.Deck,
			oncePerRunTags = new() { "LockOnWithJack" },
      lastTurnEnemyStatuses = [
        ModEntry.Instance.LockOnStatus.Status,
        ModEntry.Instance.ALockOnStatus.Status
      ],
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Target Confirmed",
          loopTag = "serious"
        }
			}
		};
    DB.story.all[$"{jack}_LockOnWithoutJack_0"] = new()
		{
			type = NodeType.combat,
			nonePresent = new() { jack },
			oncePerRun = true,
      whoDidThat = ModEntry.Instance.Jack_Deck.Deck,
			oncePerRunTags = new() { "LockOnWithoutJack" },
      lastTurnEnemyStatuses = [
        ModEntry.Instance.LockOnStatus.Status,
        ModEntry.Instance.ALockOnStatus.Status
      ],
			lines = new()
			{
        new CustomSay()
        {
          who = "comp",
          Text = "We have a lock on them!",
          loopTag = "neutral"
        },
				new CustomSay()
        {
          who = isaac,
          Text = "But my drones don't need that...",
          loopTag = "squint"
        }
			}
		};
    DB.story.all[$"jack_EXE_0"] = new()
		{
			type = NodeType.combat,
			oncePerCombat = true,
      lookup = new() { "jack_EXE" },
			oncePerCombatTags = new() { "jack_EXE" },
			lines = new()
			{
        new CustomSay()
        {
          who = "comp",
          Text = "Ooooh, so many military documents.",
          loopTag = "neutral"
        },
				new CustomSay()
        {
          who = jack,
          Text = "Keep a lid on it, computer.",
          loopTag = "squint"
        }
			}
		};
    DB.story.all[$"jack_EXE_1"] = new()
		{
			type = NodeType.combat,
			oncePerCombat = true,
			oncePerCombatTags = new() { "jack_EXE" },
      lookup = new() { "jack_EXE" },
			lines = new()
			{
        new CustomSay()
        {
          who = "comp",
          Text = "We will need some missiles for this one.",
          loopTag = "neutral"
        },
				new CustomSay()
        {
          who = jack,
          Text = "Finally some sensible words.",
          loopTag = "happy"
        }
			}
		};
    DB.story.all[$"{jack}_HighDamage_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      allPresent = new() {jack},
      minDamageDealtToEnemyThisAction = 6,
			oncePerRunTags = new() { "jackHigh" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Sheesh.",
          loopTag = "shock"
        }
			}
		};
    DB.story.all[$"{jack}_HighDamage_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      allPresent = new() {jack},
      minDamageDealtToEnemyThisAction = 6,
			oncePerRunTags = new() { "jackHigh" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "That's gonna leave a mark.",
          loopTag = "shock"
        }
			}
		};
    DB.story.all[$"EnemyHasWeakness_{jack}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      allPresent = new() {jack},
      enemyHasWeakPart = true,
			oncePerRunTags = new() { "yelledAboutWeakness" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "That part of the hull seems weaker than the rest.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"EnemyHasBrittle_{jack}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      allPresent = new() {jack},
      enemyHasBrittlePart = true,
			oncePerRunTags = new() { "yelledAboutBrittle" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "That part of the hull seems fragile. Aim where it hurts!",
          loopTag = "serious"
        }
			}
		};
    DB.story.all[$"LaunchingBlank_{jack}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      anyDrones = [
        "blankrocket"
      ],
			oncePerRunTags = new() { "LaunchedBlank" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "This one has its thrusters busted, just give it a light push.",
          loopTag = "neutral"
        },
        new CustomSay()
        {
          who = peri,
          Text = "A light push? Got it.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"LaunchingBlank_{jack}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      anyDrones = [
        "blankrocket"
      ],
			oncePerRunTags = new() { "LaunchedBlank" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Dang, it's broken. But it can still be useful.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"LaunchingBlank_{jack}_2"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      anyDrones = [
        "blankrocket"
      ],
			oncePerRunTags = new() { "LaunchedBlank" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "That one's busted, maybe we can use it to block incoming fire.",
          loopTag = "neutral"
        },
        new CustomSay()
        {
          who = riggs,
          Text = "I hope you're right.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"LaunchingAPMissile_{jack}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      anyDrones = [
        "aprocket"
      ],
			oncePerRunTags = new() { "LaunchedAPMissile" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "That'll pierce through their shield.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"LaunchingAPMissile_{jack}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      anyDrones = [
        "aprocket"
      ],
			oncePerRunTags = new() { "LaunchedAPMissile" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "They're gonna feel this sting.",
          loopTag = "serious"
        }
			}
		};
    DB.story.all[$"LaunchingBalistic_{jack}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      anyDrones = [
        "balisticrocketN"
      ],
			oncePerRunTags = new() { "LaunchedBalistic" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "This one is pretty strong, just give it some time.",
          loopTag = "happy"
        }
			}
		};
    DB.story.all[$"LaunchingBalistic_{jack}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      anyDrones = [
        "balisticrocketN"
      ],
			oncePerRunTags = new() { "LaunchedBalistic" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "This one needs some time to prime. We should protect it.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"LaunchingCluster_{jack}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      anyDrones = [
        "clusterrocket"
      ],
			oncePerRunTags = new() { "LaunchedCluster" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Destroying this might be beneficial if it's about to miss.",
          loopTag = "neutral"
        }
			}
		};
    DB.story.all[$"LaunchingCluster_{jack}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      anyDrones = [
        "clusterrocket"
      ],
			oncePerRunTags = new() { "LaunchedCluster" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "I put missiles in this missile, so now we can have more missile per missile.",
          loopTag = "happy"
        },
        new CustomSay()
        {
          who = isaac,
          Text = "How is that even possible?",
          loopTag = "squint"
        }
			}
		};
    DB.story.all[$"MidrowHalt_{jack}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      lastTurnPlayerStatuses = [
        ModEntry.Instance.MidrowHaltStatus.Status
      ],
			oncePerRunTags = new() { "MidrowHalted" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Hold fire, I'm locking onto them.",
          loopTag = "serious"
        },
        new CustomSay()
        {
          who = isaac,
          Text = "I hope you know what you're doing",
          loopTag = "shy"
        }
			}
		};
    DB.story.all[$"MidrowHalt_{jack}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
      priority = true,
      allPresent = new() {jack},
      lastTurnPlayerStatuses = [
        ModEntry.Instance.MidrowHaltStatus.Status
      ],
			oncePerRunTags = new() { "MidrowHalted" },
			lines = new()
			{
				new CustomSay()
        {
          who = jack,
          Text = "Wait, these missiles could use a minute or two.",
          loopTag = "neutral"
        },
        new CustomSay()
        {
          who = isaac,
          Text = "Is this neccesary?",
          loopTag = "shy"
        }
			}
		};
  }
}