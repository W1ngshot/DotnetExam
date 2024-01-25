FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DotnetExam/DotnetExam.csproj", "DotnetExam/"]
RUN dotnet restore "DotnetExam/DotnetExam.csproj"
COPY . .
WORKDIR "/src/DotnetExam"
RUN dotnet build "DotnetExam.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotnetExam.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotnetExam.dll"]
