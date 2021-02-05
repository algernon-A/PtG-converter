using ColossalFramework;
using ICities;


namespace PlopTheGrowables
{

    /// <summary>
    /// Main loading class: the mod runs from here.
    /// </summary>
    public class Loading : LoadingExtensionBase
    {
		/// <summary>
		/// Called by the game when level loading is complete.
		/// </summary>
		/// <param name="mode">Loading mode (e.g. game, editor, scenario, etc.)</param>
		public override void OnLevelLoaded(LoadMode mode)
		{
			base.OnLevelLoaded(mode);

			// Check to see if we've got Advanced Building Level Control installed and active.
			ModUtils.ABLCReflection();
			if (ModUtils.ablcLockBuildingLevel != null)
			{
				// Get building manager.
				BuildingManager instance = Singleton<BuildingManager>.instance;
				if (instance == null)
				{
					Logging.Error("couldn't get building manager");
					return;
				}

				// Iterate through each building ID in the list of deserialized PtG IDs.
				foreach (ushort buildingID in PlopTheGrowables.buildingList)
				{
					byte level = instance.m_buildings.m_buffer[buildingID].m_level;
					Logging.Message("locking building level for building ", buildingID.ToString(), " to ", level.ToString());

					ModUtils.ablcLockBuildingLevel.Invoke(null, new object[] { buildingID, level });
				}
			}
		}
    }
}