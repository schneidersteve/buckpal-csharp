# C# Example Implementation of a Hexagonal/Onion/Clean Architecture

Inspired by https://github.com/thombergs/buckpal

- Kotlin Version: https://github.com/schneidersteve/buckpal-kotlin
- Rust Version: https://github.com/schneidersteve/buckpal-rust
- Dart Version: https://github.com/schneidersteve/buckpal-dart
- Java Version: https://github.com/schneidersteve/buckpal-java

## Tech Stack

* [.NET SDK 7.0](https://dotnet.microsoft.com)
* [xUnit.net](https://xunit.net)
* [ASP.NET Core OData V4](https://learn.microsoft.com/en-us/odata/webapi-8/overview)
* [Dapper ORM](https://github.com/DapperLib/Dapper)
* [Visual Studio Code](https://code.visualstudio.com)
* [Visual Studio Code Dev Containers](https://code.visualstudio.com/docs/devcontainers/containers#_quick-start-open-a-git-repository-or-github-pr-in-an-isolated-container-volume)

## Layers and Dependency Inversion

![Dependency Inversion](di.png)

## Send Money Use Case

```gherkin
Feature: Send Money

  Scenario: Transaction succeeds
    Given a source account
    And a target account

    When money is send

    Then send money succeeds

    And source account is locked
    And source account withdrawal will succeed
    And source account is released

    And target account is locked
    And target account deposit will succeed
    And target account is released

    And accounts have been updated
```

# dotnet Examples

> dotnet test

> dotnet watch test

> find . -name "*.cs" | entr -cr dotnet test

> find Domain Domain.Test -name "*.cs" | entr -cr dotnet test
