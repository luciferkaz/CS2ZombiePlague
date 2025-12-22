using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;

namespace CS2ZombiePlague
{
    [PluginMetadata(Id = "CS2ZombiePlague", Version = "1.0.0", Name = "CS2ZombiePlague", Author = "illusion & fdrinv", Description = "Zombie Plague mode for CS2")]
    public partial class CS2ZombiePlague : BasePlugin
    {
        public CS2ZombiePlague(ISwiftlyCore core) : base(core)
        {
        }

        public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
        {
        }

        public override void UseSharedInterface(IInterfaceManager interfaceManager)
        {
        }

        public override void Load(bool hotReload)
        {

        }

        public override void Unload()
        {
        }
    }
}