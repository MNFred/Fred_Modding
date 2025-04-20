using Nickel;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nanoray.PluginManager;

namespace Fred.Jack;

internal interface IRegisterable
{
	static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
	static virtual void UpdateSettings(IPluginPackage<IModManifest> package, IModHelper helper, ProfileSettings settings) { }
}
internal interface IJackCard
{
    static abstract void Register(IPluginPackage<IModManifest> package,IModHelper helper);
    Vec ModifyTextCardScale(G g)
		=> Vec.One;

	Matrix ModifyNonTextCardRenderMatrix(G g, List<CardAction> actions)
		=> Matrix.Identity;

	Matrix ModifyCardActionRenderMatrix(G g, List<CardAction> actions, CardAction action, int actionWidth)
		=> Matrix.Identity;
}
internal interface IJackArtifact
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}
