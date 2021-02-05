using System;
using System.Reflection;
using ColossalFramework.Plugins;


namespace PlopTheGrowables
{
    /// <summary>
    /// Utilities dealing with other mods, including compatibility and functionality checks.
    /// </summary>
    public static class ModUtils
    {
        // ABLC methods.
        internal static MethodInfo ablcLockBuildingLevel;

        /// <summary>
        /// Uses reflection to find the IsRICOPopManaged and ClearWorkplaceCache methods of Advanced Building Level Control.
        /// If successful, sets ricoPopManaged and ricoClearWorkplace fields.
        /// </summary>
        internal static void ABLCReflection()
        {
            // Iterate through each loaded plugin assembly.
            foreach (PluginManager.PluginInfo plugin in PluginManager.instance.GetPluginsInfo())
            {
                foreach (Assembly assembly in plugin.GetAssemblies())
                {
                    if (assembly.GetName().Name.Equals("AdvancedBuildingLevelControl") && plugin.isEnabled)
                    {
                        Logging.Message("Found Advanced Building Level Control");

                        // Found AdvancedBuildingLevelControl.dll that's part of an enabled plugin; try to get its ExternalCalls class.
                        Type ablcExternalCalls = assembly.GetType("ABLC.ExternalCalls");

                        if (ablcExternalCalls != null)
                        {
                            // Try to get LockBuildingLevel method.
                            ablcLockBuildingLevel = ablcExternalCalls.GetMethod("LockBuildingLevel", BindingFlags.Public | BindingFlags.Static);
                            if (ablcLockBuildingLevel != null)
                            {
                                // Success!
                                Logging.Message("found LockBuildingLevel");
                            }
                        }

                        // At this point, we're done; return.
                        return;
                    }
                }
            }

            // If we got here, we were unsuccessful.
            Logging.Message("Advanced Building Level Control not found");
        }
    }
}