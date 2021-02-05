﻿using System;
using System.Collections.Generic;
using ColossalFramework;
using System.IO;
using ColossalFramework.IO;


namespace PlopTheGrowables
{
	/// <summary>
	/// Class to extract Plop the Growables data from savegame.
	/// Needs to retain original namespace and class names and hierarchies to fool the game into thinking this is Plop the Growables.
	/// This approach is required due to the onerous deserialization requirements enforced by the game.
	/// </summary>
	public static class PlopTheGrowables
	{
		// List of building IDs marked as 'plopped' by Plop the Growables.
		public static List<ushort> buildingList;


		/// <summary>
		/// PtG data container deserializer, disguised as the real thing.
		/// </summary>
		public class PlopTheGrowablesDataContainer : IDataContainer
		{
			/// <summary>
			/// Empty serializer - we don't write data, only read it.
			/// </summary>
			/// <param name="serializer">Serializer to use</param>
			public void Serialize(DataSerializer serializer)
			{
			}


			/// <summary>
			/// Deserializes savegame data from Plop the Growables.
			/// Mimics PtG's code.
			/// </summary>
			/// <param name="serializer">Serializer to use</param>
			public void Deserialize(DataSerializer serializer)
			{
				Logging.Message("Deserializing Plop the Growables savegame data");

				// Initialize list.
				buildingList = new List<ushort>();

				// Iterate through all building IDs stored in savegame.
				for (int i = 0; i < serializer.ReadInt32(); ++i)
				{
					// Read building ID and add to list.
					ushort buildingID = (ushort)serializer.ReadInt32();
					buildingList.Add(buildingID);
					Logging.Message("read building ID ", buildingID.ToString());
				}
			}


			/// <summary>
			/// Empty post-deserializer.
			/// </summary>
			/// <param name="serializer">Serializer to use</param>
			public void AfterDeserialize(DataSerializer serializer)
			{
			}
		}


		/// <summary>
		/// Loads Plop the Growables data from savegame.
		/// </summary>
		public static void LoadData()
		{
			try
			{
				Logging.Message("attempting to load Plop the Growables savegame data");
				byte[] array = Singleton<SimulationManager>.instance.m_SerializableDataWrapper.LoadData("PLOP_THE_GROWABLES_ID");
				MemoryStream stream = new MemoryStream(array);
				DataSerializer.Deserialize<PlopTheGrowablesDataContainer>(stream, DataSerializer.Mode.Memory);
			}
			catch (Exception exception)
			{
				Logging.LogException(exception, "exception loading Plop the Growables savegame data");
			}
		}
	}
}