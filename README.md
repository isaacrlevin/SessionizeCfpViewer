# Sessionize CFP Viewer

A .NET 10 Blazor Web App for browsing and managing Sessionize Call for Papers (CFP) submissions. This application fetches open CFPs from the Sessionize API and provides a user-friendly interface to search, filter, and view conference details.

## Demo

![Application Demo](demo.gif)

## Features

- 🔍 **Search & Filter**: Search by name, location, topics, tags, and more
- 📅 **Date Sorting**: Sort by CFP end date, CFP start date, event date, name, or country
- 🌍 **Location Filtering**: Filter by country
- 💰 **Expense Filters**: Filter conferences that cover travel and/or accommodation
- 📱 **Responsive Design**: Works on desktop and mobile devices
- 🔄 **Real-time Updates**: Refresh data from Sessionize API on demand
- 📊 **Pagination**: Browse through results with pagination controls
- 🔓 **CFP Status**: Quickly identify open vs closed CFPs

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- A Sessionize API token (get one from [Sessionize](https://sessionize.com))
- Windows, macOS, or Linux operating system

## Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/SessionizeCfpViewer.git
   cd SessionizeCfpViewer
   ```

2. **Navigate to the project directory**:
   ```bash
   cd src/SessionizeCfpViewer
   ```

3. **Configure the API token**:

   Edit `appsettings.json` and add your Sessionize API token:
   ```json
   {
     "API_TOKEN": "your-sessionize-api-token-here"
   }
   ```

   Alternatively, for development, use User Secrets:
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "API_TOKEN" "your-sessionize-api-token-here"
   ```

4. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

5. **Build the project**:
   ```bash
   dotnet build
   ```

## Usage

### Running the Application

1. **Start the application**:
   ```bash
   dotnet run
   ```

2. **Open your browser** and navigate to:
   - HTTPS: `https://localhost:5001`
   - HTTP: `http://localhost:5000`

   The exact port numbers will be displayed in the terminal when the application starts.

### Using the Interface

#### Home Page

The main page displays all available CFP sessions with the following features:

1. **Refresh Data**: Click "Refresh from API" to fetch the latest CFPs from Sessionize
2. **Search**: Type in the search box to filter by:
   - Conference name
   - Location (city, country)
   - Topics
   - Tags
   - Categories

3. **Sort Options**:
   - CFP End Date (default)
   - CFP Start Date
   - Event Start Date
   - Name (alphabetical)
   - Country

4. **Filter Options**:
   - **Open CFPs Only**: Show only conferences with open CFPs (checked by default)
   - **Ascending**: Change sort order from descending to ascending
   - **Travel Covered**: Show only conferences that cover travel expenses
   - **Accommodation Covered**: Show only conferences that cover accommodation

5. **Country Filter**: Use the dropdown to filter by specific country

6. **Pagination**: Navigate through results using Previous/Next buttons

#### Session Cards

Each conference card displays:
- Conference name
- Location (for in-person/hybrid events)
- CFP closing date
- Event dates
- Badges indicating:
  - 🌐 Online/In-Person/Hybrid
  - 💰 Free (if no conference fee)
  - ✈️ Travel Covered
  - 🏨 Accommodation Covered
  - 🔓 Open / 🔒 Closed (CFP status)
- Brief description
- Action buttons:
  - **View Details**: See full conference information
  - **Visit Site**: Go to conference website
  - **View CFP**: Direct link to submit your proposal

#### Session Detail Page

Click on any conference card or "View Details" to see:
- Full description
- Complete location details with timezone
- All important dates (CFP open/close, event dates, last updated)
- Topics and session formats accepted
- Tags and categories
- Social media links
- Direct links to submit proposals

## Configuration

### Application Settings

Edit `appsettings.json` to configure:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "API_TOKEN": "your-api-token-here"
}
```

### Launch Settings

The application is configured in `Properties/launchSettings.json` with the following profiles:
- **http**: Runs on http://localhost:5000
- **https**: Runs on https://localhost:5001
- **IIS Express**: For Visual Studio users

## Project Structure

```
SessionizeCfpViewer/
├── Components/
│   ├── Pages/
│   │   ├── Home.razor           # Main listing page
│   │   ├── SessionDetail.razor  # Detailed conference view
│   │   └── Error.razor          # Error handling page
│   ├── Layout/
│   │   ├── MainLayout.razor     # Application layout
│   │   └── NavMenu.razor        # Navigation menu
│   └── App.razor                # Root component
├── Models/
│   ├── CfpSession.cs           # API response model
│   └── CfpSessionFlat.cs       # Flattened data model
├── Services/
│   ├── SessionizeApiService.cs # API integration
│   ├── CfpDataService.cs       # Data management & caching
│   └── AppState.cs             # Application state
├── wwwroot/                    # Static assets
├── appsettings.json            # Configuration
└── Program.cs                  # Application entry point
```

## Features Explained

### Caching

The application uses an `AppState` service to cache API responses, reducing unnecessary API calls. Data is refreshed:
- On first load
- When clicking "Refresh from API"
- The cache persists during the application session

### Search

The search functionality uses a 300ms debounce to avoid excessive filtering while typing. It searches across:
- Conference names
- Location (city, state, country)
- Topics
- Tags
- Categories
- Session formats

### Responsive Design

The application uses Bootstrap 5 and is fully responsive:
- Cards adjust from 1 column (mobile) to 2 (tablet) to 3 (desktop)
- Navigation collapses on mobile
- Forms stack vertically on small screens

## Troubleshooting

### API Token Issues

**Error**: `API_TOKEN is not configured`

**Solution**: Ensure your API token is set in `appsettings.json` or user secrets:
```bash
dotnet user-secrets set "API_TOKEN" "your-token"
```

### No Data Showing

1. Check that your API token is valid
2. Click "Refresh from API" to fetch data
3. Check browser console for errors (F12)
4. Verify internet connectivity

### Build Errors

1. Ensure you have .NET 10 SDK or later installed:
   ```bash
   dotnet --version
   ```

2. Clean and rebuild:
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

## Development

### Adding New Features

1. **New Pages**: Add `.razor` files to `Components/Pages/`
2. **New Services**: Add classes to `Services/` and register in `Program.cs`
3. **New Models**: Add classes to `Models/`

### Running in Development Mode

```bash
dotnet run --environment Development
```

This enables:
- Detailed error pages
- Hot reload for code changes
- Enhanced logging

## Deployment

### Publishing for Production

1. **Publish the application**:
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **Deploy to a web server** (IIS, Azure, etc.)

### Azure App Service Deployment

#### Prerequisites

- An Azure account with an active subscription
- An Azure App Service (Web App) created in the Azure Portal
- Your Sessionize API token

#### Option 1: Automated Deployment with GitHub Actions

This project includes a GitHub Action workflow that automatically deploys to Azure App Service when you push to the `main` branch.

**Setup Steps:**

1. **Create an Azure App Service**:
   - Go to [Azure Portal](https://portal.azure.com)
   - Create a new Web App
   - Choose ".NET 10" as the runtime stack
   - Select your preferred region and pricing tier
   - Ensure SCM Basic Auth Publishing is enabled in Setting → Configuration

2. **Get the Publish Profile**:
   - In Azure Portal, go to your App Service
   - Click "Download publish profile" from the Overview page
   - Save the `.PublishSettings` file

3. **Configure GitHub Secrets**:
   - Go to your GitHub repository
   - Navigate to Settings → Secrets and variables → Actions
   - Click "New repository secret" and add:
     - `AZURE_WEBAPP_NAME`: Your Azure App Service name (e.g., `sessionize-cfp-viewer`)
     - `AZURE_WEBAPP_PUBLISH_PROFILE`: Paste the entire contents of the `.PublishSettings` file

4. **Configure App Service Settings**:
   - In Azure Portal, go to your App Service
   - Navigate to Configuration → Application settings
   - Add a new application setting:
     - Name: `API_TOKEN`
     - Value: Your Sessionize API token
   - Click "Save"

5. **Deploy**:
   - Push your code to the `main` branch
   - The GitHub Action will automatically build and deploy
   - Monitor the deployment in the "Actions" tab of your GitHub repository

**Manual Trigger:**

You can also manually trigger the deployment:
- Go to Actions tab in GitHub
- Select "Deploy to Azure App Service"
- Click "Run workflow"

#### Option 2: Manual Azure Deployment

1. **Install Azure CLI**:
   ```bash
   # Windows (using winget)
   winget install Microsoft.AzureCLI

   # macOS
   brew install azure-cli

   # Linux
   curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
   ```

2. **Login to Azure**:
   ```bash
   az login
   ```

3. **Create a Resource Group** (if needed):
   ```bash
   az group create --name SessionizeCfpViewerRG --location eastus
   ```

4. **Create an App Service Plan**:
   ```bash
   az appservice plan create --name SessionizeCfpViewerPlan --resource-group SessionizeCfpViewerRG --sku B1 --is-linux
   ```

5. **Create the Web App**:
   ```bash
   az webapp create --resource-group SessionizeCfpViewerRG --plan SessionizeCfpViewerPlan --name your-unique-app-name --runtime "DOTNETCORE:10.0"
   ```

6. **Configure the API Token**:
   ```bash
   az webapp config appsettings set --resource-group SessionizeCfpViewerRG --name your-unique-app-name --settings API_TOKEN="your-sessionize-api-token"
   ```

7. **Publish and Deploy**:
   ```bash
   cd src/SessionizeCfpViewer
   dotnet publish -c Release -o ./publish
   cd publish
   zip -r ../deploy.zip .
   az webapp deployment source config-zip --resource-group SessionizeCfpViewerRG --name your-unique-app-name --src ../deploy.zip
   ```

#### Option 3: Visual Studio Publish

1. Right-click on the `SessionizeCfpViewer` project in Solution Explorer
2. Select "Publish"
3. Choose "Azure" as the target
4. Select "Azure App Service (Windows)" or "Azure App Service (Linux)"
5. Sign in to your Azure account
6. Select or create your App Service
7. Click "Publish"

**Note:** Don't forget to configure the `API_TOKEN` application setting in Azure Portal after publishing.

#### Verify Deployment

After deployment:
1. Navigate to your Azure App Service URL (e.g., `https://your-app-name.azurewebsites.net`)
2. The app should load and display the CFP viewer
3. Click "Refresh from API" to verify the API token is configured correctly
4. Check Azure Portal → App Service → Log stream for any errors

#### Troubleshooting Azure Deployment

**App won't start:**
- Check Application Insights or Log stream in Azure Portal
- Verify the `API_TOKEN` is set in Application Settings
- Ensure the correct .NET runtime is selected

**API errors:**
- Verify your Sessionize API token is valid
- Check that the `API_TOKEN` application setting matches your token exactly

**GitHub Action fails:**
- Verify all secrets are set correctly in GitHub
- Check that the publish profile hasn't expired
- Review the Action logs for specific error messages

```

## Technologies Used

- **.NET 10: Core framework
- **Blazor Server**: Interactive web UI
- **Bootstrap 5**: Responsive styling
- **Bootstrap Icons**: Icon set
- **Sessionize API**: CFP data source

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the terms specified in the LICENSE.txt file.

## Support

For issues, questions, or contributions, please open an issue on GitHub.

## Acknowledgments

- [Sessionize](https://sessionize.com) for providing the CFP API
- The .NET and Blazor communities

---

**Happy CFP hunting! 🎤✨**