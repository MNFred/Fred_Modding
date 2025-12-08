using Nickel;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nanoray.PluginManager;

namespace Fred.Andromeda;

internal interface IRegisterable
{
	static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}
internal interface IAndromedaCard
{
    static abstract void Register(IPluginPackage<IModManifest> package,IModHelper helper);
    Vec ModifyTextCardScale(G g)
		=> Vec.One;

	Matrix ModifyNonTextCardRenderMatrix(G g, List<CardAction> actions)
		=> Matrix.Identity;

	Matrix ModifyCardActionRenderMatrix(G g, List<CardAction> actions, CardAction action, int actionWidth)
		=> Matrix.Identity;
}
internal interface IAndromedaArtifact
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}