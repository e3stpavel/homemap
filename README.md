# Homemap
Software-as-a-Service that lets you manage your IoT devices with ease.

# Demo
Use this to send a state (`prj/1/rcv/2/dev/2/state`)

```json
{
  "status": "on",
  "lightTemperature": 6000,
  "brightness": 60
}
```

Use this to send a log (`prj/1/rcv/2/dev/2/logs`)

```json
{
  "level": "info",
  "message": "hello world",
  "timestamp": "2024-12-16T15:28:52Z"
}
```

## Development
If you are feeling adventurous, try using Docker.

### Local development
Use instructions in readme in dedicated project's directory.

### Using Docker
To start all the services of the application use
```bash
docker-compose up -d
```

> **Note!** This will pull some images if you have not installed them already, so the first run will take some time.

To stop all the services use
```bash
docker-compose down
```

If you want also remove all the data use
```bash
docker-compose down --volumes
```

To run specific service use
```bash
docker-compose up <service>
```

#### Known issues
##### dotnet dev-certs
To start `dotnet` service you might need to create https certificates. The process explained in [MS docs](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-8.0#starting-a-container-with-https-support-using-docker-compose).

The instructions below are for Windows users. If you're using Mac then reference the [MS docs](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-8.0#starting-a-container-with-https-support-using-docker-compose).

Create a certificate
```powershell
dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx"  -p $PASSWORD$
dotnet dev-certs https --trust
```

Replace `$PASSWORD$` with your own password.

If you see (you most likely will) the following message
```
A valid HTTPS certificate is already present.
```

Then you need to do the following
```powershell
dotnet dev-certs https --format Pfx -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx" -p $PASSWORD$
```

This will create a new file
```
%USERPROFILE%/.aspnet/https/aspnetapp.pfx
```

Next you should set the environment variable `ASPNETCORE_Kestrel__Certificates__Default__Password` with your own password.

Copy `.env.example` (inside `./api` folder) and rename it to `.env`. Set your own password.

Now you're good to go!

##### Vite HMR
If you are working on the Nuxt app project you might want to see real-time updates. This feature is off by default but you can enable it.

To do that, you need to uncomment lines in `nuxt.config.ts` that set up the `$development` server.

After that the HMR will be enabled.

> **Note!** It is not recommended for Windows users as this might drastically impact on your CPU.

> When running Vite on WSL2, file system watching does not work when a file is edited by Windows applications (non-WSL2 process). This is due to a WSL2 limitation. This also applies to running on Docker with a WSL2 backend. _[See more](https://vite.dev/config/server-options#server-watch)_

##### SQLite persistence
Right now the SQLite database is not saved across different runs. 

This means that if you stop the container and then run again the **data will not be persisted**.

This will be fixed later by using volumes and mounting SQLite related data separately ([Read more](https://forums.docker.com/t/accessing-a-db-inside-a-docker-container/116106/6)).

##### MQTT client
If you want to see what is going on with your MQTT connection, which messages are sent and so on, you could download and install [MQTTX](https://mqttx.app/downloads).
