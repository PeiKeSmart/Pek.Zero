# PekMvc 模板说明

## 概述

该目录提供一套基于 PekMvc 的 dotnet new 模板包，包含以下两个模板：

- pekmvc：生成 PekMvc Web 项目
- pekmvc-sln：生成带解决方案骨架的 PekMvc 项目

模板包项目：Templates/PekMvc.Template.Package/PekMvc.Template.Package.csproj

一键脚本：Templates/PekMvc.Template.Package/pack-template.ps1

## 模板内容

项目模板生成后包含一个可直接运行的 PekMvc Web 项目。

解决方案模板额外生成：

- DemoPekMvc.slnx 风格的解决方案骨架
- Mvc/DemoPekMvcSolution/DemoPekMvcSolution.csproj 风格的项目结构

## 打包与使用

在仓库根目录执行：

```powershell
.\Templates\PekMvc.Template.Package\pack-template.ps1
```

只打包不安装：

```powershell
dotnet pack .\Templates\PekMvc.Template.Package\PekMvc.Template.Package.csproj -c Release
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
.\Templates\PekMvc.Template.Package\pack-template.ps1
```

如需安装或做 SmokeTest，请统一使用聚合模板包脚本。

## 创建项目模板

最小命令：

```powershell
dotnet new pekmvc -n DemoPekMvc -o .\Output\DemoPekMvc
```

完整示例：

```powershell
dotnet new pekmvc -n DemoPekMvc -o .\Output\DemoPekMvc --ProjectTitle DemoSite --ProjectDescription "Demo project description" --CompanyName DemoCompany --ServiceName DemoService --DbConnName DemoDb
```

生成后可编译项目：

```powershell
dotnet build .\Output\DemoPekMvc\DemoPekMvc.csproj -nologo
```

## 创建解决方案模板

最小命令：

```powershell
dotnet new pekmvc-sln -n DemoPekMvcSolution -o .\Output\DemoPekMvcSolution
```

完整示例：

```powershell
dotnet new pekmvc-sln -n DemoPekMvcSolution -o .\Output\DemoPekMvcSolution --ProjectTitle DemoSite --ProjectDescription "Demo project description" --CompanyName DemoCompany --ServiceName DemoService --DbConnName DemoDb
```

生成后可编译解决方案内项目：

```powershell
dotnet build .\Output\DemoPekMvcSolution\Mvc\DemoPekMvcSolution\DemoPekMvcSolution.csproj -nologo
```

## 生成后如何启动

### 启动项目模板生成结果

在项目目录执行：

```powershell
cd .\Output\DemoPekMvc
dotnet run
```

### 启动解决方案模板生成结果

在解决方案内项目目录执行：

```powershell
cd .\Output\DemoPekMvcSolution\Mvc\DemoPekMvcSolution
dotnet run
```

### 开发期常用动作

编译：

```powershell
dotnet build .\Output\DemoPekMvc\DemoPekMvc.csproj -nologo
```

发布：

```powershell
dotnet publish .\Output\DemoPekMvc\DemoPekMvc.csproj -c Release
```

## 模板参数

| 参数 | 默认值 | 说明 |
|------|--------|------|
| ProjectTitle | 工具网站 | 站点标题，同时写入程序集标题和默认站点标题 |
| ProjectDescription | 在线工具系统 | 项目描述，同时写入程序集描述和默认 SEO 描述 |
| CompanyName | 湖北登灏科技有限公司 | 公司名称，同时写入程序集 Company 等元数据 |
| ServiceName | __SERVICE_NAME__ | 服务注册名称。未显式传入时，运行时回退到项目名 |
| DbConnName | DH | XCode 数据库连接名，同时写入配置、实体和 Model.xml |

## 参数影响点

主要替换位置包括：

- 项目文件中的程序集元数据
- Program.cs 中的 RegisterService 服务名
- Common/PekStartup.cs 中的站点标题与描述
- Common/PekSettings.cs 中的配置分类
- appsettings.json 中的数据库连接名
- Entity/Model.xml 与实体文件中的 ConnName

## 验证状态

当前模板已完成以下验证：

- 模板包可正常打包
- 模板通过 PekBundle.Template 可正常安装
- pekmvc 可正常生成并编译通过
- pekmvc-sln 可正常生成并编译通过
- 聚合模板包 pack-template.ps1 -Install -SmokeTest 已验证通过

## 常用命令

查看模板帮助：

```powershell
dotnet new pekmvc -h
dotnet new pekmvc-sln -h
```

统一安装最新模板包：

```powershell
.\Templates\PeiKeSmart.Template.Bundle\pack-template.ps1 -Install
```