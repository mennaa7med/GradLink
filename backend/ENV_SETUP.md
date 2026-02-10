# Environment Variables Setup

## Google OAuth Configuration

To enable Google OAuth login, you need to set the following environment variables:

### Option 1: Using System Environment Variables

```bash
# Windows (PowerShell)
$env:OAuth__Google__ClientId = "your-google-client-id.apps.googleusercontent.com"
$env:OAuth__Google__ClientSecret = "your-google-client-secret"

# Windows (CMD)
set OAuth__Google__ClientId=your-google-client-id.apps.googleusercontent.com
set OAuth__Google__ClientSecret=your-google-client-secret

# Linux/Mac
export OAuth__Google__ClientId="your-google-client-id.apps.googleusercontent.com"
export OAuth__Google__ClientSecret="your-google-client-secret"
```

### Option 2: Using launchSettings.json (Development Only)

Edit `GradLink.Api/Properties/launchSettings.json` and add your credentials:

```json
"environmentVariables": {
  "ASPNETCORE_ENVIRONMENT": "Development",
  "OAuth__Google__ClientId": "your-google-client-id.apps.googleusercontent.com",
  "OAuth__Google__ClientSecret": "your-google-client-secret"
}
```

### Option 3: Using User Secrets (Recommended for Development)

```bash
cd GradLink.Api
dotnet user-secrets init
dotnet user-secrets set "OAuth:Google:ClientId" "your-google-client-id.apps.googleusercontent.com"
dotnet user-secrets set "OAuth:Google:ClientSecret" "your-google-client-secret"
```

## Getting Google OAuth Credentials

1. Go to [Google Cloud Console](https://console.cloud.google.com/apis/credentials)
2. Create a new project or select existing one
3. Go to "Credentials" → "Create Credentials" → "OAuth client ID"
4. Select "Web application"
5. Add authorized redirect URIs:
   - `http://localhost:5176/oauth/callback/google` (development)
   - `https://yourdomain.com/oauth/callback/google` (production)
6. Copy the Client ID and Client Secret

## GitHub OAuth (Optional)

```bash
OAuth__GitHub__ClientId=your-github-client-id
OAuth__GitHub__ClientSecret=your-github-client-secret
```

Get credentials from: https://github.com/settings/developers

## Notes

- In .NET Core, nested configuration keys use `__` (double underscore) in environment variables
- Example: `OAuth:Google:ClientId` becomes `OAuth__Google__ClientId`
- Never commit real credentials to the repository

