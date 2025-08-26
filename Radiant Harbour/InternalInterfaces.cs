using Nickel;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nanoray.PluginManager;

namespace FredAndRadience.Radiant_Shipyard;

internal interface IRegisterable
{
	static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}
internal interface IRadiantCard
{
    static abstract void Register(IPluginPackage<IModManifest> package,IModHelper helper);
    Vec ModifyTextCardScale(G g)
		=> Vec.One;

	Matrix ModifyNonTextCardRenderMatrix(G g, List<CardAction> actions)
		=> Matrix.Identity;

	Matrix ModifyCardActionRenderMatrix(G g, List<CardAction> actions, CardAction action, int actionWidth)
		=> Matrix.Identity;
}
internal interface IRadiantArtifact
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}
