# Cleaning Clean Architecture - PackIT Edition

The purpose of this repository is to investigate "Clean Architecture" and see if it can be improved upon. The goal is to remove complexity without reducing functionality.

To order to see the transformation incrementally, a branch has been created for each step. Simply compare the branch with the one before it to see the progression.


## Round 0 - Base State Validation

To being with we need to see if the application can be compiled and run. Likewise, we need to verify that all of the automated tests run.

There are only a handful of tests, so this was fairly easy. Just needed to change the connection string to a unique username and give is a password. Which is acceptable for a demonstration project.

Manually testing the end points, we find that the situation isn't great.

The `/api/PackingLists/{id}` endpoint doesn't work at all. Instead of `[HttpGet("{Id}")]`, the route is defined as `[HttpGet("{id:guid}")]`. Capitalization matters here. When it is incorrect, the data binder can't correctly map the values in the route to the function parameters.

Most of the other routes are also bad. Consider this example,

```
[HttpPut("{packingListId:guid}/items/{name}/pack")]
public async Task<IActionResult> Put([FromBody] PackItem command)
```

Since the `command` parameter is marked as coming from the body, the `packingListId` and `name` values are going to be semi-ignored. The client has to provide them or the routing will fail and they will get a 404. But the `Put` method will never actually see the values and instead has to read them from the request’s body, which the client had to also populate.

For now, we are only going to fix the Get method.


## Round 1 - Fixing the Compiler Warnings

Consider this warning.

    CS0108	'PackingList.Id' hides inherited member 'AggregateRoot<PackingListId>.Id'. Use the new keyword if hiding was intended.	PackIT.Domain

That's pretty serious. 

```
    PackingList a = new PackingList();
    AggregateRoot<PackingListId> b = a;
    a.Id = Guid.NewGuid();
    Print(a.Id); //123e4567-e89b-12d3-a456-426614174000
    Print(b.Id); //00000000-0000-0000-0000-000000000000
```

This is going to be really confusing for whomever needs to troubleshoot why the `Id` is blank hald the time.

Fortunately the fix is as easy as just deleting the extraneous `Id` property from the `PackingList` class.

Then to prevent similar errors in the future, we're going add `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` to each csproj file.


## Round 2 - Detecting Unused Private Fields with Static Analysis

While looking at `PackingList` in Visual Studio, we notice that `Localization` is grayed out. Checking the compiler mesages, we see

```
IDE0052	Private member 'PackingList._localization' can be removed as the value assigned to it is never read	PackIT.Domain
```

Ok, let's do that. But before we do that, we should make it into a compiler error so no other unused fields slip into the code.

To do this, we create a `.editorconfig` file and place it in the `src` folder. By putting it here, all projects below it will inherit the same settings. This avoids the problem of having to copy and paste files into each project.

Rather than creating it from scratch, we used the "Add New Item" menu and selected the editor config file with ".NET" in its name. This adds a bunch of C# and VB sections with fairly reasonable defaults.  

Inside the `[*.{cs,vb}]` of the file we add these two lines.

```
dotnet_diagnostic.CA1823.severity=error
dotnet_diagnostic.IDE0052.severity=error
```

Rebuilding the project, we see that there is an unused parameter. So we'll have the compiler check for that as well.

```
dotnet_diagnostic.IDE0060.severity=error
```

In production grade code, there are two ways to handle static analysis.

1. Turn on the minimal rules at first. Then slowly add additional rules over time as problems are seen.
2. Turn on all the rules at first. Then remove the rules that are unreliable or not applicable.

## Round 3 - Merge the Shared Projects

A 'shared' project that could later be turned into a library that is reused across multiple applications is valuable. But arbitrarily dividing it into two projects just makes it hard to figure out where a given class is.

Furthermore, there is never a situation where an application would use one of the libraries without the other. So For now we'll merge `PackIT.Shared` and `PackIT.Shared.Abstractions`.

Long term, it may make sense to create subdivisions based on role such as `PackIT.Shared.Configuration` or `PackIT.Shared.Data`. But that is only necessary if some applications will only need a subset of the roles.


## Round 4 - Remove the Dispatcher Interfaces

The `InMemoryCommandDispatcher` class has a `ICommandDispatcher` interface. Likewise, the `InMemoryQueryDispatcher` class has a `IQueryDispatcher`. But why? They have no external dependencies that would need to be mocked. Nor is there an alternative implementation that could be used in their place. 

