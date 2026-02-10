# Step 4: Configure SQL Server Connection - Detailed Guide

This guide explains how to configure your SQL Server connection string in detail.

## Overview

The connection string tells your application how to connect to your SQL Server database. It's located in the `appsettings.json` file.

## File Location

```
backend/
└── GradLink.Api/
    └── appsettings.json   ← Edit this file
```

## Current Default Configuration

Open `backend/GradLink.Api/appsettings.json` and find this section:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GradLinkDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

## Understanding the Connection String

Let's break down each part:

### Connection String Components

```
Server=localhost;Database=GradLinkDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=true
```

| Component | What it means | Example |
|-----------|---------------|---------|
| `Server=` | SQL Server location | `localhost`, `127.0.0.1`, `.\SQLEXPRESS` |
| `Database=` | Database name | `GradLinkDb` |
| `User Id=` | SQL Server username | `sa`, `your_username` |
| `Password=` | User's password | `YourStrong@Password123` |
| `TrustServerCertificate=` | Accept self-signed certificates | `True` (dev), `False` (prod) |
| `MultipleActiveResultSets=` | Multiple queries at once | `true` |

---

## Configuration Scenarios (Choose One)

### Scenario 1: Local SQL Server with SA Account (Default)

**When to use:** You installed SQL Server on your local machine and want to use the built-in SA (System Administrator) account.

#### Step-by-Step Configuration

**Step 1.1:** Ensure SQL Server is installed and running
- Check Windows Services for "SQL Server (MSSQLSERVER)"
- Or use Docker command provided in prerequisites

**Step 1.2:** Verify SA password
The default SA password in our config is: `YourStrong@Password123`

**Important:** If you set a different password during SQL Server installation, use that password!

**Step 1.3:** Update `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GradLinkDb;User Id=sa;Password=YOUR_ACTUAL_SA_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**Replace:** `YOUR_ACTUAL_SA_PASSWORD` with your actual SA password.

**Step 1.4:** Test the connection

```bash
# Using sqlcmd (if installed)
sqlcmd -S localhost -U sa -P YOUR_ACTUAL_SA_PASSWORD

# You should see:
# 1>
```

Type `EXIT` to quit.

---

### Scenario 2: Windows Authentication (No Password Needed)

**When to use:** You're on Windows and want to use your Windows login instead of SQL username/password.

**Advantages:**
- ✅ No password in config file
- ✅ More secure
- ✅ Uses your Windows credentials automatically

#### Step-by-Step Configuration

**Step 2.1:** Check your Windows user has SQL Server access
- Open SQL Server Management Studio (SSMS)
- Connect using Windows Authentication
- If successful, you have access!

**Step 2.2:** Update `appsettings.json`

**Remove** `User Id` and `Password`, **Add** `Integrated Security=true`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GradLinkDb;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**Step 2.3:** Test the connection

```bash
# Using sqlcmd
sqlcmd -S localhost -E

# -E means use Windows Authentication
```

---

### Scenario 3: SQL Server Named Instance (e.g., SQLEXPRESS)

**When to use:** You installed SQL Server Express, and your instance is named `SQLEXPRESS`.

#### Step-by-Step Configuration

**Step 3.1:** Find your instance name

```powershell
# PowerShell command to list SQL Server instances
Get-Service | Where-Object {$_.DisplayName -like "*SQL Server*"}
```

Common instance names:
- `MSSQLSERVER` (default instance) → Use `localhost`
- `SQLEXPRESS` → Use `localhost\SQLEXPRESS`
- `MSSQL$INSTANCENAME` → Use `localhost\INSTANCENAME`

**Step 3.2:** Determine authentication method

**Option A: Windows Authentication (Recommended)**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=GradLinkDb;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**Note:** Use double backslash `\\` in JSON!

**Option B: SQL Authentication**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=GradLinkDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**Step 3.3:** Enable TCP/IP for SQLEXPRESS

**Important:** SQL Express often has TCP/IP disabled by default!

1. Open **SQL Server Configuration Manager**
2. Expand **SQL Server Network Configuration**
3. Click **Protocols for SQLEXPRESS**
4. Right-click **TCP/IP** → **Enable**
5. Restart SQL Server service

**Step 3.4:** Test the connection

```bash
# With Windows Auth
sqlcmd -S localhost\SQLEXPRESS -E

