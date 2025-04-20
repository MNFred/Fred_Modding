using HarmonyLib;
using Nickel;
using System;

namespace Fred.Jack;

internal static class GExt
{
	private static readonly Lazy<Func<MG, G?>> GGetter = new(() => AccessTools.DeclaredField(typeof(MG), "g").EmitInstanceGetter<MG, G>());

	public static G? Instance
		=> GGetter.Value(MG.inst);
}