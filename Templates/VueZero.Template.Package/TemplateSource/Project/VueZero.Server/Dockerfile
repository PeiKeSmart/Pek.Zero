# 璇峰弬闃?https://aka.ms/customizecontainer 浠ヤ簡瑙ｅ浣曡嚜瀹氫箟璋冭瘯瀹瑰櫒锛屼互鍙?Visual Studio 濡備綍浣跨敤姝?Dockerfile 鐢熸垚鏄犲儚浠ユ洿蹇湴杩涜璋冭瘯銆?

# 姝ら樁娈电敤浜庡湪蹇€熸ā寮?榛樿涓鸿皟璇曢厤缃?涓嬩粠 VS 杩愯鏃?
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# 姝ら樁娈电敤浜庣敓鎴愭湇鍔￠」鐩?
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs


FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["VueZero.Client/VueZero.Client.esproj", "VueZero.Client/"]
COPY ["VueZero.Server/VueZero.Server.csproj", "VueZero.Server/"]
COPY ["VueZero.Client/VueZero.Client.esproj", "VueZero.Client/"]
RUN dotnet restore "./VueZero.Server/VueZero.Server.csproj"
COPY . .
WORKDIR "/src/VueZero.Server"
RUN dotnet build "./VueZero.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 姝ら樁娈电敤浜庡彂甯冭澶嶅埗鍒版渶缁堥樁娈电殑鏈嶅姟椤圭洰
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VueZero.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 姝ら樁娈靛湪鐢熶骇涓娇鐢紝鎴栧湪甯歌妯″紡涓嬩粠 VS 杩愯鏃朵娇鐢?鍦ㄤ笉浣跨敤璋冭瘯閰嶇疆鏃朵负榛樿鍊?
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VueZero.Server.dll"]

