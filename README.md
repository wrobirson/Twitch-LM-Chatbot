# Twitch LM Chat Bot

A Twitch Chat Bot that intelligently responds to chat messages using a Large Language Model (LLM) Completions API, similar to ChatGPT.

## Features

- **Intelligent Responses**: Utilizes LLM to deliver context-aware and dynamic responses in Twitch chat.
- **Seamless Integration**: Easily connects to your Twitch channel with minimal setup.
- **Highly Customizable**: Offers flexible configuration options to tailor the bot's behavior to your specific needs.

## Development Setup

### Prerequisites

- Visual Studio 2022 or later
- .NET Core 8.0 or later
- [Nuke Build](https://nuke.build/) tool installed globally

### Installation

#### Clone the Repository

Start by cloning the repository to your local machine:

```bash
git clone https://github.com/wrobirson/Twitch-LLM-Chat-Bot.git
```

#### Open the Project

Navigate to the project directory and open the solution file `Twitch LLM Chat Bot.sln` in Visual Studio.

### Configuration

Configure the bot settings by editing the `appsettings.json` file located in the project root:

```json
{
  "Twitch": {
    "ClientId": "<Your_Client_Id>",
    "ClientSecret": "<Your_Client_Secret>",
    "RedirectUri": "https://localhost:7231/auth/twitch/callback"
  }
}
```

#### Configuration Details

- **`ClientId`**:
  - **Description**: Your Twitch application’s Client ID.
  - **How to Obtain**: Available from the [Twitch Developer Console](https://dev.twitch.tv/console).
  - **Security Note**: Treat this ID as sensitive information.
- **`ClientSecret`**:
  - **Description**: Your Twitch application’s Client Secret.
  - **How to Obtain**: Available from the [Twitch Developer Console](https://dev.twitch.tv/console).
  - **Security Note**: Keep this secret safe and do not share it publicly.
- **`RedirectUri`**:
  - **Description**: The URI where Twitch will redirect users after successful authentication.
  - **Default (Development)**: `https://localhost:7231/auth/twitch/callback`

### Installing Nuke Build Tool

To build the project using Nuke Build, ensure that the Nuke Build tool is installed globally:

1. Open a terminal or command prompt.

2. Install Nuke Build globally by running:

   ```bash
   dotnet tool install Nuke.GlobalTool --global
   ```
   
3. Verify the installation by checking the version:

   ```bash
   nuke --version
   ```
   
   If installed correctly, the version number will be displayed.

### Running the Server

Follow these steps to run the server:

1. Open the solution in Visual Studio.
2. Configure the necessary settings in `appsettings.json`.
3. Build and run the project using Visual Studio.

### Running the Client

To start the client:

1. Navigate to the `TwitchLMChatBot.Client` directory in your terminal.

2. Execute the following command to start the development server:

   ```bash
   npm run dev
   ```

## Deployment

To deploy the bot:

1. **Compile the Release Version**

   From the project root, compile the release version by executing:

   ```bash
   nuke compile
   ```
   
   This command will generate the compiled files in the `publish` folder and will:

   - Clean the `publish` directory.
   - Restore NuGet dependencies.
   - Publish the server project as a single self-contained executable.
   - Build and integrate the client application.
   - Copy the client files to the server's `wwwroot` directory.
   - Remove unnecessary files, such as `appsettings.Development.json`.
   
2. **Update Production Settings**

   Add the `appsettings.Production.json` file with your production Twitch credentials from the [Twitch Developer Console](https://dev.twitch.tv/console).

3. **Launch the Bot**

   To start using the bot, run `TwitchLMChatBot.Server.exe`. Then, open your browser and go to `https://localhost:5001/`. Link your Twitch account, and once connected, you can begin interacting with your Twitch chat by using the `!ia` command.

## Contributing

We welcome contributions! If you find a bug or have a feature request, please open an issue. Pull requests are also encouraged.

## License

This project is licensed under the [MIT License](LICENSE).