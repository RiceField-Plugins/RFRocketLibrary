using SDG.Unturned;

namespace RFRocketLibrary.Helpers
{
    public static class ImageHelper
    {
        public static string VanillaItem =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/vanilla/items/{0}.png";

        public static string WorkshopItem =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/workshop/items/{0}/{1}.png";

        public static string VanillaVehicle =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/vanilla/vehicles/{0}.png";

        public static string WorkshopVehicle =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/workshop/vehicles/{0}/{1}.png";

        public static string Plugin =>
            "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/plugin/{0}/{1}.png";

        public static string GetVanillaItemImage(string id) => string.Format(VanillaItem, id);

        public static string GetVanillaVehicleImage(string id) => string.Format(VanillaVehicle, id);

        public static string GetWorkshopItemImage(string assetBundleName, string id) =>
            string.Format(WorkshopItem, assetBundleName, id);

        public static string GetWorkshopVehicleImage(string assetBundleName, string id) =>
            string.Format(WorkshopVehicle, assetBundleName, id);

        public static string GetItemImage(ItemAsset asset) => asset.assetOrigin == EAssetOrigin.OFFICIAL
            ? GetVanillaItemImage(asset.id.ToString())
            : GetWorkshopItemImage(asset.originMasterBundle?.assetBundleNameWithoutExtension ?? "unknown",
                asset.id.ToString());

        public static string GetVehicleImage(VehicleAsset asset) => asset.assetOrigin == EAssetOrigin.OFFICIAL
            ? GetVanillaVehicleImage(asset.id.ToString())
            : GetWorkshopVehicleImage(asset.originMasterBundle?.assetBundleNameWithoutExtension ?? "unknown",
                asset.id.ToString());
        
        public static string GetPluginImage(string pluginName, string imageName) =>
            string.Format(Plugin, pluginName, imageName);
    }
}