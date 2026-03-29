# PeiKeSmart 聚合模板包说明

## 概述

该目录提供一套基于 dotnet new 的聚合模板包，用于一次安装多个模板。

当前聚合的模板包括：

- pekmvc：生成 PekMvc Web 项目
- pekmvc-sln：生成 PekMvc 解决方案骨架
- pekvuezero：生成 PekVueZero 前后端项目
- pekvuezero-sln：生成 PekVueZero 解决方案骨架

模板包项目：Templates/PeiKeSmart.Template.Bundle/PeiKeSmart.Template.Bundle.csproj

模板包 ID：PekBundle.Template

一键脚本：Templates/PeiKeSmart.Template.Bundle/pack-template.ps1

## 打包与安装

在仓库根目录执行：

```powershell
.\Templates\PeiKeSmart.Template.Bundle\pack-template.ps1
```

只打包不安装：

```powershell
dotnet pack .\Templates\PeiKeSmart.Template.Bundle\PeiKeSmart.Template.Bundle.csproj -c Release
```

安装最新模板包：

```powershell
Get-ChildItem .\Templates\PeiKeSmart.Template.Bundle\bin\Release\PekBundle.Template.*.nupkg |
  Sort-Object LastWriteTime -Descending |
  Select-Object -First 1 -ExpandProperty FullName |
  ForEach-Object { dotnet new install $_ }
```

发布到 NuGet 后，也可以直接安装：

```powershell
dotnet new install PekBundle.Template
```

安装指定 beta 版本：

```powershell
dotnet new install PekBundle.Template::1.0.2026.0329-beta0906
```

卸载模板包：

```powershell
dotnet new uninstall PekBundle.Template
```

## 一键脚本

脚本支持以下参数：

| 参数 | 说明 |
|------|------|
| Configuration | 打包配置，默认 Release |
| PackageVersion | 指定模板包版本号 |
| Install | 打包后自动安装模板 |
| SmokeTest | 打包后自动安装并执行生成与编译验证 |

示例：

```powershell
.\Templates\PeiKeSmart.Template.Bundle\pack-template.ps1 -Install -SmokeTest
```

执行安装时，脚本会先尝试卸载聚合包以及 Templates 目录下现有的单模板包，避免本机同时保留相同模板 identity 而出现重复警告。

在打包前，脚本会先执行 `Templates/check-template-conventions.ps1`，检查模板显示名、短名、重复项以及模板包必需文件是否符合规范。

## 生成模板机制

聚合包不会复制一份模板源代码，而是自动导入 Templates 目录下各模板包提供的 *.Template.Content.props 文件。

当前导入规则：

```xml
<Import Project="..\*.Template.Package\*.Template.Content.props" />
```

这意味着后续新增模板时，只要满足以下约定，就可以自动并入聚合包：

1. 模板包目录命名为 *.Template.Package
2. 模板包目录下提供一个 *.Template.Content.props 文件
3. props 文件中声明该模板需要打包到 nupkg 的 Content 项

满足上述约定后，重新打包聚合包即可自动带出新增模板，无需再修改聚合包项目文件。

另外，所有模板的显示名必须以 Pek 开头，shortName 必须以 pek 开头。聚合包脚本会在打包前自动检查这两条约定，避免不符合命名规范的模板进入安装包。

独立执行模板规范检查：

```powershell
.\Templates\check-template-conventions.ps1
```

仓库已提供自动校验工作流 [Templates Validation](.github/workflows/templates-validation.yml)。提交涉及 Templates 或模板指令的改动时，会自动执行规范检查、聚合安装、SmokeTest 生成编译验证和模板帮助验证。

仓库还提供发布工作流 [Publish Template Package](.github/workflows/publish-template-package.yml)。

- 推送模板相关更新到 `main` 或 `master` 时，默认自动发布 beta 版本
- 推送 `template-v11.9.2026.0329` 这类 tag 时，自动发布正式版本
- 手动触发时可选择 `beta` 或 `release`，也可以显式覆盖 `packageVersion`
- 发布前会先执行规范检查、聚合安装和 SmokeTest
- 仅发布 `PekBundle.Template`，不发布单模板包

版本号规则与仓库约定保持一致：

- 正式版：`{主}.{子}.{年}.{月日}`，例如 `1.0.2026.0329`
- 测试版：`{主}.{子}.{年}.{月日}-beta{时分}`，例如 `1.0.2026.0329-beta0906`

要使该工作流可用，需要在 GitHub 仓库 Secrets 中配置 `NUGET_API_KEY`。

## 常用命令

查看已安装模板：

```powershell
dotnet new list | Select-String -Pattern 'pekmvc|pekvuezero'
```

查看模板帮助：

```powershell
dotnet new pekmvc -h
dotnet new pekmvc-sln -h
dotnet new pekvuezero -h
dotnet new pekvuezero-sln -h
```