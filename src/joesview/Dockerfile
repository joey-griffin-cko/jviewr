FROM microsoft/dotnet:2.2.104-sdk-alpine AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# copy csproj and restore as distinct layers
COPY ./src/joesview/joesview.csproj ./joesview/
RUN dotnet restore joesview/joesview.csproj

# copy everything else and build
COPY ./src/ .
WORKDIR /joesview/
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

# build runtime image
FROM microsoft/dotnet:2.2.2-aspnetcore-runtime-alpine
WORKDIR /app
COPY --from=build joesview/out ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "joesview.dll"]