/**
 * GoogleSheets.cs
 * Created by: Toast <sam@gib.games>
 * Created on: 2/18/2024
 * Licensable under MIT
 */
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

namespace GIB.Integrations
{
    /// <summary>
    /// Handles interactions with Google Sheets, allowing for data to be sent to and retrieved from a Google Sheets document.
    /// </summary>
	public class GoogleSheets : MonoBehaviour
	{
        #region consts

        /// <summary>
        /// The URL endpoint of the Google App Script designed to interact with Google Sheets.
        /// Replace "YOUR_API_SCRIPT_LINK_HERE" with your actual Google App Script URL.
        /// </summary>
        public const string SHEETS_APP_URL = "YOUR_GOOGLE_SHEETS_APP_LINK_HERE";

        #endregion

        /// <summary>
        /// Submits playtest data to Google Sheets.
        /// </summary>
        /// <param name="timestamp">The timestamp of the playtest.</param>
        /// <param name="version">The version of the game.</param>
        /// <param name="id">A unique identifier for the playtest.</param>
        /// <param name="variables">Additional data to submit as key-value pairs.</param>
        public void SubmitPlaytest(string timestamp, string version, string id, Dictionary<string, string> variables)
        {
            StartCoroutine(PostRequest(timestamp, version, id, variables));
        }

        /// <summary>
        /// Sends a HTTP POST request to Google Sheets with the playtest data.
        /// </summary>
        /// <returns>An IEnumerator for coroutine invocation.</returns>
        public IEnumerator PostRequest(string timestamp, string version, string id, Dictionary<string, string> variables)
        {
            string encodedTimestamp = UnityWebRequest.EscapeURL(timestamp);
            string encodedVersion = UnityWebRequest.EscapeURL(version);
            string encodedId = UnityWebRequest.EscapeURL(id);

            // Start constructing the URL with fixed parameters
            string url = $"{SHEETS_APP_URL}?action=post&timestamp={encodedTimestamp}&version={encodedVersion}&id={encodedId}";

            // Append dynamic variables
            foreach (var kvp in variables)
            {
                url += $"&{UnityWebRequest.EscapeURL(kvp.Key)}={UnityWebRequest.EscapeURL(kvp.Value)}";
            }

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Error: {webRequest.error}");
                }
                else
                {
                    Debug.Log("Data sent successfully!");
                }
            }
        }

        /// <summary>
        /// Retrieves data from Google Sheets based on the provided ID and index.
        /// </summary>
        /// <param name="encodedId">The encoded ID to search for.</param>
        /// <param name="index">The index of the data to retrieve.</param>
        /// <returns>An IEnumerator for coroutine invocation.</returns>
        IEnumerator GetRequest(string encodedId,int index)
        {
            string encodedType = UnityWebRequest.EscapeURL("retrieve");
            string encodedIndex = index.ToString();
            string url = $"{SHEETS_APP_URL}?action=retrieve&id={UnityWebRequest.EscapeURL(encodedId)}&index={UnityWebRequest.EscapeURL(encodedIndex)}";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Error: {webRequest.error}");
                }
                else
                {
                    ValueString = webRequest.downloadHandler.text;
                    Debug.Log($"Retrieved fear: {webRequest.downloadHandler.text}");
                }
            }
        }
    }
	
	/// <summary>
	/// Holds information about the player, including unique identifiers and personal details.
	/// This class is used for gathering and serializing player-specific data for analytics or feedback.
	/// </summary>
	public class PlayerEnvironment
	{
		public string Resolution;
		public string OperatingSystem;
		public string DeviceModel;
		public string GraphicsDeviceName;
		public string ProcessorType;
		public int ProcessorCount;
		public string SystemMemorySize;
		public string QualitySetting;
		public string Language;
		public string InstallDir;

		public PlayerEnvironment()
		{
			Resolution = Screen.currentResolution.ToString();
			OperatingSystem = SystemInfo.operatingSystem;
			DeviceModel = SystemInfo.deviceModel;
			GraphicsDeviceName = SystemInfo.graphicsDeviceName;
			ProcessorType = SystemInfo.processorType;
			ProcessorCount = SystemInfo.processorCount;
			SystemMemorySize = $"{SystemInfo.systemMemorySize} MB";
			QualitySetting = QualitySettings.names[QualitySettings.GetQualityLevel()];
#if STEAMWORKS_NET
			Language = Application.systemLanguage.ToString() +" / " + SteamBridge.GetCountryCode();
			InstallDir = SteamBridge.GetAppInstallDir();
#else
			Language = Application.systemLanguage.ToString();
			InstallDir = "";
#endif
		}

		public string GetJson()
		{
			return JsonUtility.ToJson(this);
		}
	}
	
	/// <summary>
	/// Holds information about the player, including unique identifiers and personal details.
	/// This class is used for gathering and serializing player-specific data for analytics or feedback.
	/// </summary>
	public class PlayerData
	{
		/// <summary>
		/// Email address of the player.
		/// </summary>
		public string PlayerEmail;
		
		/// <summary>
		/// Unique player identifier.
		/// </summary>
		public string PlayerId;
		
		/// <summary>
		/// Persona name of the player, used for identifying the player in logs or feedback.
		/// </summary>
		public string Persona;
		
		/// <summary>
		/// Steam ID of the player, if available.
		/// </summary>
		public string SteamId;

		/// <summary>
		/// Constructor for PlayerData. Initializes player data from PlayerPrefs or generates new identifiers as needed.
		/// </summary>
		public PlayerData()
		{
			if (!PlayerPrefs.HasKey("PlaytesterId"))
			{
				Guid newPlayerCode = Guid.NewGuid();
				string newPlayerString = newPlayerCode.ToString();
				PlayerPrefs.SetString("PlaytesterId", newPlayerString);
			}

#if STEAMWORKS_NET
			Persona = SteamBridge.GetPersonaName();
			SteamId = SteamBridge.GetSteamUserID();
#endif
			PlayerEmail = PlayerPrefs.GetString("playtesterEmail", "Player email Missing");
			PlayerId = PlayerPrefs.GetString("playtesterId", "Error generating guid");
		}
			
		/// <summary>
		/// Serializes the player data to a JSON string.
		/// </summary>
		/// <returns>A JSON string representing the player's data.</returns>
		public string GetJson()
		{
			return JsonUtility.ToJson(this);
		}
	}
}