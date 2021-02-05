using HarmonyLib;


namespace PlopTheGrowables
{
	/// <summary>
	/// Harmony Prefix patch to replicate Plop the Growables data deserialization.
	/// </summary>
	[HarmonyPatch(typeof(SimulationManager), "Managers_UpdateData")]
	public static class SimulationManagerManagersUpdateDataPatch
	{
		/// <summary>
		/// Harmony prefix to load Plop the Growables data on pre-update.
		/// Never pre-empts.
		/// </summary>
		/// <param name="mode">Loading manager load mode</param>
		public static void Prefix(SimulationManager.UpdateMode mode)
		{
			if (mode == SimulationManager.UpdateMode.LoadGame)
			{
				PlopTheGrowables.LoadData();
			}
		}
	}
}