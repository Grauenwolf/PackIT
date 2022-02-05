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

Since the `command` parameter is marked as coming from the body, the `packingListId` and `name` values are going to be semi-ignored. The client has to provide them or the routing will fail and they will get a 404. But the `Put` method will never actually see the values and instead has to read them from the requests's body, which the client had to also populate.

For now we are only going to fix the Get method.


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

# PackIT
PackIT is simple "packing list app" built on top of clean architecture and CQRS.