So those interfaces need to go away. Things that don't benefit the code, that have no real purpose, should be removed.

And while we're at it, the classes will be renamed `CommandDispatcher` and `QueryDispatcher`. The prefix is not necessary as obviously they are "in-memory".


## Round 5 - Remove the Hidden Dependency from the Command Dispatcher

Hidden dependencies are a bad thing. They make it very difficult to understand the program's flow, which in turn means that it isn't easy to understand where future changes need to be made. 

An example of a hidden dependency is the `LoggingCommandHandlerDecorator`. There is no mention of it in the handlers themselves. Nor is it mentioned in the `CommandDispatcher` code. By all appearances, the `CommandDispatcher` doesn't even look like it could support any sort of filter or pipelining capabilities.

Fortunately it is a trivial operation to move the logging directly into the `CommandDispatcher` class.

## Round 6 - Programming to the Interface, not the Implementation

Back in round 2, we removed the unused `_localization` field from `PackingList`. That was not without risk. This project doesn't have any tests to verify that data is actually being written to the database, all integration tests have to be performed by hand.

While the program appeared the be working correctly, it was silently losing data. Localization data was coming in from the UI and being lost along the way.

When you look at the code, you have to ask, "Why is a private field that the application itself can't see being stored in the database?". 

Turns out the magic is happening inside the `WriteConfiguration.Configure` class

```
            builder
                .Property(typeof(Localization), "_localization")
                .HasConversion(localizationConverter)
                .HasColumnName("Localization");
```

Rather than reading a property normally, some reflection magic is being used to look at the internals of the class.

You may have heard of the expression, "Program to the interface, not the implementation". A common misunderstanding of that term is that you should wrap everything in a shadow interface, for example all calls to a `Widget` object should be via a `IWidget` interface with exactly the same list of methods.

In actuality, what that expression means is that you should only access an object through its API. That would include our `IWidget` interface, but also any public method or property on `Widget` itself. 

What it doesn't include is reaching inside the object to examine its private fields. Those are private for a reason. They can change at any time and those changes should have zero impact on code outside of the class. 

### Fix

Step 1, restore the code that was deleted back in round 2.
Step 2, change the fields to be properties.

```
public PackingListName Name { get; init; }
public Localization Localization { get; init; }
```

Step 3, change the mapping to use said properties.

```
builder
    .Property(pl => pl.Localization)
    .HasConversion(localizationConverter)
    .HasColumnName("Localization");

builder
    .Property(pl => pl.Name)
    .HasConversion(packingListNameConverter)
    .HasColumnName("Name");
```


## Round 7 - Removing the Reflection from the Query Dispatcher

The query dispatcher has an... interesting design.

```
public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
{
    using var scope = _serviceProvider.CreateScope();
    var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
    var handler = scope.ServiceProvider.GetRequiredService(handlerType);

    return await (Task<TResult>)handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))?
        .Invoke(handler, new[] { query });
}
```

Reflection is quite expensive in .NET. You really shouldn't be using it unless you absolutely need it. And even then, only if you can cache the reflection calls so you don't need to make them multiple times.


We can remove the refleciton by adding a type parameter.

```
public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query)
    where TQuery : class, IQuery<TResult>
{
    using var scope = _serviceProvider.CreateScope();
    var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();


    var result = await handler.HandleAsync((TQuery)query);
    return (TResult)result;
}
```

Unfortunately, this means we have to likewise add that type parameter in the caller. 

```
var result = await _queryDispatcher.QueryAsync<GetPackingList, PackingListDto>(query);
```

A tiny amount of boilerplate added for potentially a lot of performance gain. (Though it should be noted that this would only matter if the web servers are under a heavy CPU load.)

## Round 8 - Removing the Dispatchers

Let's look at that code again. What exactly are the dispatchers doing?

* They use the DI framework to locate a service.
* They allow the developer to invoke the service.

And what does the ASP.NET Core pipeline do?

* They use the DI framework to locate a service.
* They allow the developer to invoke the service.

So why do we need the dispatcher? What does it give us what we didn't already have?

1. You don't have to instantiate services you don't need.

That's all? Ok, we can do that.

### Fix


Step 1 is to separate the controllers based on dependencies. We'll need three:

* `PackingListsCommandController`
* `PackingListsCreatePackingListWithItemsController`
* `PackingListsQueryController`

