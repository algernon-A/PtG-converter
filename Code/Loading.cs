using ColossalFramework;
using ICities;
using PlopTheGrowables.MessageBox;


namespace PlopTheGrowables
{

    /// <summary>
    /// Main loading class: the mod runs from here.
    /// </summary>
    public class Loading : LoadingExtensionBase
    {
		// Plop the Growables detection flag.
		internal static bool ptgDetected;

		// Data loading flag (error checking).
		internal static bool dataLoaded;


		/// <summary>
		/// Called by the game when level loading is complete.
		/// </summary>
		/// <param name="mode">Loading mode (e.g. game, editor, scenario, etc.)</param>
		public override void OnLevelLoaded(LoadMode mode)
		{
			base.OnLevelLoaded(mode);

			// Check to see that Harmony 2 was properly loaded.
			if (!Patcher.Patched)
			{
				// Harmony 2 wasn't loaded; display warning notification and exit.
				ListMessageBox harmonyBox = MessageBoxBase.ShowModal<ListMessageBox>();

				// Key text items.
				harmonyBox.AddParas(Translations.Translate("ERR_HAR0"), Translations.Translate("PGC_ERR_HAR"), Translations.Translate("PGC_ERR_FAT"), Translations.Translate("ERR_HAR1"));

				// List of dot points.
				harmonyBox.AddList(Translations.Translate("ERR_HAR2"), Translations.Translate("ERR_HAR3"));

				// Closing para.
				harmonyBox.AddParas(Translations.Translate("MES_PAGE"));

				// Don't do anything further.
				return;
			}

			// Was Plop the Growables detected?
			if (ptgDetected)
			{
				// Plop the Growables detected - display warning notification and exit.
				ListMessageBox modConflictBox = MessageBoxBase.ShowModal<ListMessageBox>();

				// Key text items.
				modConflictBox.AddParas(Translations.Translate("PGC_PTG0"), Translations.Translate("PGC_PTG1"), Translations.Translate("PGC_PTG2"));

				// Don't do anything further.
				return;
			}

			// Did data read succesfully occur?
			if (!dataLoaded)
			{
				// Mod conflict detected - display warning notification and exit.
				ListMessageBox modConflictBox = MessageBoxBase.ShowModal<ListMessageBox>();

				// Key text items.
				modConflictBox.AddParas(Translations.Translate("PGC_ERR_DSZ"), Translations.Translate("PGC_ERR_FAT"));

				// Don't do anything further.
				return;
			}


			// Checks passed - now check to see if we've got Advanced Building Level Control installed and active.
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