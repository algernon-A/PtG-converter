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
            Logging.Error("Advanced Building Level Control not found");
        }


        /// <summary>
        ///  Checks for the Plop the Growables mod, as distinct from the PtG converter.
        /// </summary>
        /// <returns>True if the original Plop the Growables mod is installed and active, false otherwise</returns>
        internal static bool IsPtGInstalled()
        {
            // Iterate through the full list of plugins.
            foreach (PluginManager.PluginInfo plugin in PluginManager.instance.GetPluginsInfo())
            {
                foreach (Assembly assembly in plugin.GetAssemblies())
                {
                    // Looking for an assembly named "PlopTheGrowables" that's active.
                    if (assembly.GetName().Name.Equals("PlopTheGrowables") && plugin.isEnabled)
                    {
                        // Found one - is this the converter mod class?
                        if (!plugin.userModInstance.GetType().ToString().Equals("PlopTheGrowables.PtGReaderMod"))
                        {
                            // Not converter mod class - assume it's the original.
                            return true;
                        }
                        else
                        {
                            // Converter mod class - log and continue.
                            Logging.Message("found Plop the Growables converter");
                        }
                    }
                }
            }

            // If we got here, no active PtG was detected.
            return false;
        }
    }
}