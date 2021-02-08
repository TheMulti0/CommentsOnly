#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["CommentsOnly/CommentsOnly.csproj", "CommentsOnly/"]
RUN dotnet restore "CommentsOnly/CommentsOnly.csproj"
COPY . .
WORKDIR "/src/CommentsOnly"
RUN dotnet build "CommentsOnly.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommentsOnly.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommentsOnly.dll"]