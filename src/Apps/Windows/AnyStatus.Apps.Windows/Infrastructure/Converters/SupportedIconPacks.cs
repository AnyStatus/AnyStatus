using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    internal class SupportedIconPacks
    {
        internal static readonly Dictionary<string, Type> IconPacks = new()
        {
            { "Material", typeof(PackIconMaterialKind) },
            { "MaterialLight", typeof(PackIconMaterialLightKind) },
            { "BootstrapIcons", typeof(PackIconBootstrapIconsKind) }
        };
    }
}