# With SQL Auth
sqlcmd -S localhost\SQLEXPRESS -U sa -P YOUR_PASSWORD
```

---

### Scenario 4: SQL Server on Different Port

**When to use:** Your SQL Server is running on a non-standard port (not 1433).

#### Step-by-Step Configuration

**Step 4.1:** Find the port number

**Method 1: SQL Server Configuration Manager**
1. Open SQL Server Configuration Manager
2. Expand **SQL Server Network Configuration**
3. Click **Protocols for MSSQLSERVER** (or your instance)
4. Right-click **TCP/IP** → **Properties**
5. Go to **IP Addresses** tab
6. Look for **TCP Dynamic Ports** or **TCP Port**

**Method 2: Check Services**
The port is usually shown in SQL Server error logs or service properties.

**Step 4.2:** Update connection string with port

Add the port after the server name with a comma:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1434;Database=GradLinkDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**Format:** `Server=hostname,port`

**Step 4.3:** Test the connection

```bash
sqlcmd -S localhost,1434 -U sa -P YOUR_PASSWORD
```

---

### Scenario 5: SQL Server on Network/Remote Server

**When to use:** SQL Server is on another computer in your network or a remote server.

#### Step-by-Step Configuration

**Step 5.1:** Get server information
You need:
- Server name or IP address
- Port (usually 1433)
- Username and password
- Whether it's a named instance

**Step 5.2:** Update connection string

**With IP Address:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=192.168.1.100;Database=GradLinkDb;User Id=your_user;Password=your_password;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**With Server Name:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SERVERNAME;Database=GradLinkDb;User Id=your_user;Password=your_password;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**With IP and Port:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=192.168.1.100,1433;Database=GradLinkDb;User Id=your_user;Password=your_password;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**Step 5.3:** Ensure SQL Server allows remote connections

On the SQL Server machine:
1. Open **SQL Server Management Studio**
2. Right-click server name → **Properties**
3. Go to **Connections** page
4. Check **"Allow remote connections to this server"**
5. Click OK and restart SQL Server

**Step 5.4:** Check firewall

Windows Firewall must allow SQL Server:
1. Open **Windows Firewall with Advanced Security**
2. Click **Inbound Rules** → **New Rule**
3. Select **Port** → **Next**
4. Select **TCP** → Enter port `1433` → **Next**
5. Select **Allow the connection** → **Next**
6. Name it "SQL Server" → **Finish**

**Step 5.5:** Test the connection

```bash
# From your local machine
sqlcmd -S 192.168.1.100 -U your_user -P your_password

# Or with server name
sqlcmd -S SERVERNAME -U your_user -P your_password
```

---

### Scenario 6: SQL Server in Docker

**When to use:** You're running SQL Server in a Docker container.

#### Step-by-Step Configuration

**Step 6.1:** Start SQL Server Docker container

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

**Step 6.2:** Verify container is running

```bash
docker ps

# You should see sqlserver container with status "Up"
```

**Step 6.3:** Get container IP (if needed)

```bash
docker inspect sqlserver | grep IPAddress
```

But usually, `localhost` works because we mapped port 1433.

**Step 6.4:** Update connection string

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GradLinkDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SqlServer"
}
```

**Important:** Use the same password you set in `-e "SA_PASSWORD=..."`

**Step 6.5:** Test the connection

```bash
# Test from host machine
sqlcmd -S localhost -U sa -P YourStrong@Password123

# Or test from inside container
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Password123
```

---

### Scenario 7: Azure SQL Database

**When to use:** You're using Azure SQL Database in the cloud.

#### Step-by-Step Configuration

**Step 7.1:** Get Azure SQL connection string

1. Go to [Azure Portal](https://portal.azure.com)
2. Navigate to your SQL Database
3. Click **"Connection strings"** in left menu
4. Copy the **ADO.NET** connection string

**Step 7.2:** Update connection string

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Database=GradLinkDb;User ID=yourusername@yourserver;Password=yourpassword;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  },
  "DatabaseProvider": "SqlServer"
}
```

**Replace:**
- `yourserver` → Your Azure SQL server name
- `yourusername` → Your database username
- `yourpassword` → Your database password

**Key differences from local SQL:**
- ✅ Must use `tcp:` prefix
- ✅ `Encrypt=True` (encrypted connection)
- ✅ `TrustServerCertificate=False` (verify certificate)
- ✅ Full username format: `username@servername`

---

## Special Connection String Parameters

### Common Additional Parameters

```json
"Server=localhost;Database=GradLinkDb;User Id=sa;Password=pass;TrustServerCertificate=True;MultipleActiveResultSets=true;Connection Timeout=30;Encrypt=False;Min Pool Size=0;Max Pool Size=100"
```

| Parameter | Description | Default | When to Change |
|-----------|-------------|---------|----------------|
| `Connection Timeout=30` | Seconds to wait for connection | 15 | Slow networks |
| `Encrypt=True/False` | Encrypt connection | False | Always True in production |
| `Min Pool Size=0` | Minimum connections in pool | 0 | High traffic apps |
| `Max Pool Size=100` | Maximum connections in pool | 100 | Very high traffic |
| `Application Name=GradLink` | App name in SQL logs | None | Debugging |

### For Production (Recommended)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=GradLinkDb;User Id=app_user;Password=strong_password;TrustServerCertificate=False;Encrypt=True;MultipleActiveResultSets=true;Connection Timeout=30"
  },
  "DatabaseProvider": "SqlServer"
}
```