Note that the routes do not need to be changed. From the client's perspective, everything is still accessed via `/api/PackingLists/...`.

Step 2 is to create the 3 matching service classes.

* `PackingListCommandService` 
* `CreatePackingListWithItemsService` (Formally `CreatePackingListWithItemsHandler`)
* `PackingListQueryService` 

These three service classes replace the multitude of single-method handler classes. The `CreatePackingListWithItemsService` class is kept separate from the others because it has additional dependencies.

If you were really into extreme segregation, you could even create one controller per endpoint. Or even get rid of controllers entirely and use ASP.NET Core 6's minimal APIs. But we don’t need to go that far to achieve our goal.

## Round 9 - Taming the Extensions

In round 8 we were able to delete two of the `Extensions` classes as part of removing the dispatchers. But there are still 6 more identically names classes. Like a class named `Utilities` or `Miscellaneous`, a class named `Extensions` implies it's just a dumping ground for random functions.


### `PackIT.Shared.Options.Extensions`

This has a single function with the signature `TOptions GetOptions<TOptions>(IConfiguration, string)`.

The whole function can be replaced with `.GetSection(string).Get<T>` as shown below.

```
var options1 = configuration.GetOptions<PostgresOptions>("Postgres");
var options2 = configuration.GetSection("Postgres").Get<PostgresOptions>();
```

### `PackIT.Shared.Extensions`

This class contains `AddShared` and `UseShared`. It's functionality makes sense, as it registers all of the services from the shared library. But the generic name disguises its purpose. When used, you just see...

```
services.AddShared();
services.AddApplication();
services.AddInfrastructure(Configuration);
```

Shared what? There are no context clues from the name. And in theory all libraries could have an extension method named `AddShared`. So by convention, the library name or something equally specific is usually included in the method name.

With that said, we are going to make the following changes:

* `class StartupExtensions`
* `IServiceCollection AddPackITShared(this IServiceCollection services)`
* `IApplicationBuilder UsePackITShared(this IApplicationBuilder app)`

Now when someone looks at the startup logic, they can see identify what's being loaded without having to view the source. 

```
services.AddPackITShared();
services.AddApplication();
services.AddInfrastructure(Configuration);
```

### `PackIT.Application.Extensions`

Like `PackIT.Shared.Extensions`, we are going to rename the class to `StartupExtensions` and the methods to `AddPackITApplication`.

The name `Application` is less onerous that `Shared` because other libraries shouldn't have an extension with the same name. But we changed it anyways for the sake of consistency. 


### `PackIT.Infrastructure.EF.Queries.Extensions`

This has a single function with the signature `PackingListDto AsDto(this PackingListReadModel readModel)`.

There is zero reason for this to be an extension method as opposed to a regular method. Extension methods can be really useful when extending a class from another library or adding shared functionality to an interface. But if a normal method can be used, then it should be used.

Therefore `AsDto` is going to be moved into the class `PackingListReadModel`.


### `PackIT.Infrastructure.Extensions` and `PackIT.Infrastructure.EF.Extensions`

Similar to what we've seen, `PackIT.Infrastructure.Extensions` holds the extension method(s) for registering services found in the Infrastructure project. 

It does have an oddity though. Part of its `AddInfrastructure` method has been pulled into another `Extensions` class elsewhere in the project. So, you have to chase down the code to see what's really going on.

Hitting the "Inline and Delete" command pulls in that code, and this is what you get.

```
public static IServiceCollection AddPackITInfrastructure(this IServiceCollection services, IConfiguration configuration)
{
    services.AddScoped<IPackingListRepository, PostgresPackingListRepository>();
    services.AddScoped<IPackingListReadService, PostgresPackingListReadService>();

    var options = configuration.GetSection("Postgres").Get<PostgresOptions>();
    services.AddDbContext<ReadDbContext>(ctx => ctx.UseNpgsql(options.ConnectionString));
    services.AddDbContext<WriteDbContext>(ctx => ctx.UseNpgsql(options.ConnectionString));

    services.AddScoped<PackingListQueryService>();
    services.AddSingleton<IWeatherService, DumbWeatherService>();

    return services;
}
```

If you hadn't seen which lines were pulled into a separate method, could you guess what they were?

Probably not, because those lines are nearly identical to the ones before and after it. There's no complexity being hidden; it's just method calls for the sake of method calls.




# PackIT
PackIT is simple "packing list app" built on top of clean architecture and CQRS.



