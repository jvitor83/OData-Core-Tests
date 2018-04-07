FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY DotNet.AspNetCore.WebApi.OData.Sample.sln ./
COPY DotNet.AspNetCore.WebApi.OData.Sample/DotNet.AspNetCore.WebApi.OData.Sample.csproj DotNet.AspNetCore.WebApi.OData.Sample/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/DotNet.AspNetCore.WebApi.OData.Sample
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DotNet.AspNetCore.WebApi.OData.Sample.dll"]