---

## Security Best Practices

### ⚠️ NEVER Do This

**DON'T** commit passwords to Git:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;...;Password=MyActualPassword123"
  }
}
```

### ✅ DO This Instead

**Option 1: Use User Secrets (Development)**

```bash
cd GradLink.Api

# Initialize user secrets
dotnet user-secrets init

# Set connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=GradLinkDb;User Id=sa;Password=ActualPassword123;TrustServerCertificate=True"
```

Then in `appsettings.json`, you can remove or use a placeholder:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GradLinkDb;Integrated Security=true;TrustServerCertificate=True"
  }
}
```

**Option 2: Environment Variables (Production)**

Set environment variable:
```bash
# Windows
set ConnectionStrings__DefaultConnection=Server=...

# Linux/Mac
export ConnectionStrings__DefaultConnection="Server=..."
```

**Option 3: Azure Key Vault (Production)**

Store connection strings in Azure Key Vault and reference them in your app.

---

## Verification Checklist

After configuring your connection string, verify:

✅ **Step 1:** Can you connect using sqlcmd?
```bash
sqlcmd -S your-server -U your-user -P your-password
```

✅ **Step 2:** Can you connect using SSMS or Azure Data Studio?

✅ **Step 3:** Build the project without errors:
```bash
dotnet build
```

✅ **Step 4:** Run the application:
```bash
dotnet run
```

✅ **Step 5:** Check for database creation:
- Look in SSMS
- Or query: `SELECT name FROM sys.databases WHERE name = 'GradLinkDb'`

---

## Troubleshooting Common Issues

### Issue 1: "Login failed for user 'sa'"

**Causes:**
- Wrong password
- SA account disabled
- SQL Server not in mixed mode authentication

**Solutions:**

1. **Enable mixed mode authentication:**
```sql
-- In SSMS, run as admin
USE master;
GO
EXEC xp_instance_regwrite 
  N'HKEY_LOCAL_MACHINE', 
  N'Software\Microsoft\MSSQLServer\MSSQLServer',
  N'LoginMode', REG_DWORD, 2;
GO
```
Then restart SQL Server.

2. **Enable SA account:**
```sql
ALTER LOGIN sa ENABLE;
GO
ALTER LOGIN sa WITH PASSWORD = 'YourStrong@Password123';
GO
```

### Issue 2: "A network-related or instance-specific error"

**Causes:**
- SQL Server not running
- Wrong server name
- Firewall blocking
- TCP/IP disabled

**Solutions:**

1. **Check SQL Server is running:**
```powershell
Get-Service MSSQLSERVER
Start-Service MSSQLSERVER
```

2. **Enable TCP/IP:**
- SQL Server Configuration Manager
- Protocols for MSSQLSERVER
- Enable TCP/IP
- Restart service

3. **Check firewall:**
- Allow port 1433
- Allow SQL Server program

### Issue 3: "Cannot open database 'GradLinkDb'"

**Solution:** Database will be created automatically on first run. If not:

```sql
CREATE DATABASE GradLinkDb;
GO
```

### Issue 4: "TrustServerCertificate" errors

**Solution:** For development, use:
```
TrustServerCertificate=True
```

For production with proper SSL, use:
```
TrustServerCertificate=False;Encrypt=True
```

---

## Quick Reference: Connection String Templates

### Local Development
```
Server=localhost;Database=GradLinkDb;Integrated Security=true;TrustServerCertificate=True
```

### Local with SA
```
Server=localhost;Database=GradLinkDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True
```

### SQL Express
```
Server=localhost\\SQLEXPRESS;Database=GradLinkDb;Integrated Security=true;TrustServerCertificate=True
```

### Docker
```
Server=localhost;Database=GradLinkDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True
```

### Remote Server
```
Server=192.168.1.100,1433;Database=GradLinkDb;User Id=user;Password=pass;TrustServerCertificate=True
```

### Azure SQL
```
Server=tcp:server.database.windows.net,1433;Database=GradLinkDb;User ID=user@server;Password=pass;Encrypt=True
```

---

## Summary

1. **Find** your SQL Server location (localhost, IP, server name)
2. **Choose** authentication method (Windows or SQL)
3. **Get** credentials (username/password if using SQL auth)
4. **Update** `appsettings.json` with correct connection string
5. **Test** connection with sqlcmd or SSMS
6. **Run** the application

**Default works for most:** If you installed SQL Server locally with default settings, the current configuration should work as-is!

---

**Need More Help?**
- See `DATABASE_SETUP.md` for SQL Server installation
- See `README.md` for complete setup guide
- Check SQL Server error logs for detailed error messages

