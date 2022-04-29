FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app

RUN apk update && apk add nodejs npm

COPY ./Backend/*.csproj ./Backend/
COPY ./Backend.Tests/*.csproj ./Backend.Tests/
COPY *.sln ./

RUN dotnet restore

COPY ./Frontend/package*.json ./Frontend/

RUN cd Frontend \
    && npm ci

COPY . .
RUN dotnet publish -c Release -o build

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app

COPY --from=build /app/build .
COPY --from=build /app/Frontend/build Frontend

EXPOSE 80
EXPOSE 443

ENTRYPOINT [ "dotnet", "./Backend.dll" ]
