[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) ![GitHub followers](https://img.shields.io/github/followers/DorkToast?style=social) ![Twitter Follow](https://img.shields.io/twitter/follow/dorktoast?style=social) [![Discord](https://img.shields.io/discord/539176606468669440?label=discord)](https://discord.gg/jvZkahu)

# Unity-Google Sheets integration
This repo created a seamless Unity-Google Sheets integration using a simple API for a Google Spreadsheet using Google Apps Script, allowing for basic CRUD (Create, Read, Update, Delete) operations through a web interface.

This guide provides a step-by-step process to integrate Unity with Google Sheets, allowing Unity developers to send playtest data directly to a Google Sheet using Google Apps Script. This can be particularly useful for collecting playtester feedback, environment data, and more.

### Key Concepts

-   **Parameter Handling**: The script uses the `e.parameter` object to access GET request parameters.
-   **Sheet Access**: Dynamically selects sheets based on request parameters, with a fallback to the 'Master' sheet.
-   **Data Manipulation**: Demonstrates adding and retrieving data, showcasing how to handle variable data fields.
-   **Error Handling**: Basic error handling by returning specific messages for invalid actions or when data cannot be found.

## How to

### Creating the Custom Function in Google Sheets

1.  **Open Your Google Sheet**: In the Google Sheet where you're working, go to `Extensions` > `Apps Script`.
2.  **Set up the columns** for data you wish to collect (e.g., Email, Playtest Key, Status, NickName).
3.  **Note the URL** of your Google Sheet, you'll need it for your Unity script. 
4.  **Import Script**: Import the `PlaytestData.gs` script.
5. **Modify the script** as needed to fit your specific data collection requirements.
6. **Save** the AppsScript.
7. **Deploy**: In the Google Apps Script editor, click on `Deploy` > `New deployment`. Select `Web app` as the deployment type. Fill in the details, set *Who has access* to `Anyone`, and click `Deploy`.
8.  **Copy the web app URL** provided after deployment.

### Importing into Unity

1. **Dependency**: Import [Odin Serializer](https://github.com/TeamSirenix/odin-serializer) from github or [Odin Inspector](https://odininspector.com/) asset. (You can alternately populate your playtest Dictionary manually.)
2. **Import** the two scripts in this repo into your unity project.
3. **Replace the placeholder URL** in `SHEETS_APP_URL` with the URL of your Google Apps Script web app from the previous section.
4.   Attach the script to a GameObject in your Unity scene to start using it.

### Test it!

1.  **Run your Unity project** and perform actions that trigger the script to send data to Google Sheets.
2.  **Check your Google Sheet** to see if the data appears as expected.

### Protips

-   Customize the Unity and Google Apps Script code as necessary to fit your project's specific needs.
-   Ensure you follow Google's guidelines and limitations for Apps Script and web app deployments.

