 using HarmonyLib;
 using Nanoray.PluginManager;
 using Newtonsoft.Json;
 using Nickel;
 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Reflection;

 namespace Fred.Jack;

 internal sealed class JackHullEvent : IRegisterable
 {
 	private static string EventName = null!;

 	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
 	{
 		EventName = $"{package.Manifest.UniqueName}::{MethodBase.GetCurrentMethod()!.DeclaringType!.Name}";

 		DB.story.all[EventName] = new()
 		{
 			type = NodeType.@event,
 			canSpawnOnMap = true,
      zones = ["zone_lawless", "zone_three"],
 			oncePerRun = true,
 			lines = [
 				new CustomSay
 				{
 					who = "JackE",
 					loopTag = "neutral",
 					flipped = true,
 					Text = "Ahoy strangers."
 				},
        new CustomSay()
        {
          who = "comp",
          Text = "What?",
          loopTag = "squint"
        },
 				new CustomSay
 				{
 					who = "JackE",
 					loopTag = "neutral",
          flipped = true,
 					Text = "I provide services. Care to try?"
 				},
        new CustomSay()
        {
          who = "comp",
          Text = "Do we?",
          loopTag = "squint"
        },
        new CustomSay()
        {
          who = "JackE",
          Text = "",
          flipped = true,
          loopTag = "neutral"
        }
 			],
 			choiceFunc = EventName
 		};

 		DB.eventChoiceFns[EventName] = AccessTools.DeclaredMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(GetChoices));
 	}
 	private static List<Choice> GetChoices(State state)
 		=> [
 			new Choice
 			{
 				label = "Patch up the hull.\n-<c=heal>Heal to full</c>, gain <c=card>Debris</c>.",
 				actions = [
 					new AHeal{healAmount = state.ship.hullMax, targetPlayer = true},
          new AAddCard{card = new NonTempTrash()}
 				]
 			},
      new Choice
      {
        label = "Clear the cargo.\n-Remove 2 cards, <c=downside>Take 3 hull damage</c>.",
        actions = [
          new ARemoveCard{allowCancel = true},
          new ARemoveCard{allowCancel = true},
          new AHurt{hurtAmount = 3, targetPlayer = true}
        ]
      },
      new Choice 
      {
        label = "Some system improvements.\n-2 random cards gain upgrade B.",
        actions = [
          new AUpgradeCardRandom{upgradePath = Upgrade.B, count = 2}
        ]
      },
      new Choice
      {
        label = "No, let's leave."
      }
 		];
  public static void UpdateSettings(IPluginPackage<IModManifest> package, IModHelper helper, ProfileSettings settings)
	{
		var node = DB.story.all[EventName];
		node.never = !settings.JackEventEnabled ? true : null;
		node.dontCountForProgression = settings.JackEventEnabled;
	}
 }