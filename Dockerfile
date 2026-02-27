FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copiar arquivos de projeto para melhor cache do Docker
COPY HackathonDataAnalysis.sln ./
COPY src/HackathonDataAnalysis.Api/HackathonDataAnalysis.Api.csproj src/HackathonDataAnalysis.Api/
COPY src/HackathonDataAnalysis.Application/HackathonDataAnalysis.Application.csproj src/HackathonDataAnalysis.Application/
COPY src/HackathonDataAnalysis.Data/HackathonDataAnalysis.Data.csproj src/HackathonDataAnalysis.Data/
COPY src/HackathonDataAnalysis.Domain/HackathonDataAnalysis.Domain.csproj src/HackathonDataAnalysis.Domain/
COPY src/HackathonDataAnalysis.Security/HackathonDataAnalysis.Security.csproj src/HackathonDataAnalysis.Security/
COPY src/HackathonDataAnalysis.Auth/HackathonDataAnalysis.Auth.csproj src/HackathonDataAnalysis.Auth/
COPY src/HackathonDataAnalysis.NewRelicEvent/HackathonDataAnalysis.NewRelicEvent.csproj src/HackathonDataAnalysis.NewRelicEvent/
COPY src/HackathonDataAnalysis.Plots/HackathonDataAnalysis.Plots.csproj src/HackathonDataAnalysis.Plots/

# Copiar arquivos
COPY src/ src/

# Publicar o projeto
RUN dotnet publish src/HackathonDataAnalysis.Api/HackathonDataAnalysis.Api.csproj -c Release -o /app/publish

# Runtime stage - usando Alpine para imagem mais leve
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Instalar New Relic
RUN apk update && apk add --no-cache wget tar \
    && wget https://download.newrelic.com/dot_net_agent/latest_release/newrelic-dotnet-agent_amd64.tar.gz -r \
    && tar -xzf download.newrelic.com/dot_net_agent/latest_release/newrelic-dotnet-agent_amd64.tar.gz -C /usr/local \ 
    && rm -rf download.newrelic.com

# Configurações New Relic
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_APPLICATION_LOGGING_ENABLED=true \
NEW_RELIC_APPLICATION_LOGGING_FORWARDING_ENABLED=true \
NEW_RELIC_APPLICATION_LOGGING_FORWARDING_CONTEXT_DATA_ENABLED=true \
NEW_RELIC_APPLICATION_LOGGING_FORWARDING_MAX_SAMPLES_STORED=10000 \
NEW_RELIC_APPLICATION_LOGGING_LOCAL_DECORATING_ENABLED=true \
NEW_RELIC_APP_NAME="hackathon-dataanalysis-newrelic"

WORKDIR /app

# Criar non-root user (Alpine Linux)
RUN addgroup -S appuser && adduser -S appuser -G appuser

# Copiar os arquivos publicados
COPY --from=build /app/publish .

# Trocar ownership para non-root user
RUN chown -R appuser:appuser /app
USER appuser

# Expor a porta
EXPOSE 8080

ENTRYPOINT ["dotnet", "HackathonDataAnalysis.Api.dll"]
