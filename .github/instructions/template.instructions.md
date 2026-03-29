---
applyTo: "Templates/**"
description: "Use when working on dotnet new template packages, template.json, pack-template.ps1, template README files, or aggregated template installation in the Templates directory."
---

# 模板开发指令

适用于 `Templates/` 目录下的 dotnet new 模板包、聚合模板包、模板文档与打包脚本。

## 1. 命名规范

- 模板显示名 `name` 必须以 `Pek` 开头，例如 `PekMvc Web 项目模板`、`PekVueZero 解决方案模板`
- 模板短名 `shortName` 必须以 `pek` 开头，例如 `pekmvc`、`pekvuezero-sln`
- 模板包 `PackageId` 与 `Title` 必须采用 `PekXxx.Template` 风格，例如 `PekMvc.Template`、`PekVueZero.Template`、`PekBundle.Template`
- 示例项目名、解决方案名、服务名优先使用 `DemoPekXxx` 风格
- 新增模板时，禁止引入不带 `Pek` 前缀的公开模板入口

## 2. 聚合安装约束

- 新增模板后必须保证聚合模板包一次安装即可提供全部模板
- 对外只保留 `PekBundle.Template` 一种安装方式，单模板包不得再提供直接安装入口
- 对外发布到 NuGet 时，也只允许发布 `PekBundle.Template`，禁止发布单模板包
- 优先复用聚合模板包现有自动导入机制，不要手工复制模板内容形成第二份来源
- 若新增模板需要新增校验规则，必须同步更新聚合模板包脚本

## 3. 同步修改要求

修改模板时，以下内容必须一起检查并按需同步：

- `template.json`
- `pack-template.ps1`
- 模板包 `README.md`
- 聚合模板包 `README.md`
- 聚合模板包验证或 SmokeTest 逻辑

## 4. 验证要求

- 至少验证 `dotnet pack`
- 如修改了模板入口或命名，必须验证 `dotnet new <shortName> -h`
- 若修改聚合模板包，必须验证一次安装后多个模板同时可用
- 修改模板规范后，必须运行 `Templates/check-template-conventions.ps1`