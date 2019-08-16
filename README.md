# StoryBlog #

Домашний проект для исследования возможностей платформы ASP.NET CORE 2.

[![Build Status](https://dev.azure.com/tolmachewladimir/tolmachewladimir/_apis/build/status/VlaTo.storyblog?branchName=master)](https://dev.azure.com/tolmachewladimir/tolmachewladimir/_build/latest?definitionId=3&branchName=master)

### Общие сведения ###

* Quick summary
* Version
* [Color palette](http://colormind.io/)
* [Learn Markdown](https://bitbucket.org/tutorials/markdowndemo)
* https://getbootstrap.com/docs/4.0/examples/blog/
* http://zoyothemes.com/blogezy/
* [Using Identity with IdentityServer4](https://github.com/IdentityServer/IdentityServer4.Samples/tree/master/Quickstarts/8_AspNetIdentity)
* [OpenID Connect Client](https://github.com/IdentityModel/IdentityModel.OidcClient2)
* [Base for the OIDC Client](https://github.com/IdentityModel/IdentityModel2)
* [Material Kit](https://demos.creative-tim.com/marketplace/material-kit-pro/index.html)
* [MDBootstrap](https://mdbootstrap.com/docs/jquery/components/demo/)
* [Start Bootstrap blog post](https://blackrockdigital.github.io/startbootstrap-blog-post/)
* [Start Bootstrap blog](https://blackrockdigital.github.io/startbootstrap-blog-home/)

### Работа с .NET Core Entity Framework ###

Открыть консоль диспетчера пакетов. Выбрать проект `src\Services\Blog\Blog.Persistent`, команды выполнять в этом окне
* Создаем новую миграцию
  > `Add-Migration <NameOfTheMigration>`
* Синхронизируем структуру база данных
  > `Update-Database`
