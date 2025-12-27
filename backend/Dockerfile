FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS runtime
WORKDIR /app

# Install EF Core tools for migrations
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef

RUN mkdir -p /app/wwwroot/uploads

COPY --from=build /app/publish .
COPY --from=build /src .

# Create startup script that runs migrations and starts the app
RUN echo '#!/bin/bash\nset -e\necho "Running database migrations..."\ndotnet ef database update --no-build || echo "Migration failed, continuing..."\necho "Starting application..."\nexec dotnet backend.dll' > /app/start.sh && chmod +x /app/start.sh

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["/bin/bash", "/app/start.sh"]