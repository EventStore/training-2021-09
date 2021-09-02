FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["XeroTraining/XeroTraining.csproj", "XeroTraining/"]
RUN dotnet restore "XeroTraining/XeroTraining.csproj"
COPY . .
WORKDIR "/src/XeroTraining"
RUN dotnet build "XeroTraining.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XeroTraining.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XeroTraining.dll"]
