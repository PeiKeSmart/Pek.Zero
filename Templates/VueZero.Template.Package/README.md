# PekVueZero 模板说明

## 概述

该目录提供一套基于 PekVueZero 的 dotnet new 模板包，包含以下两个模板：

- pekvuezero：生成 Vue 3 + Vite + ASP.NET Core 的前后端项目骨架
- pekvuezero-sln：生成带解决方案文件的完整骨架

模板包项目：Templates/VueZero.Template.Package/VueZero.Template.Package.csproj

模板包 ID：PekVueZero.Template

一键脚本：Templates/VueZero.Template.Package/pack-template.ps1

## 模板内容

项目模板生成后包含以下目录：

- DemoVueZero.Server：后端 ASP.NET Core 项目
- DemoVueZero.Client：前端 Vue 3 + Vite 项目

解决方案模板额外生成：

- DemoVueZero.slnx：解决方案文件

## 打包与使用

在仓库根目录执行：

```powershell
.\Templates\VueZero.Template.Package\pack-template.ps1
```

只打包不安装：

```powershell
dotnet pack .\Templates\VueZero.Template.Package\VueZero.Template.Package.csproj -c Release
```

该模板包不再直接提供安装入口。统一安装方式请使用聚合模板包：

```powershell
.\Templates\PeiKeSmart.Template.Bundle\pack-template.ps1 -Install
```

## 一键脚本

脚本支持以下参数：

| 参数 | 说明 |
|------|------|
| Configuration | 打包配置，默认 Release |
| PackageVersion | 指定模板包版本号 |

说明：

```powershell
.\Templates\VueZero.Template.Package\pack-template.ps1
```

如需安装或做 SmokeTest，请统一使用聚合模板包脚本。

## 创建项目模板

最小命令：

```powershell
dotnet new pekvuezero -n DemoPekVueZero -o .\Output\DemoPekVueZero
```

完整示例：

```powershell
dotnet new pekvuezero -n DemoPekVueZero -o .\Output\DemoPekVueZero --ProjectTitle DemoApp --ProjectDescription "Demo full stack template" --CompanyName DemoCompany --ServiceName DemoPekVueZero.Service --ServerHttpPort 5217 --ServerHttpsPort 7372 --ClientDevPort 53017 --ClientPackageName demo.client
```

生成后可编译后端：

```powershell
dotnet build .\Output\DemoPekVueZero\DemoPekVueZero.Server\DemoPekVueZero.Server.csproj -nologo
```

## 创建解决方案模板

最小命令：

```powershell
dotnet new pekvuezero-sln -n DemoPekVueZero -o .\Output\DemoPekVueZero
```

完整示例：

```powershell
dotnet new pekvuezero-sln -n DemoPekVueZero -o .\Output\DemoPekVueZero --ProjectTitle DemoApp --ProjectDescription "Demo full stack template" --CompanyName DemoCompany --ServiceName DemoPekVueZero.Service --ServerHttpPort 5217 --ServerHttpsPort 7372 --ClientDevPort 53017 --ClientPackageName demo.client
```

生成后可编译整个解决方案：

```powershell
dotnet build .\Output\DemoPekVueZero\DemoPekVueZero.slnx -nologo
```

## 生成后如何启动

### 启动后端

在后端项目目录执行：

```powershell
cd .\Output\DemoPekVueZero\DemoPekVueZero.Server
dotnet run
```

默认监听地址取决于模板参数：

- HTTP：ServerHttpPort
- HTTPS：ServerHttpsPort

例如使用默认值时：

- http://localhost:5117
- https://localhost:7272

### 启动前端开发服务器

在前端项目目录执行：

```powershell
cd .\Output\DemoPekVueZero\DemoPekVueZero.Client
npm install
npm run dev
```

默认前端开发地址取决于 ClientDevPort，默认是：

- https://localhost:53007

### 前后端联调

推荐顺序：

1. 先启动后端 DemoPekVueZero.Server
2. 再启动前端 DemoPekVueZero.Client
3. 浏览器访问前端 Vite 地址进行联调

前端开发代理会根据后端启动参数自动转发到 ASP.NET Core 服务。

## 模板参数

| 参数 | 默认值 | 说明 |
|------|--------|------|
| ProjectTitle | VueZero 应用 | 项目标题，同时写入后端程序集标题、Swagger 标题、前端 README 标题 |
| ProjectDescription | 基于 Vue 3 + Vite + ASP.NET Core 的前后端模板 | 项目描述，同时写入后端程序集描述和 Swagger 描述 |
| CompanyName | PeiKeSmart | 公司名称，同时写入程序集 Company 等元数据 |
| ServiceName | __SERVICE_NAME__ | 服务注册名称。未显式传入时，运行时回退到后端应用名 |
| ServerHttpPort | 5117 | 后端 HTTP 端口 |
| ServerHttpsPort | 7272 | 后端 HTTPS 端口 |
| ClientDevPort | 53007 | 前端 Vite 开发端口，同时用于 SpaProxyServerUrl |
| ClientPackageName | vuezero.client | 前端包名，同时用于本地开发证书名 |

## 参数影响点

主要替换位置包括：

- 后端项目文件中的程序集元数据
- 后端 Program.cs 中的 RegisterService 服务名
- 后端 appsettings.json 和 launchSettings.json 中的端口
- 后端 Data/Settings/Swagger.json 中的标题、描述和联系信息
- 前端 package.json 中的 name
- 前端 vite.config.ts 中的开发证书名和开发端口

## 验证状态

当前模板已完成以下验证：

- 模板包可正常打包
- 模板通过 PekBundle.Template 可正常安装
- pekvuezero 可正常生成并编译通过
- pekvuezero-sln 可正常生成并编译通过
- 聚合模板包 pack-template.ps1 -Install -SmokeTest 已验证通过

## 常用命令

查看模板帮助：

```powershell
dotnet new pekvuezero -h
dotnet new pekvuezero-sln -h
```

统一安装最新模板包：

```powershell
.\Templates\PeiKeSmart.Template.Bundle\pack-template.ps1 -Install
```