# Railway Deployment Guide

## Required Environment Variables

Set these in Railway dashboard (Variables tab):

### Database
```
ConnectionStrings__DefaultConnection=Server=your-server;Database=your-db;User Id=your-user;Password=your-password;Encrypt=True;TrustServerCertificate=True;
DatabaseProvider=SqlServer
```

### JWT Authentication
```
Jwt__Secret=YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!
```

### Email (SMTP)
```
Email__Username=your-email@gmail.com
Email__Password=your-app-password
Email__FromEmail=your-email@gmail.com
```

### CORS - Frontend URLs
```
Cors__AllowedOrigins__0=https://your-app.vercel.app
Cors__AllowedOrigins__1=http://localhost:5176
```

### OAuth (Optional)
```
OAuth__Google__ClientId=your-google-client-id
OAuth__Google__ClientSecret=your-google-client-secret
```

### Frontend URL (for OAuth redirects)
```
FrontendUrl=https://your-app.vercel.app
```

## Deployment Steps

1. **Create Railway Account**
   - Go to [railway.app](https://railway.app)
   - Sign up with GitHub

2. **Create New Project**
   - Click "New Project"
   - Select "Deploy from GitHub repo"
   - Choose `mennaa7med/GradLink`

3. **Configure Root Directory**
   - Go to Settings → Root Directory
   - Set to: `backend`

4. **Add Environment Variables**
   - Go to Variables tab
   - Add all required variables listed above

5. **Deploy**
   - Railway will automatically build and deploy
   - Check Deployments tab for logs

## Database Options

### Option 1: Use existing database
Keep your current SQL Server connection string.

### Option 2: Railway PostgreSQL (Free tier available)
1. Click "New" → "Database" → "PostgreSQL"
2. Railway will provide connection string
3. Note: You'll need to change `DatabaseProvider` to `Sqlite` or add PostgreSQL support

### Option 3: Railway MySQL
Similar to PostgreSQL setup.

## Healthcheck

The app exposes Swagger at `/swagger` which Railway uses for healthcheck.

## Troubleshooting

### Build fails
- Check the Dockerfile path is correct
- Ensure all .csproj files are present

### App crashes on start
- Check environment variables are set correctly
- View logs in Deployments tab

### Database connection fails
- Verify connection string format
- Check firewall rules on database server

